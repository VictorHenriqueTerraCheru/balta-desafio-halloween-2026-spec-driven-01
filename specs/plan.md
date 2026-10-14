# Plano técnico

## Contexto

Implementa a `spec.md` (Gerador de Senhas Fortes) respeitando a `constitution.md`. Duas capacidades: gerar uma senha forte armazenada com identificador, e consultar a senha pelo identificador.

## Arquitetura

Um projeto de API e um projeto de testes, com separação por pastas dentro da API. A dependência só aponta para dentro: Endpoints → Application → Domain; Infrastructure implementa as interfaces.

```
src/
├── PasswordGenerator.Api/
│   ├── Domain/
│   │   ├── Entities/      # Entidade Password, regras do padrão de senha
│   │   └── Dtos/          # DTOs de request/response
│   ├── Interfaces/        # IPasswordService, IPasswordGenerator, IPasswordRepository
│   ├── Application/       # Implementação do serviço de aplicação
│   ├── Infrastructure/    # DbContext, repositório EF Core, gerador aleatório
│   ├── Endpoints/         # MapPasswordEndpoints
│   ├── Extensions/        # AddApplication / AddInfrastructure (registro de DI)
│   └── Program.cs         # Só chama as extensões e mapeia endpoints
└── PasswordGenerator.Tests/
    ├── Unit/              # Domínio e serviço de aplicação
    └── Integration/       # Endpoints via WebApplicationFactory
```

Fluxo de geração: endpoint → `IPasswordService.GenerateAsync` → gerador cria o valor → entidade `Password` nasce com seu GUID → repositório persiste → endpoint devolve id e senha.

Fluxo de consulta: endpoint recebe o id já tipado como GUID (id inválido nem chega ao serviço) → `IPasswordService.GetAsync` → repositório busca → devolve senha ou "não encontrado".

## Decisões

| ID | Decisão | Justificativa | Atende |
|---|---|---|---|
| D-01 | Senha gerada com tamanho fixo de 20 caracteres | A spec fixa só o piso (≥16). Um valor fixo cumpre RN-08 (padrão sempre igual, usuário não escolhe) e dá folga sobre o mínimo. | RN-01, RN-08 |
| D-02 | Gerar com `RandomNumberGenerator` (criptograficamente seguro), não `Random` | Senha previsível não é "forte". `Random` é determinístico por seed. | RN-09 |
| D-03 | Algoritmo: sortear 1 caractere de cada categoria (maiúscula, minúscula, número, especial), completar o restante sorteando do conjunto total e embaralhar o resultado | Garante RN-03 por construção, sem laço de "gerar e testar até passar". O embaralhamento evita posição previsível das categorias. | RN-02 a RN-05 |
| D-04 | Conjunto de especiais: `! @ # $ % ^ & * ( ) - _ = + [ ] { } ? .` e alfabeto sem espaço | Lista literal da RN-05. Espaço nunca pertence a nenhum conjunto, então RN-04 vale por construção. | RN-04, RN-05 |
| D-05 | O GUID é gerado dentro da entidade de domínio `Password` (fábrica estática `Create`), com `Guid.NewGuid()` | Pedido do dono do projeto e coerente com RN-07: o identificador nunca vem de fora. Mantém a regra no domínio, não no endpoint ou no banco. | RN-06, RN-07 |
| D-06 | SQLite com EF Core, chave primária = GUID | Constituição fixa a stack. Persistência necessária por RF-02. | RF-02, RF-05 |
| D-07 | Criar o schema com `EnsureCreated` na subida, sem migrations | Uma única tabela e projeto lúdico; migrations adicionam arquivos e comandos sem ganho agora. Se o modelo crescer, esta decisão é revista. | RF-02 |
| D-08 | Senha armazenada em texto puro | Fora de escopo explícito da spec (criptografia) e RF-04 exige devolver o valor original. | RF-04, Fora de escopo |
| D-09 | Rota `GET /passwords/{id}` com o parâmetro do handler tipado como `Guid id`, **sem** constraint `{id:guid}` na rota | O binding do Minimal API converte o id e, se falhar, responde 400 automaticamente, sem código manual. A constraint `{id:guid}` faria o id inválido não casar com a rota e cair em 404, contrariando a spec. Com `AddProblemDetails`, o corpo do 400 é `ProblemDetails`. | RF-06, B-01 |
| D-10 | O parse nativo de GUID já aceita maiúsculas/minúsculas | Atende B-06 sem código extra. | B-06 |
| D-11 | `IPasswordGenerator` como interface injetada no serviço | Permite testar o serviço com gerador falso e testar o gerador real separadamente. | Constituição (testes) |
| D-12 | Swagger via `Microsoft.AspNetCore.OpenApi` + `Swashbuckle.AspNetCore.SwaggerUI` | O .NET 10 gera o documento OpenAPI nativamente, mas não traz a interface visual. O UI do Swagger é necessário porque o desafio será testado via Swagger. Única dependência extra além da stack. | Constituição (Swagger) |
| D-13 | Testes de integração com `WebApplicationFactory` e SQLite em memória (conexão mantida aberta) | Constituição exige banco isolado. SQLite em memória exercita o mesmo provedor da produção. | Constituição (testes) |
| D-14 | Registro de DI em métodos de extensão `AddApplication` e `AddInfrastructure` | Regra da constituição: `Program.cs` não registra serviços direto. | Constituição |
| D-15 | A criação do schema acontece na subida da aplicação, antes de atender requisições: a classe `DatabaseInitializer` (em `Infrastructure/`) executa `Database.EnsureCreatedAsync()` dentro de um escopo de DI, e é chamada pelo método de extensão `UseDatabaseInitialization` (em `Extensions/`) invocado em `Program.cs` antes de `app.Run()`. Os testes de integração usam esse mesmo caminho, sem chamada manual. | Sem isso a API sobe com a tabela ausente e toda geração falha com 500 (`no such table: Passwords`); os testes não pegaram o defeito porque a factory criava o schema por conta própria. O `DbContext` fica restrito à camada de persistência, como manda a constituição, e `Program.cs` só chama extensões. Não dá para fazer em `AddInfrastructure`, porque ali ainda não existe `IServiceProvider` construído. | D-07, RF-02, Constituição (Arquitetura) |

## Modelo de dados

Tabela `Passwords`:

| Coluna | Tipo | Restrições |
|---|---|---|
| `Id` | GUID (TEXT no SQLite) | Chave primária, gerado pelo domínio |
| `Value` | TEXT | Obrigatório |

Sem coluna de data ou outros metadados: nenhum requisito da spec os exige.

## Contratos

### Gerar senha

`POST /passwords` (sem corpo)

Resposta `201 Created`:

```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3301",
  "password": "aB3$kL9#mQ2!xZ7&pR4@"
}
```

### Consultar senha

`GET /passwords/{id}`

Resposta `200 OK`:

```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3301",
  "password": "aB3$kL9#mQ2!xZ7&pR4@"
}
```

### Erros (`ProblemDetails`)

| Situação | Status | Referência |
|---|---|---|
| `id` com formato inválido | `400 Bad Request` | RF-06, B-01 |
| `id` válido e inexistente | `404 Not Found` | RF-07, B-02, B-03 |

## Riscos

| ID | Risco | Mitigação |
|---|---|---|
| R-01 | Compatibilidade do Swashbuckle com o OpenAPI nativo do .NET 10 | Validar na primeira tarefa de Swagger; se falhar, registrar a troca em decisão nova (ex.: outra UI) antes de seguir. |
| R-05 | O 400 automático do binding pode não sair como `ProblemDetails` conforme a configuração do pipeline | Teste de integração valida status 400 e corpo `ProblemDetails`; se falhar, ajustar o pipeline e registrar a mudança aqui. |
| R-02 | Banco em memória nos testes perde dados se a conexão fechar | Manter a conexão aberta durante o ciclo de vida da factory (D-13). |
| R-03 | Texto puro pode ser levado a sério fora do contexto de aprendizado | Aviso na documentação e na `readme.md`: não usar em produção. |
| R-04 | Colisão de GUID | Probabilidade desprezível; a chave primária garantiria falha explícita em vez de sobrescrita. |

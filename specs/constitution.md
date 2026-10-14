# Constituição do projeto

Regras que valem para qualquer funcionalidade deste projeto. Toda regra é verificável: dá para olhar o código ou rodar um comando e dizer se foi cumprida.

## Stack

- Linguagem e runtime: C# com .NET 10.
- API: Minimal API (sem controllers).
- Persistência: SQLite via EF Core.
- Testes: xUnit.
- Documentação e teste manual da API: Swagger/OpenAPI exposto pela própria aplicação.
- Nenhuma dependência nova entra sem justificativa escrita em `specs/plan.md`.

## Arquitetura

- Endpoint não acessa o banco diretamente: sempre chama um serviço de aplicação.
- Regra de negócio não vive no endpoint nem no handler HTTP; vive no serviço de aplicação ou no domínio.
- O `DbContext` só é referenciado pela camada de persistência e pelos serviços de aplicação, nunca pelos endpoints.
- Endpoints são agrupados por funcionalidade em métodos de extensão (`MapXxxEndpoints`), e `Program.cs` apenas os registra.
- Dependências são injetadas pelo contêiner do .NET; nenhuma classe de negócio usa `new` para criar serviço ou `DbContext`.
- O registro de dependências vive em um arquivo próprio de extensão (`AddXxx` em `IServiceCollection`), e `Program.cs` apenas o chama, sem registrar serviço diretamente.

## Qualidade

- A aplicação compila com zero erros e zero warnings (`TreatWarningsAsErrors` habilitado).
- `dotnet test` passa sem falhas e sem testes ignorados.
- Toda regra de negócio tem ao menos um teste unitário.
- Todo endpoint tem ao menos um teste de integração do caminho feliz.
- Todo erro de validação retorna HTTP 400 com corpo `ProblemDetails`.
- Testes de integração usam banco isolado (SQLite em memória ou arquivo temporário), nunca o banco de desenvolvimento.

## Convenções

- Código (classes, métodos, variáveis, rotas) em inglês; documentação em `specs/` em português.
- Rotas em minúsculas, no plural quando representam coleção (ex.: `/passwords`).
- Identificadores públicos são GUID; o `Id` interno nunca é inteiro sequencial exposto na API.
- Cada arquivo contém um único tipo público.
- Nenhum segredo, string de conexão de produção ou dado pessoal é commitado.
- Estrutura: código em `src/`, testes em `src/` em projeto separado com sufixo `.Tests`.

## Governança

- Toda mudança de código nasce de uma tarefa registrada em `specs/tasks.md`.
- Cada tarefa referencia o requisito que a originou (RF, RN ou borda da `spec.md`).
- A IA implementa uma tarefa por vez e não altera arquivos fora do escopo dela.
- Decisão técnica nova ou alterada é registrada em `specs/plan.md` com justificativa, antes de ser implementada.
- Mudança nesta constituição só acontece de forma explícita e em commit próprio.
- Este projeto é lúdico, para aprendizado de SDD: não usar em produção e não há exigência de criptografia ou outros recursos de segurança.

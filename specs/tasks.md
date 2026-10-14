# Tarefas

Cada tarefa é pequena, ordenada e verificável sozinha. A coluna **Origem** aponta para o requisito (`spec.md`), decisão (`plan.md`, D-xx) ou regra da `constitution.md` que a originou.

| ID | Tarefa | Origem | Depende de | Concluída quando |
|---|---|---|---|---|
| T-01 | Criar solução, projetos `PasswordGenerator.Api` e `PasswordGenerator.Tests` e estrutura de pastas do plano; habilitar `TreatWarningsAsErrors` | Constituição (Stack, Qualidade), Plano (Arquitetura) | | `dotnet build` compila com zero erros e zero warnings; `dotnet test` roda (mesmo sem testes) |
| T-02 | Criar entidade `Password` com fábrica `Create(value)` que gera o GUID dentro do domínio | RN-06, RN-07, D-05 | T-01 | Teste unitário: `Id` diferente de `Guid.Empty`, dois `Create` geram ids distintos, `Value` é preservado |
| T-03 | Criar `IPasswordGenerator` e implementação com `RandomNumberGenerator`, 20 caracteres, 1 de cada categoria e embaralhamento | RN-01 a RN-05, RN-08, RN-09, D-01 a D-04, D-11 | T-01 | Teste unitário (repetido em 1000 gerações): tamanho 20, ao menos 1 maiúscula, minúscula, número e especial, só caracteres do conjunto permitido, nenhum espaço; duas gerações diferem |
| T-04 | Criar `IPasswordRepository`, `DbContext` e mapeamento da tabela `Passwords`, com `EnsureCreated` | RF-02, D-06, D-07, Plano (Modelo de dados) | T-02 | Teste cria o schema em SQLite em memória e a tabela `Passwords` tem `Id` como chave e `Value` obrigatório |
| T-05 | Implementar repositório EF Core (adicionar e buscar por `Id`) | RF-02, RF-04, RF-05 | T-04 | Teste: salva e recupera a mesma senha; id inexistente retorna nulo; duas leituras seguidas retornam o mesmo valor |
| T-06 | Criar `IPasswordService` e implementar `GenerateAsync` (gera valor, cria entidade, persiste, devolve id e senha) | RF-01, RF-02, RF-03 | T-02, T-03, T-05 | Teste unitário com gerador e repositório falsos: entidade é persistida e o retorno contém id e senha |
| T-07 | Implementar `GetAsync` no serviço (devolve a senha ou indica não encontrado) | RF-04, RF-05, RF-07 | T-06 | Teste unitário: id existente retorna a senha; id inexistente indica não encontrado; duas consultas retornam o mesmo valor |
| T-08 | Criar DTOs de resposta em `Domain/Dtos` | Plano (Contratos, Arquitetura) | T-01 | Projeto compila; DTO contém `id` e `password` |
| T-09 | Criar extensões `AddApplication` e `AddInfrastructure` e ligar no `Program.cs` | D-14, Constituição (Arquitetura) | T-05, T-07 | Aplicação sobe e resolve `IPasswordService`; `Program.cs` não registra serviço diretamente |
| T-10 | Endpoint `POST /passwords` retornando 201 com id e senha | RF-01, RF-03, CA-01, CA-02 | T-08, T-09 | Teste de integração: 201, `id` é GUID válido, `password` tem 20 caracteres e atende às regras de composição |
| T-11 | Endpoint `GET /passwords/{id}` com parâmetro `Guid id` retornando 200 | RF-04, RF-05, D-09, CA-03, B-04, B-06 | T-10 | Teste de integração: gerar e consultar duas vezes retorna a mesma senha; consulta com o id em maiúsculas também retorna 200 |
| T-12 | Retornar 404 com `ProblemDetails` quando o id não existe | RF-07, B-02, B-03, CA-05 | T-11 | Teste de integração: id inexistente e id só com zeros retornam 404 com corpo `ProblemDetails` |
| T-13 | Garantir 400 com `ProblemDetails` quando o id é mal formatado (`AddProblemDetails`) | RF-06, B-01, D-09, R-05, CA-04 | T-11 | Teste de integração: `abc` retorna 400 com corpo `ProblemDetails` |
| T-14 | Teste de integração de gerações simultâneas | RN-06, B-05, CA-06 | T-10 | Teste dispara 50 gerações em paralelo e todos os ids e senhas retornados são distintos |
| T-15 | Habilitar Swagger UI | D-12, R-01, Constituição (Stack) | T-10 | `/swagger` abre e lista `POST /passwords` e `GET /passwords/{id}`; build sem warnings |
| T-16 | Validação final da definição de pronto | Desafio (Definição de pronto), Constituição (Qualidade) | T-01 a T-15, T-17 | `dotnet build` com zero erros e warnings; `dotnet test` todo verde; a API sobe e responde pelo Swagger |
| T-17 | Criar o schema na subida da aplicação: `DatabaseInitializer` em `Infrastructure/` com `EnsureCreatedAsync`, exposto por `UseDatabaseInitialization` em `Extensions/` e chamado em `Program.cs` antes de `app.Run()` | D-07, D-15, RF-02 | T-09, T-10 | Teste de integração sem chamada manual de schema: `POST /passwords` em base vazia retorna 201; `PasswordApiFactory` não chama mais `EnsureDatabaseCreated` (método removido) e a suíte continua verde |

<img width="100%" alt="Halloween 2026" src="https://baltaio.blob.core.windows.net/static/images/v4/challenges/halloween-2026/banner.jpg" />

## 🎃 Halloween - Desafio 1

Oi, eu sou o Victor Henrique Terra Cherú e este é o espaço onde compartilho minha jornada de aprendizado durante o desafio **Halloween 2026**, realizado pelo [balta.io](https://balta.io). 👻

Aqui você vai encontrar projetos, exercícios e códigos que estou desenvolvendo durante o desafio.

### Sobre este desafio
Neste desafio o objetivo é consolidar os fundamentos do Spec Driven Development, criando uma constituição, especificação, planejamento e tarefas manualmente para serem implementadas pela IA posteriormente.

#### O que foi implementado
- Gerador de senhas com 20 caracteres, com ao menos 1 maiúscula, 1 minúscula, 1 número e 1 caractere especial, embaralhado com `RandomNumberGenerator`
- `POST /passwords`: gera e persiste uma senha, retornando `201` com `id` (GUID) e `password`
- `GET /passwords/{id}`: consulta uma senha existente pelo id
- `404` com `ProblemDetails` para id inexistente e `400` com `ProblemDetails` para id mal formatado
- Criação do schema na subida da aplicação (`DatabaseInitializer`)
- Swagger/OpenAPI disponível em `/swagger`
- Testes unitários e de integração com SQLite temporário, incluindo teste de gerações simultâneas

Stack: C# com .NET 10, Minimal API, EF Core com SQLite e xUnit.

Neste processo eu aprendi:
* ✅ **Spec Driven Development**: o código nasce de documentos escritos antes, e não o contrário
* ✅ **Constituição**: regras que valem para todo o projeto e podem ser verificadas
* ✅ **Especificação (`spec.md`)**: o quê e por quê, com requisitos, regras, bordas e critérios de aceite, sem falar de tecnologia
* ✅ **Plano (`plan.md`)**: o como, com decisões justificadas, modelo de dados e contratos
* ✅ **Tarefas (`tasks.md`)**: pequenas, ordenadas e ligadas à origem (requisito ou decisão), implementadas uma por vez pela IA
* ✅ A ordem de autoridade (constituição > spec > plano > tarefas) ajuda a resolver conflitos entre os documentos

## Bagde
<img src="https://baltaio.blob.core.windows.net/static/images/v4/challenges/halloween-2026/01.png" width="200" />

## Problema
--

## Sobre o Halloween 2026
O desafio **Halloween 2026** consiste em implementar implementar o modelo Spec Driven Development de ponta a ponta, criando apps completas com IA.

### Veja meu progresso no desafio
- ✅ Constituição, especificação, plano e tarefas (`specs/`)
- ✅ Tarefas T-01 a T-17 implementadas, uma por vez
- ✅ Validação final (T-16): `dotnet build` sem erros e sem warnings, 26 testes passando e Swagger funcionando

Repositório central com o progresso de todos os desafios: https://github.com/VictorHenriqueTerraCheru/balta-desafio-halloween-2026

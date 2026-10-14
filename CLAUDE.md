 Todo o código é gerado a partir das especificações em `specs/`.

## Constituição (vale sempre)

@specs/constitution.md

## Documentos do projeto

Ordem de autoridade: `constitution.md` > `spec.md` > `plan.md` > `tasks.md`. Em caso de conflito, vence o de cima.

- `specs/spec.md`: o quê e por quê (requisitos RF, regras RN, bordas B, critérios CA). Não contém tecnologia.
- `specs/plan.md`: como (decisões D-xx com justificativa, modelo de dados, contratos, riscos).
- `specs/tasks.md`: lista ordenada de tarefas T-xx. A coluna **Origem** aponta o requisito ou decisão que a originou.

Leia `spec.md` e `plan.md` sob demanda: para cada tarefa, consulte os itens citados na coluna **Origem** e a seção **Decisões** do plano.

## Como trabalhar

- Implemente **uma tarefa por vez**, a que o usuário indicar (ex.: T-03). Nunca outra.
- Não altere arquivos fora do escopo da tarefa.
- Antes de começar, confirme que as tarefas da coluna **Depende de** estão concluídas.
- Escreva o teste junto com a implementação, conforme a constituição.
- Código vai em `src/`. Nunca altere `specs/` sem o usuário pedir.
- Se a spec, o plano ou a tarefa estiver ambíguo ou contraditório, **pare e pergunte** em vez de assumir.
- Ao final, informe: o que mudou, qual requisito isso atende e o resultado de `dotnet build` e `dotnet test`.

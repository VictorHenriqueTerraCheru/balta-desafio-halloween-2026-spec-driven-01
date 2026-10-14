# Gerador de Senhas Fortes

## Problema

Pessoas precisam de senhas fortes e não querem inventá-las, e muitas vezes precisam recuperar a mesma senha mais tarde. Hoje não existe um lugar simples para gerar uma senha padronizada e consultá-la de novo depois.

## Objetivo

Oferecer duas capacidades:

1. Gerar uma senha forte seguindo sempre o mesmo padrão e devolver um identificador único para ela.
2. Consultar, a qualquer momento, a senha associada a um identificador.

Projeto lúdico, para aprendizado de Spec-Driven Development. Não deve ser usado em produção.

## Usuários

- **Pessoa usuária**: quem precisa gerar e consultar senhas, usando a interface de documentação interativa da API.
- **Sistema cliente**: qualquer programa que consuma a API para gerar e consultar senhas.

## Histórias

- **H-01**: Como usuário, quero gerar uma senha forte padronizada, para não precisar inventar uma.
- **H-02**: Como usuário, quero receber um identificador junto com a senha, para poder encontrá-la depois.
- **H-03**: Como usuário, quero consultar uma senha pelo seu identificador, para recuperá-la quando precisar.
- **H-04**: Como usuário, quero consultar a mesma senha mais de uma vez, para não perdê-la na primeira consulta.
- **H-05**: Como usuário, quero receber um erro claro quando o identificador é inválido ou não existe, para saber o que corrigir.

## Requisitos funcionais

- **RF-01**: O sistema gera uma nova senha a cada solicitação de geração, sem receber parâmetros do usuário.
- **RF-02**: O sistema armazena toda senha gerada, de forma que ela possa ser recuperada depois.
- **RF-03**: O sistema retorna, junto com a senha gerada, um identificador único associado a ela.
- **RF-04**: O sistema permite consultar uma senha informando o identificador recebido na geração.
- **RF-05**: O sistema permite consultar a mesma senha quantas vezes forem necessárias, sem alterá-la nem removê-la.
- **RF-06**: O sistema retorna um erro de requisição inválida quando o identificador informado na consulta não tem formato válido.
- **RF-07**: O sistema retorna um erro de não encontrado quando o identificador tem formato válido, mas não existe nenhuma senha associada a ele.

## Regras de negócio

- **RN-01**: Toda senha gerada tem no mínimo 16 caracteres.
- **RN-02**: Toda senha é composta apenas por letras, números e caracteres especiais.
- **RN-03**: Toda senha contém ao menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial.
- **RN-04**: Nenhuma senha contém espaço em branco.
- **RN-05**: Os caracteres especiais permitidos são: `! @ # $ % ^ & * ( ) - _ = + [ ] { } ? .`
- **RN-06**: O identificador é único: duas senhas nunca compartilham o mesmo identificador.
- **RN-07**: O identificador é gerado pelo próprio sistema (nunca informado pelo usuário) e tem o formato GUID.
- **RN-08**: O padrão de senha é sempre o mesmo: o usuário não escolhe tamanho nem tipos de caractere.
- **RN-09**: Duas gerações consecutivas não devem produzir a mesma senha de forma previsível (a geração é aleatória).

## Casos de borda

- **B-01**: Identificador mal formatado (ex.: `abc`, vazio, caracteres inválidos) retorna o erro de requisição inválida (RF-06).
- **B-02**: Identificador bem formatado mas inexistente retorna o erro de não encontrado (RF-07).
- **B-03**: Identificador com todos os zeros (`00000000-0000-0000-0000-000000000000`) é bem formatado; se não existir, retorna não encontrado.
- **B-04**: Consultas repetidas ao mesmo identificador retornam sempre a mesma senha.
- **B-05**: Gerações simultâneas produzem senhas e identificadores distintos, sem colisão.
- **B-06**: Letras maiúsculas e minúsculas no identificador são tratadas como equivalentes.

## Fora de escopo

- Criptografia ou proteção da senha armazenada.
- Autenticação, autorização ou controle de quem pode consultar.
- Expiração, atualização ou remoção de senhas.
- Escolha de tamanho ou tipos de caractere pelo usuário.
- Listagem de todas as senhas geradas.
- Interface gráfica própria (além da documentação interativa da API).
- Uso em produção.

## Critérios de aceite

- **CA-01** (RF-01, RF-03, RN-01): Ao gerar uma senha, recebo uma senha com no mínimo 16 caracteres e um identificador.
- **CA-02** (RN-02, RN-03, RN-04, RN-05): A senha gerada contém maiúscula, minúscula, número e caractere especial permitido, e não contém espaço.
- **CA-03** (RF-04, RF-05, B-04): Ao consultar o identificador recebido, obtenho a mesma senha, em quantas consultas eu fizer.
- **CA-04** (RF-06, B-01): Ao consultar um identificador mal formatado, recebo erro de requisição inválida.
- **CA-05** (RF-07, B-02): Ao consultar um identificador bem formatado e inexistente, recebo erro de não encontrado.
- **CA-06** (RN-06, B-05): Duas gerações distintas retornam identificadores distintos.

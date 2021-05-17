# R.O.B.O
Implementação do R.O.B.O (Robô Operacional Binariamente Orientado).

## How To
Para testar a aplicação, basta clonar o respositório em sua máquina e buildar os dois projetos (set startup projects) que estão na camada de apresentação (Becomex.Robo.Api e Becomex.Robo.UserInterface). Na interface gráfica existem duas páginas:

* Registrar: traz uma lista com os robôs cadastrados na aplicação e possibilita incluir novos robôs para simulação.
* Controlador: inicialmente traz apenas um campo para informar o Id do Robô e, após clicar no botão "Aplicar", expõe os dados do robô com os botões de simulação. 

### Observações
Antes de subir os projetos em conjunto é necessário buildar a Api de forma isolada para criação do XML de documentação do swagger.

### Endpoints implementados

* `POST /api/v1/robots`
* `GET /api/v1/robots`
* `GET /api/v1/robots/{robotId}`
* `PUT /api/v1/robots/{robotId}`
* `PUT /api/v1/robots/{robotId}/fix`
* `PUT /api/v1/robots/{robotId}/moveHead`
* `PUT /api/v1/robots/{robotId}/moveElbow`
* `PUT /api/v1/robots/{robotId}/moveFist`

### Importante
Aplicação desenvolvida em .NET Core 3.1.

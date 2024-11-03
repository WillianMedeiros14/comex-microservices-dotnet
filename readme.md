## Sobre o Projeto 🚀

### Contexto

Esse projeto baseado em microsserviços oferece uma excelente oportunidade para o aprendizado de arquitetura distribuída. Com dois serviços independentes (Pedido e Estoque) que se comunicam de maneira síncrona e assíncrona, exploramos conceitos fundamentais de design de APIs, mensageria com RabbitMQ, e manipulação de banco de dados. Este exercício promove a compreensão das melhores práticas em desenvolvimento de software, como desacoplamento, escalabilidade e uso de Docker para conteinerização. Apesar de ser uma solução simples, prepara o terreno para desafios mais complexos em projetos de larga escala, distribuindo a responsabilidade dos serviços de forma organizada e eficiente.

### Atividade

Esta solução consiste em dois microsserviços:

- Microsserviço de Pedido
- Microsserviço de Estoque

### Regras de Negócio

Gestão de Pedidos:

- Usuários podem criar pedidos com um ou mais itens.
- Após a criação do pedido, o serviço de pedidos reduz o estoque de cada item associado, garantindo a disponibilidade no estoque.
- Pedidos são recusados se não houver quantidade suficiente de itens no estoque.

Gestão de Estoque:

- Permite consulta de itens disponíveis no estoque.
- Atualiza a quantidade de itens sempre que um pedido é confirmado.
- Garantia de que itens no estoque não tenham quantidade negativa.

### Requisitos Técnicos

- Dois projetos Web API separados, representando os microsserviços de Pedido e Estoque.
- Comunicação entre os microsserviços usando HTTP síncrono (para consulta de disponibilidade de estoque) e RabbitMQ (para processamento assíncrono da redução de estoque após a criação do pedido).
- Cada serviço possui seu próprio banco de dados SQL, mas ambos estão na mesma instância.
- Docker é usado para orquestrar os serviços e RabbitMQ.

### Resultado Esperado

- Microsserviço de Pedido: Responsável pela criação de pedidos e validação da disponibilidade de estoque.
- Microsserviço de Estoque: Responsável por manter e atualizar a quantidade de itens em estoque.
- Comunicação entre os microsserviços utilizando HTTP e RabbitMQ.
- Implementação de uma arquitetura que permite a escalabilidade e manutenção dos serviços.

## Tecnologias Utilizadas 🛠️

As principais tecnologias e bibliotecas utilizadas no projeto incluem:

- **.NET Core 6.0**
- **PostgreSQL**
- **Swagger**
- **Docker e Docker Compose**
- **RabbitMQ**

---

## Configuração do Ambiente ⚙️

1. Clone o repositório e entre na pasta:

   ```bash
   git clone https://github.com/WillianMedeiros14/comex-microservices-dotnet
   cd comex-microservices-dotnet
   ```

## Rodando a Aplicação ▶️

- Esteja na raiz do projeto e rode o comando abaixo:

  ```bash
  docker compose up
  ```

## Acesso as aplicações

Depois de rodar as aplicações, elas estarão disponíveis em:

- Serviço de Estoque: http://localhost:8081/swagger/index.html
- Serviço de Pedidos: http://localhost:8082/swagger/index.html
- interface de gerenciamento do RabbitMQ: http://localhost:15672

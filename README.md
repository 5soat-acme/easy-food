# Introdução :hamburger:

O Easy Food é um sistema de atendimento e gestão de pedidos para lanchonetes. O objetivo é melhorar a experiência do cliente, reduzindo erros de atendimento e otimizando a processo de realização e entrega de pedidos aos clientes.

O projeto é parte do trabalho de conclusão do curso de Arquitetura de Software da FIAP o intuito é aplicar os conceitos aprendidos durante o curso, como Domain Driven Design, arquitetura de microsserviços, design de APIs, DevOps, etc.

**Para maiores informações sobre o projeto, modelo estratatégico, entre outros, acesse a nossa [Wiki](https://5soat-acme.github.io/easy-food/docs/intro)**

# Tecnologias utilizadas :computer:

- .NET 8.0
  - ASP.NET Identity Core
  - ASP.NET Web API
  - Entity Framework Core
- PostgreSQL
- Docker

# Arquitetura :triangular_ruler:

- Arquitetura Hexagonal
- DDD - Domain Driven Design
- Domain Events
- Domain Validations
- Repository Pattern
- Unit Of Work Pattern
- CQS - Command Query Separation

# Overview da arquitetura :mag:
Na primeira fase do projeto, foi desenvolvido um monolito modular para fazer uma separação clara dos contextos delimitados mapeados na modelagem estratégica. Separamos a implementação em 3 pastas principais:
- **Presentation:** É a camada que expõe os serviços da aplicação. É responsável por receber as requisições HTTP, fazer a validação dos dados de entrada, mapear os dados de entrada para os objetos de domínio, chamar os serviços de aplicação e retornar os dados de saída.
- **Services:** É onde estão implementados os serviços de aplicação. Dentro desta pasta dividimos em subpastas que representam os contextos delimitados. Cada subpasta contém as camadas do serviço, como **Application, Domain, Infra**, entre outras.
- **Shared:** É aqui que compartilhamos o que é comum entre os diferentes módulos, inclusive os objetos de domíno e os serviços de infraestrutura que podem ser utilizados por mais de um contexto delimitado.

### Estrutura
![img.png](docs/img/img.png) </br>
![img_1.png](docs/img/img_1.png) </br>

### Overview
![img_2.png](docs/img/img_2.png)

# Como executar :rocket:

A seguir estão as instruções para executar o projeto localmente ou utilizando o Docker.

## Docker :whale:
### Pré-requisitos :clipboard:
É necessário que o **Docker** esteja instalado na máquina. Para instalar, siga as instruções do site oficial: https://docs.docker.com/get-docker/

### Executando :running:

Deixamos disponível um arquivo docker-compose-local.yml para facilitar a execução do projeto. Para executar, basta executar o comando abaixo na raiz do projeto no terminal do seu sistema operacional:

```bash
docker-compose -f ./deploy/docker/docker-compose-local.yml up -d
```

O comando acima irá criar um container para a aplicação e outro para a base de dados. Além disso, o volume da base também será criado para que os dados sejam persistidos mesmo após a parada do container.
A primeira vez que o volume for criado a criação das tabelas e a inserção dos dados iniciais será feita automaticamente. Caso queira recriar as tabelas e inserir novamente os dados, basta excluir o volume e executar o comando acima outra vez.

Com a aplicação em execução, basta acessar a URL **[http://localhost:8080/swagger](http://localhost:8080/swagger)** para acessar a documentação da API.

## Localmente :computer:
### Pré-requisitos :clipboard:
Para executar localmente certifique-se de ter a sua IDE de preferência instalada, além do **.NET 8.0 SDK**. Para instalar o SDK, siga as instruções do site oficial **[aqui](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)**. Além disso, é necessário ter o **PostgreSQL** instalado na máquina ou em um container. Para instalar diretamente na sua máquina verifique a documentação **[aqui](https://www.postgresql.org/download/)**. Se preferir utilizar um container, verifique como fazer **[aqui](https://hub.docker.com/_/postgres)**.

### Executando :running:
Com o PostgreSQL instalado e configurado, crie um banco de dados com o nome `easyfood`. Para isso, você pode utilizar o **[pgAdmin](https://www.pgadmin.org/)** ou qualquer outra ferramenta de sua preferência. Após criar o banco de dados, execute o script  **[init.sql](deploy/database/init.sql)** disponível na pasta **[./deploy/database](deploy/database)**. Esse script irá criar as tabelas e inserir os dados iniciais.
Certifique-se de colocar a string de conexão correta no arquivo **[appsettings.json](src/Presentation/EF.Api/appsettings.json)**.
Pronto! Agora é só executar a aplicação utilizando a sua IDE de preferência. A documentação estará disponível na URL **[http://localhost:[PORTA]/swagger](http://localhost:5002/swagger) (substitua pela porta em que a aplicação está rodando)**.

# Como utilizar :bulb:

A documentação da API está disponível na URL **[http://localhost:[PORTA]/swagger](http://localhost:8080/swagger) (não esqueça de colocar a porta onde a aplicação está rodando)**. Lá você encontrará todos os endpoints disponíveis, além de exemplos de como utilizar cada um deles.

## Token :key:

Para manter a associação de clientes com um carrinho estamos utilizando um **[Json Web Token (JWT)](https://jwt.io/)**. Para as requisições no contexto de **pedidos e carrinho**, é necessário informar o token no header da requisição. Para isso, basta copiar o token gerado no endpoint `[POST] /api/identidade/acessar` e incluir no header da requisição com a chave `Authorization` a palavra `Bearer` seguida do token gerado.
Incluimos no swagger um botão para facilitar a inclusão do token no header. Basta clicar no botão **Authorize** e colar o token no campo **Value**. Após isso, basta clicar em **Authorize** e o token será incluído automaticamente no header de todas as requisições.

![img_3.png](docs/img/img_3.png)
![img_3.png](docs/img/img_4.png)

O mesmo pode ser feito na requisição de cada endpoint:

![img_5.png](docs/img/img_5.png)

**Prono! Agora você já pode utilizar a API** :smile:
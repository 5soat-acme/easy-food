from diagrams import Diagram
from diagrams.c4 import Person, Container, Database, System, SystemBoundary, Relationship

graph_attr = {
    "splines": "spline",
}

with Diagram("Easy Food - C4", filename="c4_diagram", outformat=["png"], show=False, direction="TB", graph_attr=graph_attr):
    customer = Person(
        name="Cliente do Easy Food", 
        description="Um cliente do Easy Food. Que possua ou não um cadastro."
    )

    with SystemBoundary("Sistema do Easy Food"):
        webapp = Container(
            name="Web Application - Totem",
            description="Fornece todas as funcionalidades do sistema aos clientes por meio de um totem.",
        )

        api = Container(
            name="API",
            technology=".NET 8",
            description="Fornece funcionalidades do negócio de uma lanchonete por meio de uma api JSON/HTTP",
        )

        database = Database(
            name="Database",
            technology="PostgreSQL",
            description="Armazena informações para a operação do sistema da lanchonete, como por exemplo: clientes, produtos, estoque, pedidos...",
        )

    pagamento = System(name="Sistema de Pagamentos", description="Sistema externo para processar o pagamento do pedido.", external=True)

    customer >> Relationship("Visualiza cardápio e efetua pedidos em") >> webapp
    webapp >> Relationship("Faz chamadas de API para [JSON/HTTP]") >> api

    api >> Relationship("Lê e escreve em") >> database
    api >> Relationship("Faz chamadas de API para [JSON/HTTP]") >> pagamento
    api << Relationship("Retorna resultado do pagamento para [Webhook]") << pagamento
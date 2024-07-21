from diagrams import Diagram, Cluster
from diagrams.aws.compute import EC2
from diagrams.aws.integration import SQS, SimpleQueueServiceSqsQueue

with Diagram("Easy Food - Microsserviços", filename="diagrama_microsservicos", outformat=["png"], show=False):
    with Cluster("Componentes"):
        with Cluster("Microsserviços"):
            clients = [EC2("pedido"),
                    EC2("pagamento"),
                    EC2("preparo-entrega")]
            
        sqs = SQS("SQS")

        with Cluster("Queues"):
            queues = [SimpleQueueServiceSqsQueue("PedidoCriado"),
                    SimpleQueueServiceSqsQueue("PagamentoAutorizado"),
                    SimpleQueueServiceSqsQueue("PagamentoRecusado"),
                    SimpleQueueServiceSqsQueue("PedidoRecebido"),
                    SimpleQueueServiceSqsQueue("PreparoPedidoIniciado"),
                    SimpleQueueServiceSqsQueue("PreparoPedidoFinalizado"),
                    SimpleQueueServiceSqsQueue("EntregaPedidoRealizada")]
            
        clients - sqs - queues
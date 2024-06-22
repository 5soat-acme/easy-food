from diagrams import Diagram, Cluster
from diagrams.aws.compute import EC2
from diagrams.aws.integration import SQS, SimpleQueueServiceSqsQueue

with Diagram("Easy Food - Microsserviços", filename="diagrama_microsservicos", outformat=["png"], show=False):
    with Cluster("Fluxo"):
        svc_pedido = EC2("pedido")
        svc_pagamento = EC2("pagamento")
        svc_preparo_entrega = EC2("preparo-entrega")

        sqs_pagamento_autorizado = SimpleQueueServiceSqsQueue("PagamentoAutorizado")
        sqs_pedido_recebido = SimpleQueueServiceSqsQueue("PedidoRecebido")
        sqs_preparo_pedido_iniciado = SimpleQueueServiceSqsQueue("PreparoPedidoIniciado")
        sqs_preparo_pedido_finalizado = SimpleQueueServiceSqsQueue("PreparoPedidoFinalizado")
        sqs_entrega_pedido_realizada = SimpleQueueServiceSqsQueue("EntregaPedidoRealizada")       
        

        svc_pagamento >> sqs_pagamento_autorizado >> svc_pedido >> sqs_pedido_recebido >> svc_preparo_entrega
        svc_preparo_entrega >> sqs_preparo_pedido_iniciado >> svc_pedido
        svc_preparo_entrega >> sqs_preparo_pedido_finalizado >> svc_pedido
        svc_preparo_entrega >> sqs_entrega_pedido_realizada >> svc_pedido

    with Cluster("Componentes"):
        with Cluster("Microsserviços"):
            clients = [EC2("pedido"),
                    EC2("pagamento"),
                    EC2("preparo-entrega")]
            
        sqs = SQS("SQS")

        with Cluster("Queues"):
            queues = [SimpleQueueServiceSqsQueue("PagamentoAutorizado"),
                    SimpleQueueServiceSqsQueue("PedidoRecebido"),
                    SimpleQueueServiceSqsQueue("PreparoPedidoIniciado"),
                    SimpleQueueServiceSqsQueue("PreparoPedidoFinalizado"),
                    SimpleQueueServiceSqsQueue("EntregaPedidoRealizada")]
            
        clients - sqs - queues
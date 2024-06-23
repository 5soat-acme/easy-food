from diagrams import Diagram, Cluster
from diagrams.aws.compute import EC2
from diagrams.onprem.client import Client
from diagrams.aws.network import ElbNetworkLoadBalancer, InternetGateway, NATGateway, APIGateway
from diagrams.aws.compute import EKS
from diagrams.aws.compute import Lambda
from diagrams.aws.security import Cognito
from diagrams.aws.database import Aurora, Dynamodb
from diagrams.aws.integration import SQS

with Diagram("Easy Food - AWS", filename="diagrama_aws", outformat=["png"], show=False):
    with Cluster("Clients"):
        clients = [Client("client_1"),
                    Client("client_2"),
                    Client("client_3")]

    with Cluster("AWS Cloud"):
        api_gateway = APIGateway("api-gateway")
        create_user_lambda = Lambda("create-user-lambda")
        auth_lambda = Lambda("auth-lambda")
        auth_add_claim_lambda = Lambda("auth-add-claim-lambda")        
        cognito = Cognito("cognito")

        with Cluster("VPC"):
            internet_gateway = InternetGateway("internet-gateway")
            load_balancer = ElbNetworkLoadBalancer("load-balancer")
            eks = EKS("eks-cluster")
            with Cluster("Subnet"):
                nat_gateway = NATGateway("nat-gateway")
                with Cluster("Node Group"):
                    nodes = [EC2("EC2_1"),
                                EC2("EC2_2")]
                    
        aurora_postgresql_pedido = Aurora("aurora-postgresql-pedido")
        aurora_postgresql_preparoentrega = Aurora("aurora-postgresql-preparoentrega")
        dynamodb_pagamento = Dynamodb("dynamodb-pagamento")

        sqs = SQS("event-queue")
    
    clients >> load_balancer >> nodes
    internet_gateway - nat_gateway
    eks - nodes
    nodes >> aurora_postgresql_pedido
    nodes >> aurora_postgresql_preparoentrega
    nodes >> dynamodb_pagamento
    nodes - sqs

    clients >> api_gateway
    api_gateway >> create_user_lambda >> cognito
    api_gateway >> auth_lambda >> cognito
    cognito - auth_add_claim_lambda
from diagrams import Diagram, Cluster
from diagrams.aws.compute import EC2
from diagrams.onprem.client import Client
from diagrams.aws.network import ElbNetworkLoadBalancer
from diagrams.aws.network import InternetGateway, NATGateway
from diagrams.aws.compute import EKS

with Diagram("Easy Food - AWS", filename="diagrama_aws", outformat=["png"], show=False):
    with Cluster("Clients"):
        clients = [Client("client_1"),
                    Client("client_2"),
                    Client("client_3")]

    with Cluster("AWS Cloud"):
        with Cluster("VPC"):
            internet_gateway = InternetGateway("internet-gateway")
            load_balancer = ElbNetworkLoadBalancer("load-balancer")
            eks = EKS("eks-cluster")
            with Cluster("Subnet"):
                nat_gateway = NATGateway("nat-gateway")
                with Cluster("Node Group"):
                    nodes = [EC2("EC2_1"),
                                EC2("EC2_2")]

    clients >> load_balancer >> nodes
    internet_gateway - nat_gateway
    eks - nodes
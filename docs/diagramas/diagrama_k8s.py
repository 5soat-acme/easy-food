from diagrams import Diagram, Cluster
from diagrams.k8s.clusterconfig import HPA
from diagrams.k8s.compute import Pod, ReplicaSet, Deployment, StatefulSet
from diagrams.k8s.network import SVC, Ing
from diagrams.onprem.client import Client
from diagrams.k8s.storage import PV, PVC, StorageClass
from diagrams.k8s.podconfig import Secret
from diagrams.onprem.network import Nginx
from diagrams.k8s.ecosystem import Helm

with Diagram("Easy Food - K8S", filename="diagrama_k8s", outformat=["png"], show=False):
    with Cluster("Clients"):
        clients = [Client("client_1"),
                    Client("client_2"),
                    Client("client_3")]

    with Cluster("AWS Cloud"):
        ingress_controller = Nginx("ingress-controller")

        with Cluster("K8S"):
            ## API
            ingress = Ing("easy-food-api-ingress")
            svc_api_pedido = SVC("svc-pedido-clusterip")
            svc_api_pagamento = SVC("svc-pagamento-clusterip")
            svc_api_preparoentrega = SVC("svc-preparoentrega-clusterip")

            with Cluster("Node API Pedido"):
                pods_api_pedido = [Pod("api-pedido-1"),
                        Pod("api-pedido-2")]
                
            with Cluster("Node API Pagamento"):
                pods_api_pagamento = [Pod("api-pagamento-1"),
                        Pod("api-pagamento-2")]
                
            with Cluster("Node API Preparo/Entrega"):
                pods_api_preparoentrega = [Pod("api-preparoentrega-1"),
                        Pod("api-preparoentrega-2")]
                
            replicaSet_pedido = ReplicaSet("rs-pedido")
            replicaSet_pagamento = ReplicaSet("rs-pagamento")
            replicaSet_preparoentrega = ReplicaSet("rs-preparoentrega")
            ##deployment_api = Secret("easy-food-api-secrets") - Deployment("easy-food-api-deploy")
            deployment_pedido = Secret("secrets-pedido") - Deployment("deploy-pedido")
            deployment_pagamento = Secret("secrets-pagamento") - Deployment("deploy-pagamento")
            deployment_preparoentrega = Secret("secrets-preparoentrega") - Deployment("deploy-preparoentrega")
            hpa_pedido = HPA("hpa-pedido")
            hpa_pagamento = HPA("hpa-pagamento")
            hpa_preparoentrega = HPA("hpa-preparoentrega")
            secret_aws = Secret("secrets-aws")


    clients >> ingress_controller >> ingress
    ingress >> svc_api_pedido
    ingress >> svc_api_pagamento
    ingress >> svc_api_preparoentrega

    secret_aws - deployment_pedido
    secret_aws - deployment_pagamento
    secret_aws - deployment_preparoentrega
    
    svc_api_pedido >> pods_api_pedido << replicaSet_pedido << deployment_pedido << hpa_pedido
    svc_api_pagamento >> pods_api_pagamento << replicaSet_pagamento << deployment_pagamento << hpa_pagamento
    svc_api_preparoentrega >> pods_api_preparoentrega << replicaSet_preparoentrega << deployment_preparoentrega << hpa_preparoentrega
from diagrams import Diagram, Cluster
from diagrams.k8s.clusterconfig import HPA
from diagrams.k8s.compute import Pod, ReplicaSet, Deployment, StatefulSet
from diagrams.k8s.network import SVC, Ing
from diagrams.onprem.client import Client
from diagrams.k8s.storage import PV, PVC, StorageClass
from diagrams.k8s.podconfig import Secret

with Diagram("Easy Food - K8S", filename="k8s_diagram", outformat=["png"], show=False):
    with Cluster("Clients"):
        clients = [Client("client_1"),
                    Client("client_2"),
                    Client("client_3")]    

    with Cluster("K8S"):
        ## API
        svc_api = SVC("easy-food-api-loadbalancer")

        with Cluster("Node API"):
            pods_api = [Pod("easy-food-api-1"),
                    Pod("easy-food-api-2"),
                    Pod("easy-food-api-3")]
            
            pod_db = Pod("easy-food-db")
            
        replicaSet_api = ReplicaSet("easy-food-api-rs")
        deployment_api = Secret("easy-food-api-secrets") - Deployment("easy-food-api-deploy")
        hpa_api = HPA("easy-food-api-hpa")

        svc_db =  SVC("easy-food-db-clusterip")
        statefulSet = Secret("easy-food-db-secrets") - StatefulSet("easy-food-db-stateful")
        pvc = PVC("easy-food-db-pvc")
        pv = PV("easy-food-db-pv")
        storageClass = StorageClass("easy-food-db-sc")


    clients >> svc_api >> pods_api << replicaSet_api << deployment_api << hpa_api
    pods_api >> svc_db >> pod_db >> pvc << pv << storageClass
    pod_db - statefulSet - pvc
helm install kafka-release --namespace kafka bitnami/kafka  

helm install kafka-ui kafka-ui/kafka-ui --namespace kafka-ui \
--set envs.config.KAFKA_CLUSTERS_0_NAME=local \
--set envs.config.KAFKA_CLUSTERS_0_BOOTSTRAPSERVERS=kafka-release-0.kafka-release-headless.kafka.svc.cluster.local:9092

helm install redis-release bitnami/redis --namespace redis

helm upgrade --namespace monitoring prom-release \
bitnami/kube-prometheus \
--set prometheus.additionalScrapeConfigs.enabled=true \
--set prometheus.additionalScrapeConfigs.type=external \
--set prometheus.additionalScrapeConfigs.external.name=secret-kubescrape \
--set prometheus.additionalScrapeConfigs.external.key=kubernetes.yaml

helm install grafana-release grafana/grafana --namespace monitoring

helm install prom-pushgateway-release prometheus-community/prometheus-pushgateway --namespace monitorin
# Configuring Kong Admin API
# creating services and routes

# Catalog  API
curl -i -X POST   --url https://40.118.41.46:8444/services/   --data 'name=catalog-api-svc'   --data 'url=http://catalog-api-service/api/products' --insecure
curl -i -X POST   --url https://40.118.41.46:8444/services/catalog-api-svc/routes   --data 'hosts[]=mvc-client-catalog' --insecure
curl -i -X GET --url http://104.45.20.246 --header 'Host: mvc-client-catalog'

# Basket API
curl -i -X POST   --url https://40.118.41.46:8444/services/   --data 'name=basket-api-svc'   --data 'url=http://basket-api-service/api/﻿basketitems' --insecure
curl -i -X POST   --url https://40.118.41.46:8444/services/basket-api-svc/routes   --data 'hosts[]=﻿mvc-client-basket' --insecure
curl -i -X GET --url http://104.45.20.246 --header 'Host: mvc-client-basket'

# catalog Healthz
curl -i -X POST   --url https://40.118.41.46:8444/services/   --data 'name=catalog-api-healthz'   --data 'url=http://catalog-api-service/healthz' --insecure
curl -i -X POST --url https://40.118.41.46:8444/services/catalog-api-healthz/routes --data 'hosts[]=catalog-healthz' --insecure
curl -i -X GET --url http://104.45.20.246 --header 'Host: catalog-healthz'

# Basket Healthz
curl -i -X POST   --url https://40.118.41.46:8444/services/   --data 'name=basket-api-healthz'   --data 'url=http://basket-api-service/healthz' --insecure
curl -i -X POST --url https://40.118.41.46:8444/services/basket-api-healthz/routes --data 'hosts[]=basket-healthz' --insecure
curl -i -X GET --url http://104.45.20.246 --header 'Host:basket-healthz'
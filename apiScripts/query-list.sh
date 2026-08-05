LIST_ID=$1

curl -Ss "http://localhost:5064/${LIST_ID}" | jq -r '.' 

LIST_ID=$1

curl -X POST -sS "http://localhost:5064/${LIST_ID}/add" | jq -r '.' 


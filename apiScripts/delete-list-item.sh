LIST_ID=$1
ITEM_ID=$2

curl -X DEL -sS "http://localhost:5064/${LIST_ID}/${ITEM_ID}"


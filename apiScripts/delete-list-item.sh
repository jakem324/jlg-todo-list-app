LIST_ID=$1
ITEM_ID=$2

curl -X POST -sS "http://localhost:5064/${LIST_ID}/${ITEM_ID}/delete"


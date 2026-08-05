LIST_ID=$1
ITEM_ID=$2
NEW_TITLE=$3
NEW_BODY=$4

curl -X POST -sS "http://localhost:5064/${LIST_ID}/${ITEM_ID}/update" \
  -H "Content-Type: application/json" \
  -d "{\"title\": \"${NEW_TITLE}\", \"body\": \"${NEW_BODY}\"}"


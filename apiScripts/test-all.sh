LIST_ID=$(bash ./initialize-new-list.sh)
ITEM_ID=$(bash ./add-list-item.sh "${LIST_ID}")
echo "${LIST_ID} - ${ITEM_ID}"
bash ./update-list-item.sh "${LIST_ID}" "${ITEM_ID}" "Hello" "Hello world"
bash ./query-list.sh "${LIST_ID}" "${ITEM_ID}"

import { Service, Signal, computed } from '@angular/core';
import { httpResource, HttpErrorResponse } from '@angular/common/http';

interface ListItem {
  itemID: string;
  sequence: number;
  title: string;
  body: string;
}

export interface ListItemsQuery {
  listId: string | undefined;
  page: number | undefined;
}

interface ListItemsQueryResult {
  error: boolean;
  loading: boolean;
  listFound: boolean;
  pageCount: number;
  items: ListItem[];
}

export interface FetchListItemQuery {
  listId: string | undefined;
  itemId: string | undefined;
}

interface FetchListItemQueryResult {
  error: boolean;
  loading: boolean;
  itemFound: boolean;
  item: ListItem | undefined;
}

interface ListItemsQueryApiResponse {
  totalAvailable: number;
  items: ListItem[];
}

const pageSize = 10;

@Service()
export class ListQueryService {
  getItemsList = (querySignal: Signal<ListItemsQuery>): Signal<ListItemsQueryResult> => {
    const parameters = computed(() => {
      const page = querySignal().page ?? 1;
      const skip = page * pageSize - pageSize;
      const take = pageSize;

      return { skip, take };
    });
    const apiResponse = httpResource(() => {
      if (!querySignal().listId || !querySignal().page) return undefined;
      const url = `${querySignal().listId}?skip=${parameters().skip}&take=${parameters().take}`;
      return url;
    });
    const result = computed(() => {
      const error = apiResponse.error() as HttpErrorResponse | undefined;
      const status = error?.status;

      if (status && status === 404)
        return {
          error: false,
          loading: false,
          listFound: false,
          pageCount: 0,
          items: [],
        };

      if (status === null)
        return {
          error: false,
          loading: true,
          listFound: false,
          pageCount: 0,
          items: [],
        };

      if (status && status >= 500)
        return {
          error: true,
          loading: false,
          listFound: false,
          pageCount: 0,
          items: [],
        };

      const data: ListItemsQueryApiResponse | undefined = apiResponse.value() as unknown as
        ListItemsQueryApiResponse | undefined;
      if (data) {
        const pageCount = Math.ceil(data.totalAvailable / pageSize);
        return {
          error: false,
          loading: false,
          listFound: true,
          pageCount,
          items: data.items,
        };
      }

      return {
        error: false,
        loading: true,
        listFound: false,
        pageCount: 0,
        items: [],
      };
    });

    return result;
  };

  getListItem = (querySignal: Signal<FetchListItemQuery>): Signal<FetchListItemQueryResult> => {
    const apiResponse = httpResource(() => {
      const listIdValue = querySignal().listId;
      const itemIdValue = querySignal().itemId;
      if (!listIdValue || !itemIdValue) return undefined;
      const url = `${listIdValue}/${itemIdValue}`;
      return url;
    });

    const result = computed(() => {
      const error = apiResponse.error() as HttpErrorResponse | undefined;
      const status = error?.status;

      if (status && status === 404)
        return {
          error: false,
          loading: false,
          itemFound: false,
        } as FetchListItemQueryResult;

      if (status === null)
        return {
          error: false,
          loading: true,
          itemFound: false,
        } as FetchListItemQueryResult;

      if (status && status >= 500)
        return {
          error: true,
          loading: false,
          itemFound: false,
        } as FetchListItemQueryResult;

      const data: ListItem | undefined = apiResponse.value() as unknown as ListItem | undefined;
      if (data) {
        return {
          error: false,
          loading: false,
          itemFound: true,
          item: data,
        } as FetchListItemQueryResult;
      }

      return {
        error: false,
        loading: true,
        itemFound: false,
      } as FetchListItemQueryResult;
    });

    return result;
  };
}

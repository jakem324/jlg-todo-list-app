import { Service, Signal, computed } from '@angular/core';
import { httpResource, HttpResourceRef, HttpErrorResponse } from '@angular/common/http';

interface FetchResult {
  loading: boolean;
  notFoundError: boolean;
  serverError: boolean;
}

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

type ListItemsQueryResult = FetchResult & {
  pageCount: number;
  items: ListItem[];
};

export interface FetchListItemQuery {
  listId: string | undefined;
  itemId: string | undefined;
}

type FetchListItemQueryResult = FetchResult & {
  item: ListItem | undefined;
};

interface ListItemsQueryApiResponse {
  totalAvailable: number;
  items: ListItem[];
}

const pageSize = 10;

@Service()
export class ListQueryService {
  private getFetchResult = <T>(resource: HttpResourceRef<T>): FetchResult => {
    const error = resource.error() as HttpErrorResponse | undefined;
    const errorStatus = error?.status;
    const base = {
      loading: resource.isLoading(),
    };

    if (error && errorStatus === 404)
      return {
        ...base,
        serverError: false,
        notFoundError: true,
      };

    if (error)
      return {
        ...base,
        serverError: true,
        notFoundError: false,
      };

    return {
      ...base,
      serverError: false,
      notFoundError: false,
    };
  };

  getItemsList = (
    querySignal: Signal<ListItemsQuery>,
  ): [Signal<ListItemsQueryResult>, () => void] => {
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

    const callback = () => apiResponse.reload();
    const result: Signal<ListItemsQueryResult> = computed(() => {
      const baseResult = this.getFetchResult(apiResponse);

      const data: ListItemsQueryApiResponse | undefined = apiResponse.value() as unknown as
        ListItemsQueryApiResponse | undefined;
      if (data) {
        const pageCount = Math.ceil(data.totalAvailable / pageSize);
        return {
          ...baseResult,
          pageCount,
          items: data.items,
        };
      }

      return {
        ...baseResult,
        pageCount: 0,
        items: [],
      } as ListItemsQueryResult;
    });

    return [result, callback];
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
      const baseResult = this.getFetchResult(apiResponse);

      const data: ListItem | undefined = apiResponse.value() as unknown as ListItem | undefined;
      if (data) {
        return {
          ...baseResult,
          item: data,
        } as FetchListItemQueryResult;
      }

      return baseResult as FetchListItemQueryResult;
    });

    return result;
  };
}

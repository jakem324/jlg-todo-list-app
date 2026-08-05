import { Service, Signal, computed } from '@angular/core';
import { httpResource, HttpErrorResponse } from '@angular/common/http';

interface ListItem {
    itemID: string;
    sequence: number;
    title: string;
    body: string;
}

interface ListItemsQuery {
  listId: string;
  page: number;
}

interface ListItemsQueryResult {
  error: boolean;
  loading: boolean;
  listFound: boolean;
  pageCount: number;
  items: ListItem[];
}

interface ListItemsQueryApiResponse {
  totalAvailable: number;
  items: ListItem[];
}

const pageSize = 50;

@Service()
export class ListQueryService {

  getItemsList = (querySignal: Signal<ListItemsQuery>): Signal<ListItemsQueryResult> => {
    const parameters = computed(() => {
      const page = querySignal().page;
      const skip = (page * pageSize) - pageSize;
      const take = pageSize;

      return { skip, take };
    });
    const apiResponse = httpResource(() => `${querySignal().listId}?skip=${parameters().skip}&take=${parameters().take}`);
    const result = computed(() => {
      const data: ListItemsQueryApiResponse | undefined = apiResponse.value() as unknown as ListItemsQueryApiResponse | undefined;
      const error = apiResponse.error() as HttpErrorResponse | undefined;
      const status = error?.status;

      if (status && status === 404)
        return {
          error: false,
          loading: false,
          listFound: false,
          pageCount: 0,
          items: []
        };

      if (status === null)
        return {
          error: false,
          loading: true,
          listFound: false,
          pageCount: 0,
          items: []
        };

      if (status && status >= 500)
        return {
          error: true,
          loading: false,
          listFound: false,
          pageCount: 0,
          items: []
        };

      if (data) {
        const pageCount = Math.ceil(data.totalAvailable / pageSize);
        return {
          error: false,
          loading: false,
          listFound: true,
          pageCount,
          items: data.items
        };
      }

      return {
        error: false,
        loading: true,
        listFound: false,
        pageCount: 0,
        items: []
      };

    });

    return result;
  }

}

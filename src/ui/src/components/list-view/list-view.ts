import { Signal, Component, input, inject, computed } from '@angular/core';
import { ListQueryService, ListItemsQuery } from '@services/list-query-service';

@Component({
  selector: 'app-list-view',
  imports: [],
  templateUrl: './list-view.html',
  styleUrl: './list-view.css',
})
export class ListView {
  private listQueryService = inject(ListQueryService);
  //listId = input.required<string>();
  //page = input.required<number>();
   // 3eb8ec4a-cd90-4923-94c6-8966e06f5e57/1
  listId = '3eb8ec4a-cd90-4923-94c6-8966e06f5e57';
  page = 1;

  private query: Signal<ListItemsQuery> = computed(() => ({
    listId: this.listId, //(),
    page: this.page
  }));

  queryResult = this.listQueryService.getItemsList(this.query);
  preview = computed(() => JSON.stringify(this.queryResult(), null, 2));
}

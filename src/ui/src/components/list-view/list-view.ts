import { Signal, Component, input, inject, computed } from '@angular/core';
import { ListQueryService, ListItemsQuery } from '@services/list-query-service';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-list-view',
  imports: [JsonPipe],
  templateUrl: './list-view.html',
  styleUrl: './list-view.css',
})
export class ListView {
  private listQueryService = inject(ListQueryService);
  listId = input<string>();
  page = input<number>();

  private query: Signal<ListItemsQuery> = computed(() => ({
    listId: this.listId(),
    page: this.page()
  }));

  queryResult = this.listQueryService.getItemsList(this.query);
  preview = computed(() => JSON.stringify(this.queryResult(), null, 2));
}

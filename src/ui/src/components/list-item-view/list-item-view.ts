import { JsonPipe } from '@angular/common';
import { Component, Signal, input, inject, computed } from '@angular/core';
import { Router } from '@angular/router';
import { ListQueryService } from '@services/list-query-service';
import { FetchListItemQuery } from '@services/list-query-service';

@Component({
  selector: 'app-list-item-view',
  imports: [JsonPipe],
  templateUrl: './list-item-view.html',
  styleUrl: './list-item-view.css',
})
export class ListItemView {
  private router = inject(Router);

  private listQueryService = inject(ListQueryService);
  listId = input<string>();
  itemId = input<string>();

  private query: Signal<FetchListItemQuery> = computed(() => ({
    listId: this.listId(),
    itemId: this.itemId(),
  }));

  queryResult = this.listQueryService.getListItem(this.query);
}

import { Component, Signal, input, inject, computed, linkedSignal } from '@angular/core';
import { form, FormField, FormRoot } from '@angular/forms/signals';
import { Router } from '@angular/router';
import { ListCommandService } from '@services/list-command-service';
import { ListQueryService } from '@services/list-query-service';
import { FetchListItemQuery } from '@services/list-query-service';

@Component({
  selector: 'app-list-item-view',
  imports: [FormField, FormRoot],
  templateUrl: './list-item-view.html',
  styleUrl: './list-item-view.css',
})
export class ListItemView {
  private router = inject(Router);

  private listQueryService = inject(ListQueryService);
  private listCommandService = inject(ListCommandService);
  listId = input<string>();
  itemId = input<string>();

  private query: Signal<FetchListItemQuery> = computed(() => ({
    listId: this.listId(),
    itemId: this.itemId(),
  }));

  queryResult = this.listQueryService.getListItem(this.query);

  listItemFormModel = linkedSignal({
    source: () => this.queryResult(),
    computation: (sourceValue) => ({
      title: sourceValue.item?.title ?? '',
      body: sourceValue.item?.body ?? '',
    }),
  });

  listItemForm = form(
    this.listItemFormModel,
    () => {
      /* Intentional empty function (linter) */
    },
    {
      submission: {
        action: async (payload) => {
          const listIdValue = this.listId();
          const itemIdValue = this.itemId();
          const payloadValue = payload().value();
          if (!listIdValue || !itemIdValue || !payloadValue) return;

          this.listCommandService
            .updateListItem(listIdValue, itemIdValue, payloadValue.title, payloadValue.body)
            .subscribe({
              next: () => this.router.navigate([`/${listIdValue}`]),
            });
        },
      },
    },
  );

  cancel() {
    this.router.navigate([`${this.listId()}`]);
  }
}

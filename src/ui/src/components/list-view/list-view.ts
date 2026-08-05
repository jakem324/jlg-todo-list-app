import { Signal, Component, input, inject, computed, signal } from '@angular/core';
import { ListQueryService, ListItemsQuery } from '@services/list-query-service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ListCommandService } from '@services/list-command-service';

@Component({
  selector: 'app-list-view',
  imports: [RouterLink],
  templateUrl: './list-view.html',
  styleUrl: './list-view.css',
})
export class ListView {
  protected route = inject(ActivatedRoute);

  private router = inject(Router);
  private listQueryService = inject(ListQueryService);
  private listCommandService = inject(ListCommandService);
  listId = input<string>();
  page = input<number>();

  private query: Signal<ListItemsQuery> = computed(() => ({
    listId: this.listId(),
    page: this.page(),
  }));

  queryResult = this.listQueryService.getItemsList(this.query);
  preview = computed(() => JSON.stringify(this.queryResult(), null, 2));

  commandError = signal(false);

  initializeItem() {
    const listIdValue = this.listId();
    if (!listIdValue) return;
    this.listCommandService.inititalizeListItem(listIdValue).subscribe({
      next: (itemId) => this.router.navigate([`${listIdValue}/edit/${itemId}`]),
      error: () => this.commandError.set(true),
    });
  }
}

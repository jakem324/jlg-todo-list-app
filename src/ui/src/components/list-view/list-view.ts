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

  commandError = signal(false);

  initializeItem() {
    const listIdValue = this.listId();
    if (!listIdValue) return;
    this.listCommandService.inititalizeListItem(listIdValue).subscribe({
      next: (itemId) => this.router.navigate([`${listIdValue}/edit/${itemId}`]),
      error: () => this.commandError.set(true),
    });
  }

  editItem(itemId: string) {
    const listIdValue = this.listId();
    if (!listIdValue) return;
    this.router.navigate([`${listIdValue}/edit/${itemId}`]);
  }

  deleteItem(itemId: string) {
    const listIdValue = this.listId();
    const pageValue = this.page();
    if (!listIdValue || !pageValue) return;
    this.listCommandService.deleteListItem(listIdValue, itemId).subscribe({
      // TODO: not ideal
      // I would ideally opt to re-trigger the mechanism which fetches the page data. Since the mechanism is a signal, doing so
      // without changing it to an imperative function call + subscription would instead involve having the service return a 
      // "reload" callback which invokes the underlying httpResource's reload() method.
      next: () => window.location.reload(),
    });
  }
}

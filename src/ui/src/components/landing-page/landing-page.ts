import { Component, inject, signal } from '@angular/core';
import { ListCommandService } from '@services/list-command-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-landing-page',
  imports: [],
  templateUrl: './landing-page.html',
})
export class LandingPage {
  private router = inject(Router);
  private listCommandService = inject(ListCommandService);
  hasError = signal(false);

  ngOnInit() {
    this.listCommandService.initializeList().subscribe({
      next: (listId) => this.router.navigate([`/${listId}`]),
      error: () => this.hasError.set(true),
    });
  }
}

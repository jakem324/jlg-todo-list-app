import { Service, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { map } from 'rxjs';

@Service()
export class ListCommandService {
  private http = inject(HttpClient);
  private router = inject(Router);

  inititalizeList() {
    return this.http
      .post('initialize', null, { responseType: 'text' })
      .pipe(map((res) => JSON.parse(res)));
  }

  inititalizeListItem(listId: string) {
    return this.http
      .post(`${listId}/add`, null, { responseType: 'text' })
      .pipe(map((res) => JSON.parse(res)));
  }
  /*
  inititalizeListItem(listId: string) {
    this.http.post(`${listId}/add`, null, { responseType: 'text' }).subscribe({
      next: response => {
        const itemId = JSON.parse(response);
        this.router.navigate([`${listId}/edit/${itemId}`]);
      },
      error: err =>
    })
  }
  */
}

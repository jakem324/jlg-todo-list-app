import { Service, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';

@Service()
export class ListCommandService {
  private http = inject(HttpClient);

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

  deleteListItem(listId: string, itemId: string) {
    return this.http.delete(`${listId}/${itemId}`);
  }

  updateListItem(listId: string, itemId: string, title: string, body: string) {
    return this.http.put(`${listId}/${itemId}`, { title, body });
  }
}

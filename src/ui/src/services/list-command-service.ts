import { Service, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';

@Service()
export class ListCommandService {
  private http = inject(HttpClient);

  inititalizeList(): Observable<string> {
    return this.http
      .post('initialize', null, { responseType: 'text' })
      .pipe(map((res) => JSON.parse(res)));
  }

  inititalizeListItem(listId: string): Observable<string> {
    return this.http
      .post(`${listId}/add`, null, { responseType: 'text' })
      .pipe(map((res) => JSON.parse(res)));
  }

  deleteListItem(listId: string, itemId: string): Observable<void> {
    return this.http.delete<void>(`${listId}/${itemId}`);
  }

  updateListItem(listId: string, itemId: string, title: string, body: string): Observable<void> {
    return this.http.put<void>(`${listId}/${itemId}`, { title, body });
  }
}

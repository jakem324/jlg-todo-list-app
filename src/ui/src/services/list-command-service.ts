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
}

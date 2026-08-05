import { TestBed } from '@angular/core/testing';

import { ListCommandService } from './list-command-service';

describe('ListCommandService', () => {
  let service: ListCommandService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ListCommandService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListItemView } from './list-item-view';

describe('ListItemView', () => {
  let component: ListItemView;
  let fixture: ComponentFixture<ListItemView>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListItemView],
    }).compileComponents();

    fixture = TestBed.createComponent(ListItemView);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

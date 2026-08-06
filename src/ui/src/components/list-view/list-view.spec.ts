import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';

import { ListView } from './list-view';

describe('ListView', () => {
  let component: ListView;
  let fixture: ComponentFixture<ListView>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListView],
      providers: [provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(ListView);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

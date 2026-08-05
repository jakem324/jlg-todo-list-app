import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter, Routes, withComponentInputBinding } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { baseUrlInterceptor } from './base-url-interceptor';
import { ListView } from '@components/list-view/list-view';
import { LandingPage } from '@components/landing-page/landing-page';
import { ListItemView } from '@components/list-item-view/list-item-view';

const routes: Routes = [
  {
    path: '',
    component: LandingPage,
  },
  {
    path: ':listId',
    redirectTo: ':listId/1',
  },
  {
    path: ':listId/:page',
    component: ListView,
  },
  {
    path: ':listId/edit/:itemId',
    component: ListItemView,
  },
];

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(withInterceptors([baseUrlInterceptor])),
  ],
};

import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '@environments/environment';

export const baseUrlInterceptor: HttpInterceptorFn = (req, next) => {
  const modifiedRequest = req.clone({
    url: `${environment.baseUrl}${req.url}`,
  });

  return next(modifiedRequest);
};

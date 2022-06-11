import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, delay } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error) {
          if (error.status === 400) {
            if (error.error.errors) {
              const modalStateErrors=[];
              for(const key in error.error.errors) {
                if(error.error.errors[key]) {
                  modalStateErrors.push(error.error.errors[key]);
                }
              }
              throw modalStateErrors.flat();
            } else {
              this.toastr.error(error.error.message, error.error.statusCode);
            }
          } else if (error.status === 401) {
            if (error.error?.message !==null && error.error?.message !== undefined) {
              this.toastr.error(error.error.message, error.error.statusCode);
            } else {
              //this.toastr.error(error.message, error.error.statusCode);
              this.toastr.error(error.message, error.statusCode);
            }
          } else if (error.status === 404) {
            this.toastr.error(error.error?.message, error.error?.statusCode);
            this.router.navigateByUrl('/notfound');
          } else if (error.status === 500) {
            const navigationExtras: NavigationExtras = {state: {error: error.error}}
            this.router.navigateByUrl('/servererror', navigationExtras);
          }
        }
        return throwError(error);
      })
    );
  }
}

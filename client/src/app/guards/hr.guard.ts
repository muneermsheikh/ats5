import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../account/account.service';

@Injectable({
  providedIn: 'root'
})
export class HrGuard implements CanActivate {
  constructor(private accountService: AccountService, private toastr: ToastrService) { }
  
  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(user => {
        if (
          user?.roles?.includes('Admin') || user?.roles?.includes('HRManager') || user?.roles?.includes('HRSupervisor') 
          || user?.roles?.includes('HRExecutive') || user?.roles?.includes('HRTrainee')
        ) {
          return true;
        }
        this.toastr.error('Unauthorized - This role requires HR privileges');
      })
    )
  }
  
}

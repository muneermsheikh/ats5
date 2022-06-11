import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../account/account.service';

@Injectable({
  providedIn: 'root'
})
export class ProcessGuard implements CanActivate {
  constructor(private accountService: AccountService, private toastr: ToastrService) { }
  
  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(user => {
        if (
          user?.roles?.includes('ProcessManager') || user?.roles?.includes('ProcessExecutive') 
          || user?.roles?.includes('MedicalExecutive') || user?.roles?.includes('MedicalExecutiveGAMMCA')
          || user?.roles?.includes('VisaExecutiveDubai') || user?.roles?.includes('VisaExecutiveKSA') 
          || user?.roles?.includes('VisaExecutiveSharjah') || user?.roles?.includes('VisaExecutiveOman') 
          || user?.roles?.includes('VisaExecutiveQatar') || user?.roles?.includes('DocumentControllerProcess')
        ) {
          return true;
        }
        this.toastr.error('Unauthorized - This role requires Document Processing privileges');
      })
    )
  }
  
}

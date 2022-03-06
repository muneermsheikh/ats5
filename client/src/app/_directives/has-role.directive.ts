import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { take } from 'rxjs/operators';
import { AccountService } from '../account/account.service';
import { IUser } from '../shared/models/user';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[];
  user: IUser;

  constructor(private viewContainerRef: ViewContainerRef, 
    private templateRef: TemplateRef<any>,
    private accountService: AccountService) { 
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
        this.user = user;
      })
    }

  ngOnInit(): void {
    //clear view if no roles
    if (!this.user?.roles || this.user == null) {
      this.viewContainerRef.clear();
      return;
    }
    if (this.user?.roles.some(r => this.appHasRole.includes(r))) {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    }
  }

}

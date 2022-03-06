import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { IUser } from 'src/app/shared/models/user';
import { HrService } from '../hr.service';

@Component({
  selector: 'app-hrforward',
  templateUrl: './hrforward.component.html',
  styleUrls: ['./hrforward.component.css']
})
export class HrforwardComponent implements OnInit {
  routeId: string;
  user: IUser;

  constructor(private activatedRoute: ActivatedRoute, 
      private router: Router,
      private accountService:AccountService,
      private hrService: HrService) {
        this.routeId = this.activatedRoute.snapshot.params['id'];
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    
       }

  ngOnInit(): void {
  }

}

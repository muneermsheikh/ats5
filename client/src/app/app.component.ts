import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
import { IUser } from './shared/models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    this.loadCurrentUser();
    //this.setCurrentUser();
  }
  
  setCurrentUser() {
    const user: IUser = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);
  }

  loadCurrentUser() {
    const token = localStorage.getItem('token');
    console.log('token from local storage', token);
    this.accountService.loadCurrentUser(token).subscribe(() => {
      console.log('loaded user');
    }, error => {
      console.log(error);
    })
  }
}

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  
  constructor(private http: HttpClient, private router: Router, private toastr: ToastrService
    //, private presence: PresenceService
    ) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: IUser) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
          //this.presence.createHubConnection(user);
        }
      })
    )
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: IUser) => {
        if (user) {
         this.setCurrentUser(user);
        }
      })
    )
  }

  setCurrentUser(user: IUser) {
    //console.log('in setcurrentuser, user is: ', user);
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    //console.log('user roles in setCurrentUser : ',user.roles);
    localStorage.setItem('user', JSON.stringify(user));
    localStorage.setItem('token', user.token);
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
    //this.presence.stopHubConnection();
  }

  getDecodedToken(token) {
    return JSON.parse(atob(token.split('.')[1]));
  }

  checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + 'account/emailexists?email=' + email);
  }

  checkPPExists(ppnumber: string) {
    return this.http.get(this.baseUrl + 'account/ppexists?ppnumber=' + ppnumber);
  }

  checkAadharExists(aadharno: string) {
    return this.http.get(this.baseUrl + 'account/aadharnoexsts?aadahrno=' + aadharno);
  }

  getCandidate(id: number) {
    return this.http.get(this.baseUrl + 'candidate/byid/' + id);
  }

  loadCurrentUser(token: string) {
    if (token === null || token === '') {
      this.toastr.info('no user loaded');
      this.currentUserSource.next(null);
      return of(null);
    }

    return this.http.get(this.baseUrl + 'account').pipe(
      map((user: IUser) => {
        console.log('returned from api');
        if (user) {
          this.toastr.info('current user retrieved');
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      }, error => {
        this.toastr.warning('no current user');
      })
    )
  }

  
}

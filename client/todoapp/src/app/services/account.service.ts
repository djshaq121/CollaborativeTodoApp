import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../model/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseurl = 'https://localhost:5001/account/';

  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  private logOutSource = new Subject();
  logOutUser$ = this.logOutSource.asObservable();
  
  constructor(private http: HttpClient) { }

  login(userCreditials: any) {
    return this.http.post<User>(this.baseurl + 'login', userCreditials).pipe(
      map((user: User) => {
        if(user.token) {
          localStorage.setItem('user', JSON.stringify(user));
          this.setCurrentUser(user);
        }
      })
    )
  }

  logout() {
    this.setCurrentUser(null);
    localStorage.removeItem('user');

    this.logOutSource.next();
  }

  register(userCreditials: any) {
    return this.http.post<User>(this.baseurl + 'register', userCreditials).pipe(
      map((user: User) => {
        if(user.token) {
          localStorage.setItem('user', JSON.stringify(user));
          this.setCurrentUser(user);
        }
      })
    )
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }
}

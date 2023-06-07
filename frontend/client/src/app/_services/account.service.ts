import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs'
import { User } from '../_models/user';


@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl: string = 'https://localhost:5001/api/';
  private currentUserSource : BehaviorSubject<User | null> = new BehaviorSubject<User | null>(null);
  currentUser$  = this.currentUserSource.asObservable();

  constructor(private httpClient : HttpClient) { 

  }
  login(model: any) {
    return this.httpClient.post<User>(this.baseUrl + 'accounts/login',model).pipe(
      map((res: User) => {
        const user = res;
        if (user){
          localStorage.setItem('user' , JSON.stringify(user));
          this.currentUserSource.next(user);
        }
    })
    )
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}

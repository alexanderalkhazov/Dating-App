import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs'
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl: string = environment.apiUrl;
  private currentUserSource : BehaviorSubject<User | null> = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();
 
  constructor(private httpClient : HttpClient) {}

  
  login(model: any) {
    return this.httpClient.post<User>(this.baseUrl + 'accounts/login',model).pipe(
      map((res: User) => {
        const user = res;
        if (user){
          this.setCurrentUser(user);
        }
    })
    )
  }

  register(model: any) {
    return this.httpClient.post<User>(this.baseUrl + 'accounts/register' , model).pipe(
      map(user => {
        if (user){
          this.setCurrentUser(user);
        }
        return user;
      })
    );
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user' , JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}

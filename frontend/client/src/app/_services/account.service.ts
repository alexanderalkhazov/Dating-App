import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs'
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';
import { PresenceService } from './presence.service';


@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl: string = environment.apiUrl;
  private currentUserSource : BehaviorSubject<User | null> = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();
 
  constructor(private httpClient : HttpClient , private presenceService: PresenceService) {}

  
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
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user' , JSON.stringify(user));
    this.currentUserSource.next(user);
    this.presenceService.createHubConnection(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presenceService.stopHubConnection();
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}

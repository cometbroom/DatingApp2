import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import {JwtHelperService} from '@auth0/angular-jwt';

// Services aren't injectable so you must add the injectable keyword inject things in service
@Injectable({
  providedIn: 'root' // Tells the compiler where is the service providedin (app.module)
})
export class AuthService {
  baseUrl = 'http://localhost:5000/api/auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model)
      .pipe(          // Map the response to http request pipeline to do things after it
        map((response: any) => {
          const user = response;
          if (user) {
            localStorage.setItem('token', user.token);  // Add the token to client side local storage
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
            console.log(this.decodedToken);
          }
        })
      );
    }

  register(model: any) {
  return this.http.post(this.baseUrl + 'register', model);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

}

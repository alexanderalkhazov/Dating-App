import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit {

  baseUrl: string = 'https://localhost:5001/api/'; 
  validationErrors: string[] = [];

  constructor(private httpClient : HttpClient) { }

  ngOnInit(): void {
  }


  get404Error() {
    return this.httpClient.get(this.baseUrl + 'buggy/not-found').subscribe({
      next: res => console.log(res),
      error: err => console.log(err)
    });
  }

  get400Error() {
    return this.httpClient.get(this.baseUrl + 'buggy/bad-request').subscribe({
      next: res => console.log(res),
      error: err => console.log(err)
    });
  }

  get500Error() {
    return this.httpClient.get(this.baseUrl + 'buggy/server-error').subscribe({
      next: res => console.log(res),
      error: err => console.log(err)
    });
  }

  get401Error() {
    return this.httpClient.get(this.baseUrl + 'buggy/auth').subscribe({
      next: res => console.log(res),
      error: err => console.log(err)
    });
  }

  get400ValidationError() {
    return this.httpClient.post(this.baseUrl + 'accounts/register' , {}).subscribe({
      next: res => console.log(res),
      error: err => {
        console.log(err);
        this.validationErrors = err;
      }
    });
  }

}

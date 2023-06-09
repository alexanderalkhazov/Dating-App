import { Component, OnInit } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';


@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent implements OnInit {

  errorMsg: any;
  
  constructor(private router: Router) {
    const navgiation = this.router.getCurrentNavigation();
    this.errorMsg = navgiation?.extras?.state?.['error'];
    console.log(this.errorMsg);
  }


  ngOnInit(): void { }

}

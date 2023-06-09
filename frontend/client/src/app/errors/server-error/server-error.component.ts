import { Component, OnInit } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';


@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent implements OnInit {

  error: any;
  
  constructor(private router: Router) {
    const navgiation = this.router.getCurrentNavigation();
    this.error = navgiation?.extras?.state?.['error'];
  }


  ngOnInit(): void { }

}

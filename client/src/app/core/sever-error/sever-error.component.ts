import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sever-error',
  templateUrl: './sever-error.component.html',
  styleUrls: ['./sever-error.component.css']
})
export class SeverErrorComponent implements OnInit {

  error: any;

  constructor(private router: Router) { 
    const navigation = this.router.getCurrentNavigation();
    this.error = navigation?.extras?.state?.error;
  }

  ngOnInit(): void {
  }
  
}

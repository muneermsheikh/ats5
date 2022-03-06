import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-masters',
  templateUrl: './masters.component.html',
  styleUrls: ['./masters.component.css']
})
export class MastersComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  openCategoryListing() {
    this.router.navigateByUrl("");
  }

  openQualificationListing() {
    this.router.navigateByUrl("/qualifications");
  }

}

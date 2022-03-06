import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-right-click-menu',
  templateUrl: './right-click-menu.component.html',
  styleUrls: ['./right-click-menu.component.css']
})
export class RightClickMenuComponent implements OnInit {

  @Input() x = 0;
  @Input() y = 0;

  constructor() { }

  ngOnInit(): void {
  }

}

import { Component, Input, OnInit } from '@angular/core';
import { ICustomer } from 'src/app/shared/models/customer';

@Component({
  selector: 'app-client-item',
  templateUrl: './client-item.component.html',
  styleUrls: ['./client-item.component.css']
})

export class ClientItemComponent implements OnInit {
  @Input() cust: ICustomer;
  
  constructor() { }

  ngOnInit(): void {
  }

}

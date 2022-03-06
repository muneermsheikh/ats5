import { Component, Input, OnInit } from '@angular/core';
import { IOrderBrief } from 'src/app/shared/models/orderBrief';

@Component({
  selector: 'app-order-line',
  templateUrl: './order-line.component.html',
  styleUrls: ['./order-line.component.css']
})
export class OrderLineComponent implements OnInit {
  @Input() order: IOrderBrief;

  constructor() { }

  ngOnInit(): void {
  }

}

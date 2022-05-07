import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { IOrderItemBriefDto } from 'src/app/shared/models/orderItemBriefDto';
import { IUser } from 'src/app/shared/models/user';

@Component({
  selector: 'app-order-item-modal',
  templateUrl: './order-item-modal.component.html',
  styleUrls: ['./order-item-modal.component.css']
})
export class OrderItemModalComponent implements OnInit {

  @Input() emitterObj = new EventEmitter();
  user: IUser;
  title: string;
  orderItems: IOrderItemBriefDto[];
  orderItemSelected: IOrderItemBriefDto;

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }

  emitSelectedItem() {
    this.emitterObj.emit(this.orderItemSelected);
    this.bsModalRef.hide();
  }

}

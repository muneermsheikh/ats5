import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { IMessagesDto } from 'src/app/shared/dtos/messagesDto';
import { IMessage } from 'src/app/shared/models/message';

@Component({
  selector: 'app-msg-modal',
  templateUrl: './msg-modal.component.html',
  styleUrls: ['./msg-modal.component.css']
})
export class MsgModalComponent implements OnInit {
  @Input() updateChecklist = new EventEmitter();
  
  msg: IMessage;
  
  constructor(public bsModalRef: BsModalRef, private toastr:ToastrService) { }

  ngOnInit(): void {
  }

  sendMessage() {

  }

  checkednoerror() {

  }

}

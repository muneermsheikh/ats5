import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { IChooseAgentDto } from 'src/app/shared/models/chooseAgentDto';
import { IOrderBriefDto } from 'src/app/shared/models/orderBriefDto';
import { IUser } from 'src/app/shared/models/user';

@Component({
  selector: 'app-choose-agents-modal',
  templateUrl: './choose-agents-modal.component.html',
  styleUrls: ['./choose-agents-modal.component.css']
})

//called by order-edit.component.ts
export class ChooseAgentsModalComponent implements OnInit {

  @Input() updateSelectedOfficialIds = new EventEmitter();
  order: IOrderBriefDto;
  agents: IChooseAgentDto[]; 

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }

  updateAgentsSelected() {
    this.updateSelectedOfficialIds.emit(this.agents);
    this.bsModalRef.hide();
  }

}

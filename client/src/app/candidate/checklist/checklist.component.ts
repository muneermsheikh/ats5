import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ChecklistHRDto, IChecklistHRDto } from 'src/app/shared/models/checklistHRDto';
import { ChecklistHRItem, IChecklistHRItem } from 'src/app/shared/models/checklistHRItem';
import { ChecklistService } from '../checklist.service';

@Component({
  selector: 'app-checklist',
  templateUrl: './checklist.component.html',
  styleUrls: ['./checklist.component.css']
})
export class ChecklistComponent implements OnInit {
  @Input() updateChecklist = new EventEmitter();
  chklst: IChecklistHRDto;
  items: IChecklistHRItem[];
  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
    //this.activatedRoute.data.subscribe(data => { this.chklst = data.checklistdto;})
    this.items = this.chklst.checklistHRItems;
    console.log('in checklist.componentts ngOnInit, items is:', this.items);
    for(var i= 0 ; i <= this.items.length; i++ ){
      let item=new ChecklistHRItem();
      //this.trxNumberList.push({count:i++});  // push new object every time!
      item = this.chklst.checklistHRItems[i];
      this.items.push(item);
    }
  }

  updatechecklist() {

    this.updateChecklist.emit(this.chklst);
    this.bsModalRef.hide();
  }

}

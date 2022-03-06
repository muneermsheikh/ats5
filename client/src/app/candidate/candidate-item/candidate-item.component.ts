import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ICandidateBriefDto } from 'src/app/shared/models/candidateBriefDto';

@Component({
  selector: 'app-candidate-item',
  templateUrl: './candidate-item.component.html',
  styleUrls: ['./candidate-item.component.css']
})
export class CandidateItemComponent implements OnInit {
  @Input() cv: ICandidateBriefDto;
  @Output() msgEvent = new EventEmitter<number>();
  
  cvidForDocumentView: number;

  public isHidden: boolean = true;
  xPosTabMenu: number;
  yPosTabMenu: number;

  constructor() { }

  ngOnInit(): void {
  }

   //right click
   rightClick(event) {
    event.stopPropagation();
    this.xPosTabMenu = event.clientX;
    this.yPosTabMenu = event.clientY;
    this.isHidden = false;
    return false;
  }

  closeRightClickMenu() {
    this.isHidden = true;
  }

  async onClickLoadDocument(cvid: number) {
    // get a document from the Web API endpoint 'LoadDocument'
    this.msgEvent.emit(cvid);
  }
}

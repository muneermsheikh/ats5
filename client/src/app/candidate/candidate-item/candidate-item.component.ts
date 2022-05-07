import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ICandidateAssessment } from 'src/app/shared/models/candidateAssessment';
import { ICandidateBriefDto } from 'src/app/shared/models/candidateBriefDto';
import { ICvAssessmentHeader } from 'src/app/shared/models/cvAssessmentHeader';

@Component({
  selector: 'app-candidate-item',
  templateUrl: './candidate-item.component.html',
  styleUrls: ['./candidate-item.component.css']
})
export class CandidateItemComponent implements OnInit {
  @Input() cv: ICandidateBriefDto;
  @Output() msgEvent = new EventEmitter<number>();
  currentId=0;
  header: ICvAssessmentHeader;
  assessment: ICandidateAssessment;
  
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

  setCurrentId(id: number) {
    this.currentId = id;
  }

  //
  

}

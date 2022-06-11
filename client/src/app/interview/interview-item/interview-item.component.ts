import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IInterviewBrief } from 'src/app/shared/models/hr/interviewBrief';
import { IInterviewItemCandidate } from 'src/app/shared/models/hr/interviewItemCandidate';
import { IInterviewItemDto } from 'src/app/shared/models/hr/interviewItemDto';
import { InterviewService } from '../interview.service';

@Component({
  selector: 'app-interview-item',
  templateUrl: './interview-item.component.html',
  styleUrls: ['./interview-item.component.css']
})
export class InterviewItemComponent implements OnInit {

  @Input() interview: IInterviewBrief;
  @Output() editEvent = new EventEmitter<number>();
  @Output() deleteEvent = new EventEmitter<number>();
  
  catsandcands: IInterviewItemDto[];
  currentId=0;

  constructor(private service: InterviewService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {

  }

  editInterview(id: number) {
    this.editEvent.emit(id);
  }

  deleteInterview(id: number) {
    this.deleteEvent.emit(id);
  }

  displayCategories(id: number) {
    this.service.getInterviewItemCatAndCandidates(id).subscribe(response => {
      this.catsandcands = response;
    })
  }
}

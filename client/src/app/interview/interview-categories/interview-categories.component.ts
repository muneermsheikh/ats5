import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IInterviewItem } from 'src/app/shared/models/hr/interviewItem';
import { IInterviewItemDto } from 'src/app/shared/models/hr/interviewItemDto';


@Component({
  selector: 'app-interview-categories',
  templateUrl: './interview-categories.component.html',
  styleUrls: ['./interview-categories.component.css']
})
export class InterviewCategoriesComponent implements OnInit {

    @Input() cats: IInterviewItemDto[];
    @Output() candidatesEvent = new EventEmitter<IInterviewItemDto[]>();

    attendanceStatuses=["attended", "selected", "rejected"];

  constructor() { }

  ngOnInit(): void {
  }

  EmitData() {
    this.candidatesEvent.emit(this.cats);
  }


}

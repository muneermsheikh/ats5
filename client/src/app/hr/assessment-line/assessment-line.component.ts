import { Component, Input, OnInit } from '@angular/core';
import { IAssessmentQBank } from 'src/app/shared/models/assessmentQBank';

@Component({
  selector: 'app-assessment-line',
  templateUrl: './assessment-line.component.html',
  styleUrls: ['./assessment-line.component.css']
})
export class AssessmentLineComponent implements OnInit {
  @Input() q: IAssessmentQBank;

  
  constructor() { }

  ngOnInit(): void {
  }

}

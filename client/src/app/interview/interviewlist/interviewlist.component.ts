import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IInterviewBrief } from 'src/app/shared/models/hr/interviewBrief';

@Component({
  selector: 'app-interviewlist',
  templateUrl: './interviewlist.component.html',
  styleUrls: ['./interviewlist.component.css']
})
export class InterviewlistComponent implements OnInit {

  interviews: IInterviewBrief[];

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => { 
      this.interviews = data.interviews;
      console.log('entered interviewlist.ts')
    })

  }

}

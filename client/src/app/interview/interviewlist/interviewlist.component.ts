import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IInterviewBrief } from 'src/app/shared/models/hr/interviewBrief';
import { interviewParams } from 'src/app/shared/params/interviewParams';
import { InterviewService } from '../interview.service';

@Component({
  selector: 'app-interviewlist',
  templateUrl: './interviewlist.component.html',
  styleUrls: ['./interviewlist.component.css']
})
export class InterviewlistComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  iParams = new interviewParams();

  totalCount: number;

  interviews: IInterviewBrief[];

  sortOptions = [
    {name:'By Order No', value:'orderno'},
    {name:'By Order No Desc', value:'ordernodesc'},
    {name:'By Order Date', value:'orderdate'},
    {name:'By Order Date Desc', value:'orderdatedesc'},
    {name:'By Category Name', value:'category'},
    {name:'By Category Name Desc', value:'categorydesc'},
    {name:'By Status', value:'status'},
    {name:'By Venue', value:'venue'}
  ]


  constructor(private activatedRoute: ActivatedRoute, private service: InterviewService) { }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => { 
      this.interviews = data.interviews;
      this.totalCount = data.count;
      console.log('entered interviewlist.ts')
    })
  }

  getInterviews() {
    this.service.getInterviews(true).subscribe(response => {
      this.interviews=response.data;
      this.totalCount=response.count;
    })
  }

  onSearch() {
    const params = this.service.getParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.service.setParams(params);
    this.getInterviews();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.iParams = new interviewParams();
    this.service.setParams(this.iParams);
    this.getInterviews();
  }

  onSortSelected(sort: string) {
    this.iParams.sort = sort;
    this.getInterviews();
  }


}

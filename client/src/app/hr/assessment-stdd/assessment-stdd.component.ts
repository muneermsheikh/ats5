import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IAssessmentStandardQ } from 'src/app/shared/models/assessmentStandardQ';
import { HrService } from '../hr.service';

@Component({
  selector: 'app-assessment-stdd',
  templateUrl: './assessment-stdd.component.html',
  styleUrls: ['./assessment-stdd.component.css']
})
export class AssessmentStddComponent implements OnInit {

  stddqs: IAssessmentStandardQ[];

  constructor(private activatedRoute: ActivatedRoute, 
      private service: HrService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => { 
      this.stddqs = data.stddqs;
    })
  }

  deletestddq(id: number) {
    this.service.deletestddq(id).subscribe(response => {
      this.toastr.success("successfully deleted the standard question");
    }, error => {
      this.toastr.error(error);
    })
  }
}

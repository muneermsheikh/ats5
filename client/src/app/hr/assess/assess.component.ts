import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IAssessment } from 'src/app/shared/models/assessment';
import { IOrder } from 'src/app/shared/models/order';
import { IOrderItem } from 'src/app/shared/models/orderItem';

@Component({
  selector: 'app-assess',
  templateUrl: './assess.component.html',
  styleUrls: ['./assess.component.css']
})
export class AssessComponent implements OnInit {

  order: IOrder;
  qs: IAssessment[];
  form: FormGroup;

  constructor(private activatedRouter: ActivatedRoute, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.activatedRouter.data.subscribe(data => { 
      this.order = data.order;
      console.log('assess.component.ts, order', this.order);
    })
  }

  displayQ(id: number) {

  }

  updateQs(assessments: IAssessment[]) {
    this.toastr.info('received from child');
    console.log(assessments);
  }

}

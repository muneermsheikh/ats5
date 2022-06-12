import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { IInterview } from 'src/app/shared/models/hr/interview';
import { IUser } from 'src/app/shared/models/user';
import { ConfirmService } from 'src/app/shared/services/confirm.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { InterviewService } from '../interview.service';
import { take } from 'rxjs/operators';
import { IInterviewItem } from 'src/app/shared/models/hr/interviewItem';
import { IEmployeeIdAndKnownAs } from 'src/app/shared/models/employeeIdAndKnownAs';

@Component({
  selector: 'app-interview-add',
  templateUrl: './interview-add.component.html',
  styleUrls: ['./interview-add.component.css']
})
export class InterviewAddComponent implements OnInit {

  interview: IInterview;
  employees: IEmployeeIdAndKnownAs[];
  //routeId: string;

  errors: string[];
  
  isAddMode: boolean;
  bsValue = new Date();
  bsRangeValue: Date[];
  maxDate = new Date();
  minDate = new Date();

  user: IUser;
  
  form: FormGroup;


  constructor(private service: InterviewService, private bcService: BreadcrumbService,
      private activatedRoute: ActivatedRoute, 
      private router: Router, 
      private accountService: AccountService, 
      private confirmService: ConfirmService,
      private toastr: ToastrService, 
      private fb: FormBuilder) 
      {
        this.bcService.set('@orderDetail',' ');
        //this.routeId = this.activatedRoute.snapshot.params['id'];
        this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
        //this.maxDate.setFullYear(this.maxDate.getFullYear() - 1);  //10 years later
        //this.minDate.setFullYear(this.minDate.getFullYear() + 20);
        //this.bsRangeValue = [this.bsValue, this.maxDate];
        this.router.routeReuseStrategy.shouldReuseRoute = () => false;
      }

  ngOnInit(): void {
    this.createForm();
    this.activatedRoute.data.subscribe(data => {
      this.interview=data.interview;
      this.employees = data.employees;
      this.isAddMode = this.interview !== null && this.interview.id ===0;
       this.interview===null;
      
        this.populateForm(this.interview);
      })
    }
  

  createForm() {
    this.form = this.fb.group({
      id: [null], orderId:0,  orderNo: 0, orderDate: '',
      customerId: 0, customerName: '', interviewMode: 0, 
      interviewerName: '', interviewVenue: '',
      interviewDateFrom: '', interviewDateUpto: '',
      interviewLeaderId: 0, customerRepresentative: '', 
      interviewStatus: '', concludingRemarks: '',
      interviewItems: this.fb.array([])
    });

    //if (!this.isAddMode) this.loadMember();
  }

  populateForm(intervw: IInterview) {
    this.form.patchValue( {
      id: intervw.id, orderNo: intervw.orderNo, orderId: intervw.orderId, 
      orderDate: intervw.orderDate, customerId: intervw.customerId,
      customerName: intervw.customerName, interviewerName: intervw.interviewerName, 
      interviewVenue: intervw.interviewVenue, 
      interviewDateFrom: intervw.interviewDateFrom, interviewDateUpto: intervw.interviewDateUpto,
      interviewLeaderId: intervw.interviewLeaderId, customerRepresentative: intervw.customerRepresentative,
      interviewStatus: intervw.interviewStatus, concludingRemarks: intervw.concludingRemarks
    });

    if (intervw.interviewItems !== null) this.form.setControl('interviewItems', this.setExistingItems(intervw.interviewItems));
  }

  
  setExistingItems(items: IInterviewItem[]): FormArray {
      const formArray = new FormArray([]);
      items.forEach(ph => {
        formArray.push(this.fb.group({
          id: ph.id, interviewId: ph.interviewId, 
          orderItemId: ph.orderItemId,
          categoryId: ph.categoryId, 
          interviewDateFrom: ph.interviewDateFrom, 
          interviewDateUpto: ph.interviewDateUpto,
          interviewMode: ph.interviewMode, 
          interviewerName: ph.interviewerName,
          interviewStatus: ph.interviewStatus,
          concludingRemarks: ph.concludingRemarks
        }))
      });
      return formArray;
  }

  
    get interviewItems() : FormArray {
      return this.form.get("interviewItems") as FormArray
    }

    
}

import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { ICustomerReview } from 'src/app/shared/models/customerReview';
import { ICustomerReviewData } from 'src/app/shared/models/customerReviewData';
import { ICustomerReviewItem } from 'src/app/shared/models/customerReviewItem';
import { IUser } from 'src/app/shared/models/user';
import { ClientReviewService } from '../client-review.service';

@Component({
  selector: 'app-client-review',
  templateUrl: './client-review.component.html',
  styleUrls: ['./client-review.component.css']
})
export class ClientReviewComponent implements OnInit {

  routeId: string;
  user: IUser;
  customerReview: ICustomerReview;
  customerReviewStatusData: ICustomerReviewData[];
  form: FormGroup;
  loading: boolean=false;
  currentStatus: ['Active', 'Blacklisted'];
  hasUserManagerRole: boolean;

  constructor(private activatedRouter: ActivatedRoute, private accountService: AccountService, private router: Router,
      private service: ClientReviewService, private fb: FormBuilder, private toastr: ToastrService) {
    this.routeId = this.activatedRouter.snapshot.params['id'];
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    this.hasUserManagerRole = this.user.roles.some(r => r.toLowerCase() === 'admin')
   }

  ngOnInit(): void {
    this.customerReview = this.activatedRouter.snapshot.data['customerReview'];
    this.customerReviewStatusData = this.activatedRouter.snapshot.data['reviewStatusData'];
    
    this.createForm();
    
  }
  createForm() {
    this.form = this.fb.group({
      id: [null],
      customerId: 0,
      customerName: '',
      currentStatus: 0,
      remarks: '',
      customerReviewItems: this.fb.array([])
    } 
    );

    this.patchForm(this.customerReview);
  }


  patchForm(rvw: ICustomerReview) {
    this.form.patchValue( {
        id: rvw.id, 
        customerId: rvw.customerId, 
        currentStatus: rvw.currentStatus, 
        customerName: rvw.customerName,
        remarks: rvw.remarks
    });

    if (rvw.customerReviewItems != null) this.form.setControl('customerReviewItems', this.setExistingItems(rvw.customerReviewItems));
  }

  setExistingItems(items: ICustomerReviewItem[]): FormArray {
    const formArray = new FormArray([]);
    items.forEach(ph => {
      formArray.push(this.fb.group({
        id: ph.id,
        customerReviewId: ph.customerReviewId,
        reviewTransactionDate: ph.reviewTransactionDate,
        userId: ph.userId,
        customerReviewDataId: ph.customerReviewDataId,
        remarks: ph.remarks,
        approvedBySup: ph.approvedBySup,
        approvedById: ph.approvedById
      }))
    });
    return formArray;
}

  get customerReviewItems() : FormArray {
    return this.form.get("customerReviewItems") as FormArray
  }
  
  newCustomerReviewItem(): FormGroup {
    return this.fb.group({
      id: new FormControl({value: 0, disabled: true}),
      customerReviewId: new FormControl({value: 0, disabled: true}),
      reviewTransactionDate: '',
      userId: 0,
      customerReviewDataId: 0,
      remarks: '',
      approvedBySup:new FormControl({value: false, disabled: true}),
      approvedById: new FormControl({value: 0, disabled: true})
    })
  }

  addCustomerReviewItem() {
    this.customerReviewItems.push(this.newCustomerReviewItem());
  }

  removeCustomerReviewItem(i:number) {
    this.customerReviewItems.removeAt(i);
    this.customerReviewItems.markAsDirty();
    this.customerReviewItems.markAsTouched();
  }
  
  onSubmit() {
    this.service.updateCustomerReview(this.form.value).subscribe(response => {
      this.form.markAsPristine();
      this.toastr.success('customer review updated');
      //this.router.navigateByUrl('/customerreview');

    }, error => {
      console.log(error);
    })
  }
}

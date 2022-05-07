import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { IContractReview } from 'src/app/shared/models/contractReview';
import { IUser } from 'src/app/shared/models/user';
import { SharedService } from 'src/app/shared/services/shared.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ReviewService } from '../review.service';

@Component({
  selector: 'app-review',
  templateUrl: './review.component.html',
  styleUrls: ['./review.component.css']
})
export class ReviewComponent {
  
  data: IContractReview[];
  user: IUser;
  
  form: FormGroup;
  selectedCategoryIds: number[];
  
  events: Event[] = [];

  loading = false;
  submitted = false;

  errors: string[]=[];

  constructor(private service: ReviewService, private bcService: BreadcrumbService, 
      private activatedRoute: ActivatedRoute, private router: Router, private sharedService: SharedService,
      private accountService: AccountService, private fb: FormBuilder) {
    this.bcService.set('@contractReview',' ');
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    this.activatedRoute.data.subscribe(response => {
      this.data = response.review
    })
      this.form = this.fb.group({
        reviews: this.fb.array([]),
      });
      this.patchForm();
   }


   patchForm() {
     /* var tempdata = { 
       reviews: [ 
         { id: 1, orderId: 3, orderNo: 1000, OrderDate: '2020-10-10T10:00:00', customerId: 5, reviewedBy: 0, reviewedOn: '2022-03-24T21:54:33', rvwStatusId: 0, releasedForProduction: 0, 
            "reviewItems": [] }, 
          { "id": 0, "orderId": 0, "orderNo": 0, "OrderDate": "", "customerId": 0, "reviewedBy": 0, "reviewedOn": "", "rvwStatusId": 0, "releasedForProduction": 0, "reviewItems": [ { "id": 0, "contractReviewId": 0, "orderId": 0, "orderItemId": 0, "categoryName": "", "quantity": 0, "ecnr": false, "requireAssess": false, "sourceFrom": "", "reviewItemStatus": 0, "reviewQs": [ { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" } ] }, { "id": 0, "contractReviewId": 0, "orderId": 0, "orderItemId": 0, "categoryName": "", "quantity": 0, "ecnr": false, "requireAssess": false, "sourceFrom": "", "reviewItemStatus": 0, "reviewQs": [ { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" }, { "id": 0, "orderItemId": 0, "contractReviewItemId": 0, "srNo": 0, "reviewParameter": "", "response": false, "isMandatoryTrue": false, "remarks": "" } ] } ] } ] }
    */
    this.reviews().clear();
    var review: FormGroup = this.newReview();
    this.reviews().push(review);

     this.data.forEach(r => {
      var review: FormGroup = this.newReview();
      this.reviews().push(review);
 
      r.contractReviewItems.forEach(i => {
        var item = this.newReviewItem();
        (review.get("reviewItems") as FormArray).push(item);
        
        i.reviewItems.forEach(q => {
          (item.get("reviewQs") as FormArray).push(this.newReviewQ())
        })
 
    });
  });
  
  console.log(this.data);
  this.form.patchValue(this.data);
  }
  
  //reviews
      reviews(): FormArray {
        return this.form.get("reviews") as FormArray
      }

      newReview(): FormGroup {
        return this.fb.group({
          id: 0,  orderId: 0,  orderNo: 0, OrderDate: '', customerId: 0, reviewedBy: 0, 
          reviewedOn: '', rvwStatusId: 0, releasedForProduction: 0, reviewItems: this.fb.array([])
        })
      }

      addNewReview() {
        this.reviews().push(this.newReview());
      }

  //reviewItems
      reviewItems(i: number): FormArray {
        return this.reviews().at(i).get("reviewItems") as FormArray
      }

      newReviewItem(): FormGroup {
        return this.fb.group({
            id: 0, contractReviewId: 0, orderId: 0, orderItemId: 0, categoryName: '', quantity: 0, ecnr: false, 
            requireAssess: false, sourceFrom: '', reviewItemStatus: 0,
            reviewQs: this.fb.array([])
        })
      }

      addNewReviewItem(i: number) {
        this.reviewItems(i).push(this.newReviewItem());
      }

  //Questions
      reviewQs(i: number, j: number) : FormArray {
        return this.reviewItems(i).at(j).get("reviewQs") as FormArray
      }

      newReviewQ(): FormGroup {
        return this.fb.group({
          id: 0, orderItemId: 0, contractReviewItemId: 0, srNo: 0, reviewParameter: ['', Validators.required], response: false,
            isMandatoryTrue: false, remarks: ''
        })
      }

      addReviewQ(i: number, j: number) {
        this.reviewQs(i, j).push(this.newReviewQ());
      }

      removeReviewQ(i:number, j: number, k: number) {
        this.reviewQs(i, j).removeAt(k);
        this.reviewQs(i, j).markAsDirty();
        this.reviewQs(i, j).markAsTouched();
      }

  onSubmit() {
    this.service.register(this.form.value).subscribe(response => {
    }, error => {
      console.log(error);
      this.errors = error.errors;
    })
  }
 
      

}
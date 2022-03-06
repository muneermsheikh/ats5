import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { IContractReview } from 'src/app/shared/models/contractReview';
import { IContractReviewItem } from 'src/app/shared/models/contractReviewItem';
import { IReviewItem } from 'src/app/shared/models/reviewItem';
import { IUser } from 'src/app/shared/models/user';
import { SharedService } from 'src/app/shared/services/shared.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ReviewService } from '../review.service';

@Component({
  selector: 'app-review',
  templateUrl: './review.component.html',
  styleUrls: ['./review.component.css']
})
export class ReviewComponent implements OnInit {
  //@ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  //activeTab: TabDirective;
  routeId: string;

  review: IContractReview;
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
    this.routeId = this.activatedRoute.snapshot.params['id'];
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
   }

  ngOnInit(): void {
      //this.routeId = this.activatedRoute.snapshot.params['id'];

      this.createForm();
      this.getReview(+this.routeId);
  
  }

  createForm() {
    this.form = this.fb.group({
        id: [null], 
        orderId: 0,  
        orderNo: 0, 
        OrderDate: '', 
        customerId: 0, 
        reviewedBy: 0, 
        reviewedOn: '', 
        rvwStatusId: 0, 
        releasedForProduction: 0,
        contractReviewItems: this.fb.array([])
    } 
    );

    this.loadReview();
  }

  
  loadReview() {
    this.service.getReview(+this.routeId).subscribe( response => {
      this.review = response;
      this.form.patchValue(this.review);
      if(this.review.contractReviewItems != null) {
        for(const p of this.review.contractReviewItems) {
          this.contractReviewItems.push(new FormControl(p));
        }
      }
    })
  }

  setExistingReviews(rvws: IReviewItem[]): FormArray {
      const formArray = new FormArray([]);
      rvws.forEach(q => {
        formArray.push(this.fb.group({
          id: q.id, orderItemId: q.orderItemId, contractReviewItemId: q.contractReviewItemId, 
          srNo: q.srNo, reviewParameter: q.reviewParameter, response: q.response,
          isMandatoryTrue: q.isMandatoryTrue, remarks: q.remarks
        }))
      });
      return formArray;
  }

  getReview(id: number) {
    this.service.getReview(id).subscribe( response => {
      this.review = response;
      this.editReview(this.review);
    })
  }
  
  editReview(rvw: IContractReview) {
      this.form.patchValue( {
        id: rvw.id, orderId: rvw.orderId, orderNo: rvw.orderNo, OrderDate: rvw.orderDate, 
        customerId: rvw.customerId, reviewedBy: rvw.reviewedBy, reviewedOn: rvw.reviewedOn, 
        rvwStatusId: rvw.rvwStatusId, releasedForProduction: rvw.releasedForProduction,
    });

    if (rvw.contractReviewItems != null) {
      console.log('updating contractreviewitems');
      this.form.setControl('contractReviewItems', this.setExistingItems(rvw.contractReviewItems));
    }
  }
    
  setExistingItems(items: IContractReviewItem[]): FormArray {
    const formArray = new FormArray([]);
    items.forEach(ph => {
      formArray.push(this.fb.group({
          id: ph.id, contractReviewId: ph.contractReviewId, orderId: ph.orderId, orderItemId: ph.orderItemId,
          categoryName: ph.categoryName, quantity: ph.quantity, ecnr: ph.ecnr, requireAssess: ph.requireAssess,
          sourceFrom: ph.sourceFrom, reviewItemStatus: ph.reviewItemStatus
      }));
      
      //if (ph.reviewItems !=null) this.form.setControl('reviewItems', this.setExistingReviews(ph.reviewItems));
    });
    return formArray;
  }

   

  //userPhones
      get contractReviewItems() : FormArray {
        return this.form.get("contractReviewItems") as FormArray
      }
      
      newContractReviewItem(): FormGroup {
        return this.fb.group({
            id: 0, contractReviewId: 0, orderId: 0, orderItemId: 0, categoryName: '', quantity: 0, ecnr: false, 
            requireAssess: false, sourceFrom: '', reviewItemStatus: 0
        })
      }
      
      addContractReviewItem() {
        this.contractReviewItems.push(this.newContractReviewItem());
      }
      
      removeContractReviewItem(i:number) {
        this.contractReviewItems.removeAt(i);
        this.contractReviewItems.markAsDirty();
        this.contractReviewItems.markAsTouched();
      }

  //reviewItems
      reviewItems(i: number) : FormArray {
        return this.contractReviewItems.at(i).get("reviewItems") as FormArray
        //return this.contractReviews().at(ti).get("contractReviewItems") as FormArray
      }

      newReviewItem(): FormGroup {
        return this.fb.group({
          id: 0, orderItemId: 0, contractReviewItemId: 0, srNo: 0, reviewParameter: '', response: false,
            isMandatoryTrue: false, remarks: ''
        })
      }

      addReviewItem() {
        this.contractReviewItems.push(this.newReviewItem());
        //this.contractReviewItems(ti).push(this.newContractReviewItem());
      }

      removeReviewItem(i:number, j: number) {
        this.reviewItems(i).removeAt(j);
        this.reviewItems(i).markAsDirty();
        this.reviewItems(i).markAsTouched();
      }


  // various gets
/*
      loadMember() {
        this.service.getCandidate(+this.routeId).subscribe(
          response => {
              this.reviews = response;  
              console.log('load reviews', this.reviews);
              this.form.patchValue(this.reviews);
              if(this.reviews.userPassports != null) {for(const p of this.reviews.userPassports) {this.userPassports.push(new FormControl(p));}}
              if(this.reviews.userPhones !=null){for(const ph of this.reviews.userPhones) { this.userPhones.push(new FormControl(ph)); }}
              if (this.reviews.userQualifications != null) {for(const q of this.reviews.userQualifications) { this.userQualifications.push(new FormControl(q)); }}
              if (this.reviews.userProfessions != null) {for(const p of this.reviews.userProfessions) { this.userProfessions.push(new FormControl(p)); }}
              if (this.reviews.userExperiences != null) {for(const e of this.reviews.userExperiences) { this.userExperiences.push(new FormControl(e)); }}
              if (this.reviews.userAttachments != null) {for(const a of this.reviews.userAttachments) { this.userAttachments.push(new FormControl(a)); }}
            }
        )} 
*/
      onSubmit() {
        if (+this.routeId ===0) {
          this.CreateCV();
        } else {
          //this.UpdateCandidate();
        }
      }

      private CreateCV() {
        this.service.register(this.form.value).subscribe(response => {
        }, error => {
          console.log(error);
          this.errors = error.errors;
        })
      }

      

}
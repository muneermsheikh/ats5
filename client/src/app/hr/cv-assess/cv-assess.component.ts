import { Component, HostListener, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { ChecklistModalComponent } from 'src/app/candidate/checklist-modal/checklist-modal.component';
import { ChecklistService } from 'src/app/candidate/checklist.service';
import { ICandidateAssessedDto } from 'src/app/shared/models/candidateAssessedDto';
import { ICandidateAssessment } from 'src/app/shared/models/candidateAssessment';
import { ICandidateAssessmentAndChecklist } from 'src/app/shared/models/candidateAssessmentAndChecklist';
import { ICandidateAssessmentItem } from 'src/app/shared/models/candidateAssessmentItem';
import { ICandidateBriefDto } from 'src/app/shared/models/candidateBriefDto';
import { ChecklistHRDto, IChecklistHRDto } from 'src/app/shared/models/checklistHRDto';
import { IChecklistHRItem } from 'src/app/shared/models/checklistHRItem';
import { IOrderItemBriefDto } from 'src/app/shared/models/orderItemBriefDto';
import { IUser } from 'src/app/shared/models/user';
import { ConfirmService } from 'src/app/shared/services/confirm.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { CvAssessService } from '../cv-assess.service';

@Component({
  selector: 'app-cv-assess',
  templateUrl: './cv-assess.component.html',
  styleUrls: ['./cv-assess.component.css']
})
export class CvAssessComponent implements OnInit {

  cvBrief: ICandidateBriefDto;
  openOrderItems: IOrderItemBriefDto[];
  orderItemSelected: IOrderItemBriefDto;
  orderItemSelectedId: number;
  cvAssessment: ICandidateAssessment;
  
  //checklist$: Observable<IChecklistHRDto>;

  assessmentAndChecklist: ICandidateAssessmentAndChecklist;
//checklist
  checklist: IChecklistHRDto;
  checklistitems: IChecklistHRItem[];
  bsModalRef: BsModalRef;

  user: IUser;
  totalPoints: number;
  totalGained: number;
  percentage: number;
  qDesigned: boolean = false;
  requireInternalReview: boolean;
  lastOrderItemIdSelected: number=-1;

  form: FormGroup;
  validationErrors: string[] = [];

  @HostListener('window:beforeunload', ['event']) unloadNotification($event: any) {
    if(this.form.dirty) {$event.returnValue=true}
  }

  constructor(private fb: FormBuilder, 
    private service: CvAssessService,
    private bsModalService: BsModalService,
    private checklistService: ChecklistService,
    private toastr: ToastrService,
    private activatedRoute: ActivatedRoute,
    private accountsService: AccountService, private confirmService: ConfirmService,
    private router: Router, breadcrumb: BreadcrumbService) {
      this.accountsService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
     }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => { 
      this.cvBrief = data.candidateBrief;
      this.openOrderItems = data.openOrderItemsBrief;
      this.createForm();
    })
  }

  createForm() {
    this.form = this.fb.group({
      id:0, orderItemId: 0, candidateId: 0, assessedOn: '', assessResult: 0, remarks: '',
      candidateAssessmentItems: this.fb.array([])
    })
  }

  patchForm(p: ICandidateAssessment) {
    this.form.patchValue({
      id: p.id, orderItemId: p.orderItemId, assessedOn: p.assessedOn,
      assessResult: p.assessResult, remarks: p.remarks, candidateId: p.candidateId, candidateAssessmentItems:p.candidateAssessmentItems
    })

    if (p.candidateAssessmentItems?.length > 0) this.form.setControl('candidateAssessmentItems', this.setExistingCandidateItems(p.candidateAssessmentItems));
    
    this.maxMarksTotal();
    this.totalGained = this.candidateAssessmentItems.value.map(x => x.points).reduce((a,b) => a+b,0);
    this.calculatePercentage();
  }

  setExistingCandidateItems(items: ICandidateAssessmentItem[]): FormArray {
    const formArray = new FormArray([]);
    items.forEach(i => {
      formArray.push(this.fb.group({
        id: i.id, candidateAssessmentId: i.candidateAssessmentId, questionNo: i.questionNo,
        question: i.question, assessed: i.assessed, isMandatory: i.isMandatory,
        maxPoints: i.maxPoints, points: i.points, remarks: i.remarks
      }))
    });
    return formArray;
  }

  get candidateAssessmentItems(): FormArray {
    return this.form.get('candidateAssessmentItems') as FormArray;
  }

  newCandidateAssessmentItem(): FormGroup {
    return this.fb.group({
      id: 0, candidateAssessmentId: 0, questionNo: 0,
      question: ['', Validators.required ], 
      assessed: false, 
      isMandatory: false, 
      maxPoints: 0, 
      points: [0, [Validators.min(0),   this.matchValues('maxPoints')]],
      remarks: ''
    })
  }

  
  matchValues(matchTo: string): ValidatorFn {
    
    return (control: AbstractControl) => {
      var matched = control?.value <= control?.parent?.controls[matchTo].value
        ? null : {isMatching: true};
        console.log('matchTo', control?.parent?.controls[matchTo].value);
        console.log('control value', control?.value);
        console.log('matched', matched);  
      return matched;
    }
  }

  matchVsalues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      var matches = control?.value > 0 && control?.value <= control?.parent?.controls[matchTo].value 
        ? null : {isMatching: true};
      return matches
    }
  }

  addCandidateAssessmentItem() {
    this.qDesigned==true;
    this.toastr.info('qDesigned set to true');
    this.candidateAssessmentItems.push(this.newCandidateAssessmentItem());
  }

  removeCandidateAssessmentItem(i: number) {
    this.candidateAssessmentItems.removeAt(i);
    this.candidateAssessmentItems.markAsDirty();
    this.candidateAssessmentItems.markAsTouched();
  }

  deleteAssessment() {
    this.service.deleteAssessment(this.cvAssessment.id).subscribe(response => {
        if (response) {
          this.toastr.success('Candidate Assessment object deleted');
          this.orderItemSelected = null;
          this.cvAssessment=null;
          this.initializeTotals();
        } else {
          this.toastr.warning('failed to delete the assessment object');
        }
    }, error => {
      this.toastr.error('error in deleting the object', error);
    });
  }
  
  initializeTotals() {
    this.totalGained =0;
    this.totalPoints=0;
    this.percentage=0;

  }

  createNewAssessment(){

    if (this.orderItemSelectedId <= 0 || this.orderItemSelectedId === undefined) {
      this.toastr.warning('choose the order category before creating the assessment');
      return;
    }

    this.service.getCVAssessmentObject(this.requireInternalReview, this.cvBrief.id, this.orderItemSelectedId, new Date()).subscribe(response => {
      this.cvAssessment = response;
      this.patchForm(this.cvAssessment);
    }, error => {
      this.toastr.error('failed to retrieve assessment object from api');
    })
    /*
    this.lastOrderItemIdSelected = this.orderItemSelectedId;
    var cvAssess: ICandidateAssessment;

    var cvItems: ICandidateAssessmentItem[];
    if (!this.requireInternalReview) {
      console.log('entered not requireinternalreview');
      cvAssess = new CandidateAssessment(this.orderItemSelectedId, this.cvBrief.id, this.user.loggedinEmployeeId, new Date(), this.orderItemSelected.requireInternalReview, cvItems );
    } else {
      console.log('entered requireinternalreview');
      var items: IOrderItemAssessmentQ[];
      this.service.getOrderItemAssessmentQs(this.orderItemSelectedId).subscribe(response => {
        console.log('returned from api');
          items = response;
          if (items.length === 0) {
            this.toastr.warning('The Order Item selected requires internal assessment of candidates, for which ' + 
              'assessment Questions for the Order Item must be designed.  The Selected order item has not been designed');
            return null;
          }
          
          items.forEach(i => {
            var cItem = new CandidateAssessmentItem(i.questionNo,i.isMandatory, i.question, i.maxMarks);
            cvItems.push(cItem);
          })
          console.log('constructed items');
  
          cvAssess = new CandidateAssessment(this.orderItemSelectedId, this.cvBrief.id, this.user.loggedinEmployeeId, new Date(), this.orderItemSelected.requireInternalReview, cvItems );
          console.log('cvAssess', cvAssess);
      }, error => {
          this.toastr.error('failed to create Assessment item', error);
          return null;
      })
      
    }
    
    this.cvAssessment = cvAssess;
    console.log('createnew returned cvAssessment', cvAssess);
    console.log('createnew returned cvAssessmentItems', this.cvAssessment.candidateAssessmentItems);

    */
  
  }
  
  chooseSelectedOrderItem() {
    if (this.orderItemSelectedId <= 0 || this.orderItemSelectedId === undefined) return;
    if (this.lastOrderItemIdSelected === this.orderItemSelectedId) return;
    this.lastOrderItemIdSelected = this.orderItemSelectedId;

    this.initializeTotals();

    this.orderItemSelected = this.openOrderItems.find(x => x.orderItemId===this.orderItemSelectedId);
    
    if (this.orderItemSelected === null ||this.orderItemSelected === undefined) {
      this.candidateAssessmentItems.clear();
      return;
    } 

    this.requireInternalReview = this.orderItemSelected.requireInternalReview??false;
    
    if(this.requireInternalReview) {
        this.qDesigned = this.orderItemSelected?.assessmentQDesigned;

        if (!this.qDesigned) {
          this.toastr.warning('assessment Questions not designed - from component');
          this.candidateAssessmentItems.clear();
          return;
        }
    }
    /* this.service.getCVAssessment(this.cvBrief.id, this.orderItemSelected.orderItemId).subscribe(response => {
      this.cvAssessment = response;
      console.log('cv assessment', this.cvAssessment);
      if (this.cvAssessment !== null && this.cvAssessment !== undefined) {
        this.patchForm(this.cvAssessment);
      } else {
        this.toastr.warning('the candidate has not been assessed for the category selected.');
      }
    }, error => {
      this.toastr.error('failed to retrieve candidate assessment', error);
      this.candidateAssessmentItems.clear();
    })
    */
    this.service.getCVAssessmentAndChecklist(this.cvBrief.id, this.orderItemSelected.orderItemId).subscribe(response => {
      this.assessmentAndChecklist = response;
      
      this.checklist = this.assessmentAndChecklist.checklistHRDto;
      this.cvAssessment = this.assessmentAndChecklist.assessed;
      console.log('orderItemSelected', this.orderItemSelected);
      console.log('cvAssessment', this.cvAssessment);
      console.log('checklist', this.checklist);
      if (this.cvAssessment !== null && this.cvAssessment !== undefined) {
        this.patchForm(this.cvAssessment);
      } else {
        this.toastr.warning('the candidate has not been assessed for the category selected.');
      }
    }, error => {
      this.toastr.error('failed to retrieve candidate assessment', error);
      this.candidateAssessmentItems.clear();
    })

  }

  routeChange() {
    if (this.form.dirty) {
        this.confirmService.confirm('Confirm move to another page', 
        'This candidate assessment data is edited, but not saved. ' + 
        'Do you want to move to another page without saving the data?')
        .subscribe(result => {
          if (result) {
            this.router.navigateByUrl('');
          }
        })
    } else {
      this.router.navigateByUrl('');
    }
  }


  maxMarksTotal() {
    this.totalPoints =  this.candidateAssessmentItems.value.map(x => x.maxPoints).reduce((a, b) => a + b, 0);
    //this.totalPoints =  this.candidateAssessmentItems.value.filter(x => x.assessed===true). map(x => x.maxPoints).reduce((a, b) => a + b, 0);
    this.calculatePercentage();
  }

  pointsGainedTotal(i: number){
    this.totalGained = this.candidateAssessmentItems.value.map(x => x.points).reduce((a,b) => a+b,0);
    this.calculatePercentage();
    //(<FormArray>this.candidateAssessmentItems).controls['assessed'].at(i).patchValue(true);
    (<FormArray>this.form.controls['candidateAssessmentItems']).at(i).get("assessed").setValue(true);
  }

  calculatePercentage() {
    this.percentage = Math.round(100*this.totalGained / this.totalPoints);
  }

  /*
  openChecklistModal(user: IUser) {
    const title = 'Choose Order Item to refer selected CVs to';
    var returnvalue:any;
    var ids: number[];
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        user,
        title,
        orderItems: this.openOrderItems,
        ids
      }
    }
    this.bsModalRef = this.modalService.show(OrderItemModalComponent, config);
    this.bsModalRef.content.updateSelectedRoles.subscribe(values => {
      this.orderItemSelected = values;
      if (this.orderItemSelected===null) {
        
      }
    })
  }
  */

  update() {
    //if (this.cvAssessment.id === 0) {
      /* return this.service.insertCVAssessment(this.form.value).subscribe(response => {
        if (response) {
          this.toastr.success('created the Candidate Assessment');
          this.cvAssessment=null;
        } else {
          this.toastr.warning('failed to create the candidate assessment');
        }
      }, error => {
        this.toastr.error('error in creating a new assessment', error);
        this.validationErrors = error;
      })
    */
    //} else {
      this.toastr.info('updating ...');
      return this.service.updateCVAssessment(this.form.value).subscribe(response => {
        if (response) {
          this.toastr.success('updated the Candidate Assessment');
          this.cvAssessment=null;
        } else {
          this.toastr.warning('failed to update the candidate assessment');
        }
      }, error => {
        this.toastr.error('failed to update the candidate assessment', error);
      })
   // }
    
  }



  shortlistForForwarding() {
    if(!this.checklist) {
      this.toastr.warning('no checklisting done on the candidate');
      return;
    }
    if (this.cvAssessment !==null) {
      this.toastr.warning('this candidate is already assessed.');
      return;
    }

    //if (this.cvAssessment === null) {
      return this.service.insertCVAssessmentHeader(this.requireInternalReview, this.cvBrief.id, this.orderItemSelectedId, new Date()).subscribe(response => {
        
        if ((response.errorString==='' || response.errorString ===undefined || response.errorString === null) ) {
          
          this.cvAssessment = response.candidateAssessment;
          
          if (this.cvAssessment !==null) {
            this.patchForm(this.cvAssessment);
            this.toastr.success('shortlisted for forwarding to client');
          }
        } else if (response.errorString !== '') {
          this.toastr.error('failed to shortlist the candidate, ', response.errorString);
          console.log(response.errorString);
        }
      }, error => {
        this.toastr.error('error in creating the shortlisting', error);
        this.validationErrors = error;
      })
    
    //} 
    /*
    else {
      return this.service.updateCVAssessmentHeader(this.cvAssessment).subscribe(response => {
        
        if (response.errorString==='' && response.candidateAssessment !== null ) {
          this.cvAssessment = response.candidateAssessment;

          this.patchForm(this.cvAssessment);
          this.toastr.success('Edited the shortlisting for forwarding to client');
        } else if (response.errorString !== '') {
          this.toastr.error('failed to edit the shortlisting of the candidate, ', response.errorString);
        }
      }, error => {
        this.toastr.error('error in editing the shortlisting', error);
        this.validationErrors = error;
      })
    }
    */
    
  }

  //checklist modal
  openChecklistModal() {
    if (this.orderItemSelected === null || this.orderItemSelected === undefined) {
      this.toastr.warning('Order Item not selected');
      return;
    } else if (this.cvBrief.id === 0) {
      this.toastr.warning("Candidate Id not provided");
      return;
    }
    
    //this.checklist = this.getChecklistHRDto(this.cvBrief.id, this.orderItemSelectedId);
    this.checklistitems = this.checklist?.checklistHRItems;

    if (this.checklist === undefined || this.checklist === null) {
      this.toastr.warning("failed to get checklist values");
      return;
    }
    const config = {
        class: 'modal-dialog-centered modal-lg',
        initialState: {
        chklst: this.checklist //this.getChecklistHRDto(this.cvBrief.id, this.orderItemSelectedId),
        
      }
    }
    
    this.bsModalRef = this.bsModalService.show(ChecklistModalComponent, config);
    this.bsModalRef.content.updateChecklist.subscribe(values => {
      this.checklist = values;
      
      if (this.cvAssessment) this.cvAssessment.hrChecklistId=this.checklist.id;
      console.log('after modal, cvASsessment is:', this.cvAssessment);
      this.checklistService.updateChecklist(this.checklist).subscribe(() => {
      
        this.toastr.success('updated Checklist');
    
    }, error => {
      this.toastr.error('failed to update the checklist', error);
    });
  })
}

/*  
private getChecklistHRDto(candidateid: number, orderitemid: number) {
    let checklisthr= new ChecklistHRDto();
    let x: IChecklistHRDto;
    this.checklistService.getChecklist(candidateid, orderitemid).subscribe(response => {
      x = response;
      checklisthr.applicationNo = x.applicationNo;
      checklisthr.candidateId = x.candidateId;
      checklisthr.candidateName = x.candidateName;
      checklisthr.categoryRef = x.categoryRef;
      checklisthr.checkedOn = x.checkedOn;
      checklisthr.checklistHRItems = x.checklistHRItems;
      checklisthr.hrExecComments = x.hrExecComments;
      checklisthr.id=x.id;
      checklisthr.orderItemId=x.orderItemId;
      checklisthr.orderRef=x.orderRef;
      checklisthr.userLoggedId = x.userLoggedId;
      
      return checklisthr;
    }, error => {

    })
    return checklisthr;
  }

  getChecklist(candidateid: number, orderitemid: number): any {
    return this.checklistService.getChecklist(candidateid, orderitemid).subscribe(response => {
        this.toastr.success('got values of checklist from api');
        this.checklist = response;
        console.log('getchecklist', this.checklist);
        return response;
    }, error => {
      this.toastr.error('failed to get checklist object from api', error);
      return null;
    })

  }
 
  */
}


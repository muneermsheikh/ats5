import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subscription } from 'rxjs';
import { ICandidateAssessment } from 'src/app/shared/models/candidateAssessment';
import { ICandidateAssessmentItem } from 'src/app/shared/models/candidateAssessmentItem';
import { ICandidateBriefDto } from 'src/app/shared/models/candidateBriefDto';
import { IOrderItemBriefDto } from 'src/app/shared/models/orderItemBriefDto';
import { IUser } from 'src/app/shared/models/user';
import { CvAssessService } from '../cv-assess.service';

@Component({
  selector: 'app-candidate-assessment',
  templateUrl: './candidate-assessment.component.html',
  styleUrls: ['./candidate-assessment.component.css']
})
export class CandidateAssessmentComponent implements OnInit {

  @Output() updateAssessment = new EventEmitter();      //outputs form value for update in the parent
  
  @Input() cvAssessment: ICandidateAssessment;
  @Input() cvBrief: ICandidateBriefDto;
  @Input() requireInternalReview: boolean;
  @Input() orderItemSelected: IOrderItemBriefDto;
  //@Input() checklist: IChecklistHRDto;
  
  //parent to child 
  @Input() events: Observable<ICandidateAssessment>;      //emitted by parent whenever the value cvAssessment (of type ICandidateAssessment) changes
  
  private eventsSubscription: Subscription;


  user: IUser;

  totalPoints: number;
  totalGained: number;
  percentage: number;
  qDesigned: boolean = false;
  
  validationErrors: string[]=[];

  form: FormGroup;

  constructor(private fb: FormBuilder, private service: CvAssessService, private toastr: ToastrService) { }

  ngOnInit(): void {
    //receive emtited data from parent whenever the value CVAssessment changes
    this.eventsSubscription = this.events.subscribe(data => {
      this.candidateAssessmentItems.clear();
      this.cvAssessment = data;
      this.patchForm(this.cvAssessment);
    }
    );

    this.qDesigned = this.orderItemSelected?.assessmentQDesigned;
    this.createForm();
    this.patchForm(this.cvAssessment);
  }

  addCandidateAssessmentItem() {
    this.qDesigned==true;
    this.toastr.info('qDesigned set to true');
    this.candidateAssessmentItems.push(this.newCandidateAssessmentItem());
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


  removeCandidateAssessmentItem(i: number) {
    this.candidateAssessmentItems.removeAt(i);
    this.candidateAssessmentItems.markAsDirty();
    this.candidateAssessmentItems.markAsTouched();
  }

  deleteAssessment() {
    this.service.deleteAssessment(this.cvAssessment.id).subscribe(response => {
        if (response) {
          this.toastr.success('Candidate Assessment object deleted');
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
    this.totalGained = this.candidateAssessmentItems.value.map(x => x.points).reduce((a:number,b:number) => a+b,0);

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

  
  maxMarksTotal() {

    this.totalPoints =  this.candidateAssessmentItems.value.map(x => x.maxPoints).reduce((a:number, b: number) => a + b,0);
    //this.totalPoints =  this.candidateAssessmentItems.value.filter(x => x.assessed===true). map(x => x.maxPoints).reduce((a, b) => a + b, 0);
    this.calculatePercentage();
  }

  pointsGainedTotal(i: number){
    this.totalGained = this.candidateAssessmentItems.value.map(x => x.points).reduce((a:number,b:number) => a+b,0);
    this.calculatePercentage();
    //(<FormArray>this.form.controls['candidateAssessmentItems']).at(i).get("assessed").setValue(true);   //set value of the DOM assessed to true
  }

  calculatePercentage() {
    this.percentage = Math.round(100*this.totalGained / this.totalPoints);
  }

  //modals
  openChecklistModal() {

  }
  
  update() {
    console.log(this.form.dirty);
    if(this.form.dirty) {
      //this.patchForm(this.cvAssessment);
      console.log('emitting values:', this.form.value);
      this.updateAssessment.emit(this.form.value);
    }
  }

  ngOnDestroy() {
    this.eventsSubscription.unsubscribe();
  }
}

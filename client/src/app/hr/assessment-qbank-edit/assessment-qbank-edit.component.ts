import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { IAssessmentQBank, IAssessmentQBankItem } from 'src/app/shared/models/assessmentQBank';
import { IUser } from 'src/app/shared/models/user';
import { BreadcrumbService } from 'xng-breadcrumb';
import { HrService } from '../hr.service';

@Component({
  selector: 'app-assessment-qbank-edit',
  templateUrl: './assessment-qbank-edit.component.html',
  styleUrls: ['./assessment-qbank-edit.component.css']
})
export class AssessmentQBankEditComponent implements OnInit {

  user: IUser;
  form: FormGroup;
  routeId: string;
  qbank: IAssessmentQBank;
  isAddMode: boolean;
  loading = false;
  submitted = false;
  errors: string[]=[];

  constructor(private service: HrService, private bcService: BreadcrumbService,
      private activatedRoute: ActivatedRoute, private router: Router, private fb: FormBuilder,
      private accountService: AccountService, private toastr: ToastrService) { 
        this.routeId = this.activatedRoute.snapshot.params['id'];
        this.bcService.set('@assessmentQBank', ' ');
      }

  ngOnInit(): void {

    this.isAddMode = !this.routeId;

    this.activatedRoute.data.subscribe(data => { 
      this.qbank = data.qbank;
    })

    if (!this.isAddMode) {
      this.getQBank(+this.routeId);
    }

  }

  getQBank(id: number) {
    this.service.getQBankByCategoryId(id).subscribe( 
      response => {
        this.qbank = response;
        this.editQBank(this.qbank);
      }
    )
  }

  createForm() {
    this.form = this.fb.group({
      id: [null],
      categoryId: 0,
      categoryName: '',
      assessmentQBankItems: this.fb.array([])
    } );

    if (!this.isAddMode) this.loadQBank();
  }

  editQBank(qb: IAssessmentQBank) {
    this.form.patchValue( {
      id: qb.id,
      categoryId: qb.categoryId,
      categoryName: qb.categoryName
    });

    if (qb.assessmentQBankItems !== null) {
        this.form.setControl('assessmentQBankItems', this.setExistingItems(qb.assessmentQBankItems));
    }
  }

  setExistingItems(items: IAssessmentQBankItem[]): FormArray {
    const formArray = new FormArray([]);

    items.forEach(item => {
      formArray.push(this.fb.group({
        id: item.id,
        assessmentQBankId: item.assessmentQBankId,
        assessmentParameter: item.assessmentParameter,
        qNo: item.qNo,
        isStandardQ: item.isStandardQ,
        question: item.question,
        maxPoints: item.maxPoints
      }));
    })
    return formArray;
  }

  get assessmentQBankItems() : FormArray {
    return this.form.get("assessmentQBankItems") as FormArray
  }

  newAssessmentQBankItem(): FormGroup {
    return this.fb.group({
      id: 0,
      assessmentQBankId: 0,
      assessmentParameter: ['', Validators.required],
      qNo: [0, Validators.required],
      isStandardQ: [false],
      question: ['', Validators.required],
      maxPoints: [0, Validators.required]
    })
  }
  addAssessmentQBankItem() {
    this.assessmentQBankItems.push(this.newAssessmentQBankItem());
  }

  removeAssessmentQBankItem(i:number) {
    this.assessmentQBankItems.removeAt(i);
    this.assessmentQBankItems.markAsDirty();
    this.assessmentQBankItems.markAsTouched();
  }

  loadQBank(){
    this.form.patchValue(this.qbank);
    if(this.qbank.assessmentQBankItems !== null) {
      for(const q of this.qbank.assessmentQBankItems) {
        this.assessmentQBankItems.push(new FormControl(q));
      }
    }
  }

  onSubmit(){
    if (+this.routeId === 0) {
      this.CreateQBank();
    } else {
      this.UpdateQBank();
    }
  }

  private CreateQBank() {
    this.service.insertQBank(this.form.value).subscribe(response => {

    }, error => {
      console.log(error);
      this.errors = error.errors;
    })
  }

  private UpdateQBank() {
    this.service.updateQBank(this.form.value).subscribe(response => {
      this.toastr.success('Question Bank updated');
    }, error => {
      this.toastr.error(error);
    })
  }

}

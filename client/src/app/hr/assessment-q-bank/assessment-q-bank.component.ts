import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { IAssessmentQBank, IAssessmentQBankItem } from 'src/app/shared/models/assessmentQBank';
import { IProfession } from 'src/app/shared/models/profession';
import { qBankParams } from 'src/app/shared/models/qBankParams';
import { IUser } from 'src/app/shared/models/user';
import { SharedService } from 'src/app/shared/services/shared.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { HrService } from '../hr.service';

@Component({
  selector: 'app-assessment-q-bank',
  templateUrl: './assessment-q-bank.component.html',
  styleUrls: ['./assessment-q-bank.component.css']
})
export class AssessmentQBankComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  qs: IAssessmentQBank[];
  user: IUser;
  
  qBankParams = new qBankParams();
  totalCount: number;
  assessmentParameters: string[];
  existingQBankCats: IProfession[];
  
  form: FormGroup;
  
  constructor(private service: HrService, private bcService: BreadcrumbService,
    private fb: FormBuilder, private activatedRoute: ActivatedRoute,
    private toastr: ToastrService) { 
      this.bcService.set('@assessmentQBank', ' ');
    }

ngOnInit(): void {


  this.activatedRoute.data.subscribe(data => { 
    this.qs = data.qbank;
  })

  this.createForm();  
}

createForm() {
  this.form = this.fb.group({
    id: [null],
    categoryId: 0,
    categoryName: '',
    assessmentQBankItems: this.fb.array([])
  } );
}

createCategoriesForm() {
  this.form = this.fb.group({
    categories: this.fb.array([])
  })
}
categories(): FormArray {
  return this.form.get('categories') as FormArray;
}


assessmentQBankItems(categoryIndex: number) : FormArray {
  return this.categories().at(categoryIndex).get('assessmentQBankItems') as FormArray;
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

/*
get assessmentQBankItems() : FormArray {
  return this.form.get("assessmentQBankItems") as FormArray
}
*/
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
addAssessmentQBankItem(categoryIndex: number) {
  this.assessmentQBankItems(categoryIndex).push(this.newAssessmentQBankItem());
}

removeAssessmentQBankItem(categoryIndex:number, qItemIndex: number) {
  this.assessmentQBankItems(categoryIndex).removeAt(qItemIndex);
  this.assessmentQBankItems(categoryIndex).markAsDirty();
  this.assessmentQBankItems(categoryIndex).markAsTouched();
}

loadQBank(){
  var indx=0;
  this.form.patchValue(this.qs);
  this.qs.forEach(q => {
    if(q.assessmentQBankItems !== null) {
        for(const item of q.assessmentQBankItems) {
          this.assessmentQBankItems(indx).push(new FormControl(q));
        }
    }
    indx++;
  })
}

onSubmit() {
  console.log(this.form.value);
}

  /*
  constructor(private service: HrService, 
      private accountService: AccountService,
      private sharedService: SharedService,
      private activatedRoute: ActivatedRoute) {
        this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
       }

  ngOnInit(): void {
    this.service.setQParams(this.qBankParams);
    this.activatedRoute.data.subscribe(data => { 
      this.qs = data.qs;
      this.totalCount = data.qs.count;
      this.existingQBankCats = data.existingCats;
    }, error => {
      console.log(error);
    })
    console.log(this.qs);
    console.log(this.existingQBankCats);
  }

  getQs(useCache=false) {
    this.service.getQBank(useCache).subscribe(response => {
      this.qs = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    })
  }
  
  onSearch() {
    const params = this.service.getQParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.service.setQParams(params);
    this.getQs();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.qBankParams = new qBankParams();
    this.service.setQParams(this.qBankParams);
    this.getQs();
  }

  
  onCategorySelected(categoryId: number) {
    const prms = this.service.getQParams();
    prms.categoryId = categoryId;
    prms.pageNumber=1;
    this.service.setQParams(prms);
    this.getQs();
  }

  onSortSelected(sort: string) {
    this.qBankParams.sort = sort;
    this.getQs();
  }
  
  onPageChanged(event: any){
    const params = this.service.getQParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.service.setQParams(params);
      this.getQs(true);
    }
  }
*/

}

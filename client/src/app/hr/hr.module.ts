import { NgModule } from '@angular/core';
import { AssessmentQBankComponent } from './assessment-q-bank/assessment-q-bank.component';
import { SharedModule } from '../shared/shared.module';
import { AssessmentQBankEditComponent } from './assessment-qbank-edit/assessment-qbank-edit.component';
import { AssessmentStddComponent } from './assessment-stdd/assessment-stdd.component';
import { HrRoutingModule } from './hr-routing.module';
import { CvAssessComponent } from './cv-assess/cv-assess.component';
import { HrchecklistComponent } from './hrchecklist/hrchecklist.component';
import { HrindexComponent } from './hrindex/hrindex.component';
import { StddqEditComponent } from './stddq-edit/stddq-edit.component';
import { AssessQComponent } from './assess-q/assess-q.component';
import { CommonModule } from '@angular/common';
import { AssessComponent } from './assess/assess.component';
import { ItemQComponent } from './item-q/item-q.component';
import { CandidateAssessmentComponent } from './candidate-assessment/candidate-assessment.component';




@NgModule({
  declarations: [
    AssessmentQBankComponent,
    AssessmentQBankEditComponent,
    AssessmentStddComponent,
    CvAssessComponent,
    HrchecklistComponent,
    HrindexComponent,
    StddqEditComponent,
    AssessQComponent,
    AssessComponent,
    ItemQComponent,
    CandidateAssessmentComponent,
  ],
  imports: [
    SharedModule,
    CommonModule,
    HrRoutingModule
  ]
})
export class HrModule { }

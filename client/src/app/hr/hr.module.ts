import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HrforwardComponent } from './hrforward/hrforward.component';
import { AssessmentQComponent } from './assessment-q/assessment-q.component';
import { AssessmentQBankComponent } from './assessment-q-bank/assessment-q-bank.component';
import { SharedModule } from '../shared/shared.module';
import { AssessmentQBankEditComponent } from './assessment-qbank-edit/assessment-qbank-edit.component';
import { AssessmentStddComponent } from './assessment-stdd/assessment-stdd.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HrRoutingModule } from './hr-routing.module';
import { CvAssessComponent } from './cv-assess/cv-assess.component';



@NgModule({
  declarations: [
    HrforwardComponent,
    AssessmentQComponent,
    AssessmentQBankComponent,
    AssessmentQBankEditComponent,
    AssessmentStddComponent,
    CvAssessComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    //FormsModule,
    //ReactiveFormsModule,
    HrRoutingModule
  ]
})
export class HrModule { }

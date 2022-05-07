import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CvrefComponent } from './cvref/cvref.component';
import { SharedModule } from '../shared/shared.module';
import { AssessmentComponent } from './assessment/assessment.component';
import { CvfwdComponent } from './cvfwd/cvfwd.component';
import { SeldecisionComponent } from './seldecision/seldecision.component';
import { MedicaltestComponent } from './medicaltest/medicaltest.component';
import { EmigComponent } from './emig/emig.component';
import { TravelComponent } from './travel/travel.component';
import { VisaComponent } from './visa/visa.component';
import { FeedbackComponent } from './feedback/feedback.component';



@NgModule({
  declarations: [
    CvrefComponent,
    AssessmentComponent,
    CvfwdComponent,
    SeldecisionComponent,
    MedicaltestComponent,
    EmigComponent,
    TravelComponent,
    VisaComponent,
    FeedbackComponent
  ],
  imports: [
    CommonModule,
    SharedModule
  ]
})
export class CvrefModule { }

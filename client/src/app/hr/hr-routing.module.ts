import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AssessmentStddComponent } from './assessment-stdd/assessment-stdd.component';
import { AssessmentStddQsResolver } from '../resolvers/assessmentStddQsResolver';
import { HrchecklistComponent } from './hrchecklist/hrchecklist.component';
import { HrindexComponent } from './hrindex/hrindex.component';
import { StddqEditComponent } from './stddq-edit/stddq-edit.component';
import { AssessmentStddQResolver } from '../resolvers/assessmentStddQResolver';
import { AssessmentQBankComponent } from './assessment-q-bank/assessment-q-bank.component';
import { AssessmentQBankResolver } from '../resolvers/assessmentQBankResolver';
import { CategoriesResolver } from '../resolvers/categoriesResolver';
import { AssessComponent } from './assess/assess.component';
import { OrderResolver } from '../resolvers/orderResolver';
import { AssessQComponent } from './assess-q/assess-q.component';
import { OrderItemBriefResolver } from '../resolvers/orderItemBriefResolver';
import { AssessmentQsResolver } from '../resolvers/assessmentQsResolver';
import { CvAssessComponent } from './cv-assess/cv-assess.component';
import { CandidateBriefResolver } from '../resolvers/candidateBriefResolver';
import { OpenOrderItemsResolver } from '../resolvers/openOrderItemsResolver';

import { PreventUnsavedChangesGuard } from '../guards/prevent-unsaved-changes.guard';
import { CvrefComponent } from '../admin/cvref/cvref.component';
import { AssessedAndApprovedCVsResolver } from '../resolvers/assessedAndApprovedCVsResolver';


const routes = [
  {path: '', component: HrindexComponent,  data: {breadcrumb: 'HR Division'}},
  {path: 'stddqs', component: AssessmentStddComponent,
    resolve: { stddqs: AssessmentStddQsResolver },data: {breadcrumb: {breadcrumb: 'Standard Assessment Questions'}}
  },
  
  {path: 'editstdd/:id', component: StddqEditComponent,
    resolve: {stddq: AssessmentStddQResolver}
  },
  {path: 'checklist', component: HrchecklistComponent,
    resolve: {
      //stddqs: AssessmentStddQsResolver
    }},
    {path: 'qs', component: AssessmentQBankComponent,
    resolve: {
      qs: AssessmentQBankResolver,
      categories: CategoriesResolver
    }},
    {path: 'orderassess/:id', component: AssessComponent,
    resolve: {
      order: OrderResolver
    }},
    
    {path: 'itemassess/:id', component: AssessQComponent,
    resolve: {
      itembrief: OrderItemBriefResolver,
      assessment: AssessmentQsResolver
    }}, 
    
    {path: 'cvassess/:id', component: CvAssessComponent,
      canDeactivate: [PreventUnsavedChangesGuard],
    resolve: {
      candidateBrief: CandidateBriefResolver,
      openOrderItemsBrief: OpenOrderItemsResolver
    }},

    {path: 'cvforward', component: CvrefComponent,
    //canDeactivate: [PreventUnsavedChangesGuard],
    resolve: {assessedcvs: AssessedAndApprovedCVsResolver},
  }
    
    
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class HrRoutingModule { }

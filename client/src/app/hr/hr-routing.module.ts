import { NgModule } from '@angular/core';
import { AssessmentQBankComponent } from './assessment-q-bank/assessment-q-bank.component';
import { AssessmentQBankResolver } from '../resolvers/assessmentQBankResolver';
import { AssessmentQBankExistingCatsResolver } from '../resolvers/assessmentQBankExistingCatsResolver';
import { RouterModule } from '@angular/router';
import { AssessmentStddComponent } from './assessment-stdd/assessment-stdd.component';
import { AssessmentStddQResolver } from '../resolvers/assessmentStddQResolver';
import { OpenOrderItemsResolver } from '../resolvers/openOrderItemsResolver';
import { CandidateViewComponent } from '../candidate/candidate-view/candidate-view.component';
import { ChecklistComponent } from '../candidate/checklist/checklist.component';
import { ChecklistResolver } from '../resolvers/checklistResolver';

const routes = [
  {path: '', component: AssessmentQBankComponent,
    resolve: {qs: AssessmentQBankResolver,
        existingCats: AssessmentQBankExistingCatsResolver
    },
    data: {breadcrumb: {alias: 'QBankListing'}}
  },
  
  {path: 'edit/:id', component: AssessmentQBankComponent,
    resolve: {qbank: AssessmentQBankResolver}
  },
  {path: 'stddqs', component: AssessmentStddComponent,
    resolve: {
      stddqs: AssessmentStddQResolver
    }},
    /* {path: 'hrchecklist', component: ChecklistComponent, 
      resolve: {
        checklistdto: ChecklistResolver
      },
      data: {breadcrumb: 'HR Checklist'}},
    */
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

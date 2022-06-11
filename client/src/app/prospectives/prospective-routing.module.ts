import { NgModule } from '@angular/core';
import { AddToDBComponent } from './add-to-db/add-to-db.component';
import { ProspectiveListingComponent } from './prospective-listing/prospective-listing.component';
import { CandidateHistoryComponent } from '../candidate/candidate-history/candidate-history.component';
import { ContactResultsResolver } from '../resolvers/contactResultsResolver';
import { EmployeeIdsAndKnownAsResolver } from '../resolvers/employeeIdsAndKnownAsResolver';
import { RouterModule } from '@angular/router';

const routes = [
  {path: '', component: ProspectiveListingComponent, 
  resolve: 
      { 
        results: ContactResultsResolver,
        employees: EmployeeIdsAndKnownAsResolver},
    data: {breadcrumb: {alias: 'Prospective candidates'}} },
  {path: 'add', component: AddToDBComponent, data: {breadcrumb: {alias: 'Add Candidate to DB'}}},
  {path: 'historyfromprospective/:prospectiveId', component: CandidateHistoryComponent, 
      data: {breadcrumb: {alias: 'Prospective Candidate History'}}},
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
   ],
})
export class ProspectiveRoutingModule { }

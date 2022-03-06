import { NgModule } from '@angular/core';
import { ListingComponent } from './listing.component';
import { CandidateEditComponent } from './candidate-edit/candidate-edit.component';
import { RouterModule } from '@angular/router';
import { UploadDownloadComponent } from './upload-download/upload-download.component';
import { CandidateHistoryComponent } from './candidate-history/candidate-history.component';
import { CandidateHistoryResolver } from '../resolvers/candidateHistoryResolver';
import { ContactResultsResolver } from '../resolvers/contactResultsResolver';
import { EmployeeIdsAndKnownAsResolver } from '../resolvers/employeeIdsAndKnownAsResolver';
import { CandidateHistoryFromHistoryIdResolver } from '../resolvers/candidateHistoryFromHistoryIdResolver';

const routes = [
  {path: '', component: ListingComponent},
  {path: 'add', component:CandidateEditComponent , data: {breadcrumb: {alias: 'candidateEdit'}}},
  {path: 'edit/:id', component: CandidateEditComponent, data: {breadcrumb: {alias: 'candidateAdd'}}},
  {path: 'view/:id', component: CandidateEditComponent , data: {breadcrumb: {alias: 'candidateView'}}},
  {path: 'upload/:id', component: UploadDownloadComponent  , data: {breadcrumb: {alias: 'upload'}}},
  {path: 'download/:id', component: UploadDownloadComponent  , data: {breadcrumb: {alias: 'upload'}}},
  {path: 'newtask/:id', component: UploadDownloadComponent  , data: {breadcrumb: {alias: 'upload'}}},
  {path: 'history/:id', component: CandidateHistoryComponent, 
    resolve: 
      { history: CandidateHistoryResolver,
        results: ContactResultsResolver,
        employees: EmployeeIdsAndKnownAsResolver},
      data: {breadcrumb: {alias: 'customerDetail'}}},
  {path: 'historybyid/:id', component: CandidateHistoryComponent, 
      resolve: 
        { history: CandidateHistoryFromHistoryIdResolver,
          results: ContactResultsResolver,
          employees: EmployeeIdsAndKnownAsResolver},
        data: {breadcrumb: {alias: 'customerDetail'}}}
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
export class CandidateRoutingModule { }

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientComponent } from './client.component';
import { ClientReviewComponent } from './client-review/client-review.component';
import { CustomerResolver } from '../resolvers/customerResolver';
import { ClientEditComponent } from './client-edit/client-edit.component';
import { CustomerReviewResolver } from '../resolvers/customerReviewResolver';
import { CustomerReviewStatusDataResolver } from '../resolvers/customerReviewStatusDataResolver';

const routes: Routes = [
  {path: ':custType', component: ClientComponent, data: {breadcrumb: {alias: 'customerlist'}}},
  {path: 'review/:id', component: ClientReviewComponent , 
    resolve: {
      customerReview: CustomerReviewResolver,
      reviewStatusData: CustomerReviewStatusDataResolver  
    },  
    data: {breadcrumb: {alias: 'customerReview'}}},
  {path: ':id', component: ClientEditComponent, resolve: {customer: CustomerResolver}, data: {breadcrumb: {alias: 'customerDetail'}}}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class ClientRoutingModule { }

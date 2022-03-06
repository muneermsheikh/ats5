import { NgModule } from '@angular/core';
import { OrderComponent } from './order.component';
import { OrderEditComponent } from './order-edit/order-edit.component';
import { RouterModule } from '@angular/router';
import { ReviewComponent } from './review/review.component';
import { OrderResolver } from '../resolvers/orderResolver';


const routes = [
  {path: '', component: OrderComponent},
  {path: 'add', component:OrderEditComponent , data: {breadcrumb: {alias: 'OrderAdd'}}},
  {path: 'edit/:id', component: OrderEditComponent, data: {breadcrumb: {alias: 'OrderEdit'}}},
  {path: 'view/:id', component: OrderEditComponent , data: {breadcrumb: {alias: 'OrderView'}}},
  {path: 'review/:id', component: ReviewComponent , data: {breadcrumb: {alias: 'ContractReview'}}}
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

export class OrderRoutingModule { }

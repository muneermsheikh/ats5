import { NgModule } from '@angular/core';
import { OrderComponent } from './order.component';
import { OrderEditComponent } from './order-edit/order-edit.component';
import { RouterModule } from '@angular/router';
import { ReviewComponent } from './review/review.component';
import { OrderResolver } from '../resolvers/orderResolver';
import { CustomerOfficialsResolver } from '../resolvers/customerOfficialsResolver';
import { CategoriesResolver } from '../resolvers/categoriesResolver';
import { EmployeeIdsAndKnownAsResolver } from '../resolvers/employeeIdsAndKnownAsResolver';
import { CustomerNameCityResolver } from '../resolvers/customerNameCityResolver';
import { ContractReviewResolver } from '../resolvers/contractReviewResolver';
import { ReviewItemDataResolver } from '../resolvers/reviewItemDataResolver';
import { TeachersComponent } from './teachers/teachers.component';


const routes = [
  {path: '', component: OrderComponent},
  {path: 'add', component:OrderEditComponent , data: {breadcrumb: {alias: 'OrderAdd'}}},
  {path: 'edit/:id', component: OrderEditComponent, 
    resolve: {
      order: OrderResolver,
      associates: CustomerOfficialsResolver,
      professions: CategoriesResolver,
      employees: EmployeeIdsAndKnownAsResolver,
      customers: CustomerNameCityResolver,
      
    },
    data: {breadcrumb: {alias: 'OrderEdit'}}},
  {path: 'view/:id', component: OrderEditComponent , data: {breadcrumb: {alias: 'OrderView'}}},
  {path: 'review/:id', component: ReviewComponent , 
    resolve: {
      review: ContractReviewResolver,
      reviewQs: ReviewItemDataResolver
    },
  data: {breadcrumb: {alias: 'ContractReview'}}},
  {path: 'teachers', component: TeachersComponent}
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

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderComponent } from './order.component';
import { OrderEditComponent } from './order-edit/order-edit.component';
import { OrderLineComponent } from './order-line/order-line.component';
import { OrderRoutingModule } from './order-routing.module';
import { SharedModule } from '../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TeachersComponent } from './teachers/teachers.component';
import { ReviewComponent } from './review/review.component';
import { JobDescriptionModalComponent } from './job-description-modal/job-description-modal.component';
import { RemunerationModalComponent } from './remuneration-modal/remuneration-modal.component';
import { ReviewModalComponent } from './review-modal/review-modal.component';
import { ChooseAgentsModalComponent } from './choose-agents-modal/choose-agents-modal.component';
import { IdsModalComponent } from './ids-modal/ids-modal.component';


@NgModule({
  declarations: [
    OrderComponent,
    OrderEditComponent,
    OrderLineComponent,
    TeachersComponent,
    ReviewComponent,
    JobDescriptionModalComponent,
    RemunerationModalComponent,
    ReviewModalComponent,
    ChooseAgentsModalComponent,
    IdsModalComponent,
  ],
  imports: [
    CommonModule, 
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    TabsModule,
    OrderRoutingModule
  ]
})
export class OrdersModule { }

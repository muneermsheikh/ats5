import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClientComponent } from './client.component';
import { ClientItemComponent } from './client-item/client-item.component';
import { ClientDetailComponent } from './client-detail/client-detail.component';
import { SharedModule } from '../shared/shared.module';
import { ClientRoutingModule } from './client-routing.module';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ClientReviewComponent } from './client-review/client-review.component';
import { ClientEditComponent } from './client-edit/client-edit.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    ClientComponent,
    ClientItemComponent,
    ClientDetailComponent,
    ClientReviewComponent,
    ClientEditComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    SharedModule,
    ClientRoutingModule,
    TabsModule
  ]
})
export class ClientModule { }

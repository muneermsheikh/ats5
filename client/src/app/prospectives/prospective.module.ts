import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProspectiveRoutingModule } from './prospective-routing.module';
import { SharedModule } from '../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AddToDBComponent } from './add-to-db/add-to-db.component';
import { ProspectiveListingComponent } from './prospective-listing/prospective-listing.component';
import { ProspectiveItemComponent } from './prospective-item/prospective-item.component';



@NgModule({
  declarations: [
    AddToDBComponent,
    ProspectiveListingComponent,
    ProspectiveItemComponent
  ],
  imports: [
    CommonModule, 
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    ProspectiveRoutingModule
  ]
})
export class ProspectiveModule { }

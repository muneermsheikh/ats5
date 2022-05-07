import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListingComponent } from './listing.component';
import { CandidateEditComponent } from './candidate-edit/candidate-edit.component';
import { CandidateItemComponent } from './candidate-item/candidate-item.component';
import { SharedModule } from '../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CandidateRoutingModule } from './candidate-routing.module';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { CandidateHistoryComponent } from './candidate-history/candidate-history.component';
import { ChecklistModalComponent } from './checklist-modal/checklist-modal.component';
import { CallModalComponent } from '../shared/call-modal/call-modal.component';
import { CallrecordsComponent } from '../shared/callrecords/callrecords.component';


@NgModule({
  declarations: [
    ListingComponent,
    CandidateEditComponent,
    CandidateItemComponent,
    CandidateHistoryComponent,
    ChecklistModalComponent,
    CallModalComponent,
    CallrecordsComponent
  ],
  imports: [
    CommonModule,
    CandidateRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule,
    TabsModule
  ]
})
export class CandidateModule { }

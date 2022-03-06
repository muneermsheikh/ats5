import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListingComponent } from './listing.component';
import { CandidateEditComponent } from './candidate-edit/candidate-edit.component';
import { CandidateItemComponent } from './candidate-item/candidate-item.component';
import { CandidateViewComponent } from './candidate-view/candidate-view.component';
import { SharedModule } from '../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CandidateRoutingModule } from './candidate-routing.module';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { UploadDownloadComponent } from './upload-download/upload-download.component';
import { CandidateHistoryComponent } from './candidate-history/candidate-history.component';
import { ChecklistModalComponent } from './checklist-modal/checklist-modal.component';
import { ChecklistComponent } from './checklist/checklist.component';
import { ChecklistModalArrayComponent } from './checklist-modal-array/checklist-modal-array.component';
import { ChecklistPlainComponent } from './checklist-plain/checklist-plain.component';
import { CallModalComponent } from './call-modal/call-modal.component';
import { CallrecordsComponent } from './callrecords/callrecords.component';


@NgModule({
  declarations: [
    ListingComponent,
    CandidateEditComponent,
    CandidateItemComponent,
    CandidateViewComponent,
    UploadDownloadComponent,
    CandidateHistoryComponent,
    ChecklistModalComponent,
    ChecklistComponent,
    ChecklistModalArrayComponent,
    ChecklistPlainComponent,
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

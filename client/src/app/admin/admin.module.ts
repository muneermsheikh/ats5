import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { CvrefComponent } from './cvref/cvref.component';
import { AdminindexComponent } from './adminindex/adminindex.component';
import { AdminRoutingModule } from './admin-routing.module';
import { MsgModalComponent } from './msg-modal/msg-modal.component';
import { MessagesComponent } from './messages/messages.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { SelectionComponent } from './selection/selection.component';
import { SelectionModalComponent } from './selection-modal/selection-modal.component';
import { SelDecisionComponent } from './sel-decision/sel-decision.component';
import { EmploymentComponent } from './employment/employment.component';
import { ProjectsComponent } from './projects/projects.component';
import { EmploymentsComponent } from './employments/employments.component';



@NgModule({
  declarations: [
  
    CvrefComponent,
        AdminindexComponent,
        MsgModalComponent,
        MessagesComponent,
        SelectionComponent,
        SelectionModalComponent,
        SelDecisionComponent,
        EmploymentComponent,
        ProjectsComponent,
        EmploymentsComponent
        
  ],
  imports: [
    CommonModule,
    SharedModule,
    AdminRoutingModule, 
    AngularEditorModule
  ]
})
export class AdminModule { }

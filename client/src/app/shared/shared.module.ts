import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';
import { RouterModule } from '@angular/router';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {BsDropdownModule} from 'ngx-bootstrap/dropdown';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TextInputComponent } from './components/text-input/text-input.component';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgSelectModule } from '@ng-select/ng-select';
import { DateInputComponent } from './components/date-input/date-input.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ConfirmModalComponent } from './components/modal/confirm-modal/confirm-modal.component';
import { RightClickMenuComponent } from './components/right-click-menu/right-click-menu.component';
//import { NgOptionHighlightModule} from '@ng-select/ng-option-highlight';
import { CdkStepperModule } from '@angular/cdk/stepper/';
import { StepperComponent } from './components/stepper/stepper.component';
import { UploadComponent } from './components/upload/upload.component';

import { MatTableModule } from '@angular/material/table';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@NgModule({
  declarations: [
    PagingHeaderComponent,
    PagerComponent,
    TextInputComponent,
    DateInputComponent,
    ConfirmModalComponent,
    RightClickMenuComponent,
    StepperComponent,
    UploadComponent
  ],

  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    CarouselModule.forRoot(),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    RouterModule,
    TabsModule,
    ReactiveFormsModule
    , FormsModule
    , NgxGalleryModule
    , NgSelectModule
    , ModalModule.forRoot()
    , CdkStepperModule
    , MatTableModule
    , MatListModule
    , MatButtonModule
    , MatDatepickerModule
    , MatNativeDateModule
    
    
  ],
  exports: [
    PaginationModule,
    PagingHeaderComponent,
    PagerComponent,
    CarouselModule,
    TabsModule,
    ReactiveFormsModule,
    FormsModule,
    BsDropdownModule,
    TextInputComponent
    , DateInputComponent
    , NgxGalleryModule
    , NgSelectModule
    , BsDatepickerModule
    , ModalModule
    , CdkStepperModule
    , StepperComponent
    , UploadComponent
    , MatTableModule
    , MatListModule
    ,MatButtonModule
    , MatDatepickerModule
    , MatNativeDateModule
  ]
})
export class SharedModule { }

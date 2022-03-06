import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoriesComponent } from './categories/categories.component';
import { QualificationsComponent } from './qualifications/qualifications.component';
import { CategoryEditComponent } from './category-edit/category-edit.component';
import { QualificationEditComponent } from './qualification-edit/qualification-edit.component';
import { SharedModule } from '../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CategoryEditModalComponent } from './category-edit-modal/category-edit-modal.component';
import { ListingComponent } from './qualification/listing/listing.component';
import { MastersComponent } from './masters/masters.component';



@NgModule({
  declarations: [
    CategoriesComponent,
    QualificationsComponent,
    CategoryEditComponent,
    QualificationEditComponent,
    CategoryEditModalComponent,
    ListingComponent,
    MastersComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class MastersModule { }

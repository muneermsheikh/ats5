import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoriesComponent } from './categories/categories.component';
import { CategoryEditComponent } from './category-edit/category-edit.component';
import { QualificationsComponent } from './qualifications/qualifications.component';
import { QualificationEditComponent } from './qualification-edit/qualification-edit.component';
import { RouterModule } from '@angular/router';
import { CategoriesResolver } from '../resolvers/categoriesResolver';
import { CategoryResolver } from '../resolvers/categoryResolver';
import { QualificationsResolver } from '../resolvers/qualificationsResolver';

const routes = [
  {path: '', component: CategoriesComponent, 
      resolve: {cats: CategoriesResolver}},
  {path: 'categoryadd', component:CategoryEditComponent , data: {breadcrumb: {alias: 'categoryAdd'}}},
  {path: 'categoryedit/:id', component: CategoryEditComponent, resolve: {cat: CategoryResolver}, data: {breadcrumb: {alias: 'categoryEdit'}}},
  {path: 'qualifications', component: QualificationsComponent, 
      resolve: {quals: QualificationsResolver},
    data: {breadcrumb: {alias: 'qualifications'}}},
  {path: 'qualificationadd', component: QualificationEditComponent , data: {breadcrumb: {alias: 'qualificationsAdd'}}},
  {path: 'qualificationedit/:id', component: QualificationEditComponent  , data: {breadcrumb: {alias: 'qualificationEdit'}}},
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
export class MastersRoutingModule { }

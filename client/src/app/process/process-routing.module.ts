import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProcessComponent } from './process/process.component';
import { ProcessResolver } from '../resolvers/processResolver';
import { SharedModule } from '../shared/shared.module';
import { ProcessAddComponent } from './process-add/process-add.component';
import { DeploymentStatusResolver } from '../resolvers/deploymentStatusResolver';
import { RouterModule } from '@angular/router';
import { DepsComponent } from './deps/deps.component';
import { CVRefResolver } from '../resolvers/cvRefResolver';
import { ProcessGuard } from '../guards/process.guard';

const routes = [
  {path: '', component: ProcessComponent,
     resolve: {
      processes: ProcessResolver,
      statuses: DeploymentStatusResolver},
      data: {breadcrumb: {alias: 'Deployment Process'}}},
  {path: 'add', component: ProcessAddComponent , 
      canActivate: [ProcessGuard],
      data: {breadcrumb: {alias: 'Process Add'}}},
  {path: 'deploys', component: DepsComponent, 
    resolve: { cvrefdeploys: CVRefResolver,
      statuses: DeploymentStatusResolver},
    canActivate: [ProcessGuard],
    data: {breadcrumb: {alias: 'Deployment Processes'}}},
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
export class ProcessRoutingModule { }

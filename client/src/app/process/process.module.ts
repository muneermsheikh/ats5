import { NgModule } from '@angular/core';
import { ProcessComponent } from './process/process.component';
import { SharedModule } from '../shared/shared.module';
import { ProcessAddComponent } from './process-add/process-add.component';
import { ProcessRoutingModule } from './process-routing.module';
import { CommonModule } from '@angular/common';
import { DepModalComponent } from './dep-modal/dep-modal.component';
import { DepsComponent } from './deps/deps.component';



@NgModule({
  declarations: [
    ProcessComponent,
    ProcessAddComponent,
    DepModalComponent,
    DepsComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    ProcessRoutingModule
  ]
})
export class ProcessModule { }

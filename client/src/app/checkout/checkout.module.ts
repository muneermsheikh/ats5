import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckoutComponent } from './checkout.component';
import { CheckoutSuccessComponent } from './checkout-success/checkout-success.component';



@NgModule({
  declarations: [
    CheckoutComponent,
    CheckoutSuccessComponent
  ],
  imports: [
    CommonModule
  ]
})
export class CheckoutModule { }

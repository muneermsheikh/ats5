import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastRef, ToastrService } from 'ngx-toastr';
import { IOrderBriefDto, IOrderItemDto } from 'src/app/shared/models/orderBriefDto';

@Component({
  selector: 'app-employments',
  templateUrl: './employments.component.html',
  styleUrls: ['./employments.component.css']
})
export class EmploymentsComponent implements OnInit {

  title = 'Edit Employments of selected candidates';
  ordersDto: IOrderBriefDto[];
  ordersForm: FormGroup;
 
  constructor(private fb: FormBuilder, private activatedRoute: ActivatedRoute, private toastr: ToastrService ) {
    this.ordersForm = this.fb.group({
      teachers: this.fb.array([]),
    });
    
  }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => { 
      this.ordersDto = data.ordersbrief.data;
      if (this.ordersDto.length > 0) {
        this.createForm();}
      else {
        this.toastr.warning('no order records found');
      }
    })
  }
 
  createForm() {
    console.log('etered createForm');
    console.log(this.ordersDto);
    this.ordersForm = this.fb.group({
      orders: this.fb.array([]),
      /*id: [null], orderNo: Number, orderDate: '', customerName: '',
      orderItems: this.fb.array([]),
      */
    } 
    );
    
    //this.patchOrders(this.ordersDto);

    //patchForm
    this.ordersDto.forEach(o => {
      console.log(o);
      this.orders().push(this.newOrder());

      this.ordersForm.patchValue({
        id: o.id, orderNo: o.orderNo, orderDate: o.orderDate, customerName: o.customerName  
      })
      
      if (o.items != null) {
        this.ordersForm.setControl('orderItems', this.setExistingOrderItems(o.items));
      }
      
    })

  }

  
  addNewInsteadOfPatch() {
  }


  /** Teachers */
  orders(): FormArray {
    return this.ordersForm.get("orders") as FormArray
  }
 
 newOrderA(o: IOrderBriefDto): FormGroup {
    return this.fb.group({
      id: o.id,
      orderNo: o.orderNo,
      orderDate: o.orderDate,
      customerName: o.customerName,
      
    })
 }

  newOrder(): FormGroup {
    return this.fb.group({
      id: 0,
      orderNo: '',
      orderDate: '',
      customerName: '',

      orderItems: this.fb.array([])
    })
  }
 
 
  addOrder() {
    this.orders().push(this.newOrder());
  }
 
 
  removeOrder(ti) {
    this.orders().removeAt(ti);
  }
 
 
  /** batches */
 
  orderItems(ti): FormArray {
    return this.orders().at(ti).get("orderItems") as FormArray
  }
 
 
  newOrderItem(): FormGroup {
    return this.fb.group({
      name: '',
      students: this.fb.array([])
    })
  }
 
  addOrderItem(ti: number) {
    this.orderItems(ti).push(this.newOrderItem());
  }
 
  removeOrderItem(ti: number, bi: number) {
    this.orderItems(ti).removeAt(ti);
  }
 
  /** students */
 /*
  students(ti, bi): FormArray {
    return this.batches(ti).at(bi).get("students") as FormArray
  }
 
  newStudent(): FormGroup {
    return this.fb.group({
      name: '',
    })
  }
 
  addStudent(ti: number, bi: number) {
    this.students(ti, bi).push(this.newStudent());
  }
 
  removeStudent(ti: number, bi: number, si: number) {
    this.students(ti, bi).removeAt(si);
  }
 */

 
  onSubmit() {
    console.log(this.ordersForm.value);
  }


  patchOrders(_orderdata: IOrderBriefDto[]) {
    console.log('in patchOrder', _orderdata);
    _orderdata.forEach(d => {
      this.ordersForm.patchValue( {
        id: d.id, orderNo: d.orderNo, orderDate: d.orderDate, customerName: d.customerName
      });

    if (d.items != null) {
      this.ordersForm.setControl('orderItems', this.setExistingOrderItems(d.items));
    }
  })
  }
  
  
  setExistingOrderItems(items: IOrderItemDto[]): FormArray {
      const formArray = new FormArray([]);
      items.forEach(ph => {
        formArray.push(this.fb.group({
          checked: false,
          orderItemid: ph.orderItemId,
          orderId: ph.orderId,
          categoryRef: ph.categoryRef,
          categoryName: ph.categoryName,
          quantity: ph.quantity,
          status: ph.status,
        }))
      });
      return formArray;
  }

 /*
  patchValue2() {
    var orderdata = this.ordersDto;
    if (orderdata.length 
    var data = {
      orders: [
        {
          name: 'Order 1', orderItems: [
            { name: 'Batch No 1', students: [{ name: 'Ramesh' }, { name: 'Suresh' }, { name: 'Naresh' }] },
            { name: 'Batch No 2', students: [{ name: 'Vikas' }, { name: 'Harish' }, { name: 'Lokesh' }] },
          ]
        },
        {
          name: 'Orderr 2', orderItems: [
            { name: 'Batch No 3', students: [{ name: 'Ramesh 2' }, { name: 'Suresh 3' }, { name: 'Naresh 4' }] },
            { name: 'Batch No 4', students: [{ name: 'Vikas 3' }, { name: 'Harish 3' }, { name: 'Lokesh 4'  }] },
          ]
        }
      ]
    }
    this.clearFormArray();
    
    console.log('data', data);
   
    data.orders.forEach(t => {
      var order: FormGroup = this.newOrder();
      this.orders().push(order);
   
      t.orderItems.forEach(b => {
        var orderItem = this.newOrderItem();
   
        (order.get("orderItems") as FormArray).push(order)
   
        /*
        b.students.forEach(s => {
          (batch.get("students") as FormArray).push(this.newStudent())
        })
        */
      /*
      });
    });
   
    this.ordersForm.patchValue(data);
  }
  */
   
  clearFormArray() {
   
    //Angular 8 +
    this.orders().clear();
   
    //older Versions of angualar
    //while (this.teachers().length) {
    //  this.teachers().removeAt(0);
    //}
  }
}

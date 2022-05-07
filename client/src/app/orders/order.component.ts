import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ICustomerNameAndCity } from '../shared/models/customernameandcity';
import { IOrderBrief } from '../shared/models/orderBrief';
import { IOrderCity } from '../shared/models/orderCity';
import { orderParams } from '../shared/params/orderParams';
import { IProfession } from '../shared/models/profession';
import { SharedService } from '../shared/services/shared.service';
import { OrderService } from './order.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  orders: IOrderBrief[];
  oParams = new orderParams();
  totalCount: number;
  orderCities: IOrderCity[];
  professions: IProfession[];
  customers: ICustomerNameAndCity[];
  
  sortOptions = [
    {name:'By Order No Asc', value:'orderno'},
    {name:'By Order No Desc', value:'ordernodesc'},
    {name:'By City Asc', value:'city'},
    {name:'By City Desc', value:'citydesc'},
    {name:'By Profession Asc', value:'prof'},
    {name:'By Profession Desc', value:'profdesc'},
  ]

  orderStatus = [
    {name: 'Not Reviewed', value: 'NotReviewed'},
    {name: 'Reviewed and Approved', value: 'ReviewedAndApproved'},
    {name: 'Reviewed and declined', value: 'ReviewedAndDeclined'}
  ]

  constructor(private service: OrderService, private sharedService: SharedService) { }

  ngOnInit(): void {
    this.service.setOParams(this.oParams);
    this.getOrders(true);
    this.getCities();
    this.getProfessions();
    
  }

  getOrders(useCache=false) {
    this.service.getOrders(useCache).subscribe(response => {
      this.orders = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    })
  }

  getCities() {
    this.service.getOrderCities().subscribe(response => {
      this.orderCities = [{id: 9999999, cityName: 'All'}, ...response];
    })
  }

  getProfessions() {
    this.sharedService.getProfessions().subscribe(response => {
      this.professions = [{id: 99999999, name: 'All'}, ...response];
    }, error => {
      console.log(error);
    })
  }
  
  getAgents() {
    this.sharedService.getAgents().subscribe(response => {
      this.customers = [{id: 99999999, customerName: 'All', 'city': 'All' }, ...response];
    }, error => {
      console.log(error);
    })
  }
  
  onSearch() {
    const params = this.service.getOParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.service.setOParams(params);
    this.getOrders();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.oParams = new orderParams();
    this.service.setOParams(this.oParams);
    this.getOrders();
  }

  onSortSelected(sort: string) {
    this.oParams.sort = sort;
    this.getOrders();
  }
  
  onCitySelected(citySelected: string) {
    console.log('cityseleceted', citySelected);
    const prms = this.service.getOParams();
    prms.city = citySelected;
    prms.pageNumber=1;
    this.service.setOParams(prms);
    this.getOrders();
  }

  
  onProfSelected(profId: number) {
    const prms = this.service.getOParams();
    prms.categoryId = profId;
    prms.pageNumber=1;
    this.service.setOParams(prms);
    this.getOrders();

  }
  
  onCustomerSelected(agentId: number) {
    const prms = this.service.getOParams();
    prms.customerId = agentId;
    prms.pageNumber=1;
    this.service.setOParams(prms);
    this.getOrders();
  }


  onPageChanged(event: any){
    const params = this.service.getOParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.service.setOParams(params);
      this.getOrders(true);
    }
  }

 
}

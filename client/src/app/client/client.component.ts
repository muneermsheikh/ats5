import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Toast, ToastrService } from 'ngx-toastr';
import { ICustomer } from '../shared/models/customer';
import { ICustomerCity } from '../shared/models/customerCity';
import { customerParams } from '../shared/models/customerParams';
import { IIndustryType } from '../shared/models/industryType';
import { SharedService } from '../shared/services/shared.service';
import { ClientService } from './client.service';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css']
})
export class ClientComponent implements OnInit {
  
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  customers: ICustomer[];
  customerCities: ICustomerCity[];
  industryTypes: IIndustryType[];
  cParams = new customerParams();
  totalCount: number;
  custType: string;

  sortOptions = [
    {name:'By Name Asc', value:'name'},
    {name:'By Name Desc', value:'namedesc'},
    {name:'By City Asc', value:'city'},
    {name:'By City Desc', value:'citydesc'},
    {name:'By Industry Type Asc', value:'indtype'},
    {name:'By Industry Type Desc', value:'indtypedesc'}
  ]

  constructor(private service: ClientService, 
      private activatedRouter: ActivatedRoute, 
      private sharedService: SharedService,
      private toastrService: ToastrService) {
   }

  ngOnInit(): void {
    this.custType = this.activatedRouter.snapshot.paramMap.get('custType');
    this.cParams.custType=this.custType;
    this.cParams.custType = this.custType;

    this.getCustomers();
    this.getCities();
    this.getIndustryTypes();
  }

  getCustomers() {
    this.service.getCustomers(this.cParams).subscribe(response => {
      this.customers = response.data;
      this.cParams.pageNumber = response.pageIndex;
      this.cParams.pageSize = response.pageSize;
      this.totalCount = response.count;
      /* this.customerCities = this.customers.map(x => ({cityName: x.city}))
        .filter((x, y, arr) => arr.findIndex(t => t.cityName == x.cityName) === y);
      this.customerCities = [{cityName: 'All'}, ...this.customerCities];
      */
    }, error => {
      console.log(error);
    })
  }

  getCities() {
    this.service.getCustomerCities().subscribe(response => {
      this.customerCities = [{cityName: 'All'}, ...response];
      //console.log('CUSTOMER CITIES', this.customerCities);
    })
    /*
    this.customerCities = [{cityName: 'All'}, ...this.customerCities];
    */
  }

  getIndustryTypes() {
    //this.industryTypes = [{id: 1, industryName:'real estate'}, {industryName: 'power generation'}, {industryName: 'power distribution'}];
    return this.sharedService.getIndustries().subscribe(response => {
      this.industryTypes = response;
    }, error => {
      this.toastrService.error('failed to get industry types from api');
    })
  }

  onCitySelected(citySelected: string) {
    this.cParams.customerCityName = citySelected;
    this.cParams.pageNumber = 1;
    this.getCustomers();
  }

  onSortSelected(sort: string) {
    this.cParams.sort = sort;
    this.getCustomers();
  }

  onIndTypeSelected(typ: number) {
    this.cParams.customerIndustryId = typ;
    this.getCustomers();
  }


  onSearch() {
    console.log('onsearch', this.searchTerm.nativeElement.value);
    this.cParams.search = this.searchTerm.nativeElement.value;
    this.cParams.pageNumber = 1;
    this.getCustomers();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.cParams = new customerParams();
    this.getCustomers();
  }

  onPageChanged(event: any){
    //if (this.customerParams.pageNumber !== event) {
        this.cParams.pageNumber = event;
        this.getCustomers();
    //}
  }
}

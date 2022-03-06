import { Component, createPlatform, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { ToastrService } from 'ngx-toastr';
import { stringify } from 'querystring';
import { ICustomer } from 'src/app/shared/models/customer';
import { IIndustryType } from 'src/app/shared/models/industryType';
import { IProfession } from 'src/app/shared/models/profession';
import { IUser } from 'src/app/shared/models/user';
import { SharedService } from 'src/app/shared/services/shared.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ClientService } from '../client.service';

@Component({
  selector: 'app-client-detail',
  templateUrl: './client-detail.component.html',
  styleUrls: ['./client-detail.component.css']
})
export class ClientDetailComponent implements OnInit  {
  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  activeTab: TabDirective;
  
  routerId: string;
  routerIdValue: number;
  customer: ICustomer;
  professions: IProfession[]=[];
  industries: IIndustryType[]=[];
  user: IUser;
  form: FormGroup;
  isAddMode: boolean;

  constructor(private service: ClientService
    , private activatedRouter: ActivatedRoute
    , private bcService: BreadcrumbService
    , private router: Router
    , private fb: FormBuilder
    , private toastr: ToastrService
    , private sharedService: SharedService
    ) {
    this.bcService.set('@customerDetail',' ');
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.routerId = activatedRouter.snapshot.paramMap.get('id');
    if (this.routerId !== '') this.routerIdValue=+this.routerId;
    this.isAddMode = this.routerIdValue===0;
    
   }

  ngOnInit(): void {
    this.activatedRouter.data.subscribe(data => { this.customer = data.customer;})
    this.getProfessions();
    this.getIndustryTypes();
    //this.getCustomer();
    this.createForm();
    
    //this.activatedRouter.queryParams.subscribe(params => {
      //params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    //})

  }

  createForm() {
    this.form = this.fb.group({
      id: [null],
      customerType: '',
      customerName: '',
      knownAs: ['', Validators.required],
      add: '',
      add2: '', 
      city: ['', Validators.required],
      pin: '', 
      district: '',
      state: '', country: '',
      email: [null, 
        [Validators.required, Validators
        .pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]
      ],
      website: '',
      phone: '',
      phone2: '', 
      logoUrl: '',
      customerStatus: 0,
      createdOn: '',
      introduction: '',
      customerIndustries: this.fb.array([]),
      customerOfficials: this.fb.array([]),
      agencySpecialties: this.fb.array([])
      
    } 
    );

    if (!this.isAddMode) this.patchCustomer();
  }

  getProfessions() {
    return this.sharedService.getProfessions().subscribe(response => {
      this.professions = response;
    })
  }
  getIndustryTypes() {
    return this.sharedService.getIndustries().subscribe(response => {
      this.industries = response;
    })
  }



  getCustomer() {
    this.service.getCustomer(this.routerIdValue)
      .subscribe(response => {
      this.customer = response;
      console.log('in getCustomer', this.customer);
      this.bcService.set('@customerDetail', this.customer.knownAs);
    }, error => {
      console.log(error);
    })
  }

  patchCustomer() {
        this.form.patchValue(this.customer);
        if(this.customer.customerOfficials != null) {
          for(const p of this.customer.customerOfficials) {
            this.customerOfficials.push(new FormControl(p));
          }
        }
        if(this.customer.customerType==='customer') {
          if(this.customer.customerIndustries !=null){
            for(const ph of this.customer.customerIndustries) { 
              this.customerIndustries.push(new FormControl(ph)); }
          }
        } else {
          if(this.customer.agencySpecialties !=null){
            for(const ph of this.customer.agencySpecialties) { 
              this.agencySpecialties.push(new FormControl(ph)); }
          }
        }
    } 

    get customerOfficials() : FormArray {
      return this.form.get("customerOfficials") as FormArray
    }

    newCustomerOfficial(): FormGroup {
      return this.fb.group({
        id: 0, customerId: 0, logInCredential: true, appUserId: 0, gender: '', 
        title: '', officialName: '', designation: '', divn: '', phoneNo: '',
        mobile: '', email:  [null, 
          [Validators.required, Validators
          .pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]
        ],
        imageUrl: '', isValid: true
      })
    }
    addCustomerOfficial() {
      this.customerOfficials.push(this.newCustomerOfficial());
    }  
    removeCustomerOfficial(i:number) {
      this.customerOfficials.removeAt(i);
      this.customerOfficials.markAsDirty();
      this.customerOfficials.markAsTouched();
    }

    get customerIndustries() : FormArray {
      return this.form.get("customerIndustries") as FormArray
    }

    newCustomerIndustry(): FormGroup {
      return this.fb.group({
        id: 0, customerId: 0, industryId: 0
      })
    }
    addCustomerIndustry() {
      this.customerIndustries.push(this.newCustomerIndustry());
    }  
    removeCustomerIndustry(i:number) {
      this.customerIndustries.removeAt(i);
      this.customerIndustries.markAsDirty();
      this.customerIndustries.markAsTouched();
    }

    get agencySpecialties() : FormArray {
      return this.form.get("agencySpecialties") as FormArray
    }

    newAgencySpecialty(): FormGroup {
      return this.fb.group({
        id: 0, 
        customerId: [0, Validators.required], 
        industryId: 0, 
        name: ['', Validators.required]
      })
    }
    addAGencySpecialty() {
      this.agencySpecialties.push(this.newAgencySpecialty());
    }  
    removeAgencySpecialty(i:number) {
      this.agencySpecialties.removeAt(i);
      this.agencySpecialties.markAsDirty();
      this.agencySpecialties.markAsTouched();
    }

  onSubmit() {
    if (this.routerIdValue ===0) {
      this.CreateCustomer();
    } else {
      this.UpdateCustomer();
    }
  }

  private CreateCustomer() {
    this.service.createCustomer(this.form.value).subscribe(response => {
    }, error => {
      console.log(error);
      //this.errors = error.errors;
    })
  }

  private UpdateCustomer() {
    /*
    let formData = new FormData();
    formData=this.form.value;
    this.selectedFiles.forEach((f) => {
      console.log(f);
      if (f !== null) {
        formData.append('userFormFiles', f);
    }})
    */
    this.service.updateCustomer(this.form.value).subscribe(response => {
      this.toastr.success('customer updated');
      this.router.navigateByUrl('/client');

    }, error => {
      console.log(error);
    })
  }

  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }


  onTabActivated(data: TabDirective) {
    this.activeTab = data;
  }



}

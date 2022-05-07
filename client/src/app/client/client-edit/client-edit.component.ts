import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { IAgencySpecialty } from 'src/app/shared/models/agencySpecialty';
import { ICustomer } from 'src/app/shared/models/customer';
import { ICustomerIndustry } from 'src/app/shared/models/customerIndustry';
import { ICustomerOfficial } from 'src/app/shared/models/customerOfficial';
import { IIndustryType } from 'src/app/shared/models/industryType';
import { IProfession } from 'src/app/shared/models/profession';
import { IUser } from 'src/app/shared/models/user';
import { SharedService } from 'src/app/shared/services/shared.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ClientService } from '../client.service';

@Component({
  selector: 'app-client-edit',
  templateUrl: './client-edit.component.html',
  styleUrls: ['./client-edit.component.css']
})
export class ClientEditComponent implements OnInit {

  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  activeTab: TabDirective;
  @ViewChild('editForm') editForm: NgForm;
  routeId: string;
  member: ICustomer;
  user: IUser;
  
  form: FormGroup;
  selectedCategoryIds: number[];
  categories: IProfession[]=[];
  industries: IIndustryType[]=[];

  isAddMode: boolean;
  loading = false;
  submitted = false;

  errors: string[]=[];

  constructor(private service: ClientService, private bcService: BreadcrumbService, 
      private activatedRoute: ActivatedRoute, private router: Router, private sharedService: SharedService,
      private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder) {
    this.bcService.set('@customerDetail',' ');
    this.routeId = this.activatedRoute.snapshot.params['id'];
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
   }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => { this.member = data.customer;})
      this.isAddMode = !this.routeId;
      this.getIndustries();
      this.getCategories();
      this.createForm();
  }

  
  getMember(id: number) {
    this.service.getCustomer(id).subscribe( 
      response => {
        this.member = response;
        this.patchCustomer(this.member);
      }
    )
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

      if (!this.isAddMode && !this.member) {
        this.toastr.error('failed to retrieve customer');
      } 
      if (!this.isAddMode && this.member) this.patchCustomer(this.member);
  }

    patchCustomer(cv: ICustomer) {
      this.form.patchValue( {
        id: cv.id, customerType: cv.customerType, customerName: cv.customerName, 
        knownAs: cv.knownAs, add: cv.add, add2: cv.add2, city: cv.city,
        pin: cv.pin, district: cv.district, state: cv.state, country: cv.country,
        email: cv.email, website: cv.website, phone: cv.phone, phone2: cv.phone2,
        logoUrl: cv.logoUrl, customerStatus: cv.customerStatus, createdOn: cv.createdOn,
        introduction: cv.introduction, 
      });

      if (cv.customerOfficials != null) {
        this.form.setControl('customerOfficials', this.setExistingCustomerOfficials(cv.customerOfficials));
      }

      if (cv.customerIndustries != null) {
        this.form.setControl('customerIndustries', this.setExistingCustomerIndustries(cv.customerIndustries));
      }
      
      if (cv.agencySpecialties != null) {
        this.form.setControl('agencySpecialties', this.setExistingAgencySpecialties(cv.agencySpecialties));
      }
      
    }

    
    setExistingCustomerOfficials(off: ICustomerOfficial[]): FormArray {
        const formArray = new FormArray([]);
        off.forEach(ph => {
          formArray.push(this.fb.group({
            id: ph.id,
            customerId: ph.customerId,
            logInCredential: ph.logInCredential,
            appUserId: ph.appUserId,
            gender: ph.gender,
            title: ph.title,
            officialName: ph.officialName,
            designation: ph.designation,
            divn: ph.divn,
            phoneNo: ph.phoneNo,
            mobile: ph.mobile,
            email: ph.email,
            imageUrl: ph.imageUrl,
            isValid: ph.isValid
          }))
        });
        return formArray;
    }

    setExistingCustomerIndustries(inds: ICustomerIndustry[]): FormArray {
        const formArray = new FormArray([]);
        inds.forEach(q => {
          formArray.push(this.fb.group({
            id: q.id,
            customerId: q.customerId,
            industryId: q.industryId,
            name: q.name
          }))
        });
        return formArray;
    }

    setExistingAgencySpecialties(specs: IAgencySpecialty[]): FormArray {
      const formArray = new FormArray([]);
      specs.forEach(p => {
        formArray.push(this.fb.group({
          id: p.id,
          customerId: p.customerId,
          professionId: p.professionId,
          name: p.name,
        }))
      });
      return formArray;
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
          id: 0, customerId: 0, industryId: 0, name: ''
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

// userProfessions
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
      addAgencySpecialty() {
        this.agencySpecialties.push(this.newAgencySpecialty());
      }  
      removeAgencySpecialty(i:number) {
        this.agencySpecialties.removeAt(i);
        this.agencySpecialties.markAsDirty();
        this.agencySpecialties.markAsTouched();
      }

  // various gets from APis

      getCategories() {
        this.sharedService.getProfessions().subscribe(response => {
          this.categories = response;
        }, error => {
          console.log(error);
        })
      }

      getIndustries() {
        this.sharedService.getIndustries().subscribe(response => {
          this.industries = response;
        }, error => {
          this.toastr.error('failed to get the industries', error);
        })
      }

      onSubmit() {
        if (+this.routeId ===0) {
          this.CreateCustomer();
        } else {
          this.UpdateCustomer();
        }
      }

      private CreateCustomer() {
        this.service.createCustomer(this.form.value).subscribe(response => {
        }, error => {
          console.log(error);
          this.errors = error.errors;
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

      showReview() {
        this.router.navigateByUrl('/client/review/' + this.routeId);
      }
      selectTab(tabId: number) {
        this.memberTabs.tabs[tabId].active = true;
      }


      onTabActivated(data: TabDirective) {
        this.activeTab = data;
      }

  

}
 
import { DatePipe, formatDate } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormArray, FormBuilder, FormControl, FormGroup, NgForm, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { ToastrService } from 'ngx-toastr';
import { EMPTY, of, timer } from 'rxjs';
import { map, switchMap, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { IAddress } from 'src/app/shared/models/address';
import { IEmployee } from 'src/app/shared/models/employee';
import { IEmployeeAddress } from 'src/app/shared/models/employeeAddress';
import { IEmployeeHRSkill } from 'src/app/shared/models/employeeHRSkill';
import { IEmployeeOtherSkill } from 'src/app/shared/models/employeeOtherSkill';
import { IEmployeePhone } from 'src/app/shared/models/employeePhone';
import { IEmployeeQualification } from 'src/app/shared/models/employeeQualification';
import { IIndustryType } from 'src/app/shared/models/industryType';
import { IProfession } from 'src/app/shared/models/profession';
import { IQualification } from 'src/app/shared/models/qualification';
import { ISkillData } from 'src/app/shared/models/skillData';
import { IUser } from 'src/app/shared/models/user';
import { ConfirmService } from 'src/app/shared/services/confirm.service';
import { SharedService } from 'src/app/shared/services/shared.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { EmployeeService } from '../employee.service';

@Component({
  selector: 'app-employee-detail',
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.css']
})
export class EmployeeDetailComponent implements OnInit {
  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  activeTab: TabDirective;
  @ViewChild('editForm') editForm: NgForm;

  routeId: string;

  employee: IEmployee;
  user: IUser;
  
  form: FormGroup;
  selectedCategoryIds: number[];
  categories: IProfession[];
  industries: IIndustryType[];
  masterQualifications: IQualification[];
  skillData: ISkillData[];
  
  isAddMode: boolean;
  loading = false;
  submitted = false;

  eerrors: string[]=[];

  //bsValueDOB = new Date();
  //bsValueDOJ = new Date();
  bsValueDOB = new Date().toISOString();
  bsValueDOJ = new Date().toISOString();
  bsModalRef: BsModalRef;
  bsRangeValue: Date[];

  todayISOString : string = new Date().toISOString();

  constructor(private service: EmployeeService, private bcService: BreadcrumbService, private confirmService: ConfirmService,
      private activatedRoute: ActivatedRoute, private router: Router, private sharedService: SharedService,
      private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder) {
    this.bcService.set('@candidateDetail',' ');
    this.routeId = this.activatedRoute.snapshot.params['id'];
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
   }

  ngOnInit(): void {
      //this.routeId = this.activatedRoute.snapshot.params['id'];
      this.isAddMode = !this.routeId;
      this.getQualifications();
      this.getCategories();
      this.getIndustries();
      this.getSkillData();
      this.createForm();

      if (!this.isAddMode) {
        this.getEmployee(+this.routeId);
    }
  
  }

  get confirmPassword() { return this.form.get('confirmpPassword'); }
  get password() { return this.form.get('password'); }
  
    getEmployee(empid: number) {
      this.service.getEmployee(empid).subscribe( 
        response => {
          this.employee = response;
          this.editEmployee(this.employee);
        }
      )
    }

  
    createForm() {
      this.form = this.fb.group({
        appUserId: 0, id: 0,  gender: ['M', Validators.required], firstName:  ['', Validators.required],
        secondName:  ['', Validators.required], familyName:  ['', Validators.required], knownAs: ['', Validators.required],
        position: ['', Validators.required],  
        dateOfBirth: ['', Validators.required],  dateOfJoining: ['', Validators.required],
        placeOfBirth:'', aadharNo: ['', Validators.required],
        nationality: ['Indian', Validators.required],  department: ['', Validators.required], 
        email: [null, [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')], [this.validateEmailNotTaken()]],
        password: ['', Validators.required],
        confirmPassword: ['', [Validators.required, this.matchValues('password')]],
        status: [100, Validators.required],  remarks: ['', Validators.required], 
        employeeQualifications: this.fb.array([]),
        hrSkills: this.fb.array([]),
        otherSkills: this.fb.array([]),
        employeePhones: this.fb.array([]),
        employeeAddresses: this.fb.array([])
      } //, {validator: MustMatch('password', 'confirmPassword')}
      );

      if (!this.isAddMode) this.loadEmployee();
    }

    matchValues(matchTo: string): ValidatorFn {
      return (control: AbstractControl) => {
        return control?.value === control?.parent?.controls[matchTo].value 
          ? null : {isMatching: true}
      }
    }
    
    editEmployee(emp: IEmployee) {
      this.form.patchValue( {
        appUserId: emp.appUserId, id: emp.id,  gender: emp.gender, secondName: emp.secondName, familyName: emp.familyName, 
        knownAs: emp.knownAs, position: emp.position, placeOfBirth: emp.placeOfBirth, aadharNo: emp.aadharNo, 
        nationality: emp.nationality, department: emp.department, email: emp.email, userName: emp.userName,
        dOB: emp.dateOfBirth, dOJ: emp.dateOfJoining
      });

      if (emp.employeeAddresses != null) this.form.setControl('employeeAddresses', this.setExistingAddresses(emp.employeeAddresses));
      if (emp.employeePhones != null) this.form.setControl('employeePhones', this.setExistingPhones(emp.employeePhones));
      if (emp.employeeQualifications != null) this.form.setControl('employeeQualifications', this.setExistingQ(emp.employeeQualifications));
      if (emp.hrSkills != null) this.form.setControl('hrSkills', this.setExistingHRSkills(emp.hrSkills));
      if (emp.otherSkills != null) this.form.setControl('otherSkills', this.setExistingOtherSkills(emp.otherSkills));

    }

    setExistingAddresses(adds: IEmployeeAddress[]): FormArray {
      const formArray = new FormArray([]);
      adds.forEach(p => {
        formArray.push(this.fb.group({
          employeeId: p.employeeId,
          id: p.id,
          addressType: p.addressType,
          add: p.add,
          streetAdd: p.streetAdd,
          city: p.city,
          pin: p.pin,
          district: p.district,
          state: p.state,
          country: p.country,
          isMain: p.isMain
        }))
      });
      return formArray;
    }  

    setExistingPhones(empphones: IEmployeePhone[]): FormArray {
        const formArray = new FormArray([]);
        empphones.forEach(ph => {
          formArray.push(this.fb.group({
            id: ph.id,
            employeeId: ph.employeeId,
            mobileNo: ph.mobileNo,
            isMain: ph.isMain,
            remarks: ph.remarks,
            isValid: ph.isValid
          }))
        });
        return formArray;
    }

    setExistingQ(userQ: IEmployeeQualification[]): FormArray {
        const formArray = new FormArray([]);
        userQ.forEach(q => {
          formArray.push(this.fb.group({
            id: q.id,
            employeeId: q.employeeId,
            qualificationId: q.qualificationId,
            qualification: q.qualification,
            isMain: q.isMain
          }))
        });
        return formArray;
    }

    setExistingHRSkills(hrskills: IEmployeeHRSkill[]): FormArray {
      const formArray = new FormArray([]);
      hrskills.forEach(p => {
        formArray.push(this.fb.group({
            id: p.id, employeeId: p.employeeId, categoryId: p.categoryId, industryId: p.industryId, 
            categoryName: p.categoryName, skillLevel: p.skillLevel, skillLevelName: p.skillLevelName
        }))
      });
      return formArray;
    }  

    setExistingOtherSkills(otherskills: IEmployeeOtherSkill[]): FormArray {
      const formArray = new FormArray([]);
      otherskills.forEach(p => {
        formArray.push(this.fb.group({
            id: p.id, employeeId: p.employeeId, skillDataId: p.skillDataId, skillLevel: p.skillLevel, isMain: p.isMain
        }))
      });
      return formArray;
    }  

    //employee addresses
    get employeeAddresses(): FormArray {
      return this.form.get("employeeAddresses") as FormArray
    }

    newEmployeeAddress(): FormGroup {
      return this.fb.group({
        addressType: 'R',
        add: '',
        streetAdd: '',
        city: '',
        pin: '',
        state: '',
        district: '', 
        country: '',
        isMain: '',
        employeeId: 0
      })
    }
    addEmployeeAddress() {
      this.employeeAddresses.push(this.newEmployeeAddress());
    }
    removeEmployeeAddress(i: number) {
      this.employeeAddresses.removeAt(i);
      this.employeeAddresses.markAsDirty();
      this.employeeAddresses.markAsTouched();
    }

//userPhones
      get employeePhones() : FormArray {
        return this.form.get("employeePhones") as FormArray;
      }

      newPhone(): FormGroup {
        return this.fb.group({
          id: 0,
          employeeId: 0,
          mobileNo: ['', Validators.required],
          isMain: false,
          isValid: true,
          remarks: ''
        })
      }
      addPhone() {
        this.employeePhones.push(this.newPhone());
      }

      removePhone(i:number) {
        this.employeePhones.removeAt(i);
        this.employeePhones.markAsDirty();
        this.employeePhones.markAsTouched();
      }

  //employeeQualifications
      get employeeQualifications() : FormArray {
        return this.form.get("employeeQualifications") as FormArray;
      }

      newEmployeeQualification(): FormGroup {
        return this.fb.group({
          //id: '',
          employeeId: 0,
          qualificationId: 0,
          qualification: '',
          isMain: false
        })
      }

      addQualification() {
        this.employeeQualifications.push(this.newEmployeeQualification());
      }

      removeEmployeeQualification(i:number) {
        this.employeeQualifications.removeAt(i);
        this.employeeQualifications.markAsDirty();
        this.employeeQualifications.markAsTouched();
      }

      //hr skills
      get hrSkills() : FormArray {
        return this.form.get("hrSkills") as FormArray
      }
      newHRSkill(): FormGroup {
        return this.fb.group({
          id: 0, employeeId: 0, categoryId: 0, industryId: 0, skillLevel: 0
        })
      }

      addHRSkill() {
        this.hrSkills.push(this.newHRSkill());
      }

      removeHRSkill(i:number) {
        this.hrSkills.removeAt(i);
        this.hrSkills.markAsDirty();
        this.hrSkills.markAsTouched();
      }

    //other skills
      get otherSkills() : FormArray {
        return this.form.get("otherSkills") as FormArray
      }
      newOtherSkill(): FormGroup {
        return this.fb.group({
          id: 0, employeeId: 0, skillDataId: 0, skillLevel: 0, isMain: false
        })
      }
  
      addOtherSkill() {
        this.otherSkills.push(this.newOtherSkill());
      }
  
      removeOtherSkill(i:number) {
        this.otherSkills.removeAt(i);
        this.otherSkills.markAsDirty();
        this.otherSkills.markAsTouched();
      }
    
//validations  
      validateEmailNotTaken(): AsyncValidatorFn {
        return control => {
          //return timer(500).pipe(
          return timer(10).pipe(
            switchMap(() => {
              if (!control.value) {
                return of(null);
              }
              return this.accountService.checkEmailExists(control.value).pipe(
                map(res => {
                  return res ? {emailExists: true} : null;
                })
              );
            })
          )
        }
      }

      validateAadharNotTaken(): AsyncValidatorFn {
        return control => {
          return timer(10).pipe(
            switchMap(() => {
              if (!control.value) {
                return of(null);
              }
              return this.accountService.checkAadharExists(control.value).pipe(
                map(res => {
                  if (res !== null) this.toastr.warning('that passport number is taken by ' + res);
                  return res ? {ppExists: true} : null;
                })
              );
            })
          )
        }
      }

  // various gets


    //qualifications
      getQualifications() {
        return this.sharedService.getQualifications().subscribe(response => {
          this.masterQualifications = response;
          console.log('getQualifications', this.masterQualifications);
        }, error => {
          console.log(error);
        })
      }

      getCategories() {
        return this.sharedService.getProfessions().subscribe(response => {
          this.categories = response;

        }, error => {
          console.log(error);
        })
      }
      getIndustries() {
        return this.sharedService.getIndustries().subscribe(response => {
          this.industries = response;
        }, error => {
          console.log(error);
        })
      }

      getSkillData() {
        return this.sharedService.getSkillData().subscribe(response => {
          this.skillData = response;
        }, error => {
          console.log(error);
        })
      }

      loadEmployee() {
        this.service.getEmployee(+this.routeId).subscribe(
          response => {
              this.employee = response;  
              this.form.patchValue(this.employee);
              if(this.employee.employeeAddresses != null) {for(const a of this.employee.employeeAddresses) {this.employeeAddresses.push(new FormControl(a));}}
              if(this.employee.employeePhones != null) {for(const p of this.employee.employeePhones) {this.employeePhones.push(new FormControl(p));}}
              if (this.employee.employeeQualifications != null) {for(const q of this.employee.employeeQualifications) { this.employeeQualifications.push(new FormControl(q)); }}
              if (this.employee.hrSkills != null) {for(const p of this.employee.hrSkills) { this.hrSkills.push(new FormControl(p)); }}
              if (this.employee.otherSkills != null) {for(const e of this.employee.otherSkills) { this.otherSkills.push(new FormControl(e)); }}
            }
        )} 

      onSubmit() {
        if (this.isAddMode) {
          this.CreateEmployee();
        } else {
          this.UpdateEmployee();
        }
      }

      routeToList() {
        if (this.form.dirty) {
            this.confirmService.confirm('Confirm move to another page', 
            'This form has data that is not saved; moving to another page will not commit the save. ' + 
            'Do you want to move to another page without saving the data?')
            .subscribe(result => {
              if (result) {
                this.router.navigateByUrl('/employee');
              }
            })
        } else {
          this.router.navigateByUrl('/employee');
        }
      }
      

      dojSelected(dt: any) {
        this.dojSelected = dt;
        console.log('dojselected',this.dojSelected);
        return this.dojSelected;
      }
      private CreateEmployee() {
        this.form.controls['dateOfJoining'].setValue(this.dojSelected);

        this.service.register(this.form.value).subscribe(response => {
          this.toastr.success('employee created');
          this.router.navigateByUrl('/employee');
        }, error => {
          console.log(error);
          this.eerrors = error.errors;
        })
      }

      private UpdateEmployee() {
        this.service.UpdateEmployee(this.form.value).subscribe(response => {
          this.toastr.success('employee updated');
          this.router.navigateByUrl('/employee');

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

      clearCategories() {
        this.form.get('userProfessions').patchValue([]);
      }


}

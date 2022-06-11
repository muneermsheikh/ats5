import { Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { ToastrService } from 'ngx-toastr';
import { of, timer } from 'rxjs';
import { first, map, switchMap } from 'rxjs/operators';
import { CommonService } from 'src/app/services/common.service';
import { ICandidate } from 'src/app/shared/models/candidate';
import { IProfession } from 'src/app/shared/models/profession';
import { IQualification } from 'src/app/shared/models/qualification';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  activeTab: TabDirective;

  candidate: ICandidate;
  professions: IProfession[]=[{"id":1, "name": "profession"}];
  qualifications: IQualification[];
  registerForm: FormGroup;
  errors: string[]=[];

  id: string;
  isAddMode: boolean;
  loading = false;
  submitted = false;
  bsValueDate = new Date();

  fileToUpload: File | null = null;
  selectedProfession='';
  events: Event[] = [];

  constructor(private fb: FormBuilder, private accountService: AccountService, 
      private router: Router, private activatedRoute: ActivatedRoute, 
      private service: CommonService, private toastrService: ToastrService ) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];
    this.isAddMode = !this.id;

    this.getProfessions();
    this.getQualifications();
    this.createRegisterForm();

    if (!this.isAddMode) this.getCVBId(+this.id);
    
  }
  // convenience getter for easy access to form fields
    get f(): { [key: string]: AbstractControl } { return this.registerForm.controls; };

  getCVBId(cvid: number) {
    return this.accountService.getCandidate(cvid)
      .pipe(first())
      .subscribe(x => this.registerForm.patchValue(x));
  }

  getProfessions() {
    this.service.getProfessions().subscribe(response => {
      this.professions = response;
    }, error => {
      console.log(error);
    })
  }

  getQualifications() {
    this.service.getQualifications().subscribe(response => {
      this.qualifications = response;
    }, error => {
      console.log(error);
    })
  }
  
  createRegisterForm() {
    this.registerForm = this.fb.group({
      id: 0,
      userType: 'candidate',
      applicationNo: 0,
      gender: ['M', [Validators.required, Validators.maxLength(1)]],
      title: ['M', Validators.required],
      firstName: [null, [Validators.required, Validators.maxLength(25)]],
      secondName: '',
      familyName: '',
      knownAs: ['', [Validators.required, Validators.maxLength(10)]],
      referredBy: 0,
      dOB: ['1988-12-11', Validators.required],
      placeOfBirth: 'Mumbai',
      aadharNo: '123498981211',
      ppNo: 'A38065',
      ecnr: false,
      //city: [null, Validators.required],
      //pin: [null, [Validators.required, Validators.maxLength(6), Validators.minLength(6)]],
      nationality: ['Indian', Validators.required],
      email: ['', 
        [Validators.required, Validators
        .pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')],
        [this.validateEmailNotTaken()]
      ],
      address: [],
      password: ['Pa$$w0rd', this.isAddMode ? Validators.required : Validators.nullValidator],
      confirmPassword: ['Pa$$w0rd', this.isAddMode ? Validators.required : Validators.nullValidator],
      companyId:0,
      introduction: 'itnerests',
      interests: 'introduction',
      notificationDesired: [false, Validators.required],
      userPhones: this.fb.array([]),
      userQualifications: this.fb.array([]), 
      userProfessions: this.fb.array([]),
      userPassports: this.fb.array([]),
      entityAddresses: this.fb.array([]),
      userExperiences: this.fb.array([]),
      userAttachments: this.fb.array([])
    } //, {validator: MustMatch('password', 'confirmPassword')}
    );
  }

      get userPhones() : FormArray {
        return this.registerForm.get("userPhones") as FormArray
      }
      newPhone(): FormGroup {
        return this.fb.group({
          mobileNo: ['9867638000', Validators.required],
          isMain: false,
          remarks: ''
        })
      }
      addPhone() {
        this.userPhones.push(this.newPhone());
      }
      removeUserPhone(i:number) {
        this.userPhones.removeAt(i);
      }

      get userQualifications() : FormArray {
        return this.registerForm.get("userQualifications") as FormArray
      }
      newUserQualification(): FormGroup {
        return this.fb.group({
          candidateId: 0,
          qualificationId: 0,
          qualification: '',
          isMain: false
        })
      }
      addQualification() {
        this.userQualifications.push(this.newUserQualification());
      }
      removeUserQualification(i:number) {
        this.userQualifications.removeAt(i);
      }

      get userProfessions() : FormArray {
        return this.registerForm.get("userProfessions") as FormArray
      }
      newUserProfession(): FormGroup {
        return this.fb.group({
          candidateId: 0,
          profession: '',
          categoryId: [12, Validators.required],
          industryId: 0,
          isMain: true
        })
      }
      addUserProfession() {
        this.userProfessions.push(this.newUserProfession());
      }  
      removeUserProfession(i:number) {
        this.userProfessions.removeAt(i);
      }

      get userPassports() : FormArray {
        return this.registerForm.get("userPassports") as FormArray
      }
      newUserPP(): FormGroup {
        //if (this.userPassports.length > 0) return;
        return this.fb.group({
          candidateId: 0,
          passportNo: ['A38492992', Validators.required, this.validatePPNotTaken()],
          ecnr: false,
          nationality: 'Indian',
          issuedOn: '2015-12-12',
          issuedAt: 'Mumbai',
          validity: '2025-12-11',
          isValid: true
        })
      }
      addUserPP() {
        this.userPassports.push(this.newUserPP());
      }
      removeUserPP(i:number) {
        this.userPassports.removeAt(i);
      }

      get userAttachments() : FormArray {
        return this.registerForm.get("userAttachments") as FormArray
      }
          newAttachment(): FormGroup {
        return this.fb.group({
          candidateId: 0,
          appUserId: 0,
          fileName: ''
        })
      }
      addUserAttachment() {
        this.userAttachments.push(this.newAttachment());
      }
      removeAttachment(i:number) {
        this.userAttachments.removeAt(i);
      }
    
      get entityAddresses(): FormArray {
        return this.registerForm.get("entityAddresses") as FormArray
      }

      newEntityAddress(): FormGroup {
        return this.fb.group({
          addressType: 'R',
          add: '12/56 BDD Chawls',
          streetAdd: 'Worli',
          city: 'Mumbai',
          pin: '400018',
          state: 'Maharashtra',
          district: 'Mumbai', 
          country: 'India',
          isMain: false,
          candidateId: 0
        })
      }
      addEntityAddress() {
        this.entityAddresses.push(this.newEntityAddress());
      }
      removeEntityAddress(i: number) {
        this.entityAddresses.removeAt(i);
      }

      get userExperiences() : FormArray {
        return this.registerForm.get("userExperiences") as FormArray
      }
      newUserExperience(): FormGroup {
        return this.fb.group({
          candidateId: 0,
          srNo: 0,
          company: ['', Validators.required],
          positionId: 0,
          position: ['', Validators.required],
          salaryCurrency: '',
          monthlySalaryDrawn: 0,
          workedFrom: '',
          workedUpto: ''
        })
      }
      addUserExperience() {
        this.userExperiences.push(this.newUserExperience());
      }  
      removeUserExperience(i:number) {
        this.userExperiences.removeAt(i);
      }
  
      selectTab(tabId: number) {
        this.memberTabs.tabs[tabId].active = true;
      }
    
      onTabActivated(data: TabDirective) {
        this.activeTab = data;
      }
  
      onSubmit() {
        this.submitted=true;
        //reset alerts on submit
        this.toastrService.clear();
        //console.log(JSON.stringify(this.registerForm.value, null, 2));

        if (this.registerForm.invalid) {
          this.toastrService.warning('form is invalid');
          return;
        }

        this.loading=true;
        if (this.isAddMode) {
          this.CreateCV();
        } else {
          this.UpdateCV();
        }
      }

      private CreateCV() {
        this.accountService.register(this.registerForm.value).subscribe(response => {
          this.loading = false;
          this.toastrService.success('profile registered');
        }, error => {
          console.log(error);
          this.errors = error.errors;
          this.toastrService.error('failed to register the profile');
        })
      }

      private UpdateCV() {

      }

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

      validatePPNotTaken(): AsyncValidatorFn {
        return control => {
          return timer(10).pipe(
            switchMap(() => {
              if (!control.value) {
                return of(null);
              }
              return this.accountService.checkPPExists(control.value).pipe(
                map(res => {
                  if (res !== null) this.toastrService.warning('that passport number is taken by ' + res);
                  return res ? {ppExists: true} : null;
                })
              );
            })
          )
        }
      }

      handleFileInput(files: FileList) {
        this.fileToUpload = files.item(0);
      }

      getValues() {
        console.log('ngx dropdown selected', this.selectedProfession);
      } 

  /*
  onChange($event) {
    this.events.push({ name: '(change)', value: $event });
  }

  onFocus($event: Event) {
      this.events.push({ name: '(focus)', value: $event });
  }

  onBlur($event: Event) {
      this.events.push({ name: '(blur)', value: $event });
  }

  onOpen() {
      this.events.push({ name: '(open)', value: null });
  }

  onClose() {
      this.events.push({ name: '(close)', value: null });
  }

  onAdd($event) {
      this.events.push({ name: '(add)', value: $event });
  }

  onRemove($event) {
      this.events.push({ name: '(remove)', value: $event });
  }

  onClear() {
      this.events.push({ name: '(clear)', value: null });
  }

  onScrollToEnd($event) {
      this.events.push({ name: '(scrollToEnd)', value: $event });
  }

  onSearch($event) {
      this.events.push({ name: '(search)', value: $event });
  }
  */
}
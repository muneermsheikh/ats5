import { Component, OnInit, ViewChild } from '@angular/core';
import { AsyncValidatorFn, FormArray, FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { ToastrService } from 'ngx-toastr';
import { of, timer } from 'rxjs';
import { map, switchMap, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { ICandidate } from 'src/app/shared/models/candidate';
import { IEntityAddress } from 'src/app/shared/models/entityAddress';
import { IProfession } from 'src/app/shared/models/profession';
import { IQualification } from 'src/app/shared/models/qualification';
import { IUser } from 'src/app/shared/models/user';
import { IUserAttachment } from 'src/app/shared/models/userAttachment';
import { IUserExp } from 'src/app/shared/models/userExp';
import { IUserPassport } from 'src/app/shared/models/userPassport';
import { IUserPhone } from 'src/app/shared/models/userPhone';
import { IUserProfession } from 'src/app/shared/models/userProfession';
import { IUserQualification } from 'src/app/shared/models/userQualification';
import { SharedService } from 'src/app/shared/services/shared.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { CandidateService } from '../candidate.service';

@Component({
  selector: 'app-candidate-edit',
  templateUrl: './candidate-edit.component.html',
  styleUrls: ['./candidate-edit.component.css']
})
export class CandidateEditComponent implements OnInit {

  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  activeTab: TabDirective;
  @ViewChild('editForm') editForm: NgForm;
  routeId: string;
  member: ICandidate;
  user: IUser;
  
  form: FormGroup;
  selectedCategoryIds: number[];
  categories: IProfession[];
  qualifications: IQualification[];
  fileToUpload: File | null = null;

  events: Event[] = [];

  isAddMode: boolean;
  loading = false;
  submitted = false;

  errors: string[]=[];

  bsValue = new Date();
  bsRangeValue: Date[];
  maxDate = new Date();
  minDate = new Date();

  //file uploads
  uploadProgress = 0;
  selectedFiles: File[];
  uploading = false;
  fileErrorMsg = '';

  constructor(private service: CandidateService, private bcService: BreadcrumbService, 
      private activatedRoute: ActivatedRoute, private router: Router, private sharedService: SharedService,
      private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder) {
    this.bcService.set('@candidateDetail',' ');
    this.routeId = this.activatedRoute.snapshot.params['id'];
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 20);  //10 years later
    this.minDate.setFullYear(this.minDate.getFullYear() + 20);
    this.bsRangeValue = [this.bsValue, this.maxDate];
   }

  ngOnInit(): void {
      //this.routeId = this.activatedRoute.snapshot.params['id'];
      this.isAddMode = !this.routeId;
      this.getQualifications();
      this.getCategories();
      this.createForm();

      if (!this.isAddMode) {
        this.getMember(+this.routeId);
    }
  
  }

  getMember(cvid: number) {
    this.service.getCandidate(cvid).subscribe( 
      response => {
        this.member = response;
        this.editCandidate(this.member);
      }
    )
  }
  
    createForm() {
      this.form = this.fb.group({
        id: [null],
        userType: 'candidate',
        applicationNo: Number,
        gender: ['', [Validators.required, Validators.maxLength(1)]],
        firstName: [null, [Validators.required, Validators.maxLength(25)]],
        secondName: [null],
        familyName: [null],
        knownAs: [null, Validators.required],
        referredBy: Number,
        dOB: [null, Validators.required],
        placeOfBirth: [null],
        aadharNo: [null],
        passportNo: [''],
        ecnr: [false],
        city: [null, Validators.required],
        pin: [null, [Validators.required, Validators.maxLength(6), Validators.minLength(6)]],
        nationality: ['Indian', Validators.required],
        email: [null, 
          [Validators.required, Validators
          .pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')],
          [this.validateEmailNotTaken()]
        ],
        address: [],
        password: [null, this.isAddMode ? Validators.required : Validators.nullValidator],
        confirmPassword: ['', this.isAddMode ? Validators.required : Validators.nullValidator],
        companyId: [null],
        introduction: [null],
        interests: [null],
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

      if (!this.isAddMode) this.loadMember();
    }

    editCandidate(cv: ICandidate) {
      
      this.form.patchValue( {
        id: cv.id, userType: cv.userType , applicationNo: cv.applicationNo, gender: cv.gender, firstName: cv.firstName,
        secondName: cv.secondName, familyName: cv.familyName, knownAs: cv.knownAs, referredBy: cv.referredBy, dOB: cv.dOB,
        placeOfBirth: cv.placeOfBirth, aadharNo: cv.aadharNo, ppNo: cv.ppNo, ecnr: cv.ecnr, city: cv.city, pin: cv.pin,
        nationality: cv.nationality, email: cv.email, companyId: cv.companyId, introduction: cv.introduction, 
        interests: cv.interests, notificationDesired: cv.notificationDesired
      });

      if (cv.userPhones !== null) this.form.setControl('userPhones', this.setExistingPhones(cv.userPhones));
      if (cv.userQualifications !== null) this.form.setControl('userQualifications', this.setExistingQ(cv.userQualifications));
      if (cv.userProfessions !== null) this.form.setControl('userProfessions', this.setExistingProfs(cv.userProfessions));
      if (cv.userPassports !== null) this.form.setControl('userPassports', this.setExistingPPs(cv.userPassports));
      if (cv.entityAddresses !== null) this.form.setControl('entityAddresses', this.setExistingAddresses(cv.entityAddresses));
      if (cv.userExperiences !== null) this.form.setControl('userExperiences', this.setExistingExps(cv.userExperiences));
      if (cv.userAttachments !== null) this.form.setControl('userAttachments', this.setExistingAttachments(cv.userAttachments));
    }
    
    setExistingPhones(userphones: IUserPhone[]): FormArray {
        const formArray = new FormArray([]);
        userphones.forEach(ph => {
          formArray.push(this.fb.group({
            id: ph.id,
            candidateId: ph.candidateId,
            mobileNo: ph.mobileNo,
            isMain: ph.isMain,
            remarks: ph.remarks,
            isValid: ph.isValid
          }))
        });
        return formArray;
    }

    setExistingQ(userQ: IUserQualification[]): FormArray {
        const formArray = new FormArray([]);
        userQ.forEach(q => {
          formArray.push(this.fb.group({
            id: q.id,
            candidateId: q.candidateId,
            qualificationId: q.qualificationId,
            qualification: q.qualification,
            isMain: q.isMain
          }))
        });
        return formArray;
    }

    setExistingProfs(userProfs: IUserProfession[]): FormArray {
      const formArray = new FormArray([]);
      userProfs.forEach(p => {
        console.log('setExistingProfs', p);
        formArray.push(this.fb.group({
          id: p.id,
          candidateId: p.candidateId,
          categoryId: p.categoryId,
          profession: p.profession,
          isMain: p.isMain
        }))
      });
      return formArray;
    }  

    setExistingPPs(userPPs: IUserPassport[]): FormArray {
      const formArray = new FormArray([]);
      userPPs.forEach(p => {
        formArray.push(this.fb.group({
          candidateId: p.candidateId,
          ecnr: p.ecnr,
          id: p.id,
          isValid: p.isValid,
          passportNo: p.passportNo,
          nationality: p.nationality,
          issuedOn: p.issuedOn, 
          validity: p.validity,
        }))
      });
      return formArray;
    }  
 
    setExistingAddresses(adds: IEntityAddress[]): FormArray {
      const formArray = new FormArray([]);
      adds.forEach(p => {
        formArray.push(this.fb.group({
          candidateId: p.candidateId,
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

    setExistingExps(userExps: IUserExp[]): FormArray {
      const formArray = new FormArray([]);
      userExps.forEach(p => {
        formArray.push(this.fb.group({
          id: p.id,
          candidateId: p.candidateId,
          employer: p.employer,
          position: p.position,
          positionId: p.positionId,
          salaryCurrency: p.salaryCurrency,
          monthlySalaryDrawn: p.monthlySalaryDrawn,
          workedFrom: p.workedFrom,
          workedUpto: p.workedUpto
        }))
      });
      return formArray;
    }  

    setExistingAttachments(userAttachs: IUserAttachment[]): FormArray {
      const formArray = new FormArray([]);
      userAttachs.forEach(p => {
        formArray.push(this.fb.group({
          id: p.id,
          candidateId: p.candidateId,
          appUserId: p.appUserId,
          fileName: p.url
       }))
      });
      return formArray;
    }  

//userPhones
      get userPhones() : FormArray {
        return this.form.get("userPhones") as FormArray
      }
      newPhone(): FormGroup {
        return this.fb.group({
          mobileNo: ['', Validators.required],
          isMain: false,
          isValid: true,
          remarks: ''
        })
      }
      addPhone() {
        this.userPhones.push(this.newPhone());
      }
      removeUserPhone(i:number) {
        this.userPhones.removeAt(i);
        this.userPhones.markAsDirty();
        this.userPhones.markAsTouched();
      }

  //userQualifications
      get userQualifications() : FormArray {
        return this.form.get("userQualifications") as FormArray
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
        this.userQualifications.markAsDirty();
        this.userQualifications.markAsTouched();
      }

// userProfessions
      get userProfessions() : FormArray {
        return this.form.get("userProfessions") as FormArray
      }
      newUserProfession(): FormGroup {
        return this.fb.group({
          id: 0,
          candidateId: 0,
          profession: '',
          categoryId: [0, Validators.required],
          industryId: 0,
          isMain: false
        })
      }
      addUserProfession() {
        this.userProfessions.push(this.newUserProfession());
      }  
      removeUserProfession(i:number) {
        this.userProfessions.removeAt(i);
        this.userProfessions.markAsDirty();
        this.userProfessions.markAsTouched();
      }

    //uwerPassports
      get userPassports() : FormArray {
        return this.form.get("userPassports") as FormArray
      }

      newUserPP(): FormGroup {
        return this.fb.group({
          candidateId: 0,
          passportNo: ['', Validators.required, this.validatePPNotTaken()],
          ecnr: false,
          nationality: 'Indian',
          issuedOn: [''],
          validity: ['', Validators.required],
          isValid: true
        })
      }
      addUserPP() {
        this.userPassports.push(this.newUserPP());
      }
      removeUserPP(i:number) {
        this.userPassports.removeAt(i);
        this.userPassports.markAsDirty();
        this.userPassports.markAsTouched();
      }

  //user attachments
      get userAttachments() : FormArray {
        return this.form.get("userAttachments") as FormArray
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
        this.userAttachments.markAsDirty();
        this.userAttachments.markAsTouched();
      }

  //entity addresses
      get entityAddresses(): FormArray {
        return this.form.get("entityAddresses") as FormArray
      }

      newEntityAddress(): FormGroup {
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
          candidateId: 0
        })
      }
      addEntityAddress() {
        this.entityAddresses.push(this.newEntityAddress());
      }
      removeEntityAddress(i: number) {
        this.entityAddresses.removeAt(i);
        this.entityAddresses.markAsDirty();
        this.entityAddresses.markAsTouched();
      }

  //user exp
      get userExperiences() : FormArray {
        return this.form.get("userExperiences") as FormArray
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
          workedFrom: ['', Validators.required],
          workedUpto: ['', Validators.required]
        })
      }
      addUserExperience() {
        this.userExperiences.push(this.newUserExperience());
      }  
      removeUserExperience(i:number) {
        this.userExperiences.removeAt(i);
        this.userExperiences.markAsDirty();
        this.userExperiences.markAsTouched();
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

      validatePPNotTaken(): AsyncValidatorFn {
        return control => {
          return timer(10).pipe(
            switchMap(() => {
              if (!control.value) {
                return of(null);
              }
              return this.accountService.checkPPExists(control.value).pipe(
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
          this.qualifications = response;
        }, error => {
          console.log(error);
        })
      }

      getCategories() {
        this.sharedService.getProfessions().subscribe(response => {
          this.categories = response;
        }, error => {
          console.log(error);
        })
      }

      loadMember() {
        this.service.getCandidate(+this.routeId).subscribe(
          response => {
              this.member = response;  
              //console.log('load member', this.member);
              this.form.patchValue(this.member);
              if(this.member.userPassports !== null) {
                for(const p of this.member.userPassports) {
                  this.userPassports.push(new FormControl(p));
                }
              }
              if(this.member.userPhones !=null){for(const ph of this.member.userPhones) { this.userPhones.push(new FormControl(ph)); }}
              if (this.member.userQualifications !== null) {for(const q of this.member.userQualifications) { this.userQualifications.push(new FormControl(q)); }}
              if (this.member.userProfessions !== null) {for(const p of this.member.userProfessions) { this.userProfessions.push(new FormControl(p)); }}
              if (this.member.userPassports !== null) {for(const p of this.member.userPassports) { this.userPassports.push(new FormControl(p)); }}
              if (this.member.userExperiences !== null) {for(const e of this.member.userExperiences) { this.userExperiences.push(new FormControl(e)); }}
              if (this.member.userAttachments !== null) {for(const a of this.member.userAttachments) { this.userAttachments.push(new FormControl(a)); }}
            }
        )} 

      onSubmit() {
        if (+this.routeId ===0) {
          this.CreateCV();
        } else {
          this.UpdateCandidate();
        }
      }

      private CreateCV() {
        this.service.register(this.form.value).subscribe(response => {
        }, error => {
          console.log(error);
          this.errors = error.errors;
        })
      }

     private UpdateCandidate() {
        /*
        let formData = new FormData();
        formData=this.form.value;
        this.selectedFiles.forEach((f) => {
          console.log(f);
          if (f !== null) {
            formData.append('userFormFiles', f);
        }})
        */
        this.service.UpdateCandidate(this.form.value).subscribe(response => {
          this.toastr.success('candidate updated');
          this.router.navigateByUrl('/candidate');

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

    
      TransactionHistory() {
        this.router.navigateByUrl('/candidatehistory/{{member.id}}');
      }

      //file upload
      handleFileInput(files: FileList) {
        this.fileToUpload = files.item(0);
      }

      chooseFile(files: FileList) {
        this.selectedFiles = [];
        this.fileErrorMsg = '';
        this.uploadProgress = 0;
        if (files.length === 0) {
          return;
        }
        for (let i = 0; i < files.length; i++) {
          this.selectedFiles.push(files[i]);
        }
      }

     
      /*
  upload() {
    if (!this.selectedFiles || this.selectedFiles.length === 0) {
      this.fileErrorMsg = 'Please choose a file.';
      return;
    }

    const formData = new FormData();
    this.selectedFiles.forEach((f) => formData.append('certificates', f));

    const req = new HttpRequest(
      'POST',
      `api/students/${this.studentId}/certificates`,
      formData,
      {
        reportProgress: true,
      }
    );
    this.uploading = true;
    this.httpClient
      .request<CertificateSubmissionResult[]>(req)
      .pipe(
        finalize(() => {
          this.uploading = false;
          this.selectedFiles = null;
        })
      )
      .subscribe(
        (event) => {
          if (event.type === HttpEventType.UploadProgress) {
            this.uploadProgress = Math.round(
              (100 * event.loaded) / event.total
            );
          } else if (event instanceof HttpResponse) {
            this.submissionResults = event.body as CertificateSubmissionResult[];
          }
        },
        (error) => {
          // Here, you can either customize the way you want to catch the errors
          throw error; // or rethrow the error if you have a global error handler
        }
      );
  }
*/
  humanFileSize(bytes: number): string {
    if (Math.abs(bytes) < 1024) {
      return bytes + ' B';
    }
    const units = ['kB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    let u = -1;
    do {
      bytes /= 1024;
      u++;
    } while (Math.abs(bytes) >= 1024 && u < units.length - 1);
    return bytes.toFixed(1) + ' ' + units[u];
  }
}
 
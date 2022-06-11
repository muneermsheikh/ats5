import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as EventEmitter from 'events';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { CandidateHistoryService } from 'src/app/candidate/candidate-history.service';
import { IContactResult } from 'src/app/shared/models/contactResult';
import { IEmployeeIdAndKnownAs } from 'src/app/shared/models/employeeIdAndKnownAs';
import { IUser } from 'src/app/shared/models/user';
import { IUserHistory } from 'src/app/shared/models/userHistory';
import { IUserHistoryItem } from 'src/app/shared/models/userHistoryItem';
import { userHistoryParams } from 'src/app/shared/params/userHistoryParams';
import { ConfirmService } from 'src/app/shared/services/confirm.service';
import { UserTaskService } from 'src/app/userTask/user-task.service';

@Component({
  selector: 'app-user-history',
  templateUrl: './user-history.component.html',
  styleUrls: ['./user-history.component.css']
})
export class UserHistoryComponent implements OnInit {
  @Input() historyItems: IUserHistoryItem[];
  @Input() employees: IEmployeeIdAndKnownAs[];
  @Input() contactResults: IContactResult[];
  @Input() contactResultData: IContactResult[]=[];
  @Input() user: IUser;
  @Input() userPhone: string;

  @Input() emitItems = new EventEmitter();

  form: FormGroup;
  events: Event[] = [];

  isAddMode: boolean;
  loading = false;
  submitted = false;

  errors: string[]=[];
  bsModalRef: BsModalRef;
  
  bsValue = new Date();
  bsRangeValue: Date[];
  maxDate = new Date();
  minDate = new Date();
  //cvParams: candidateParams;
  histParams: userHistoryParams;

  candidateFromDb: boolean=false;
  err: string;

  constructor(private service: CandidateHistoryService, 
    private toastr: ToastrService, 
    private confirmService: ConfirmService,
    //private activatedRoute: ActivatedRoute,
    //private router: Router, 
    private modalService: BsModalService,
    private userTaskService: UserTaskService,
    private fb: FormBuilder) {
    //this.bcService.set('@candidateHistory',' ');
    //this.routeId = this.activatedRoute.snapshot.params['id'];
    //this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    //this.maxDate.setFullYear(this.maxDate.getFullYear() - 1);  //1 years later
    //this.minDate.setFullYear(this.minDate.getFullYear() + 1);
    //this.bsRangeValue = [this.bsValue, this.maxDate];
 }

  ngOnInit(): void {
     /* this.activatedRoute.data.subscribe(data => { 
      this.member= data.history;
      this.contactResults = data.results;
      this.employees = data.employees
    })
    */
    
    this.histParams = new userHistoryParams();
    
    this.createForm();
    this.contactResultData = this.contactResults.filter(x => x.personType==='prospective') ;
    this.form.setControl('userHistoryItems', this.setTransactions(this.historyItems));
  }

  createForm() {
    this.form = this.fb.group({
      /* id: 0,
      personName: '', 
      personId: 0,
      personType: '', 
      applicationNo: 0, 
      phoneNo: '',
      emailId: '',
      createdOn: '',
      */
      userHistoryItems: this.fb.array([]),
    } 
    );
   
    //this.patchMember(this.member);
  }

    patchMember(m: IUserHistory) {
      console.log('entered patchMember');
      this.form.patchValue( {
        id: m.id, 
        personType: m.personType,
        personId: m.personId,
        personName: m.personName,
        phoneNo: m.phoneNo,
        emailId: m.emailId,
        applicationNo: m.applicationNo,
        createdOn: m.createdOn
      });

      //if(m.userHistoryItems != null) {
      
        this.form.setControl('userHistoryItems', this.setTransactions(m.userHistoryItems));
      //}
    }

    setTransactions(trans: IUserHistoryItem[]): FormArray {
      console.log('setting up childrens');
      const formArray = new FormArray([]);
      trans.forEach(ph => {
        formArray.push(this.fb.group({
          id: ph.id,
          phoneNo: ph.phoneNo,
          personId: ph.personId,
          subject: ph.subject,
          personType: ph.personType,
          categoryRef: ph.categoryRef,
          dateOfContact: ph.dateOfContact,
          loggedInUserName: ph.loggedInUserName,
          contactResult: ph.contactResultId,
          gistOfDiscussions: ph.gistOfDiscussions
        }))
      });
      return formArray;
  }

  get userHistoryItems() : FormArray {
    return this.form.get("userHistoryItems") as FormArray
  }
  
  newUserHistoryItem(): FormGroup {
    const todayISOString  = new Date().toISOString();
    return this.fb.group({
      id: 0,
      phoneNo: this.userPhone,
      personType: '',
      personId: 0,
      subject: '',
      categoryRef: '',
      dateOfContact: [todayISOString, Validators.required],
      loggedInUserName: [this.user.username, Validators.required],
      contactResult: [0, Validators.required],
      gistOfDiscussions: ''
    })
  }

  subjectDblClick(i: number) {
    if (i===0) return;
    var arrayControl = this.form.get('userHistoryItems') as FormArray;
    const sub = arrayControl.at(i-1).get('subject').value;
    arrayControl.at(i).get('subject').setValue(sub);
  }

  categoryDblClick(i: number) {
    if (i===0) return;
    //get previous value
    var arrayControl = this.form.get('userHistoryItems') as FormArray;
    const categoryref = arrayControl.at(i-1).get('categoryRef').value;
    arrayControl.at(i).get('categoryRef').setValue(categoryref);
  }

  addUserHistoryItem() {
    this.userHistoryItems.push(this.newUserHistoryItem());
  }

  removeUserHistoryItem(i:number) {
    this.userHistoryItems.removeAt(i);
    this.userHistoryItems.markAsDirty();
    this.userHistoryItems.markAsTouched();
  }

  update() {

    this.emitItems.emit(this.form.value);

    /*
    var model: IUserHistory = this.form.value;
    model.applicationNo=this.member.applicationNo;
    model.createdOn=this.member.createdOn;
    model.emailId=this.member.emailId;
    model.id=this.member.id;
    model.personId=this.member.personId;
    model.personName=this.member.personName;
    model.personType=this.member.personType;
    model.phoneNo=this.member.phoneNo;
    
    var hItems: IUserHistoryItem[]=this.form.value;

    this.service.updateCandidateHistoryItems(hItems).subscribe(() => {
      this.toastr.success('candidate history updated');
      //this.router.navigateByUrl('');

    }, error => {
      this.toastr.error(error);
    }) 
    */
  }

  displayPersonTasksModal(event: any) {

    //var historyItemId = event.controls['id'].value;
    /*
    var sPerson = this.member.personType;
    var iPersonId = event.controls['personId'].value;

    let taskDto: IApplicationTask;
    
    const dt= new Date().toISOString();
    const dt1 = new Date(dt);
    var loggedinEmployeeId = this.user.loggedInEmployeeId;     //why is this.user.loggedInEmployeeId undefined?
    if(loggedinEmployeeId === 0 || loggedinEmployeeId === undefined) {
      return null;
    }
    
    let oParams = new userTaskParams;
    oParams.personType = sPerson;
    oParams.candidateId = iPersonId;
    this.userTaskService.setOParams(oParams);

    this.userTaskService.getTasks(false, true).subscribe(response => {
      var objs = response; //IApplicationTask[]
        
        const config = {
          class: 'modal-dialog-centered modal-lg',
          initialState: {
            objs, 
            emps: this.employees,
            title: 'create task for ' + this.member.personName,
            user:this.user
          }
        }
        
        console.log('config', config);

        this.bsModalRef = this.modalService.show(TasksModalComponent, config);
      }, error => {
        console.log(error);
      });
    */
  }


    routeChange() {
      if (this.form.dirty) {
          this.confirmService.confirm('Confirm move to another page', 
          'This form has data that is not saved; moving to another page will not commit the save. ' + 
          'Do you want to move to another page without saving the data?')
          .subscribe(result => {
            if (result) {
              //this.router.navigateByUrl('');
            }
          })
      } else {
        //this.router.navigateByUrl('');
      }
    }
    
/*
    getCandidate() {
      this.err='';
        this.histParams.createNewIfNull=this.candidateFromDb;
        if (this.histParams.mobileNo==='' && this.histParams.emailId ==='' && this.histParams.applicationNo === 0 ) {
          this.toastr.warning('no inputs');
          return;
        }
  
        return this.service.getHistory(this.histParams)
            .subscribe(response => {
              this.member = response.body;
              if(this.member===null || this.member ===undefined) {
                this.toastr.info('no such history transaction on record');
                return;
              }
              this.contactResultData = this.contactResults.filter(x=>x.personType === this.member?.personType);
              this.patchMember(this.member);
        }, error => {
          this.err = error;
          this.toastr.error(error);
        })
 
    }
*/
    clearMember() {
      //this.member=null;
      this.userHistoryItems.clear();
    }
}

import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { ICategoryRefDto } from 'src/app/shared/dtos/categoryRefDto';
import { IMessageDto } from 'src/app/shared/dtos/messageDto';
import { IUserHistoryReturnDto } from 'src/app/shared/dtos/userHistoryReturnDto';
import { IApplicationTask } from 'src/app/shared/models/applicationTask';
import { IContactResult } from 'src/app/shared/models/contactResult';
import { IEmployeeIdAndKnownAs } from 'src/app/shared/models/employeeIdAndKnownAs';
import { IMessage, message } from 'src/app/shared/models/message';
import { IUser } from 'src/app/shared/models/user';
import { IUserHistory } from 'src/app/shared/models/userHistory';
import { IUserHistoryDto } from 'src/app/shared/models/userHistoryDto';
import { IUserHistoryItem } from 'src/app/shared/models/userHistoryItem';
import { categoryRefParam } from 'src/app/shared/params/categoryRefParam';
import { userHistoryParams } from 'src/app/shared/params/userHistoryParams';
import { userTaskParams } from 'src/app/shared/params/userTaskParams';
import { ConfirmService } from 'src/app/shared/services/confirm.service';
import { TasksModalComponent } from 'src/app/userTask/tasks-modal/tasks-modal.component';
import { UserTaskService } from 'src/app/userTask/user-task.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { CandidateHistoryService } from '../candidate-history.service';

@Component({
  selector: 'app-candidate-history',
  templateUrl: './candidate-history.component.html',
  styleUrls: ['./candidate-history.component.css']
})
export class CandidateHistoryComponent implements OnInit {
  routeId: string;
  member: IUserHistory;
  
  user: IUser;
  
  form: FormGroup;
  events: Event[] = [];

  isAddMode: boolean;
  loading = false;
  submitted = false;

  errors: string[]=[];
  bsModalRef: BsModalRef;
  
  employees: IEmployeeIdAndKnownAs[];
  contactResults: IContactResult[];
  contactResultData: IContactResult[]=[];
  bsValue = new Date();
  bsRangeValue: Date[];
  maxDate = new Date();
  minDate = new Date();
  //cvParams: candidateParams;
  histParams: userHistoryParams;

  candidateFromDb: boolean=false;
  err: string;

  msg: IMessage=new message();

  catRefParam=new categoryRefParam();

  categoryRefTextOnHover: string;

  constructor(private service: CandidateHistoryService, 
    private bcService: BreadcrumbService, 
    private activatedRoute: ActivatedRoute, 
    private accountService: AccountService, 
    private toastr: ToastrService, 
    private confirmService: ConfirmService,
    private router: Router, 
    private modalService: BsModalService,
    private userTaskService: UserTaskService,
    private fb: FormBuilder) {
    this.bcService.set('@candidateHistory',' ');
    //this.routeId = this.activatedRoute.snapshot.params['id'];
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    //this.maxDate.setFullYear(this.maxDate.getFullYear() - 1);  //1 years later
    //this.minDate.setFullYear(this.minDate.getFullYear() + 1);
    //this.bsRangeValue = [this.bsValue, this.maxDate];
 }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => { 
      this.member= data.history?.body;
      this.contactResults = data.results;
      this.employees = data.employees
    })
    //this.activatedRoute.data.subscribe(data => { this.contactResults = data.results;})
    //this.activatedRoute.data.subscribe(data => { this.employees = data.employees;})
   
    this.histParams = new userHistoryParams();
    
    this.createForm();
    //this.contactResultData = this.contactResults.filter(x => x.personType===this.member?.personType);
    this.contactResultData = this.contactResults;
    if(this.member != null) this.patchMember(this.member);

    this.msg.content="contents one two three four";
    this.msg.recipientId=12;
    this.msg.senderId=10;
    this.msg.senderUserName='munir';
    this.msg.recipientUserName='recipient user name';

  }

  createForm() {
    this.form = this.fb.group({
      id: 0,
      personName: '', 
      personId: 0,
      personType: '', 
      applicationNo: 0, 
      phoneNo: '',
      emailId: '',
      createdOn: '',
      userHistoryItems: this.fb.array([]),
    } 
    );

   
    //this.patchMember(this.member);
  }

    patchMember(m: IUserHistory) {
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

      if(m.userHistoryItems != null) this.form.setControl('userHistoryItems', this.setTransactions(m.userHistoryItems));
    }

    setTransactions(trans: IUserHistoryItem[]): FormArray {
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
          contactResultId: ph.contactResultId,
          composeEmaiMessage: ph.composeEmailMessage,
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
        phoneNo: this.member.phoneNo,
        personType: '',
        personId: 0,
        subject: '',
        categoryRef: '',
        hoverText:'',
        dateOfContact: [todayISOString, Validators.required],
        loggedInUserName: [this.user.displayName, Validators.required],
        contactResultId: [0, Validators.required],
        composeEmailMessage: false,
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
      this.service.updateCandidateHistory(this.form.value).subscribe((response: IUserHistoryReturnDto) => {
        if(response.succeeded) {
          if (response.messageCount === 0) {
            this.toastr.success('Call Record transaction updated - no email message composed'); 
          } else {
            this.toastr.success('Call Record transaction updated, ' + response.messageCount + ' messages were composed.  The messages are available in messages, draft folder');
          }
        } else {
          this.toastr.warning('failed to update the history object');
        }
      }, error => {
        this.toastr.error(error);
      }) 
    }

    displayPersonTasksModal(event: any) {

      //var historyItemId = event.controls['id'].value;
      var sPerson = this.member.personType;
      //var iPersonId = event.controls['personId'].value;


      let taskDto: IApplicationTask;
      
      const dt= new Date().toISOString();
      const dt1 = new Date(dt);
      var loggedinEmployeeId = this.user.loggedInEmployeeId;     //why is this.user.loggedInEmployeeId undefined?
      if(loggedinEmployeeId === 0 || loggedinEmployeeId === undefined) {
        return null;
      }
      
      let oParams = new userTaskParams;
      oParams.personType = sPerson;
      oParams.candidateId = this.form.get('personId').value; // iPersonId;
      this.userTaskService.setOParams(oParams);
      console.log('oParams in displayPersonTask', oParams);
      this.userTaskService.getTasks(false, true).subscribe(response => {
        var objs = response; //IApplicationTask[]
          
          const config = {
            class: 'modal-dialog-centered modal-lg',
            initialState: {
              objs, 
              emps: this.employees,
              title: this.member.personName,
              user:this.user
            }
          }
          
          console.log('config', config);

          this.bsModalRef = this.modalService.show(TasksModalComponent, config);
        }, error => {
          console.log(error);
        });
    }

    composeEmailOfConsent() {
      
    }

    routeChange() {
      if (this.form.dirty) {
          this.confirmService.confirm('Confirm move to another page', 
          'This form has data that is not saved; moving to another page will not commit the save. ' + 
          'Do you want to move to another page without saving the data?')
          .subscribe(result => {
            if (result) {
              this.router.navigateByUrl('');
            }
          })
      } else {
        this.router.navigateByUrl('');
      }
    }
    
    goBackTo() {
      switch(this.member.personType) {
        case 'prospective':
          this.router.navigateByUrl('/prospectives');
          break;
        case 'candidate':
          this.member.personType === 'candidate';
          break;
        default:
          break;
      }
    }

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

    clearMember() {
      this.member=null;
      this.userHistoryItems.clear();
    }
//hover text



  public getCategoryRef(categoryref: string): ICategoryRefDto {

    if(categoryref==='') return null;
                
    var i = categoryref.indexOf("-");
    if (i== -1) return null;
    var orderno = categoryref.substring(0,i);
    var srno = categoryref.substring(i+1);
    if (orderno==='' || srno==='') return null;
    this.catRefParam.orderNo=+orderno;
    this.catRefParam.srNo=+srno;
    this.service.setParams(this.catRefParam);
    this.service.getCategoryRefDetailFromParam(true).subscribe(response => {
      this.categoryRefTextOnHover=response.categoryRef + response.companyName;
    })
  }
}
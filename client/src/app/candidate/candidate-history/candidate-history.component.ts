import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { ApplicationTask, IApplicationTask } from 'src/app/shared/models/applicationTask';
import { ApplicationTaskInBrief, IApplicationTaskInBrief } from 'src/app/shared/models/applicationTaskInBrief';
import { IContactResult } from 'src/app/shared/models/contactResult';
import { IEmployeeIdAndKnownAs } from 'src/app/shared/models/employeeIdAndKnownAs';
import { IUser } from 'src/app/shared/models/user';
import { IUserHistory } from 'src/app/shared/models/userHistory';
import { IUserHistoryItem } from 'src/app/shared/models/userHistoryItem';
import { userHistoryParams } from 'src/app/shared/params/userHistoryParams';
import { ConfirmService } from 'src/app/shared/services/confirm.service';
import { TaskReminderModalComponent } from 'src/app/userTask/task-reminder-modal/task-reminder-modal.component';
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
  bsValue = new Date();
  bsRangeValue: Date[];
  maxDate = new Date();
  minDate = new Date();
  //cvParams: candidateParams;
  histParams: userHistoryParams;

  candidateFromDb: boolean=false;
  err: string;

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
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 1);  //1 years later
    this.minDate.setFullYear(this.minDate.getFullYear() + 1);
    this.bsRangeValue = [this.bsValue, this.maxDate];
 }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => { this.member= data.history;})
    this.activatedRoute.data.subscribe(data => { this.contactResults = data.results;})
    this.activatedRoute.data.subscribe(data => { this.employees = data.employees;})
    this.histParams = new userHistoryParams();
    
    this.createForm();
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

    if(this.member != null) this.patchMember(this.member);
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
          subject: ph.subject,
          categoryRef: ph.categoryRef,
          dateOfContact: ph.dateOfContact,
          loggedInUserName: ph.loggedInUserName,
          contactResult: ph.contactResult,
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
    this.service.updateCandidateHistory(this.form.value).subscribe(() => {
      this.toastr.success('candidate history updated');
      this.router.navigateByUrl('');

    }, error => {
      this.toastr.error(error);
    }) 
  }

  displayModalReminder(i: number) {
    let taskDto: IApplicationTaskInBrief;
    const config = {
      class: 'modal-dialog-centered modal-lg',
      initialState: {
        obj: this.getReminderObject(i),
        emps: this.employees
      }
    }
    this.bsModalRef = this.modalService.show(TaskReminderModalComponent, config);
    
    this.bsModalRef.content.updatedRemidner.subscribe(values => {
      taskDto = values;
      if(taskDto ===undefined) {
        this.toastr.warning('failed to retrieve the emitted object');
        return;
      }
      let appTask = new ApplicationTask();
      appTask.applicationNo=taskDto.applicationNo;
      appTask.assignedToId=taskDto.assignedToId;
      appTask.candidateId=taskDto.candidateId;
      appTask.completeBy=taskDto.completeBy;
      appTask.taskDate=taskDto.taskDate;
      appTask.taskDescription=taskDto.taskDescription;
      appTask.taskOwnerId=taskDto.taskOwnerId;
      appTask.taskStatus=taskDto.taskStatus;
      appTask.taskTypeId=taskDto.taskTypeId;

      this.toastr.info('calling userTaskService to create task');
      this.userTaskService.createTaskFromAppTask(appTask).subscribe(response => {
        if (response) this.toastr.success('task created');
        }, error => {
          this.toastr.warning('failed to create the task');
        })
    }, error => {
      this.toastr.warning('failed to retrieve the task', error);
    })
  }

    private getReminderObject(i: number): IApplicationTaskInBrief {
    const dt= new Date().toISOString();
    const dt1 = new Date(dt);
    //var arrayControl = this.form.get('userHistoryItems') as FormArray;
    //const disc = arrayControl.at(i).get('gistOfDiscussions').value;
    const user: IUser = JSON.parse(localStorage.getItem('user'));
    if(user.loggedinEmployeeId === 0) {
      this.toastr.error('cannot get logged in user employee id');
      return null;
    }
    let obj = new ApplicationTaskInBrief();
    obj.id=0, obj.taskTypeId = 0, obj.taskTypeName ='',
    obj.taskDate = new Date(); obj.taskOwnerId=0,
    obj.taskOwnerName='', obj.assignedToId=2,
    obj.assignedToName='', obj.completeBy=new Date(dt1.setDate(dt1.getDate()+ 7)),
    obj.taskStatus='not started'
    obj.taskDescription='reminder for ';
    obj.taskOwnerName=user.username;
    obj.taskOwnerId=user.loggedinEmployeeId;

    return obj;
    };

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
              console.log('get candidate got', this.member);
              this.patchMember(this.member);
        }, error => {
          this.err = error;
          this.toastr.error(error);
        })
 
  }
}

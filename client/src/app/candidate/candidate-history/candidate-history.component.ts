import { Component, OnInit } from '@angular/core';
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

  constructor(private service: CandidateHistoryService, 
    private bcService: BreadcrumbService, 
    private activatedRoute: ActivatedRoute, 
    private accountService: AccountService, 
    private toastr: ToastrService, 
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
    this.createForm();
  }

  createForm() {
    this.form = this.fb.group({
      id: 0,
      partyName: '', 
      candidateId: 0,
      aadharNo: '', 
      applicationNo: 0, 
      createdOn: '',
      userHistoryItems: this.fb.array([]),
    } 
    );

     this.patchMember(this.member);
  }

    patchMember(m: IUserHistory) {
      this.form.patchValue( {
        id: m.id, 
        candidateId: m.candidateId,
        partyName: m.partyName,
        aadharNo: m.aadharNo, 
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

  onSubmit() {
    this.service.updateCandidateHistory(this.form.value).subscribe(() => {
      this.toastr.success('candidate updated');
      //this.router.navigateByUrl('/candidate');

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
      console.log('returned from task reminder modal', taskDto);
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

      this.userTaskService.createTaskFromAppTask(appTask).subscribe(response => {
        this.toastr.success('task created');
      }, error => {
        this.toastr.warning('failed to create the task');
      })
    }, error => {
      this.toastr.warning('failed to create task', error);
    })
  }

  private getReminderObject(i: number): IApplicationTaskInBrief {
    const dt= new Date().toISOString();
    const dt1 = new Date(dt);
    //var arrayControl = this.form.get('userHistoryItems') as FormArray;
    //const disc = arrayControl.at(i).get('gistOfDiscussions').value;
    
    let obj = new ApplicationTaskInBrief();
    obj.id=0, obj.taskTypeId = 0, obj.taskTypeName ='',
    obj.taskDate = new Date(); obj.taskOwnerId=0,
    obj.taskOwnerName='', obj.assignedToId=2,
    obj.assignedToName='', obj.completeBy=new Date(dt1.setDate(dt1.getDate()+ 7)),
    obj.taskStatus='not started'
    obj.taskDescription='reminder for ';

    return obj;
    };

}

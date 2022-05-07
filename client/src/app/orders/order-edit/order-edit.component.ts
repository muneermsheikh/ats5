import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, NgForm, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { ApplicationTask } from 'src/app/shared/models/applicationTask';
import { IContractReview } from 'src/app/shared/models/contractReview';
import { IContractReviewItem } from 'src/app/shared/models/contractReviewItem';
import { ICustomerNameAndCity } from 'src/app/shared/models/customernameandcity';
import { IEmployeeIdAndKnownAs } from 'src/app/shared/models/employeeIdAndKnownAs';
import { IJDDto } from 'src/app/shared/models/jdDto';
import { IOrder } from 'src/app/shared/models/order';
import { IOrderItem } from 'src/app/shared/models/orderItem';
import { IProfession } from 'src/app/shared/models/profession';
import { IRemunerationDto } from 'src/app/shared/models/remunerationDto';
import { IOrderAssignmentDto } from 'src/app/shared/models/orderAssignmentDto';
import { orderAssignmentDto } from 'src/app/shared/models/orderAssignmentDto';
import { IUser } from 'src/app/shared/models/user';
import { ConfirmService } from 'src/app/shared/services/confirm.service';
import { SharedService } from 'src/app/shared/services/shared.service';
import { UserTaskService } from 'src/app/userTask/user-task.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { JobDescriptionModalComponent } from '../job-description-modal/job-description-modal.component';
import { OrderService } from '../order.service';
import { RemunerationModalComponent } from '../remuneration-modal/remuneration-modal.component';
import { ReviewModalComponent } from '../review-modal/review-modal.component';
import { ReviewService } from '../review.service';

import { AdminService } from 'src/app/account/admin.service';
import { ChooseAgentsModalComponent } from '../choose-agents-modal/choose-agents-modal.component';
import { OrderItemsAndAgentsToFwdDto } from 'src/app/shared/models/orderItemsAndAgentsToFwdDto';
import { IOrderItemToFwdDto, OrderItemToFwdDto } from 'src/app/shared/models/orderItemToFwdDto';
import { OrderBriefDto } from 'src/app/shared/models/orderBriefDto';
import { ICustomerOfficialDto } from 'src/app/shared/models/customerOfficialDto';

@Component({
  selector: 'app-order-edit',
  templateUrl: './order-edit.component.html',
  styleUrls: ['./order-edit.component.css']
})
export class OrderEditComponent implements OnInit {

  //@ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  //activeTab: TabDirective;
  @ViewChild('editForm') editForm: NgForm;

  routeId: string;

  member: IOrder;
  user: IUser;
  
  form: FormGroup;
  selectedCategoryIds: number[];
  categories: IProfession[];
  employees: IEmployeeIdAndKnownAs[];
  customers: ICustomerNameAndCity[];
  associates: ICustomerOfficialDto[];
  
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

  bsValueDate = new Date();
  bsModalRef: BsModalRef;
  jd: IJDDto;
  remun: IRemunerationDto;
  cReview: IContractReview;
  //itemReviews: IReviewItem[];
  contractReviewItem: IContractReviewItem;
  
  mySelect = '2';
  selectedCustomerName: any;
  selectedProjManagerName: any;

  orderReviewStatus: [
    {'id': 0, statusName: 'Not Reviewed'}, 
    {'id': 1, statusName: 'Accepted'}, 
    {'id': 2, statusName: 'Declined-Salary not feasible'}, 
    {'id': 3, statusName: 'Declined-visa availability uncertain'}, 
    {'id': 4, statusName: 'Negative background report of customer'}, 
  ]

  //modal choose agents
    existingOfficialIds: ICustomerOfficialDto[]=[]; // IChooseAgentDto[]=[];

  constructor(private service: OrderService, private bcService: BreadcrumbService, private adminService: AdminService,
      private modalService: BsModalService,
      private activatedRoute: ActivatedRoute, 
      private router: Router, 
      private sharedService: SharedService, 
      private rvwService: ReviewService,
      private accountService: AccountService, 
      private confirmService: ConfirmService,
      private toastr: ToastrService, 
      private taskService: UserTaskService,
      private fb: FormBuilder) {
    this.bcService.set('@orderDetail',' ');
    this.routeId = this.activatedRoute.snapshot.params['id'];
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    //this.maxDate.setFullYear(this.maxDate.getFullYear() - 1);  //10 years later
    //this.minDate.setFullYear(this.minDate.getFullYear() + 20);
    //this.bsRangeValue = [this.bsValue, this.maxDate];
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;

   }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => { 
      this.member = data.order;
      this.associates = data.associates;
      this.customers = data.customers;
      this.employees = data.employees;
      this.categories = data.professions;
    })

      //this.routeId = this.activatedRoute.snapshot.params['id'];
      this.isAddMode = !this.routeId;
      this.createForm();

      if (!this.isAddMode) {
        this.editOrder(this.member);
      } 
  }

  createForm() {
      this.form = this.fb.group({
        id: [null],  orderNo: 0, orderDate: ['', Validators.required],
        customerId: [0, Validators.required], orderRef: '', salesmanId: 0, projectManagerId: 0,
        medicalProcessInchargeEmpId: 0, visaProcessInchargeEmpId: 0, emigProcessInchargeId: 0,
        travelProcessInchargeId: 0, cityOfWorking: '', country: '', completeBy: ['', Validators.required],
        status: 0, forwardedToHRDeptOn: '', contractReviewStatusId:0,
        orderItems: this.fb.array([])
      } //, {validator: MustMatch('password', 'confirmPassword')}
      );

      //if (!this.isAddMode) this.loadMember();
    }

    editOrder(order: IOrder) {
      this.form.patchValue( {
        id: order.id, orderNo: order.orderNo, orderDate: order.orderDate, customerId: order.customerId,
        orderRef: order.orderRef, salesmanId: order.salesmanId, projectManagerId: order.projectManagerId, 
        medicalProcessInchargeId: order.medicalProcessInchargeEmpId, visaProcessInchargeEmpId: order.visaProcessInchargeEmpId,
        emigProcessInchargeId: order.emigProcessInchargeId, travelProcessInchargeId: order.travelProcessInchargeId,
        cityOfWorking: order.cityOfWorking, country: order.country, completeBy: order.completeBy,
        status: order.status, forwardedToHRDeptOn: order.forwardedToHRDeptOn
      });

      if (order.orderItems !== null) this.form.setControl('orderItems', this.setExistingItems(order.orderItems));
    }

    
    setExistingItems(items: IOrderItem[]): FormArray {
        const formArray = new FormArray([]);
        items.forEach(ph => {
          formArray.push(this.fb.group({
            selected: false, 
            id: ph.id, orderId: ph.orderId, srNo: ph.srNo, categoryId: ph.categoryId,
            ecnr: ph.ecnr, isProcessingOnly: ph.isProcessingOnly, industryId: ph.industryId,
            sourceFrom: ph.sourceFrom, quantity: ph.quantity, minCVs: ph.minCVs, maxCVs: ph.maxCVs,
            requireInternalReview: ph.requireInternalReview, requireAssess: ph.requireAssess,
            completeBefore: ph.completeBefore, hrExecId: ph.hrExecId, hrSupId: ph.hrSupId,
            hrmId: ph.hrmId, charges: ph.charges, feeFromClientINR: ph.feeFromClientINR, status: ph.status,
            reviewItemStatusId: ph.reviewItemStatusId, noReviewBySupervisor:ph.noReviewBySupervisor
          }))
        });
        return formArray;
    }

    
      get orderItems() : FormArray {
        return this.form.get("orderItems") as FormArray
      }

      newItem(): FormGroup {
        return this.fb.group({
          selected: false, 
          id: 0, orderId: 0, srNo: 0, categoryId: 0, ecnr: false, isProcessingOnly: false, industryId: 0,
          sourceFrom: '', quantity: 0, minCVs: 0, maxCVs: 0, requireInternalReview: false, requireAssess: false,
          completeBefore: '', hrExecId: 0, hrSupId: 0, hrmId: 0, charges: 0, feeFromClientINR: 0, status: 0,
          reviewItemStatusId: 0
      })
      }

      addItem() {
        this.orderItems.push(this.newItem());
        //this.addCheckboxes();
      }
      removeItem(i:number) {
        this.orderItems.removeAt(i);
        this.orderItems.markAsDirty();
        this.orderItems.markAsTouched();
      }


      loadMember() {
        this.service.getOrder(+this.routeId).subscribe(
          response => {
              this.member = response;  
              this.form.patchValue(this.member);
              if(this.member.orderItems !== null) {
                for(const p of this.member.orderItems) {
                  this.orderItems.push(new FormControl(p));
                }
              
              }
              //this.addCheckboxes();
            }
        )} 

        private addCheckboxes() {
          this.member.orderItems.forEach(() => this.orderItems.push(new FormControl(false)));
        }

      onSubmit() {
        if (+this.routeId ===0) {
          this.CreateOrder();
        } else {
          this.toastr.warning('updating order ...');
          this.UpdateOrder();
        }
      }
/*
      deleteMessage(id: number) {
        this.confirmService.confirm('Confirm delete message', 'This cannot be undone').subscribe(result => {
          if (result) {
            this.messageService.deleteMessage(id).subscribe(() => {
              this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
            })
          }
        })
      }
  */

      private CreateOrder() {
        this.service.register(this.form.value).subscribe(response => {
          var order = response;
          this.confirmService.confirm('Create Acknowledgement message for client?', 
            'the DL is saved.  Do you want to compose the acknowledgement message to client?', 'Yes, compose', 'No, not now')
            .subscribe(result => {
            })
        }, error => {
          console.log(error);
          this.errors = error.errors;
        })
      }

      private UpdateOrder() {
        this.service.UpdateOrder(this.form.value).subscribe(response => {
          this.toastr.success('order updated');
          this.router.navigateByUrl('/orders');

        }, error => {
          console.log(error);
        })
      }

      /*
      selectTab(tabId: number) {
        this.memberTabs.tabs[tabId].active = true;
      }


      onTabActivated(data: TabDirective) {
        this.activeTab = data;
      }
*/

      handleFileInput(files: FileList) {
        this.fileToUpload = files.item(0);
      }

      getJD(orderitemid: number) {
        return this.service.getJD(orderitemid).subscribe(response => {
          this.jd=response;
        }, error => {
          this.toastr.error('failed to retrieve job description');
        })
      }

      getName(i) {
          return this.getControls()[i].value.id;
      }
    
      getControls() {
        return (<FormArray>this.form.get('orderItems')).controls;
      }

      getContractReviewItem(orderitemid:number) {
        return this.rvwService.getReviewItem(orderitemid).subscribe(response => {
          this.contractReviewItem = response;
          console.log('contractReviewItem from api', this.contractReviewItem);
        }, error => {
          this.toastr.error('failed to retrieve review items');
        })
      }

      openReviewModal(index: number) {
          var orderitemid = this.getName(index);
          
          this.rvwService.getReviewItem(orderitemid).subscribe(response => 
            {
              this.contractReviewItem = response;

              const config = {
                class: 'modal-dialog-centered modal-lg',
                initialState: {
                  review : this.contractReviewItem,
                  reviewStatus: [
                    {'id': 0, statusName: 'Not Reviewed'}, 
                    {'id': 1, statusName: 'Accepted'}, 
                    {'id': 2, statusName: 'Declined-Salary not feasible'}, 
                    {'id': 3, statusName: 'Declined-visa availability uncertain'}, 
                    {'id': 4, statusName: 'Negative background report of customer'}, 
                  ]
                }
              };

              this.bsModalRef = this.modalService.show(ReviewModalComponent, config);
              this.bsModalRef.content.updateModalReview.subscribe(values => {
                    
                    this.rvwService.updateReviewItem(values).subscribe(() => {
                      this.toastr.success("Review Item updated");
                    }, error => {
                      this.toastr.error("failed to update the Reviews");
                    })
              }, error => {
                this.toastr.error('failed to call updateModalReview');
              })
            }, error => {
              this.toastr.error('failed to retrieve contract review item from api', error);
            })
      }

      openJDModal(index: number) {
        var orderitemid = this.getName(index);
        //this.getJD(+orderitemid);
          this.service.getJD(orderitemid).subscribe(response => {
              this.jd=response;
              const initialState = {
                  id: this.jd.id,
                  title: 'job description', 
                  customerName: this.jd.customerName,
                  orderNo: this.jd.orderNo,
                  orderDate: this.jd.orderDate,
                  orderItemId: orderitemid,
                  categoryName: this.jd.categoryName,
                  jobDescInBrief: this.jd.jobDescInBrief,
                  qualificationDesired: this.jd.qualificationDesired,
                  expDesiredMin: this.jd.expDesiredMin,
                  expDesiredMax: this.jd.expDesiredMax,
                  minAge: this.jd.minAge,
                  maxAge: this.jd.maxAge
              };
              this.bsModalRef = this.modalService.show(JobDescriptionModalComponent, {initialState});
             
              this.bsModalRef.content.updateSelectedJD.subscribe(values => {
              this.service.updateJD(values).subscribe(() => {
                this.toastr.success("job description updated");
              }, error => {
                this.toastr.error("failed to update the job description");
              })
            }
            )
            
          }, error => {
            this.toastr.warning('failed to retrieve job description');
          })
          
      }

      openRemunerationModal(index: number) {
        var orderitemid = this.getName(index);
          this.service.getRemuneration(orderitemid).subscribe(response => {
              this.remun=response;
              const initialState = {
                customerName: this.remun.customerName, 
                categoryName: this.remun.categoryName, 
                orderDate: this.remun.orderDate,
                id: this.remun.id, orderItemId: this.remun.orderItemId,
                orderId: this.remun.orderId, orderNo: this.remun.orderNo, categoryId: this.remun.categoryId,
                workHours: this.remun.workHours, salaryCurrency: this.remun.salaryCurrency, salaryMin: this.remun.salaryMin, 
                salaryMax: this.remun.salaryMax, contractPeriodInMonths: this.remun.contractPeriodInMonths,
                housingProvidedFree: this.remun.housingProvidedFree, housingAllowance: this.remun.housingAllowance, 
                housingNotProvided: this.remun.housingNotProvided, foodProvidedFree: this.remun.foodProvidedFree, 
                foodAllowance: this.remun.foodAllowance, foodNotProvided: this.remun.foodNotProvided, 
                transportProvidedFree: this.remun.transportProvidedFree, transportAllowance: this.remun.transportAllowance, 
                transportNotProvided: this.remun.transportNotProvided, otherAllowance: this.remun.otherAllowance, 
                leavePerYearInDays: this.remun.leavePerYearInDays, 
                leaveAirfareEntitlementAfterMonths: this.remun.leaveAirfareEntitlementAfterMonths
              };
              this.bsModalRef = this.modalService.show(RemunerationModalComponent, {initialState});
             
              this.bsModalRef.content.updateSelectedRemuneration.subscribe(values => {
              this.service.updateRemuneration(values).subscribe(() => {
                this.toastr.success("Remuneration updated");
              }, error => {
                this.toastr.error("failed to update the Remuneration");
              })
            }
            )
            
          }, error => {
            this.toastr.warning('failed to retrieve Remuneration data');
          })
          
      }

      forwardDLToHRDept() {
        if (this.member.forwardedToHRDeptOn?.getFullYear() > 2000) {
          this.toastr.warning('this DL has been forwarded earlier on ' + this.forwardDLToHRDept);
          return;
        }
        //create task
        var completeBy = new Date();
        completeBy.setDate(completeBy.getDate() + 7);
        var taskDesc = "Order No. " + this.member.orderNo + ' dt ' + this.member.orderDate + ' is released for your execution as per details provided' +
          '.  If you do not want to accept the task, you must decline it within 6 hours of receiving it.';
        var appTask= new ApplicationTask;
        appTask.taskTypeId= 2;    //
        appTask.assignedToId= 2;
        appTask.orderId = this.member.id;
        appTask.orderNo = this.member.orderNo;
        appTask.taskOwnerId = this.form.get('projectManagerId').value;
        appTask.completeBy = completeBy;
        appTask.postTaskAction = 3; //send email msg
        appTask.taskItems=null;
        appTask.taskStatus="Not started";
        appTask.taskDate=this.member.orderDate;
        appTask.taskDescription = taskDesc;

        /* taskItem.transactionDate = this.member.orderDate;
        taskItem.quantity = 0;
        taskItem.taskStatus = 'not started';
        taskItem.taskTypeId = 1;
        taskItem.taskItemDescription = 'Order No. ' + this.member.orderNo + ' released to HR Dept for execution';
        taskItem.userId=1;
        appTask.taskItems.push(taskItem);
        */
        this.taskService.createTaskFromAppTask(appTask).subscribe(_response => {
          this.service.updateOrderWithDLFwdToHROn(this.member.id, new Date()).subscribe(() => {
            //this.getMember(+this.routeId);
            console.log('updated Order for date forwarded');
          }, error => {
            console.log('failed to update order with dateforwarded to DL');
          })
          this.toastr.success('Task created in the name of the HR Supervisor');
        }, error => {
          this.toastr.error('failed to create task in the name of the HR Supervisor', error);
        })

      }

    
      forwardDLtoAgents() {
        
        //var selectedOrderItems= this.member.orderItems.filter(el => el.selected===true);
        //const selectedOrderItems = this.member.orderItems.map((checked, i) => checked ? checked[i].id : null).filter(v => v !== null);
        var orderItemValues = this.orderItems.value;
        var dtForwarded = new Date();
        var selectedOrderItems = orderItemValues.filter(x => x.selected===true);
        
        if (selectedOrderItems.length===0) {
          this.toastr.error("No items selected");
          return;
        }

        //var returnvalue:any;
        var order = new OrderBriefDto();
        //order.customerName = this.form.controls.get['customerId'].text;
        //var nm = this.sharedService.getDropDownText(customerId, this.member.customerName);
        order.customerName = "customer name to resolve";
        order.orderNo = this.member.orderNo;
        order.orderDate=this.member.orderDate;
        let agents = this.associates;
        const config = {
          class: 'modal-dialog-centered modal-lg',
          
          initialState: {
            //order,
            agents
          }
        }

        this.bsModalRef = this.modalService.show(ChooseAgentsModalComponent, config);
        this.bsModalRef.content.updateSelectedOfficialIds.subscribe(values => {
          const officialIdsToUpdate = {   //contains only selected official Ids
              agents: [...values.filter(el => el.checked === true)]};
              for( var i = 0; i < agents.length; i++){ 
                if ( agents[i].checked) agents.splice(i,1);
              }
            
              console.log('received back from modal', agents);
/*
            if (!officialIdsToUpdate) {
              this.toastr.error("No Officials selected");
              return; }
            //console.log('officialidstoupdate', officialIdsToUpdate);
            var agts: ICustomerOfficialDto[]=[];
            officialIdsToUpdate.agents.forEach(x => {
              const elementSelected = agents.find(element => element.checked== true);
              if (elementSelected !== null && elementSelected !== undefined)  
                {
                  var agt = new CustomerOfficialDto();  
                  agt.city = elementSelected.city;
                  agt.customerId = elementSelected.customerId;
                  agt.customerName = elementSelected.customerName;
                  agt.id = elementSelected.officialId;
                  agt.officialName = elementSelected.officialName;
                  agt.designation = elementSelected.designation;
                  agt.checked = elementSelected.checked;
                  agt.checkedPhone = elementSelected.checkedPhone;
                  agt.checkedMobile = elementSelected.checkedMobile;
                  agt.officialEmailId = elementSelected.officialEmailId;  
                  agt.mobileNo = elementSelected.mobile;
                  agt.title = elementSelected.title;
                  agts.push(agt);  
                }
              })
           
              console.log('selected agents', agts);
            */
            var agentsanditems = new OrderItemsAndAgentsToFwdDto();
            
            agentsanditems.agents = agents;

            var items: IOrderItemToFwdDto[]=[];
            selectedOrderItems.forEach(i => {
              var it = new OrderItemToFwdDto();
              it.categoryId = i.categoryId;
              it.categoryName = i.categoryName;
              it.categoryRef = this.member.orderNo + '-' + i.srNo;
              it.charges = it.charges;
              it.customerCity=this.member.cityOfWorking;
              it.customerName = this.member.customerName;
              it.ecnr = i.ecnr;
              it.jobDescriptionURL = '';
              it.minCVs = i.minCVs;
              it.orderDate = this.member.orderDate;
              it.orderId = this.member.id;
              it.orderItemId = i.id;
              it.projectManagerId = this.member.projectManagerId;
              it.quantity=i.quantity;
              it.remunerationURL = '';
              it.salaryCurrency = '';
              items.push(it);
            })
            agentsanditems.items = items;
            //check for unique constraints - OrderItemId, Dateforwarded.Date, OfficialId ** TODO **
            agentsanditems.dateForwarded = dtForwarded;
            //console.log('sent to api', agentsanditems);
            this.adminService.forwardDLtoSelectedAgents(agentsanditems).subscribe(() => {
              this.toastr.success('Selected Order Categories forwarded to selected Associates');
            }, error => {
              this.toastr.error(error);
            })
          
        })
      }
    
          
      getItemReviewString(i: number)
      {
        var st: string='';  
        switch (i)
            {
              case 100:   //enumReviewItemStatus.accepted:
                st = 'Accepted';
                break;
              case 400: // enumReviewItemStatus.negativeBackGroundReport:
                  st= '-ve Backgrd Report';
                  break;
              case 200: // enumReviewItemStatus.salaryNotFeasible:
                  st= 'Salary Not Feasible';
                  break;
              case 300: // enumReviewItemStatus.visaAvailabilityUncertain:
                  st = 'visa not available';
                  break;
              default:
                  return 'Not Reviewed';
            }
            return st;
        } 
    
      /* 
      private getAgentsArray() {
        this.adminService.getOfficialDto().subscribe(response => {
          this.existingOfficialIds = response;
          if(this.existingOfficialIds.length===0) {
            this.toastr.warning('failed to retrieve any customer data to choose from');
            return;
          } 
          let agts: ICustomerOfficialDto[];
          this.existingOfficialIds.forEach(role => {
            //aagent = new chooseAgentDto();
            const agt = new CustomerOfficialDto();
            //arole.email = role.username;
            agt.customerId = role.customerId;
            agt.customerName=role.customerName;
            agt.city=role.city;
            agt.checked=false;
            agt.checkedPhone=false;
            agt.checkedMobile=false;
            agt.officialId=role.officialId;
            agt.officialName=role.officialName;
            agt.officialEmailId = role.officialEmailId;
            //aagent.phoneNo = role.phoneNo;
            agt.mobileNo = role.mobileNo;
            agt.title = role.title;
            agt.designation = role.designation;

            agts.push(agt);

          })
          return agts;
          
        }, error => {
          console.log('failed to retrieve agent official dto array', error);
        })
        
      }   
      */

      assignTasksToHRExecs() {
        if (this.isAddMode) {
          this.toastr.error('tasks cannot be assigned while in add mode.  Save the data first and then comeback to this form in edit mode to assign HR Executive tasks');
          return;
        }

        let f = this.form;

        
        var assignments:IOrderAssignmentDto[]=[]
        var assignment = new orderAssignmentDto;
        //this.form.get('controlName').value 
        assignment.orderId=this.form.get('id').value;
        assignment.orderNo=this.form.get('orderNo').value;
        assignment.orderDate=this.form.get('orderDate').value;
        assignment.cityOfWorking=this.form.get('cityOfWorking').value;
        assignment.customerId=this.form.get('customerId').value;
        assignment.projectManagerId=this.form.get('projectManagerId').value;
        assignment.postTaskAction=3;

        /* for (let i =0;i< this.member.orderItems.length;i++) {
          const element = this.orderItems.at(i);
          if (element.get('selected').value) {
              selectedItems.push(element.value);
          }
        } */

        for (let i=0;i< f.value.orderItems.length;i++) {
          const element = f.value.orderItems.at(i);
          if (element.selected) {
            assignment.orderItemId = element.id;
            assignment.hrExecId=element.hrExecId;
            assignment.categoryRef=assignment.orderNo + '-' + element.srNo;
            assignment.quantity = element.quantity;
            assignment.completeBy = element.completeBy;
            assignment.categoryId = element.categoryId;
            assignments.push(assignment);
          }
        }

        // ** TODO ** use only HREXECID values that have changed
        if (assignments.length === undefined || assignments.length===0) {
          this.confirmService.confirm('you must select the DL Items that need to be assigned to HR Executives', 'order items not selected');
          return;
        };

        console.log(assignments);
        return this.taskService.createOrderAssignmentTasks(assignments).subscribe(resposne => {
          this.toastr.success('tasks created for the chosen order items');
        }, error => {
          this.toastr.error(error, 'failed to create tasks for the chosen order items');
        })

      }      


      
      customerChange() {
        this.selectedCustomerName = this.sharedService.getDropDownText(this.mySelect, this.customers)[0].customerName;
        console.log(this.selectedCustomerName);
      }
    
      projectManagerChange() {
        this.selectedProjManagerName = this.sharedService.getDropDownText(this.mySelect, this.employees)[0].knownAs;
      }
      
      minSelectedCheckboxes(min = 1) {
        const validator: ValidatorFn = (formArray: FormArray) => {
          const totalSelected = formArray.controls
            // get a list of checkbox values (boolean)
            .map(control => control.value)
            // total up the number of checked checkboxes
            .reduce((prev, next) => next ? prev + next : prev, 0);
      
          // if the total is not greater than the minimum, return the error message
          return totalSelected >= min ? null : { required: true };
        };
      
        return validator;
      }
  
      openAssessmentModal(orderitemid: number) {

        this.router.navigateByUrl('/hr/itemassess/' + orderitemid);
      }

      showProcess() {
        
      }
}

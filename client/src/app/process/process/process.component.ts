import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { ICVRefAndDeployDto } from 'src/app/shared/dtos/cvRefAndDeployDto';
import { ICVRefDto } from 'src/app/shared/models/cvRefDto';
import { IDeploymentStatus } from 'src/app/shared/models/deployStatus';
import { IUser } from 'src/app/shared/models/user';
import { deployParams } from 'src/app/shared/params/deployParams';
import { DepModalComponent } from '../dep-modal/dep-modal.component';
import { ProcessService } from '../process.service';

@Component({
  selector: 'app-process',
  templateUrl: './process.component.html',
  styleUrls: ['./process.component.css']
})
export class ProcessComponent implements OnInit {
  
  user: IUser;

  referrals: ICVRefAndDeployDto[];
  statuses: IDeploymentStatus[]=[];
  routeId: string;
  pParams = new deployParams();
  totalCount: number;
  form: FormGroup;
  bsModalRef: BsModalRef;

  cvDto: ICVRefDto;

  constructor(private service: ProcessService, private accountService: AccountService, private toastr: ToastrService,
      private router: Router ,private activatedRoute: ActivatedRoute, private fb: FormBuilder,
      private modalService: BsModalService) { 
        this.routeId = this.activatedRoute.snapshot.params['id'];
        this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
        this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    
    this.activatedRoute.data.subscribe(data => { 
      this.statuses = data.statuses;
      this.referrals = data.processes.data;
      this.totalCount = data.processes.count;
      console.log('ngoninit', this.referrals);
      console.log('statuses in ngoninit', this.statuses);
    })
    
    this.createForm();
   this.editRef(this.referrals);
  }
  
  newItem(): FormGroup {
    return this.fb.group({
      checked: false, id: 0, cvRefId: [0, ], customerName: '', orderId: 0, orderNo: 0, orderDate: '',
        orderItemId: 0, categoryName: '', categoryRef: '', customerId: 0, candidateId: 0, 
        applicationNo: 0, candidateName: '', referredOn: '', selectedOn: '',
        refStatus: 0, deployStageName: '', deployStageDate: '', nextStageId: 0, nextStageDate:''
  })
  }

  addItem() {
    this.refArray.push(this.newItem());
    //this.addCheckboxes();
  }

  createForm() {
    this.form = this.fb.group({
      refArray: this.fb.array([])
    })
  }

  get refArray() : FormArray {
    return this.form.get("refArray") as FormArray
  }


  editRef(refs: ICVRefAndDeployDto[]) {
    if (refs !== null) this.form.setControl('refArray', this.setExistingItems(refs));
  }

  setExistingItems(items: ICVRefAndDeployDto[]): FormArray {
    const formArray = new FormArray([]);
    items.forEach(f => {
      formArray.push(this.fb.group({
        checked: f.checked, id: f.cvRefId, cvRefId: f.cvRefId, customerName: f.customerName,
        orderId: f.orderId, orderNo: f.orderNo, 
        orderDate: formatDate(f.orderDate, 'd-MMM-yy', 'en'),
        orderItemId: f.orderItemId, categoryName: f.categoryName, categoryRef: f.categoryRef,
        customerId: f.customerId, candidateId: f.candidateId, applicationNo: f.applicationNo,
        candidateName: f.candidateName, 
        referredOn: formatDate(f.referredOn, 'd-MMM-yy', 'en'),
        selectedOn: formatDate(f.selectedOn, 'd-MMM-yy', 'en'),
        refStatus: f.refStatus, deployStageName: f.deployStageName, 
        deployStageDate: formatDate(f.deployStageDate, 'd-MMM-yy', 'en'),
        nextStageId:  {stageId: f.nextStageId}, 
        nextStageDate: ''
      }))
    });
    return formArray;
  }

  getProcesses(useCache: boolean) {
    this.service.getProcesses(useCache).subscribe(response => {
      this.referrals  = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    })
  }

  
  updateDeployStatus() {
    var sErr = this.checkednoerror();
    if (sErr !=='') {
      this.toastr.warning(sErr);
      return;
    }

    console.log('referrals', this.form.value);
    var dto = this.form.get('refArray').value.filter(x => x.checked===true);
    console.log('dto', dto);
    var postDto = dto.map(({cvRefId, nextStageId, nextStageDate})=>({cvRefId: cvRefId, stageId: nextStageId.stageId, transactionDate: nextStageDate}));
    console.log('postDto', postDto);
    if (postDto.length === 0) {
      this.toastr.warning('no items selected');
      return;
    }
    this.service.InsertDeployTransactios(postDto).subscribe(response => {
      response.forEach(r => {
          const index= this.referrals.findIndex(x => x.cvRefId === r.cVRefId);
          if(index >=0) {
            var toReplace = this.referrals[index];
            toReplace.deployStageDate = r.nextStageDate
            toReplace.deployStageName = this.statuses.find(x => x.stageId == r.nextStageId).statusName;
            toReplace.nextStageId = this.statuses.find(x => x.stageId == r.nextStageId).nextStageId;
          }
          console.log('nextSTageId', toReplace.nextStageId);
      })
      this.toastr.success('transactions updated');
    }, error => {
      console.log('error in updating transactions', error);
    })
  }


  checkednoerror() {
    
    return '';
  }

  onPageChanged(event: any){
    const params = this.service.getOParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.service.setOParams(params);
      this.getProcesses(true);
    }
  }

  displayModalDeployment(indx: number)
  {
    var ctrl = this.refArray.controls[0].value.cvRefId;
    this.service.getDeployments(ctrl).subscribe(response => {
      if (response===undefined) {
        console.log('failed to get CVReferredDto from API');
        return;
      }
      this.cvDto = response;

      const config = {
        class:'modal-dialog-centered modal-lg',
        initialState: {
          cvreferEdit: response,
          depStatuses : this.statuses
        }
      };
      console.log('config', config);
      this.bsModalRef = this.modalService.show(DepModalComponent, config);
  
      this.bsModalRef.content.emitObj.subscribe(values => {
        this.toastr.success('success');
  
      }, error => {
        console.log('error modal', error);
      })
  

    }, error => {
      console.log('failed to get CVReferredDto', error);
      return;
    })
    /*thisCV.cvRefId= c.cvRefId;
    thisCV.applicationNo=c.applicationNo;
    thisCV.candidateName=c.candidateName;
    thisCV.categoryName=c.candidateName;
    thisCV.categoryRef=c.categoryRef;
    thisCV.customerName=c.customerName;
    thisCV.referredOn=c.referredOn;
    thisCV.selectedOn=c.selectedOn;
    thisCV.deployments=c.deployments; 
    */
  }

  displayApp(id: number) {
    console.log('id', id);
    if(id===0) {
      this.toastr.error('ID value 0');
      return;
    }
    this.service.getDeployments(16).subscribe(response => {
      if (response===undefined) {
        return;
      }
      this.cvDto = response;
      console.log('displayapp', this.cvDto);
  })
  }

}

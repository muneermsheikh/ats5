import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { IEmploymentDto } from 'src/app/shared/dtos/employmentDto';
import { selDecisionToAddDto } from 'src/app/shared/dtos/selDecisionToAddDto';
import { ISelPendingDto } from 'src/app/shared/dtos/selPendingDto';
import { IEmployment, ISelectionDecision } from 'src/app/shared/models/selectionDecision';
import { ISelectionStatus } from 'src/app/shared/models/selectionStatus';
import { SelDecisionSpecParams } from 'src/app/shared/params/selDecisionSpecParams';
import { ConfirmService } from 'src/app/shared/services/confirm.service';
import { SelectionModalComponent } from '../selection-modal/selection-modal.component';
import { SelectionService } from '../selection.service';

@Component({
  selector: 'app-selection',
  templateUrl: './selection.component.html',
  styleUrls: ['./selection.component.css']
})

export class SelectionComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  
  selection: ISelPendingDto;
  selectionsPending: ISelPendingDto[]=[];
  selectionStatus: ISelectionStatus[]=[];
  employmentsDto: IEmploymentDto[]=[];
  
  sParams = new SelDecisionSpecParams();
  totalCount = 0;

  cvsSelected: ISelPendingDto[]=[];

  pageIndex=1;
  pageSize=3;
  
  todayDate = new Date(Date.now());
  statusSelected=10;

  loading=false;

  form: FormGroup;
  bsModalRef: BsModalRef;

  constructor(private service: SelectionService, 
      private confirmService: ConfirmService,
      private toastr: ToastrService, private router: Router,
      private bsModalService: BsModalService) { }

  ngOnInit(): void {
    this.sParams.pageSize = this.pageSize;
    this.getPendingSelections(false);
    this.getSelectionStatus();
  }

  getPendingSelections(useCache: boolean)
  {
    this.service.setParams(this.sParams);
    this.service.getPendingSelections(useCache).subscribe(response => {
      this.selectionsPending = response.data;
      this.totalCount = response.count;
      if (this.selectionsPending.length === 0) this.toastr.warning('no pending selections');
    }, error => {
      this.toastr.warning(error);
    })
  }

  getSelectionStatus() {
    return this.service.getSelectionStatus().subscribe(response => {
      this.selectionStatus = response;
    }, error => {
      this.toastr.error(error);
    })
  }

  convertSelDecisionToDto (sel: ISelPendingDto[]): selDecisionToAddDto[] {
    console.log('sel',sel);
    var dtos: selDecisionToAddDto[]=[];

    sel.forEach(s => {
      var dto = new selDecisionToAddDto();
      dto.cvRefId = s.cvRefId;
      dto.selectionStatusId = s.selectionStatusId;
      dto.decisionDate = this.todayDate;
      dto.remarks = s.remarks ?? '';
      //var emp = this.employmentsDto.find(x => x.cVRefId === s.cVRefId);
      //if (emp !== null) dto.employment = emp;
      console.log('dto', dto);
      dtos.push(dto);
    })

    return dtos;
  }

  registerSelections() {
    this.cvsSelected = this.selectionsPending.filter(x => x.checked===true);
    if (this.cvsSelected === null) {
      this.toastr.warning('no CVs selected');
      return;
    }
    //convert selDecision to dto
    var dtos = this.convertSelDecisionToDto(this.cvsSelected);
   
    return this.service.registerSelectionDecisions(dtos).subscribe(response => {
      this.toastr.success('selection decisions registered');
      if(response.employmentDtos.length > 0) {
        this.employmentsDto = response.employmentDtos;
      }
      this.getPendingSelections(false); //false to get updated results
    }, error => {
      this.toastr.error(error);
    })

  }

  editSelection(sel: ISelectionDecision) {
    return this.service.editSelectionDecision(sel).subscribe(response => {
      this.toastr.success('selection decision updated');
    }, error => {
      this.toastr.error(error);
    })
  }

  deleteSelection(id: number) {
    return this.service.deleteSelectionDecision(id).subscribe(response => {
      this.toastr.success('the chosen selection deleted');
    }, error => {
      this.toastr.error(error);
    })
  }

 /* showEmploymentModal(sel: ISelPendingDto){

    console.log('showemploymentmodal', sel);
    
      const title = 'Employment details: Application No.: ' + sel.applicationNo  +
        ', ' + sel.candidateName + '. Selected by: ' + sel.customerName +
        ', category ref: ' + sel.categoryName;
      var emp = this.getEmployment(sel);
      if (emp === undefined) {
        this.toastr.warning('failed to get emp');
        return;
      }
      const config = {
        class: 'modal-dialog-centered',
        initialState: {
          title,
          emp
        }
      }
      console.log('config', config);
      this.bsModalRef = this.bsModalService.show(SelectionModalComponent, config);
      this.bsModalRef.content.updateEmployment.subscribe(values => {
        this.service.updateEmployment(values).subscribe(() => {
            emp = values;
            this.toastr.success('employment data created');
        }, error => this.toastr.error(error))
      })
  }
  */

 /*
  getEmployment(sel: ISelPendingDto): IEmployment {
    var id = sel.cVRefId;
    var emp: IEmployment;

      this.service.getEmployment(id).subscribe(response => {
        emp = response;
      }, error => {
        this.toastr.error(error);
      })

    return emp;
  }
  */

  routeChange() {
    if (this.form.dirty) {
        this.confirmService.confirm('Confirm move to another page', 
        'This candidate selection decision form is edited, but not saved. ' + 
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

  onPageChanged(event: any){
    const params = this.service.getParams();
    if (this.sParams.pageIndex !== event) {
      this.sParams.pageIndex = event;
      this.getPendingSelections(true);
    }
  }

}

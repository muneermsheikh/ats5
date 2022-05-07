import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { ChecklistModalComponent } from 'src/app/candidate/checklist-modal/checklist-modal.component';
import { OrderService } from 'src/app/orders/order.service';
import { ICandidateBriefDto } from 'src/app/shared/models/candidateBriefDto';
import { ICandidateCity } from 'src/app/shared/models/candidateCity';
import { candidateParams } from 'src/app/shared/models/candidateParams';
import { ChecklistHRDto, IChecklistHRDto } from 'src/app/shared/models/checklistHRDto';
import { IChecklistHRItem } from 'src/app/shared/models/checklistHRItem';
import { ICustomerNameAndCity } from 'src/app/shared/models/customernameandcity';
import { IOrderItemBriefDto } from 'src/app/shared/models/orderItemBriefDto';
import { IProfession } from 'src/app/shared/models/profession';
import { SharedService } from 'src/app/shared/services/shared.service';
import { CandidateService } from '../../candidate/candidate.service';
import { ChecklistService } from '../../candidate/checklist.service';


@Component({
  selector: 'app-hrchecklist',
  templateUrl: './hrchecklist.component.html',
  styleUrls: ['./hrchecklist.component.css']
})
export class HrchecklistComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  
  cvs: ICandidateBriefDto[]=[];
  openOrderItems: IOrderItemBriefDto[]=[];
  cvParams = new candidateParams();
  totalCount: number;
  cities: ICandidateCity[];
  professions: IProfession[];
  existingQBankCategories: IProfession[];
  agents: ICustomerNameAndCity[];
  cl: IChecklistHRDto;
  clst: IChecklistHRDto;
  items: IChecklistHRItem[];

  bsModalRef: BsModalRef;

  selectedProf: IProfession;
  selectedAgent: ICustomerNameAndCity;
  selectedCity: ICandidateCity;
  selectedOrderItem: IOrderItemBriefDto;
  events: Event[] = [];

  sortOptions = [
    {name:'By Application No Asc', value:'appno'},
    {name:'By Application No Desc', value:'apppnodesc'},
    {name:'By City Asc', value:'city'},
    {name:'By City Desc', value:'citydesc'},
    {name:'By Profession Asc', value:'prof'},
    {name:'By Profession Desc', value:'profdesc'},
    {name:'By Agent', value:'agent'},
    {name:'By Agent Desc', value:'agentdesc'}
  ]

  constructor(private orderService: OrderService, 
        private service: CandidateService, 
        private sharedService: SharedService,
        private checklistService: ChecklistService,
        private bsModalService: BsModalService,
        private toastr: ToastrService,
        private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {

    this.service.setCVParams(this.cvParams);
    this.getCities();
    this.getProfessions();
    this.getAgents();

    this.activatedRoute.data.subscribe(data => {
      this.openOrderItems = data.openorderitems;
    })

  }

   
  getCVs(useCache=false) {
    this.service.getCandidates(useCache).subscribe(response => {
      this.cvs = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    })
  }

  getCities() {
    this.service.getCandidateCities().subscribe(response => {
      this.cities = [{city: 'All'}, ...response];
    })
  }

  getProfessions() {
    this.sharedService.getProfessions().subscribe(response => {
      this.professions = [{id: 99999999, name: 'All'}, ...response];
    }, error => {
      console.log(error);
    })
  }
  
  getAgents() {
    this.sharedService.getAgents().subscribe(response => {
      this.agents = [{id: 99999999, customerName: 'All', 'city': 'All' }, ...response];
    }, error => {
      console.log(error);
    })
  }
  
  onSearch() {
    const params = this.service.getCVParams();
    if (this.selectedCity !==null && this.selectedCity !== undefined) params.city=this.selectedCity.city;
    if (this.selectedProf !== null && this.selectedProf !== undefined) params.professionId = this.selectedProf.id;
    if (this.selectedAgent !== null && this.selectedAgent !== undefined) params.agentId = this.selectedAgent.id;
  
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.service.setCVParams(params);
    this.getCVs();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.cvParams = new candidateParams();
    this.service.setCVParams(this.cvParams);
    this.getCVs();
  }
  
  onCitySelected(citySelected: string) {
    console.log('city selected',citySelected);
    const prms = this.service.getCVParams();
    prms.city = citySelected;
    prms.pageNumber=1;
    this.service.setCVParams(prms);
    //this.getCVs();
  }

  onProfSelected(profId: number) {
    const prms = this.service.getCVParams();
    prms.professionId = profId;
    prms.pageNumber=1;
    this.service.setCVParams(prms);
    //this.getCVs();

  }
  
  onAgentSelected(agentId: number) {
    const prms = this.service.getCVParams();
    prms.agentId = agentId;
    prms.pageNumber=1;
    this.service.setCVParams(prms);
    //this.getCVs();

    //console.log('after profession selected', this.cvs);

  }
  onCategorySelected(profId: number) {
    const prms = this.service.getCVParams();
    prms.professionId = profId;
    prms.pageNumber=1;
    this.service.setCVParams(prms);
    //this.getCVs();
  }

  onPageChanged(event: any){
    const params = this.service.getCVParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.service.setCVParams(params);
      this.getCVs(true);
    }
  }
  
  openChecklistPlain(candidateid: number) {
    if (this.selectedOrderItem === null || this.selectedOrderItem === undefined) {
      this.toastr.warning('Order Item not selected');
      return;
    } else if (candidateid === 0) {
      this.toastr.warning("Candidate Id not provided");
      return;
    }
    var orderitemid = this.selectedOrderItem.orderItemId;
    this.clst = this.getChecklistHRDto(candidateid, orderitemid);
  
    if (this.clst === undefined || this.clst === null) {
      this.toastr.warning("failed to get checklist values");
      return;
    }
    console.log('in candidateview.ts, hdr passed to modal is:', this.clst);
    console.log('in candidateview.ts, items passed to modal is: ', this.items);
    
    
  }

  //Having selected candidates, refer them to internal reviews or directly to client
  openChecklistModal(candidateid: number) {
      if (this.selectedOrderItem === null || this.selectedOrderItem === undefined) {
        this.toastr.warning('Order Item not selected');
        return;
      } else if (candidateid === 0) {
        this.toastr.warning("Candidate Id not provided");
        return;
      }
      var orderitemid = this.selectedOrderItem.orderItemId;
      this.clst = this.getChecklistHRDto(candidateid, orderitemid);
      this.items = this.clst.checklistHRItems;

      if (this.clst === undefined || this.clst === null) {
        this.toastr.warning("failed to get checklist values");
        return;
      }
      const config = {
          class: 'modal-dialog-centered modal-lg',
          initialState: {
          chklst:this.getChecklistHRDto(candidateid, orderitemid),
          
        }
      }
      
      this.bsModalRef = this.bsModalService.show(ChecklistModalComponent, config);
      this.bsModalRef.content.updateChecklist.subscribe(values => {
      this.cl = values;
      this.checklistService.updateChecklist(this.cl).subscribe(() => {
      this.toastr.success('updated Checklist');
      
      }, error => {
        this.toastr.error('failed to update the checklist', error);
      });
    })
}

  getChecklist(candidateid: number, orderitemid: number): any {
    return this.checklistService.getChecklist(candidateid, orderitemid).subscribe(response => {
      this.toastr.success('got values of checklist from api');
      this.clst = response;
      console.log('getchecklist', this.clst);
      return response;
    }, error => {
      this.toastr.error('failed to get checklist object from api', error);
      return null;
    })

  }

  private getChecklistHRDto(candidateid: number, orderitemid: number) {
    let checklisthr= new ChecklistHRDto();
    let x: IChecklistHRDto;
    this.checklistService.getChecklist(candidateid, orderitemid).subscribe(response => {
      x = response;
      checklisthr.applicationNo = x.applicationNo;
      checklisthr.candidateId = x.candidateId;
      checklisthr.candidateName = x.candidateName;
      checklisthr.categoryRef = x.categoryRef;
      checklisthr.checkedOn = x.checkedOn;
      checklisthr.checklistHRItems = x.checklistHRItems;
      checklisthr.hrExecComments = x.hrExecComments;
      checklisthr.id=x.id;
      checklisthr.orderItemId=x.orderItemId;
      checklisthr.orderRef=x.orderRef;
      checklisthr.userLoggedId = x.userLoggedId;

      return checklisthr;
    }, error => {

    })
    return checklisthr;
  }

}

import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { IdsModalComponent } from '../orders/ids-modal/ids-modal.component';
import { OrderService } from '../orders/order.service';
import { ICandidateBriefDto } from '../shared/models/candidateBriefDto';
import { ICandidateCity } from '../shared/models/candidateCity';
import { candidateParams } from '../shared/models/candidateParams';
import { ICustomerNameAndCity } from '../shared/models/customernameandcity';
import { IOrderItemBriefDto } from '../shared/models/orderItemBriefDto';
import { IProfession } from '../shared/models/profession';
import { IUser } from '../shared/models/user';
import { SharedService } from '../shared/services/shared.service';
import { CandidateService } from './candidate.service';

declare const loadDocument: any;
declare const loadInitialDocument: any;

@Component({
  selector: 'app-listing',
  templateUrl: './listing.component.html',
  styleUrls: ['./listing.component.css']
})


export class ListingComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  cvs: ICandidateBriefDto[];
  cvParams = new candidateParams();
  totalCount: number;
  candidateCities: ICandidateCity[];
  professions: IProfession[];
  existingQBankCategories: IProfession[];
  agents: ICustomerNameAndCity[];
  bsModalRef: BsModalRef;

  idFromChild: number=0;

//ngSelect
  selectedProfIds: number[]=[];
  events: Event[] = [];

  documentLoading: boolean;

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

  constructor(private service: CandidateService, 
      private sharedService: SharedService, 
      private modalService: BsModalService,
      private orderService: OrderService,
      private toastr: ToastrService) { }

  ngOnInit(): void {
    this.service.setCVParams(this.cvParams);
    this.getCVs(true);
    this.getCities();
    this.getProfessions();
    this.getAgents();
  }
  
  getCVs(useCache=false) {
    this.service.getCandidates(useCache).subscribe(response => {
      this.cvs = response.data;
      this.totalCount = response.count;
      console.log('candidates', this.cvs);
    }, error => {
      console.log(error);
    })
  }

  getCities() {
    this.service.getCandidateCities().subscribe(response => {
      this.candidateCities = [{city: 'All'}, ...response];
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
    const prms = this.service.getCVParams();
    prms.city = citySelected;
    prms.pageNumber=1;
    this.service.setCVParams(prms);
    this.getCVs();
  }

  onProfSelected(profId: number) {
    const prms = this.service.getCVParams();
    prms.professionId = profId;
    prms.pageNumber=1;
    this.service.setCVParams(prms);
    this.getCVs();

  }
  
  onAgentSelected(agentId: number) {
    const prms = this.service.getCVParams();
    prms.agentId = agentId;
    prms.pageNumber=1;
    this.service.setCVParams(prms);
    this.getCVs();

    //console.log('after profession selected', this.cvs);

  }
  onCategorySelected(profId: number) {
    const prms = this.service.getCVParams();
    prms.professionId = profId;
    prms.pageNumber=1;
    this.service.setCVParams(prms);
    this.getCVs();

    //console.log('after profession selected', this.cvs);

  }

  onPageChanged(event: any){
    const params = this.service.getCVParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.service.setCVParams(params);
      this.getCVs(true);
    }
  }

  //Having selected candidates, refer them to internal reviews or directly to client
  openChecklistModal(user: IUser) {
    const title = 'Choose Order Item to refer selected CVs to';
    var returnvalue:any;
    var ids: number[];
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        user,
        title,
        orderItems: this.getOpenOrderItemsArray(),
        ids
      }
    }
    this.bsModalRef = this.modalService.show(IdsModalComponent, config);
    this.bsModalRef.content.updateSelectedRoles.subscribe(values => {
      ids = values;
      if (ids.length) {
        //this.service.submitCVsForReview().subscribe(() => {
//          user.roles = [...rolesToUpdate.roles]
        //}
        //)
      }
    })
  }
  

  private getOpenOrderItemsArray(): IOrderItemBriefDto[] {
    const roles: any[] = [];
    let aitems: IOrderItemBriefDto[]=[];
    let aitem: IOrderItemBriefDto;
    this.orderService.getOrderItemsBriefDto().subscribe(response => {
      aitems = response;
      if(aitems.length===0) return;
      return aitems;
    }, error => {
      console.log('failed to retrieve roles array', error);
    })
    return aitems;
  }

  //output value from child
  showDocumentViewer(id: number)
  {
    this.idFromChild = id;
    return this.service.viewDocument(id).subscribe(result => {
      loadInitialDocument(result);
    }, error => {
      this.toastr.error(error);
    })
  }

  loadInitialDocument(document) {
      this.documentLoading = true;
      window.addEventListener("documentViewerLoaded", function () {
        if (this.documentLoading === true)
          loadDocument(document);
          this.documentLoading = false;
      });
  }

  loadDocument(document) {
    //TXDocumentViewer.loadDocument(Document.documentData, Document.documentName);
  }
  
  
  //ngSelect
  /*
  onChange(event) {
    this.events.push({ name: '(change)', value: event });
}

onFocus(event: Event) {
    this.events.push({ name: '(focus)', value: event });
}

onBlur(event: Event) {
    this.events.push({ name: '(blur)', value: event });
}

onOpen() {
    this.events.push({ name: '(open)', value: null });
}

onClose() {
    this.events.push({ name: '(close)', value: null });
}

onAdd(event) {
    this.events.push({ name: '(add)', value: event });
}

onRemove(event) {
    this.events.push({ name: '(remove)', value: event });
}

onClear() {
    this.events.push({ name: '(clear)', value: null });
}

onScrollToEnd(event) {
    this.events.push({ name: '(scrollToEnd)', value: event });
}

onSelectSearch(event) {
    this.events.push({ name: '(search)', value: event });
}
 */
}

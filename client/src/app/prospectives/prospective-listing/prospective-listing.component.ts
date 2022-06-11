import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { CandidateHistoryService } from 'src/app/candidate/candidate-history.service';
import { IProspectiveRegisterToAddDto } from 'src/app/shared/dtos/prospectiveRegisterToAddDto';
import { IContactResult } from 'src/app/shared/models/contactResult';
import { IEmployeeIdAndKnownAs } from 'src/app/shared/models/employeeIdAndKnownAs';
import { IProspectiveCandidate } from 'src/app/shared/models/prospectiveCandidate';

import { IUser } from 'src/app/shared/models/user';
import { IUserHistory } from 'src/app/shared/models/userHistory';
import { IUserHistoryItem } from 'src/app/shared/models/userHistoryItem';
import { IUserPhone } from 'src/app/shared/models/userPhone';
import { IUserProfession } from 'src/app/shared/models/userProfession';
import { prospectiveCandidateParams } from 'src/app/shared/params/prospectiveCandidateParams';
import { ProspectiveService } from '../prospective.service';

@Component({
  selector: 'app-prospective-listing',
  templateUrl: './prospective-listing.component.html',
  styleUrls: ['./prospective-listing.component.css']
})
export class ProspectiveListingComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  
  cvs: IProspectiveCandidate[];
  prospectiveIdClicked: number=0;
  recordChanged:boolean=false;

  cvParams = new prospectiveCandidateParams();
  totalCount: number;
  
  iLastId: number;

  //for app-user-history
  member: IUserHistory;
  
  employees: IEmployeeIdAndKnownAs[];
  contactResults: IContactResult[];
  contactResultData: IContactResult[]=[];
  user: IUser;
  listStatus: string='';

  //to convert prospect to cv
  register: IProspectiveRegisterToAddDto;

  documentLoading: boolean;

   constructor(private service: ProspectiveService, private historyService: CandidateHistoryService, private router: Router,
      private toastr: ToastrService, private accountService: AccountService, private activatedRoute: ActivatedRoute) {
        this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
       }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => { 
      this.contactResults = data.results;
      this.contactResultData = data.results;
      this.employees = data.employees;
    })
    this.service.setParams(this.cvParams);
    this.getCVs(true);
  }
  
  getCVs(useCache=false) {
    this.service.setParams(this.cvParams);
    return this.service.getProspectiveCandidates(useCache).subscribe(response => {
      this.cvs = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    })
  }

  
  onSearch() {
    const params = this.service.getParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.service.setParams(params);
    this.getCVs();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.cvParams = new prospectiveCandidateParams();
    this.service.setParams(this.cvParams);
    this.getCVs();
  }
  
  onPageChanged(event: any){
    const params = this.service.getParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.service.setParams(params);
      this.getCVs(true);

      this.recordChanged=false;
    }
  }

  IdValueClickedInItem(event$) {
    console.log('id value clicked');
    this.prospectiveIdClicked=event$.id;
  }

  onItemChange(s: string) {
    if (this.cvParams.status === s) return;
    if (s==='all') {
      this.cvParams.status='';
    } else if (s === 'pending'){
      this.cvParams.status='pending';
    }

    this.getCVs();
  }

  transferProfileToCV(cv: IProspectiveCandidate) {

   if (cv.source === undefined || cv.candidateName===undefined || cv.email === undefined 
      || cv.currentLocation === undefined || cv.phoneNo === undefined || cv.categoryRef === undefined ) {
        this.toastr.info('required values missing');
        return;
      }

    var dto: IProspectiveRegisterToAddDto = {} as IProspectiveRegisterToAddDto;
    dto.candidateName=cv.candidateName;
    dto.age=cv.age;
    dto.prospectiveId=cv.id;
    dto.gender='Male';
    dto.knownAs=cv.candidateName.substring(0,15);
    dto.userName=this.user.displayName;
    dto.password="pr0$pectivE";
    dto.email=cv.email;
    dto.categoryRef = cv.categoryRef;
    dto.source=cv.source;
    dto.currentLocation=cv.currentLocation;
    dto.phoneNo=cv.phoneNo;
    dto.alternatePhoneNo=cv.alternatePhoneNo;

    return this.service.createCandidateFromprospective(dto).subscribe(() => {
      this.toastr.success('converted to candidate object');
      //remove the record from prospective;
      
      var profRecordIndex = this.cvs.findIndex(x => x.id === cv.id);
      this.cvs.splice(profRecordIndex,1);
      --this.totalCount;

    }, error => {
      console.log('failed to convert the object to candidate', error);
    })
  }


  checked(id: number) {
    console.log('procedure not defined');
  }
  
  showUserContact(id: number) {
    this.router.navigateByUrl('/candidate/historyfromProspectiveId/'+ id);
  
  /*
    if (this.prospectiveIdClicked < 0) {
      this.toastr.warning('prospective Id not set');
      return;
    }
    const histparams = new userHistoryParams();
    histparams.personType="prospective";
    histparams.personId = id;
    this.historyService.setUserParams(histparams);
    return this.historyService.getHistory(histparams).subscribe(response=>{
      this.member=response.body;
      console.log('returned from api, member:', this.member);
    }, error => {
      this.toastr.error('failed to retrieve User History', error);
    })
    */
  }

  

  updateItems(items: IUserHistoryItem[]) {
    console.log('emitted from child item',items);
  }

 

}

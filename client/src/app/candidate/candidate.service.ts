import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ICandidate } from '../shared/models/candidate';
import { ICandidateBriefDto } from '../shared/models/candidateBriefDto';
import { ICandidateCity } from '../shared/models/candidateCity';
import { candidateParams } from '../shared/models/candidateParams';
import { ICustomerNameAndCity } from '../shared/models/customernameandcity';
import { cvReviewDto, ICVReviewDto } from '../shared/models/cvReviewDto';
import { IPaginationCandidate, PaginationCandidate } from '../shared/pagination/paginationCandidate';
import { IProfession } from '../shared/models/profession';
import { IUser } from '../shared/models/user';
import { PaginationCandidateBrief } from '../shared/pagination/paginationCandidateBrief';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class CandidateService {

  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  cvParams = new candidateParams();
  pagination = new PaginationCandidate();
  paginationBrief = new PaginationCandidateBrief();
  cities: ICandidateCity[]=[];
  agents: ICustomerNameAndCity[]=[];
  professions: IProfession[]=[];
  cache = new Map();
  cacheBrief = new Map();

  constructor(private http: HttpClient, private toastr: ToastrService, private router: Router) {}

  
  async onClickLoadDocument() {
    // get a document from the Web API endpoint 'LoadDocument'
    return this.http.get<any>(this.apiUrl + 'candidates/loaddocument');
  }

  getCandidates(useCache: boolean) { 

    if (useCache === false) {
      this.cache = new Map();
    }

    if (this.cache.size > 0 && useCache === true) {
      if (this.cache.has(Object.values(this.cvParams).join('-'))) {
        this.pagination.data = this.cache.get(Object.values(this.cvParams).join('-'));
        return of(this.pagination);
      }
    }

    let params = new HttpParams();
    if (this.cvParams.city !== "") {
      params = params.append('city', this.cvParams.city);
    }
    if (this.cvParams.professionId !== 0) params = params.append('professionId', this.cvParams.professionId.toString());
    
    //if (this.cvParams.agentId !== 0) params = params.append('agentId', this.cvParams.agentId.toString());
    
    if (this.cvParams.search) params = params.append('search', this.cvParams.search);
    
    params = params.append('sort', this.cvParams.sort);
    params = params.append('pageIndex', this.cvParams.pageNumber.toString());
    params = params.append('pageSize', this.cvParams.pageSize.toString());

    return this.http.get<IPaginationCandidate>(this.apiUrl + 'candidate/candidatepages', {observe: 'response', params})
      .pipe(
        map(response => {
          this.cache.set(Object.values(this.cvParams).join('-'), response.body.data);
          this.pagination = response.body;
          return response.body;
        })
      )
    }

  checkEmailExists(email: string) {
    return this.http.get(this.apiUrl + 'account/emailexists?email=' + email);
  }

  checkPPExists(ppnumber: string) {
    return this.http.get(this.apiUrl + 'account/ppexists?ppnumber=' + ppnumber);
  }
  
  getCandidate(id: number) {
    return this.http.get<ICandidate>(this.apiUrl + 'candidate/byid/' + id);
  }

  getCandidateBrief(id: number) {
    let dto: ICandidateBriefDto;

    this.cache.forEach((cv: ICandidateBriefDto) => {
      var dto = this.pagination.data.find(p => p.id === id);
    })

    if (dto) {
      return of(dto);
    }
    
    return this.http.get<ICandidateBriefDto>(this.apiUrl + 'candidate/briefbyid/' + id);
  }

  getCandidateBriefDtoFromAppNo(id: number) {
    return this.http.get<ICandidateBriefDto>(this.apiUrl + 'candidate/byappno/' + id);
  }

  register(model: any) {
    return this.http.post(this.apiUrl + 'account/register', model ).pipe(
      map((user: IUser) => {
      if (user) {
        this.setCurrentUser(user);
      }
      }));
    }
  
  UpdateCandidate(model: any) {
    return this.http.put(this.apiUrl + 'candidate', model).pipe(
      map((user: IUser) => {
        if (user) {
        this.setCurrentUser(user);
        }
      }))}
    
  setCurrentUser(user: IUser) {
 
    localStorage.setItem('token', user.token);
    this.currentUserSource.next(user);
  }
  
  setCVParams(params: candidateParams) {
    this.cvParams = params;
  }
  getCVParams() {
    return this.cvParams;
  }

  getCandidateCities() {
    if (this.cities.length > 0) {
      return of(this.cities);
    }
  
    return this.http.get<ICandidateCity[]>(this.apiUrl + 'candidate/cities' ).pipe(
      map(response => {
        this.cities = response;
        return response;
      })
    )
  }
  
  submitCVsForReview(itemIds: number[], cvids: number[]) {
      
      if (itemIds.length === 0 || cvids.length ===0) {
        this.toastr.warning("candidate Ids or item Ids data not provided");
        return;
      }

      var cvrvws: ICVReviewDto[]=[];
      cvids.forEach(c => {
        itemIds.forEach(i => {
          var cvrvw = new cvReviewDto();  
          cvrvw.candidateId=c;
          cvrvw.orderItemId=i;
          cvrvw.execRemarks='';
          cvrvws.push(cvrvw);
        })
      })

      return this.http.post(this.apiUrl + 'cvreviews', cvrvws);
  }
  viewDocument(id: number) {
    // get a document from the Web API endpoint 'LoadDocument'
    return this.http.get<any>(this.apiUrl + 'fileupload/viewdocument/' + id);
  }


}

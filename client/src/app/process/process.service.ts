import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ICVRefAndDeployDto } from '../shared/dtos/cvRefAndDeployDto';
import { IDeployAddedDto } from '../shared/dtos/deployAddedDto';
import { IDeployPostDto } from '../shared/dtos/deployPostDto';
import { ICustomerNameAndCity } from '../shared/models/customernameandcity';
import { ICVRefDto } from '../shared/models/cvRefDto';
import { IDeploymentStatus } from '../shared/models/deployStatus';
import { IUser } from '../shared/models/user';
import { IPaginationDeploy, PaginationDeploy } from '../shared/pagination/paginationDeploy';
import { deployParams } from '../shared/params/deployParams';


@Injectable({
  providedIn: 'root'
})
export class ProcessService {

  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  pageSize=3;
  pageIndex=1;
  pParams = new deployParams();
  deploys: ICVRefAndDeployDto[]=[];
  pagination = new PaginationDeploy();
  cache = new Map();
  
  constructor(private http: HttpClient) { }

      
    setOParams(params: deployParams) {
      this.pParams = params;
    }
    
    getOParams() {
      return this.pParams;
    }

    
    getProcesses(useCache: boolean) {

      const token = localStorage.getItem("token");
      if (token==='') return;
      
      if (useCache === false) {
        this.cache = new Map();
      }
      
      if (this.cache.size > 0 && useCache === true) {
        if (this.cache.has(Object.values(this.pParams).join('-'))) {
          this.pagination.data = this.cache.get(Object.values(this.pParams).join('-'));
          return of(this.pagination);
        }
      }

      let params = new HttpParams();
      console.log('params in process service', this.pParams);
      console.log('pParams.OrderId',this.pParams.orderId);
      if (this.pParams.orderId !== 0) {
        params = params.append('orderId', this.pParams.orderId.toString());
      }
        
      if (this.pParams.orderItemId !== 0) {
        params = params.append('orderItemId', this.pParams.orderItemId.toString());
      }
    
      if (this.pParams.customerName !== '') {
        params = params.append('customerName', this.pParams.customerName);
      }
        
      if (this.pParams.search) {
        params = params.append('search', this.pParams.search);
      }
      
      params = params.append('sort', this.pParams.sort);
      params = params.append('pageIndex', this.pParams.pageNumber.toString());
      params = params.append('pageSize', this.pParams.pageSize.toString());

      return this.http.get<IPaginationDeploy>(this.apiUrl + 'deploy/pending', {observe: 'response', params})
        .pipe(
          map(response => {
            this.cache.set(Object.values(this.pParams).join('-'), response.body.data);
            this.pagination = response.body;
            return response.body;
          })
        )
      }

    getDeployStatus() {
      return this.http.get<IDeploymentStatus[]>(this.apiUrl + 'deploy/depstatus');
    }

    InsertDeployTransactios(deployDto: IDeployPostDto[] ){
      return this.http.post<IDeployAddedDto[]>(this.apiUrl + 'deploy/posts', deployDto);
    }
  
    getDeployments(i: number) {
      return this.http.get<ICVRefDto>(this.apiUrl + 'deploy/' + i);
    }
}

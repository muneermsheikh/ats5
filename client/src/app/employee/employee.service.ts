import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IEmployee } from '../shared/models/employee';
import { employeeParams } from '../shared/models/employeeParams';
import { IEmployeePosition } from '../shared/models/empPosition';
import { IPaginationEmployee, PaginationEmployee } from '../shared/models/paginationEmp';
import { IProfession } from '../shared/models/profession';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  empParams = new employeeParams();
  pagination = new PaginationEmployee();
  professions: IProfession[]=[];
  positions: IEmployeePosition[]=[];
  cache = new Map();

  constructor(private http: HttpClient) { }

    getEmployees(useCache: boolean) { 
      if (useCache === false) {
        this.cache = new Map();
      }

      if (this.cache.size > 0 && useCache === true) {
        if (this.cache.has(Object.values(this.empParams).join('-'))) {
          this.pagination.data = this.cache.get(Object.values(this.empParams).join('-'));
          return of(this.pagination);
        }
      }

      let params = new HttpParams();
      if (this.empParams.employeeSkillId !== 0) {
        params = params.append('employeeSkillId', this.empParams.employeeSkillId.toString());
      }

      if (this.empParams.search) {
        params = params.append('search', this.empParams.search);
      }
      
      params = params.append('sort', this.empParams.sort);
      params = params.append('pageIndex', this.empParams.pageNumber.toString());
      params = params.append('pageSize', this.empParams.pageSize.toString());

      
      return this.http.get<IPaginationEmployee>(this.apiUrl + 'employees/employeepages', {observe: 'response', params})
        .pipe(
          map(response => {
            this.cache.set(Object.values(this.empParams).join('-'), response.body.data);
            this.pagination = response.body;
            return response.body;
          })
        )
    }

    checkEmailExists(email: string) {
      return this.http.get(this.apiUrl + 'account/emailexists?email=' + email);
    }

    checkAadharExists(aadharnumber: string) {
      return this.http.get(this.apiUrl + 'employees/aadharpexists?aadharnumber=' + aadharnumber);
    }
    
    getEmployee(id: number) {
      return this.http.get<IEmployee>(this.apiUrl + 'employees/byid/' + id);
    }

    register(model: any) {
      return this.http.post(this.apiUrl + 'account/registeremployee', model, {})
    }
    
    UpdateEmployee(model: any) {
      return this.http.put(this.apiUrl + 'employees/', model)
    }
      
    setEmpParams(params: employeeParams) {
      this.empParams = params;
    }
    getEmpParams() {
      return this.empParams;
    }

    
    getEmployeePositions() {
      if (this.positions.length > 0) {
        return of(this.positions);
      }
    
      return this.http.get<IEmployeePosition[]>(this.apiUrl + 'employees/employeepositions' ).pipe(
        map(response => {
          this.positions = response;
          return response;
        })
      )
    }


}

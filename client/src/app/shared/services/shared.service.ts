import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ICustomerNameAndCity } from '../models/customernameandcity';
import { IEmployeeIdAndKnownAs } from '../models/employeeIdAndKnownAs';
import { IIndustryType } from '../models/industryType';
import { IProfession } from '../models/profession';
import { IQualification } from '../models/qualification';
import { ISkillData } from '../models/skillData';
import * as _ from 'lodash';

@Injectable({
  providedIn: 'root'
})

export class SharedService {
  apiUrl = environment.apiUrl;
  agents: ICustomerNameAndCity[]=[];
  customers: ICustomerNameAndCity[]=[];

  constructor(private http: HttpClient) { }

  
  getAgents() {
    if (this.agents.length > 0) {
      return of(this.agents);
    }
    return this.http.get<ICustomerNameAndCity[]>(this.apiUrl + 'customers/associateidandnames/associate').pipe(
      map(response => {
        this.agents = response;
        return response;
      })
    );
  }

  getCustomers() {
    if (this.customers.length > 0) {
      return of(this.customers);
    }
    return this.http.get<ICustomerNameAndCity[]>(this.apiUrl + 'customers/associateidandnames/customer').pipe(
      map(response => {
        this.customers = response;
        return response;
      })
    );
  }

  getProfessions() {
    return this.http.get<IProfession[]>(this.apiUrl + 'categories');
  }
  getIndustries() {
    return this.http.get<IIndustryType[]>(this.apiUrl + 'masters/industrieslist');
  }

  getSkillData() {
    return this.http.get<ISkillData[]>(this.apiUrl + 'masters/skillDatalist');
  }

  getQualifications() {
    return this.http.get<IQualification[]>(this.apiUrl + 'masters/qlist');
  }
  
  checkAadharNoExists(aadharNo: string) {
    return this.http.get<boolean>(this.apiUrl + 'account/' + aadharNo);
  }

  getEmployeeIdAndKnownAs() {
    return this.http.get<IEmployeeIdAndKnownAs[]>(this.apiUrl + 'employees/idandknownas');
  }

  getCustomerOfficialIds() {
    
  }

  getDropDownText(id, object){
    const selObj = _.filter(object, function (o) {
        return (_.includes(id,o.id));
    });
    return selObj;
  }

}

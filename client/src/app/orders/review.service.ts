import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IContractReview } from '../shared/models/contractReview';
import { IContractReviewItem } from '../shared/models/contractReviewItem';
import { contractReviewParams } from '../shared/models/contractReviewParams';
import { IReviewItem } from '../shared/models/reviewItem';
import { IReviewItemData } from '../shared/models/reviewItemData';
import { IPaginationContractReview, PaginationContractReview } from '../shared/pagination/paginationContractReview';

@Injectable({
  providedIn: 'root'
})

export class ReviewService {
  apiUrl = environment.apiUrl;
  review: IContractReview;
  cache = new Map();
  oParams = new contractReviewParams();
  pagination = new PaginationContractReview();

  constructor(private http: HttpClient) { }

  getReview(id: number) {
    return this.http.get<IContractReview[]>(this.apiUrl + 'contractreview/' + id);
  }

  getReviewItem(id: number) {
    return this.http.get<IContractReviewItem>(this.apiUrl + 'contractreview/reviewitem/' + id);
  }


  getReviewItems(id: number) {
    return this.http.get<IReviewItem[]>(this.apiUrl + 'contractreview/reviewresult/' + id);
  }

  updateReviewItem(model: IContractReviewItem) 
  {
    console.log('calling contractrreview/reviewitem put with model ', model);
    return this.http.put(this.apiUrl + 'ContractReview/reviewitem', model);
  }

  getReviews(useCache: boolean, id: number) {

    if (useCache === false) {
      this.cache = new Map();
    }

    if (this.cache.size > 0 && useCache === true) {
      if (this.cache.has(Object.values(this.oParams).join('-'))) {
        this.pagination.data = this.cache.get(Object.values(this.oParams).join('-'));
        return of(this.pagination);
      }
    }

    let params = new HttpParams();
    if (this.oParams.city !== "") {
      params = params.append('city', this.oParams.city);
    }
    if (this.oParams.categoryId !== 0) {
      params = params.append('categoryId', this.oParams.categoryId.toString());
    }

    //if (this.oParams.orderidInts !=null && this.oParams.orderidInts.length !== 0) {
      //params = params.append('orderids', this.oParams.orderidInts.join(', '));
      params = params.append('orderids', id + ',' + id);
    //}

    if (this.oParams.search) {
      params = params.append('search', this.oParams.search);
    }
    
    params = params.append('sort', this.oParams.sort);
    params = params.append('pageIndex', this.oParams.pageNumber.toString());
    params = params.append('pageSize', this.oParams.pageSize.toString());

    console.log('params', params);

    return this.http.get<IPaginationContractReview>(this.apiUrl + 'contractreview/reviews', {observe: 'response', params})
      .pipe(
        map(response => {
          this.cache.set(Object.values(this.oParams).join('-'), response.body.data);
          this.pagination = response.body;
          return response.body;
        })
      )
  }
  
  getReviewData() {
    return this.http.get<IReviewItemData[]>(this.apiUrl + 'contractreview/reviewdata');
  }
  
  register(model: any) {
    return this.http.post(this.apiUrl + 'orders/review', model);
    }
  
  updateReview(model: any) {
    return this.http.put(this.apiUrl + 'orders/review', model)
  }

  updateReviews(model: any[]) {
    return this.http.put(this.apiUrl + 'orders/reviews', model)
  }
  
}

import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { mastersParams } from 'src/app/shared/models/mastersParams';
import { IProfession } from 'src/app/shared/models/profession';
import { CategoryEditModalComponent } from '../category-edit-modal/category-edit-modal.component';
import { MastersService } from '../masters.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  cats: IProfession[];
  catParams= new mastersParams();
  totalCount: number;
  bsModalRef: BsModalRef;
  
  constructor(private activatedRoute: ActivatedRoute,
      private mastersService: MastersService,
      private modalService: BsModalService,
      private toastr: ToastrService) { }

  ngOnInit(): void {
    this.mastersService.setParams(this.catParams);
    this.activatedRoute.data.subscribe(data => { this.cats = data.cats;})
  }

  getCats(useCache=false) {
    this.mastersService.getCategories(useCache).subscribe(response => {
      this.cats = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    })
  }

  onSearch() {
    const params = this.mastersService.getParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.mastersService.setParams(params);
    this.getCats();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.catParams = new mastersParams();
    this.mastersService.setParams(this.catParams);
    this.getCats();
  }

  onPageChanged(event: any){
    const params = this.mastersService.getParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.mastersService.setParams(params);
      this.getCats(true);
    }
  }

  openCategoryEditModal(index: number, categoryString: string) {
    const initialState = {
      str: categoryString
    };
    this.bsModalRef = this.modalService.show(CategoryEditModalComponent, {initialState});

    //returned from modal
    this.bsModalRef.content.update.updateStringName.subscribe(values => {
      if(values === categoryString) {
        this.toastr.warning('category value not changed');
        return;
      } else {
        this.mastersService.updateCategory(index, values).subscribe(response => {
          this.toastr.success('category value updated');
        }, error => {
          this.toastr.error(error);
        })
      }
    })
  }

}

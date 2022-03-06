import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { mastersParams } from 'src/app/shared/models/mastersParams';
import { IQualification } from 'src/app/shared/models/qualification';
import { CategoryEditModalComponent } from '../../category-edit-modal/category-edit-modal.component';
import { MastersService } from '../../masters.service';

@Component({
  selector: 'app-listing',
  templateUrl: './listing.component.html',
  styleUrls: ['./listing.component.css']
})
export class ListingComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  qualifications: IQualification[];
  catParams= new mastersParams();
  totalCount: number;
  bsModalRef: BsModalRef;
  
  constructor(private activatedRoute: ActivatedRoute,
      private mastersService: MastersService,
      private modalService: BsModalService,
      private toastr: ToastrService) { }

  ngOnInit(): void {
    this.mastersService.setParams(this.catParams);
    this.activatedRoute.data.subscribe(data => { this.qualifications = data.quals;})
  }

  getQualifications(useCache=false) {
    this.mastersService.getQualifications(useCache).subscribe(response => {
      this.qualifications = response.data;
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
    this.getQualifications();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.catParams = new mastersParams();
    this.mastersService.setParams(this.catParams);
    this.getQualifications();
  }

  onPageChanged(event: any){
    const params = this.mastersService.getParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.mastersService.setParams(params);
      this.getQualifications(true);
    }
  }

  openQualificationEditModal(index: number, qualificationString: string) {
    const initialState = {
      str: qualificationString
    };
    this.bsModalRef = this.modalService.show(CategoryEditModalComponent, {initialState});

    //returned from modal
    this.bsModalRef.content.update.updateStringName.subscribe(values => {
      if(values === qualificationString) {
        this.toastr.warning('qualification value not changed');
        return;
      } else {
        this.mastersService.updateQualification(index, values).subscribe(response => {
          this.toastr.success('Qualification value updated');
        }, error => {
          this.toastr.error(error);
        })
      }
    })
  }

}

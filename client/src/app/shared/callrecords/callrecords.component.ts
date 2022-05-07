import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastRef, ToastrService } from 'ngx-toastr';
import { CallModalComponent } from '../call-modal/call-modal.component';
import { ICandidateBriefDto } from '../models/candidateBriefDto';
import { IUserHistoryDto } from '../models/userHistoryDto';

@Component({
  selector: 'app-callrecords',
  templateUrl: './callrecords.component.html',
  styleUrls: ['./callrecords.component.css']
})
export class CallrecordsComponent implements OnInit {
  bsModalRef: BsModalRef;
  cBrief: ICandidateBriefDto;
  cHistory: IUserHistoryDto=null;
  constructor(private modalService: BsModalService, private router:Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.openCallRecordModal();
  }

  openCallRecordModal() {
      const config = {
        class: 'modal-dialog-centered modal-lg',
        cBrief: this.cBrief,
        candidateFromDb: false
      }

      this.bsModalRef = this.modalService.show(CallModalComponent, config);
      this.bsModalRef.content.callPartyId.subscribe(values => {
        this.cHistory = values;
        console.log('opencallrecordmodal, cHistory returned from modal is: ', this.cHistory);
        if (this.cHistory===null) {
            this.toastr.warning('Your inputs did not return any history');
            return;
        }
        this.router.navigateByUrl('/candidate/history/' + this.cHistory.id );
        
      }, error => {
        this.toastr.error(error);
      })
  } 
}

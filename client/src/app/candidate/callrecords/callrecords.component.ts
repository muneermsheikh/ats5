import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastRef, ToastrService } from 'ngx-toastr';
import { CallModalComponent } from '../call-modal/call-modal.component';

@Component({
  selector: 'app-callrecords',
  templateUrl: './callrecords.component.html',
  styleUrls: ['./callrecords.component.css']
})
export class CallrecordsComponent implements OnInit {
  bsModalRef: BsModalRef;
  
  constructor(private modalService: BsModalService, private router:Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.openCallRecordModal();
  }

  openCallRecordModal() {
      const config = {
        class: 'modal-dialog-centered modal-md'
      }

      this.bsModalRef = this.modalService.show(CallModalComponent, config);
      this.bsModalRef.content.callPartyId.subscribe(values => {
        console.log('returned from modal', values);
        this.router.navigateByUrl('/candidate/history/' + values);
      }, error => {
        this.toastr.error(error);
      })
  }
}

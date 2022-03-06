import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { CandidateService } from 'src/app/candidate/candidate.service';
import { ICandidateBriefDto } from 'src/app/shared/models/candidateBriefDto';
import { candidateParams } from 'src/app/shared/models/candidateParams';


@Component({
  selector: 'app-call-modal',
  templateUrl: './call-modal.component.html',
  styleUrls: ['./call-modal.component.css']
})
export class CallModalComponent implements OnInit {
  @Input() callPartyId = new EventEmitter();
  
  candidateId: number;
  cBrief: ICandidateBriefDto;
  cvParams: candidateParams = new candidateParams();

  constructor(public bsModalRef: BsModalRef, private candidateService: CandidateService, private toastr: ToastrService ) { }

  ngOnInit(): void {
    
  }

  close() {
    this.bsModalRef.hide();
  }

  updateRoles() {
    this.callPartyId.emit(this.candidateId);
    this.bsModalRef.hide();
  }

  getCandidate() {
    
    return this.candidateService.getCandidateBriefDtoFromSpecParams(this.cvParams).subscribe(response => {
      this.cBrief = response.body;
    }, error => {
      this.toastr.error(error);
    })
  } 

  cancelEmit() {
    this.cBrief=null;
  }

  emitValue() {
    this.callPartyId.emit(this.cBrief.id);
    this.bsModalRef.hide();
  }



}

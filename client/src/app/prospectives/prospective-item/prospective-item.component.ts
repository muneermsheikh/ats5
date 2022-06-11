import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { IProspectiveCandidate } from 'src/app/shared/models/prospectiveCandidate';

@Component({
  selector: 'app-prospective-item',
  templateUrl: './prospective-item.component.html',
  styleUrls: ['./prospective-item.component.css']
})
export class ProspectiveItemComponent implements OnInit {
  @Input() cv: IProspectiveCandidate;
  //@Output() msgEvent = new EventEmitter<number>();
  @Output() profileIdEmitterEvent = new EventEmitter<number>();
  @Output() prospectiveIdEmitEvent = new EventEmitter<number>();
  @Output() convertToCVEvent = new EventEmitter<IProspectiveCandidate>();

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  emitProspectiveCandidateIdValue(cvid: number) {
    //this.msgEvent.emit(cvid);
  }

  candidateclicked(id: number) {
    //this.checkedEvent.emit(id);n
    this.router.navigateByUrl('/candidate/historyfromProspectiveId/'+ id);
  }
  
 
  downloadprofile(profileid: number) {
    this.profileIdEmitterEvent.emit(profileid);
  }

  initiateUserContact(prospectiveid: number) {
    this.prospectiveIdEmitEvent.emit(prospectiveid);
  }

  transferProspectiveToCandidate() {
    this.convertToCVEvent.emit(this.cv);
  }
 
  editPhone(ph: string) {
    console.log(ph);
  }

  editEmail(e: string) {
    console.log(e);
  }

  editStatus(st: string) {
    
    console.log(st);
  }
}


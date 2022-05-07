import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { ChecklistModalComponent } from 'src/app/candidate/checklist-modal/checklist-modal.component';
import { ChecklistService } from 'src/app/candidate/checklist.service';
import { ICandidateAssessedDto } from 'src/app/shared/models/candidateAssessedDto';
import { ConfirmService } from 'src/app/shared/services/confirm.service';
import { CvRefService } from '../cvref.service';

@Component({
  selector: 'app-cvref',
  templateUrl: './cvref.component.html',
  styleUrls: ['./cvref.component.css']
})
export class CvrefComponent implements OnInit {

  cvAssessed: ICandidateAssessedDto[]=[];
  form: FormGroup;
  cvSelected: ICandidateAssessedDto[]=[];
  assessmentids: number[];
  bsModalRef: BsModalRef;

  constructor(private cvrefService: CvRefService, 
    private activatedRoute: ActivatedRoute, 
    private router: Router,
    private checklistService: ChecklistService,
    private confirmService: ConfirmService,
    private bsModalService: BsModalService,
    private toastr: ToastrService) { }

  ngOnInit(): void {

    this.activatedRoute.data.subscribe(data => { 
      this.cvAssessed = data.assessedcvs;
    })
  }

 
  checkedChanged(event$: any)
  {
    if (!event$.checked) {
      var index = this.cvSelected.findIndex(x => x.id == event$.id);
      this.cvSelected.splice(index, 1);
    } else {
      this.cvSelected.push(event$);
    }

  }

  forwardSelected() {
    this.assessmentids = this.cvAssessed.filter(x => x.checked===true).map(x => x.id);
    this.cvrefService.referCVs(this.assessmentids).subscribe(response => {
      if (response.errorString==='') {
          this.toastr.success('CVs referred and email messages composed');
      } else {
        this.toastr.warning('failed to forward CVs -- ', response.errorString);
      }
      
    }, error => {
      this.toastr.error('failed to refer selected CVs', error);
    })
  }

  onCheckedChange($event: any) {
    console.log('event', $event);
    var index = this.cvAssessed.findIndex(x => x.id === $event.id);
    if ($event.checked) {
      if (index === null) {
        this.cvSelected.push($event);
      } else {
        var obj = this.cvAssessed.find(x => x.id === $event.id);
        obj.checked=true;
      }
    } else {
      this.cvSelected.splice(index,1);
    }
  }

 showChecklistModal(candidateid: number, orderitemid: number) {
    this.checklistService.getChecklist(candidateid, orderitemid).subscribe(response => {
      if (response===null) {
        this.toastr.warning('checklist record does not exist for the candidateId');
        return;
      }
      const config = {
        class: 'modal-dialog-centered modal-lg', initialState: {chklst: response}
      }
      this.bsModalRef = this.bsModalService.show(ChecklistModalComponent, config);
    })
  }
  
  routeChange() {
    if (this.form.dirty) {
        this.confirmService.confirm('Confirm move to another page', 
        'This candidate assessment data is edited, but not saved. ' + 
        'Do you want to move to another page without saving the data?')
        .subscribe(result => {
          if (result) {
            this.router.navigateByUrl('');
          }
        })
    } else {
      this.router.navigateByUrl('');
    }
  }

}

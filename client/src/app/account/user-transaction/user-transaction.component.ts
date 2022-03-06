import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { first } from 'rxjs/operators';
import { CandidateHistoryService } from 'src/app/candidate/candidate-history.service';
import { IUserHistoryDto } from 'src/app/shared/models/userHistoryDto';
import { IUserHistorySearch } from 'src/app/shared/models/userHistorySearch';
import { userHistorySpecParams } from 'src/app/shared/models/userHistorySpecParams';

@Component({
  selector: 'app-user-transaction',
  templateUrl: './user-transaction.component.html',
  styleUrls: ['./user-transaction.component.css']
})
export class UserTransactionComponent implements OnInit {

  histories: IUserHistoryDto[];
  userHist: IUserHistoryDto;
  uParams= new userHistorySpecParams();
  search: IUserHistorySearch;
  firstTime = true;
  form: FormGroup;

  constructor(private service: CandidateHistoryService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.createform();
  }

  createform() {
    this.form = this.fb.group({
      phoneNo: '',
      emailId: '',
      aadharNo: '',
      applicationNo: 0
    });
  }

  onSubmit() {
 
    this.firstTime = true;
    var phoneno = this.form.get('phoneNo').value;
    var email = this.form.get('emailId').value;
    var applicationno = this.form.get('applicationNo').value;
    console.log(+applicationno);
    if (phoneno !== '') {
      this.service.getUserHistoriesByPhoneNo(phoneno).subscribe(response => {
        this.histories = response;
      }, error => {
        this.firstTime=false;
      })
    } else if (email !== '') {
      this.service.getUserHistoriesByEmail(email).subscribe(response => {
        this.histories = response;
      }, error => {
        this.firstTime=false;
      })
    } else if (+applicationno > 0) {
      this.service.getUserHistoriesByApplicationNo(+applicationno).subscribe(response => {
        this.histories = response;
      }, error => {
        this.firstTime = false;
      })
    }
    this.firstTime = (this.histories===null || this.histories===undefined);
    console.log(this.firstTime, this.histories);
  }

  cancelDto() {
    
  }
  showDto() {

  }

}

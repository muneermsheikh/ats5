import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { FileToUpload } from '../shared/models/fileToUpload';

const apiUrl = environment.apiUrl;
const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json'
    })
};

@Injectable({
  providedIn: 'root'
})
export class UploadDownloadService {
  
  constructor(private http: HttpClient) { }

  uploadFile(theFiles: FileToUpload[]) : Observable<any> {
    console.log('uploadFile', theFiles);
    return this.http.post<FileToUpload>(apiUrl + 'FileUpload', theFiles, httpOptions);
  }

  downloadFile(candidateid: number) {
    console.log('downloading file for candidate id:', candidateid);
    return this.http.get(apiUrl + 'FileUpload/downloadcandidatefile/' + candidateid);
  }

  
  downloadProspectiveFile(prospectiveid: number) {

    return this.http.get(apiUrl + 'FileUpload/downloadprospectivefile/' + prospectiveid);
  }
}

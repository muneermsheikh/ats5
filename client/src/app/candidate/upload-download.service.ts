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

  uploadFile(theFile: FileToUpload) : Observable<any> {
    return this.http.post<FileToUpload>(apiUrl + 'FileUpload', theFile, httpOptions);
  }

}

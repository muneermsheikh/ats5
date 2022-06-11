import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UploadDownloadService } from 'src/app/candidate/upload-download.service';
import { FileToUpload } from '../../models/fileToUpload';

  // CREDIT - https://www.codemag.com/Article/1901061/Upload-Small-Files-to-a-Web-API-Using-Angular

  // Maximum file size allowed to be uploaded = 3MB
  const MAX_SIZE: number = 1048576*3;

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {
  
  @ViewChild('fileInput', {static: false})
  myFileInput: ElementRef;
  userId: number;
  
  theFiles: any[] = [];
  messages: string[] = [];
  filesUploaded: FileToUpload[] = [];

  constructor(private service: UploadDownloadService) { }

  ngOnInit(): void {
  }

  onFileChange(event) {
    this.theFiles = [];
    
    // Any file(s) selected from the input?
    if (event.target.files && event.target.files.length > 0) {
        for (let index = 0; index < event.target.files.length; index++) {
            let file = event.target.files[index];
            // Don't allow file sizes over MAX_SIZE DEFINED
            if (file.size < MAX_SIZE) {
                // Add file to list of files
                this.theFiles.push(file);
                var fileuploaded = new FileToUpload();
                fileuploaded.fileName=file.fileName;
                fileuploaded.fileSize=file.fileSize;
                this.theFiles.push(fileuploaded);
            }
            else {
                this.messages.push("File: " + file.name + " is too large to upload.");
            }
        }
    }
  }


  private readAndUploadFile(f: any) {
    
    
        let file = new FileToUpload();
        // Set File Information
        file.fileName = f.name;
        file.fileSize = f.size;
        file.fileType = f.type;
        file.lastModifiedTime = f.lastModifiedTime;
        file.lastModifiedDate = f.lastModifiedDate;
        
        // Use FileReader() object to get file to upload
        // NOTE: FileReader only works with newer browsers
        let reader = new FileReader();
        
        // Setup onload event for reader
        reader.onload = () => {
            // Store base64 encoded representation of file
            file.fileAsBase64 = reader.result.toString();
        }
      
    // POST to server
    this.service.uploadFile(this.theFiles).subscribe(resp => { 
        this.messages.push("Upload complete"); 
      }, error => {
        console.log('error in uploading file', error);
      });

      // Read the file
      //reader.readAsDataURL(theFile);  
      
/*
    let file = new FileToUpload();
      // Set File Information
      file.fileName = theFile.name;
      file.fileSize = theFile.size;
      file.fileType = theFile.type;
      file.lastModifiedTime = theFile.lastModified;
      file.lastModifiedDate = theFile.lastModifiedDate;
      
      // Use FileReader() object to get file to upload
      // NOTE: FileReader only works with newer browsers
      let reader = new FileReader();
      
      // Setup onload event for reader
      reader.onload = () => {
          // Store base64 encoded representation of file
          file.fileAsBase64 = reader.result.toString();
          
          // POST to server
          this.service.uploadFile(file).subscribe(resp => { 
              this.messages.push("Upload complete"); });
      }
      
      // Read the file
      reader.readAsDataURL(theFile);
    */
  }

  uploadFile(): void {
    for (let index = 0; index < this.theFiles.length; index++) {
        this.readAndUploadFile(this.theFiles[index]);
      }
    }
}

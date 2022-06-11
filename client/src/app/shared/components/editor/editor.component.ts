import { Component, Input, OnInit } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { IMessage } from '../../models/message';
import { ConfirmService } from '../../services/confirm.service';
import { MessageService } from '../../services/message.service';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.css']
})
export class EditorComponent implements OnInit {
  @Input() message: IMessage;
  
  constructor(private service: MessageService, private toastr: ToastrService, private confirmService: ConfirmService) { }

  ngOnInit(): void {
  }

  saveandclose() {

  }

  saveandsend() {

    this.service.sendMessage(this.message).subscribe(response => {
      this.toastr.success('message sent');
    }, error => {
      console.log('save and send error', error);
      this.toastr.error('failed to send the email message', error);
    })
  }

  
  deleteMessage(id: number) {
    this.confirmService.confirm('Confirm delete message', 'This cannot be undone').subscribe(result => {
      if (result) {
        this.service.deleteMessage(id).subscribe(() => {
          
        })
      }
    })
  }

  editorConfig: AngularEditorConfig = {
    editable: true,
      spellcheck: true,
      height: 'auto',
      minHeight: '0',
      maxHeight: 'auto',
      width: 'auto',
      minWidth: '0',
      translate: 'yes',
      enableToolbar: true,
      showToolbar: true,
      placeholder: 'Enter text here...',
      defaultParagraphSeparator: '',
      defaultFontName: '',
      defaultFontSize: '',
      fonts: [
        {class: 'arial', name: 'Arial'},
        {class: 'times-new-roman', name: 'Times New Roman'},
        {class: 'calibri', name: 'Calibri'},
        {class: 'comic-sans-ms', name: 'Comic Sans MS'}
      ],
      customClasses: [
      {
        name: 'quote',
        class: 'quote',
      },
      {
        name: 'redText',
        class: 'redText'
      },
      {
        name: 'titleText',
        class: 'titleText',
        tag: 'h1',
      },
    ]
    
  };

}

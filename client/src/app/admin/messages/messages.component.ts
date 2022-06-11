import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { IMessage } from 'src/app/shared/models/message';
import { EmailMessageSpecParams } from 'src/app/shared/params/emailMessageSpecParams';
import { ConfirmService } from 'src/app/shared/services/confirm.service';
import { MessageService } from 'src/app/shared/services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  
  messages: IMessage[];
  message: IMessage;
  
  container = 'sent';
  
  pageIndex = 1;
  pageSize = 3;
  loading = false;

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  totalCount: number;
  mParams = new EmailMessageSpecParams();

  sortOptions = [
    {name:'By Message sent Asc', value:'messagesent'},
    {name:'By Message sent Desc', value:'messagesentdesc'},
    {name:'By Sender', value:'sender'},
    {name:'By Recipient', value:'recipient'},
  ]

  //months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];


  constructor(private service: MessageService, private toastr: ToastrService, private confirmService: ConfirmService) { }

  ngOnInit(): void {
    this.mParams.pageSize = this.pageSize;
    this.mParams.container = this.container;
    this.getMessages();
  }


  getMessages(useCache=false) {
    this.message=null;    //refresh the message panel
    this.mParams.pageSize=6;
    this.service.setParams(this.mParams);
    this.service.getMessages(useCache).subscribe(response => {
      this.messages=response.data;
      console.log(this.messages);
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    })
  }

  getInboxMessages() {
    this.mParams=new EmailMessageSpecParams();
    this.mParams.container="inbox";
    this.service.setContainer(this.container);
    this.getMessages(true);
    this.toastr.info('inbox');
  }

  getOutboxMessages() {
    this.mParams.container="sent";
    this.service.setContainer(this.container);
    
    this.getMessages(true);
  }

  getDraftMessages() {
    this.mParams.container="draft";
    this.service.setContainer(this.container);
    this.getMessages(true);
  }

  onSearch() {
    const params = this.service.getParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageIndex = 1;
    this.getMessages(true);
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.mParams = new EmailMessageSpecParams();
    this.getMessages();
  }

  onSortSelected(sort: string) {
    this.mParams.sort = sort;
    this.getMessages();
  }
  
  onMsgSelected(msgId: number) {
    this.mParams.id=msgId;
    this.getMessages();
  }

  saveandclose() {

  }

  sendMessage() {

    this.service.sendMessage(this.message).subscribe(response => {
      this.toastr.success('message sent');
    }, error => {
      console.log('send message error', error);
      this.toastr.error('failed to send the email message', error);
    })
  }

  
  deleteMessage(id: number) {
    this.confirmService.confirm('Confirm delete message', 'This cannot be undone').subscribe(result => {
      if (result) {
        this.service.deleteMessage(id).subscribe(() => {
          this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
        })
      }
    })

  }

  
  setMessageFromUsername(msg: IMessage) {
    this.message=msg;
  }
  setMessageFromEmail(msg: IMessage) {
    this.message=msg;
  }

  onPageChanged(event: any) {
    if (this.pageIndex !== event) {
      this.pageIndex = event;
      this.mParams.pageIndex=event;
      this.getMessages(true);
    }
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

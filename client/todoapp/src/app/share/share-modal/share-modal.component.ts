import { Component, Input, OnInit } from '@angular/core';
import { timer } from 'rxjs';

@Component({
  selector: 'app-share-modal',
  templateUrl: './share-modal.component.html',
  styleUrls: ['./share-modal.component.scss']
})
export class ShareModalComponent implements OnInit {

  @Input() shareLink: string;
  copyText: string = "Copy";
  constructor() { }

  ngOnInit(): void {
  }

  copyToClipboard() {
    this.copyText = "Copied";
    navigator.clipboard.writeText(this.shareLink);
    timer(2000).subscribe(() => {
      this.copyText = "Copy";
    });
  }

}

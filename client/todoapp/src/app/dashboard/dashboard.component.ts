import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { LoginComponent } from '../account/login/login.component';
import { BoardService } from '../services/board.service';
import { ShareModalComponent } from '../share/share-modal/share-modal.component';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  createTodoForm!: FormGroup;
  sharingLink = "http://localhost:4200/board/sharing/";
  sharingLinkToken = '';
  bsModalRef: BsModalRef;

  constructor(public boardService: BoardService,  private formBuilder: FormBuilder, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.createTodoForm = this.formBuilder.group({
      task: ['', Validators.required],
    })
  }
  
  createTodo() {
    //this.todoService.createTodoNW(this.createTodoForm.value).subscribe();
    this.boardService.createTodo(this.createTodoForm.value).then(() => {
      this.createTodoForm.reset();
    })
  }

  openShareModal() {
    if(this.sharingLinkToken !== "") {
      this.bsModalRef = this.modalService.show(ShareModalComponent, { initialState: { shareLink: this.sharingLink}});
      return;
    }

    this.boardService.shareBoard().subscribe(
      response => {
        this.sharingLinkToken = response;
        this.sharingLink += this.sharingLinkToken;
        this.bsModalRef = this.modalService.show(ShareModalComponent, { initialState: { shareLink: this.sharingLink}});
      },);
  }

  updateTodo() {

  }

  updateBoard() {
    
  }

}

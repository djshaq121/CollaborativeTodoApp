import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BoardService } from '../services/board.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  createTodoForm!: FormGroup;
  sharingLink = "http://localhost:4200/board/sharing/";
  sharingLinkToken = '';

  constructor(public boardService: BoardService,  private formBuilder: FormBuilder) { }

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
      alert(this.sharingLink);
      return;
    }

    this.boardService.shareBoard().subscribe(
      response => {
        this.sharingLinkToken = response;
        this.sharingLink += this.sharingLinkToken;
        alert(this.sharingLink);
      },);
  }

  updateTodo() {

  }

  updateBoard() {
    
  }

}

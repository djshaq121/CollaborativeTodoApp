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

  updateTodo() {

  }

  updateBoard() {
    
  }

}

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Todo } from './model/todo';

import { TodoService } from './services/todo.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'todoapp';
  createTodoForm!: FormGroup;

  constructor(public todoService: TodoService,  private formBuilder: FormBuilder){}

  ngOnInit(): void {
    this.createTodoForm = this.formBuilder.group({
      task: ['', Validators.required],
    })
    this.todoService.getTodoItem(1).subscribe();
    this.todoService.createHubConnection();
  }

  createTodo() {
    //this.todoService.createTodoNW(this.createTodoForm.value).subscribe();
    this.todoService.createTodo(this.createTodoForm.value).then(() => {
        
    })
  }

}

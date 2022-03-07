import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TodoService } from '../services/todo.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  createTodoForm!: FormGroup;
  
  constructor(public todoService: TodoService,  private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.initForm();
    this.todoService.getTodoItem(1).subscribe();
    this.todoService.createHubConnection();
  }

  initForm() {
    this.createTodoForm = this.formBuilder.group({
      task: ['', Validators.required],
    })
  }

  createTodo() {
    //this.todoService.createTodoNW(this.createTodoForm.value).subscribe();
    this.todoService.createTodo(this.createTodoForm.value).then(() => {
        
    })
  }

}

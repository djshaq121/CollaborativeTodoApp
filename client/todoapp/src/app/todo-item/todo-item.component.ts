import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Todo } from '../model/todo';
import { TodoService } from '../services/todo.service';

@Component({
  selector: 'app-todo-item',
  templateUrl: './todo-item.component.html',
  styleUrls: ['./todo-item.component.scss']
})
export class TodoItemComponent implements OnInit {

  @Input() todo!: Todo;
  editingTask = false;
  originalTaskValue: string;
  todoForm: FormGroup;
  
  constructor(private todoService: TodoService,  private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.originalTaskValue = this.todo.task;
    this.initForm();
  }

  private initForm() {
    this.todoForm = this.formBuilder.group({
      task: [this.originalTaskValue, Validators.required],
    })
  }

  editTask() {
    this.editingTask = true
  }

  updateTask() {
    this.todo.task = this.todoForm.get("task").value;
    this.todoService.updateTodoItem(this.todo).subscribe(() => {
      this.editingTask = false;
    }, error => {
      console.log(error);
    })
  }

  onTaskCompleteChange() {
    this.todoService.updateTodoItem(this.todo).subscribe(() => {
    }, error => {
      console.log(error);
    })
  }

  cancelChange() {
    this.editingTask = false;
    this.todo.task = this.originalTaskValue;

  }

}

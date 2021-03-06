import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Todo } from './model/todo';
import { User } from './model/user';
import { AccountService } from './services/account.service';

import { TodoService } from './services/todo.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'todoapp';
  createTodoForm!: FormGroup;

  constructor(private accountService: AccountService, public todoService: TodoService){}

  ngOnInit(): void {
    this.SetAlreadySignInUser();
  }

  SetAlreadySignInUser() {
    let user: User = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);
    
  }
  
}

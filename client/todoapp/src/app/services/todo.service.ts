import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Todo } from '../model/todo';

@Injectable({
  providedIn: 'root'
})
export class TodoService {

  baseurl = 'https://localhost:5001/'
  constructor(private http: HttpClient) { }

  createBoard(boardName: string) {
    return this.http.post(this.baseurl + 'board', { name: boardName})
  }

  createTodo(createdTodo: Todo) {
    return this.http.post(this.baseurl + 'todo', { createdTodo })
  }

}

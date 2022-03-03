import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Todo } from '../model/todo';
import { map, take } from 'rxjs/operators';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class TodoService {

  baseurl = 'https://localhost:5001/'
  huburl = 'https://localhost:5001/hubs/'
  private hubConnection!: HubConnection;
  private todoItemsSource = new BehaviorSubject<Todo[]>([]);
  todoItems$ = this.todoItemsSource.asObservable();

  constructor(private http: HttpClient) { }


  createHubConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.huburl + 'todo')
      .withAutomaticReconnect()
      .build()

      this.hubConnection
      .start()
      .catch(error => console.log(error));

      this.hubConnection.on('ReceiveTodos', todos => {
        this.todoItemsSource.next(todos);
      })

      this.hubConnection.on('AddNewTodoItem', todo => {
          this.todoItems$.pipe(take(1)).subscribe(todos => {
            this.todoItemsSource.next([...todos, todo]);
          })
      })
  }

  stopHubConnection() {
    this.hubConnection
    .stop()
    .catch(error => console.log(error));
  }

  createBoard(boardName: string) {
    return this.http.post(this.baseurl + 'board', { name: boardName})
  }

  async createTodo(createdTodo: Todo) {
    // 'AddTodo - is the method name on the server'
    return this.hubConnection.invoke('AddTodo', {  boardId: createdTodo.boardId ?? 1, title: createdTodo.title, description: createdTodo.description })
      .catch(error => console.log(error));
    
    //this.http.post(this.baseurl + 'todo', { createdTodo })
  }

  // createTodoNW(createdTodo: Todo) {
  //   return this.http.post(this.baseurl + 'todo', {  boardId: createdTodo.boardId ?? 1, title: createdTodo.title, description: createdTodo.description })
  // }

  getTodoItem(id: number) {
    return this.http.get(this.baseurl + 'todo/' + id.toString()).pipe(
      map(res => {
        console.log(res);
      })
    )
   
  }

}

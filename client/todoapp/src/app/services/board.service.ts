import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, pipe, ReplaySubject } from 'rxjs';
import { Todo } from '../model/todo';
import { map, take } from 'rxjs/operators';
import { Board } from '../model/board';
import { TodoService } from './todo.service';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { User } from '../model/user';

@Injectable({
  providedIn: 'root'
})
export class BoardService {

  baseurl = 'https://localhost:5001/';
  huburl = 'https://localhost:5001/hubs/';

  private hubConnection!: HubConnection;

  private boardsSource = new ReplaySubject<Board[]>(1);
  boards$ = this.boardsSource.asObservable();

  private selectedBoardSource = new BehaviorSubject<Board>(null);
  currentBoard$ = this.selectedBoardSource.asObservable();
  
  constructor(private http: HttpClient, private todoService: TodoService) { }

  async createHubConnection(user: User, boardId: number) {
    await this.stopHubConnection(); // Discount from current hub first

    this.hubConnection = new HubConnectionBuilder()
    .withUrl(this.huburl + 'board?boardId=' + boardId, {
      accessTokenFactory: () => user.token
    })
    .withAutomaticReconnect()
    .build()

    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('ReceiveBoardUpdatedBoard', board => {
      this.selectedBoardSource.next(board);
    })

      this.hubConnection.on('newTodoItem', todo => {
        this.currentBoard$.pipe(take(1)).subscribe(board => {
          board.todos = [...board.todos, todo];
          this.selectedBoardSource.next(board);
        })
    })
  }

  async stopHubConnection() {
    if(this.hubConnection) {
      return this.hubConnection.stop();
    }

    return Promise.resolve();
  }

  createBoard(boardName: string) {
    // const params = new HttpParams();
    // params.set('name', boardName);

    // console.log(params.toString());
    
    return this.http.post<Board>(this.baseurl + 'board?name=' + boardName, {}).pipe(
      map((response) => {
          this.boards$.pipe(take(1)).subscribe(boards => {
            this.boardsSource.next([...boards, response]);
          });
      })
    );
  }

  getBoards() {
    return this.http.get<Board[]>(this.baseurl + 'board').pipe(
      map(reponse => {
        this.boardsSource.next(reponse);
      })
    );
  }

  getTodoItemsForBoard(boardId: number) {
   // return todoService.
  }

  // selectBoard(boardId: number) {
  //   this.boards$.pipe(take(1)).subscribe(boards => {
  //       const board = boards.find(x => x.id == boardId);
  //       this.selectedBoardSource.next(board || null);
        
  //   });
  // }

  async createTodo(todoItem: Todo) {
    const board = this.selectedBoardSource.value;
    return this.hubConnection.invoke('SendTodo', {BoardId: board.id, Task: todoItem.task, IsCompleted: todoItem.isCompleted})
      .catch(error => console.log("error"));
  }


}

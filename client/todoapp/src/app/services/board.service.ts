import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, pipe, ReplaySubject } from 'rxjs';
import { Todo } from '../model/todo';
import { map, take } from 'rxjs/operators';
import { Board } from '../model/board';
import { TodoService } from './todo.service';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { User } from '../model/user';
import { Boards } from '../model/boards';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class BoardService {

  baseurl = 'https://localhost:5001/';
  huburl = 'https://localhost:5001/hubs/';

  private hubConnection!: HubConnection;

  private ownerBoardsSource = new ReplaySubject<Board[]>(1);
  ownerBoards$ = this.ownerBoardsSource.asObservable();

  private sharedBoardsSource = new ReplaySubject<Board[]>(1);
  sharedBoards$ = this.sharedBoardsSource.asObservable();

  private selectedBoardSource = new BehaviorSubject<Board>(null);
  currentBoard$ = this.selectedBoardSource.asObservable();
  
  constructor(private http: HttpClient, private todoService: TodoService,  private accountService: AccountService) {
    this.accountService.logOutUser$.subscribe(() => this.onLogOutUser());
   }

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
        });
    })

    this.hubConnection.on('TodoItemUpdated', todoItem => {
      this.updateTodoItem(todoItem);
    })
  }

  async stopHubConnection() {
    if(this.hubConnection) {
      return this.hubConnection.stop();
    }

    return Promise.resolve();
  }

  onLogOutUser() {
    this.selectedBoardSource.next(null);
    
  }

  private updateTodoItem(updatedTodoItem: Todo) {
    this.currentBoard$.pipe(take(1)).subscribe(board => {
      let foundIndex = board.todos.findIndex(item => item.id == updatedTodoItem.id);
      if (foundIndex == -1)
        return;

        board.todos[foundIndex] = updatedTodoItem;
    })
  }

  createBoard(boardName: string) {
    return this.http.post<Board>(this.baseurl + 'board?name=' + boardName, {}).pipe(
      map((response) => {
          this.ownerBoards$.pipe(take(1)).subscribe(boards => {
            this.ownerBoardsSource.next([...boards, response]);
          });
      })
    );
  }

  getBoards() {
    return this.http.get<Boards>(this.baseurl + 'board').pipe(
      map(reponse => {
        this.ownerBoardsSource.next(reponse.ownerBoards);
        this.sharedBoardsSource.next(reponse.sharedBoards);
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


  // SHARING
  addUserToBoard(token: string) {
    return this.http.post(this.baseurl + 'board/sharing?token=' + token, {});
  }

  shareBoard() {
    const board = this.selectedBoardSource.value;
    return this.http.post<any>(this.baseurl + 'board/sharing/generate/' + board.id, {}).pipe(
      map(response => {
        return response.token;
      })
    );
  }


}

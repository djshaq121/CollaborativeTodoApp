import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { Board } from '../model/board';

@Injectable({
  providedIn: 'root'
})
export class BoardService {

  baseurl = 'https://localhost:5001/';
  
  private boardsSource = new ReplaySubject<Board[]>(1);
  boards$ = this.boardsSource.asObservable();
  
  constructor(private http: HttpClient) { }

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
}

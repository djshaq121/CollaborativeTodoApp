import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Board } from '../model/board';

@Injectable({
  providedIn: 'root'
})
export class BoardService {

  baseurl = 'https://localhost:5001/';
  boards: Board[] = [];

  constructor(private http: HttpClient) { }

  createBoard(boardName: string) {
    // const params = new HttpParams();
    // params.set('name', boardName);

    // console.log(params.toString());
    
    return this.http.post(this.baseurl + 'board?name=' + boardName, {});
  }
}

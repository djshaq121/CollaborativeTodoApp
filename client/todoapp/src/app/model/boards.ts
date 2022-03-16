import { Board } from "./board";

export interface Boards {
    ownerBoards: Board[],
    sharedBoards: Board[]
}
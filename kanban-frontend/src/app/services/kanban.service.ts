import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Board } from '../models/board.model';
import { Column } from '../models/column.model';
import { Card } from '../models/card.model';
import {MoveCardRequest} from '../models/mode-card-request.model';

@Injectable({
  providedIn: 'root'
})
export class KanbanService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:7071/api';

  // ========================
  // BOARD
  // ========================

  getBoards(): Observable<Board[]> {
    return this.http.get<Board[]>(`${this.apiUrl}/boards`);
  }

  getBoardById(id: string): Observable<Board> {
    return this.http.get<Board>(`${this.apiUrl}/boards/${id}`);
  }

  createBoard(board: Partial<Board>): Observable<Board> {
    return this.http.post<Board>(`${this.apiUrl}/boards`, board);
  }

  updateBoard(id: string, board: Board): Observable<Board> {
    return this.http.put<Board>(`${this.apiUrl}/boards/${id}`, board);
  }

  deleteBoard(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/boards/${id}`);
  }

  // ========================
  // COLUMNS
  // ========================

  addColumn(boardId: string, column: Partial<Column>): Observable<Column> {
    return this.http.post<Column>(`${this.apiUrl}/boards/${boardId}/columns`, column);
  }

  updateColumn(boardId: string, columnId: string, column: Column): Observable<Column> {
    return this.http.put<Column>(`${this.apiUrl}/boards/${boardId}/columns/${columnId}`, column);
  }

  deleteColumn(boardId: string, columnId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/boards/${boardId}/columns/${columnId}`);
  }

  // ========================
  // CARDS
  // ========================

  addCard(boardId: string, columnId: string, card: Partial<Card>): Observable<Card> {
    return this.http.post<Card>(`${this.apiUrl}/boards/${boardId}/columns/${columnId}/cards`, card);
  }

  updateCard(boardId: string, columnId: string, cardId: string, card: Card): Observable<Card> {
    return this.http.put<Card>(`${this.apiUrl}/boards/${boardId}/columns/${columnId}/cards/${cardId}`, card);
  }

  deleteCard(boardId: string, columnId: string, cardId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/boards/${boardId}/columns/${columnId}/cards/${cardId}`);
  }

  moveCard(boardId: string, request: MoveCardRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/boards/${boardId}/move-card`, request);
  }
}

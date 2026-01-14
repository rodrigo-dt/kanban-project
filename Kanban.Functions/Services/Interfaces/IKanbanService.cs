using Kanban_Functions.Models;

namespace Kanban_Functions.Services.Interfaces;

public interface IKanbanService
{
    // Boards
    Task CreateBoard(BoardModel board);
    Task UpdateBoard(string id, BoardModel board);
    Task DeleteBoard(string id);
    Task<List<BoardModel>> GetAllBoards();
    Task<BoardModel> GetBoardById(string id);
    
    // Columns
    Task CreateColumn(string boardId, ColumnModel column);
    Task UpdateColumn(string boardId, string columnId, ColumnModel column);
    Task DeleteColumn(string boardId, string columnId);
    
    // Cards
    Task CreateCard(string boardId, string columnId, CardModel card);
    Task UpdateCard(string boardId, string columnId, string cardId, CardModel card);
    Task DeleteCard(string boardId, string columnId, string cardId);
    Task MoveCard(string boardId, string sourceColumnId, string targetColumnId, string cardId);
}
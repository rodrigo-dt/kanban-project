using Kanban_Functions.Models;
using Kanban_Functions.Services.Interfaces;
using MongoDB.Driver;

namespace Kanban_Functions.Services;

public class KanbanService: IKanbanService
{
    private readonly IMongoCollection<BoardModel> _boards;

    public KanbanService(MongoDbContext context)
    {
        _boards = context.Boards;
    }

    // Boards
    
    public async Task CreateBoard(BoardModel board)
    {
        await _boards.InsertOneAsync(board);
    }

    public async  Task UpdateBoard(string id, BoardModel board)
    {
        await  _boards.ReplaceOneAsync(board => board.Id == id, board);
    }

    public async  Task DeleteBoard(string id)
    {
        await  _boards.DeleteOneAsync(board => board.Id == id);
    }

    public async Task<List<BoardModel>> GetAllBoards()
    {
        return await _boards.Find(board => true).ToListAsync();
    }

    public async Task<BoardModel> GetBoardById(string id)
    {
        return await _boards.Find(board => board.Id == id).FirstOrDefaultAsync();
    }
 
    // Columns
    
    public async  Task CreateColumn(string boardId, ColumnModel column)
    {
        var board = await GetBoardById(boardId);
        if (board == null) return;
        
        board.Columns.Add(column);
        await UpdateBoard(boardId, board);
    }

    public async Task UpdateColumn(string boardId, string columnId, ColumnModel column)
    {
        var board = await GetBoardById(boardId);
        if (board == null) return;
        
        var index = board.Columns.FindIndex(column => column.Id == columnId);
        if (index != -1)
        {
            column.Cards = board.Columns[index].Cards;
            board.Columns[index] = column;
            
            await UpdateBoard(boardId, board);
        }
    }

    public async Task DeleteColumn(string boardId, string columnId)
    {
        var board = await GetBoardById(boardId);
        if (board == null) return;
        
        board.Columns.RemoveAll(column => column.Id == columnId);
        await UpdateBoard(boardId, board);
    }

    // Cards
    
    public async Task CreateCard(string boardId, string columnId, CardModel card)
    {
        var board = await GetBoardById(boardId);
        if (board == null) return;
        
        var column = board.Columns.FirstOrDefault(column => column.Id == columnId);
        if (column != null)
        {
            column.Cards.Add(card);
            await UpdateBoard(boardId, board);
        }
    }

    public async Task UpdateCard(string boardId, string columnId, string cardId, CardModel card)
    {
        var board = await GetBoardById(boardId);
        if (board == null) return;
        
        var column = board.Columns.FirstOrDefault(column => column.Id == columnId);
        if (column != null)
        {
            var cardIndex = column.Cards.FindIndex(card => card.Id == cardId);
            if (cardIndex != -1)
            {
                column.Cards[cardIndex] = card;
                await UpdateBoard(boardId, board);
            }
        }
    }

    public async Task DeleteCard(string boardId, string columnId, string cardId)
    {
        var board = await GetBoardById(boardId);
        if (board == null) return;
        
        var column = board.Columns.FirstOrDefault(column => column.Id == columnId);
        if (column != null)
        {
            column.Cards.RemoveAll(card => card.Id == cardId);
            await UpdateBoard(boardId, board);
        }
        
    }

    public async Task MoveCard(string boardId, string sourceColumnId, string targetColumnId, string cardId)
    {
        var board = await GetBoardById(boardId);
        if (board == null) return;
        
        var sourceCol = board.Columns.FirstOrDefault(c => c.Id == sourceColumnId);
        var targetCol = board.Columns.FirstOrDefault(c => c.Id == targetColumnId);

        if (sourceCol != null && targetCol != null)
        {
            var card = sourceCol.Cards.FirstOrDefault(card => card.Id == cardId);

            if (card != null)
            {
                sourceCol.Cards.Remove(card);
                targetCol.Cards.Add(card);
                await UpdateBoard(boardId, board);
            }
        }
    }
}
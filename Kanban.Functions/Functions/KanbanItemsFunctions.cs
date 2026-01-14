using Kanban_Functions.Models;
using Kanban_Functions.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Kanban_Functions.Functions;

public class KanbanItemsFunctions
{
    private readonly ILogger<KanbanItemsFunctions> _logger;
    private readonly IKanbanService _service;

    public KanbanItemsFunctions(ILogger<KanbanItemsFunctions> logger, IKanbanService service)
    {
        _logger = logger;
        _service = service;
    }

    // Column

    [Function("CreateColumn")]
    public async Task<IActionResult> CreateColumn(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "boards/{boardId}/columns")]
        HttpRequest req, string boardId)
    {
        var column = await req.ReadFromJsonAsync<ColumnModel>();
        if (column == null)
            return new BadRequestObjectResult("[CreateColumn] Invalid column data.");

        if (string.IsNullOrEmpty(column.Id)) column.Id = Guid.NewGuid().ToString();

        await _service.CreateColumn(boardId, column);
        return new OkObjectResult(column);
    }

    [Function("UpdateColumn")]
    public async Task<IActionResult> UpdateColumn(
        [HttpTrigger(AuthorizationLevel.Function, "put",
            Route = "boards/{boardId}/columns/{columnId}")]
        HttpRequest req, string boardId, string columnId)
    {
        var column = await req.ReadFromJsonAsync<ColumnModel>();

        if (column == null)
            return new BadRequestObjectResult("[UpdateColumn] Invalid column data.");

        if (column.Id != columnId)
            return new BadRequestObjectResult(
                "[UpdateColumn] The informed ID is different from the URL.");

        await _service.UpdateColumn(boardId, columnId, column);
        return new OkObjectResult(column);
    }

    [Function("DeleteColumn")]
    public async Task<IActionResult> DeleteColumn(
        [HttpTrigger(AuthorizationLevel.Function, "delete",
            Route = "boards/{boardId}/columns/{columnId}")]
        HttpRequest req,
        string boardId, string columnId)
    {
        await _service.DeleteColumn(boardId, columnId);
        return new NoContentResult();
    }

    // Card

    [Function("CreateCard")]
    public async Task<IActionResult> CreateCard(
        [HttpTrigger(AuthorizationLevel.Function, "post",
            Route = "boards/{boardId}/columns/{columnId}/cards")]
        HttpRequest req, string boardId, string columnId)
    {
        var card = await req.ReadFromJsonAsync<CardModel>();
        if (card == null) return new BadRequestResult();

        if (string.IsNullOrEmpty(card.Id)) card.Id = Guid.NewGuid().ToString();

        if (card.CreatedAt == default) card.CreatedAt = DateTime.UtcNow;

        await _service.CreateCard(boardId, columnId, card);
        return new OkObjectResult(card);
    }

    [Function("UpdateCard")]
    public async Task<IActionResult> UpdateCard(
        [HttpTrigger(AuthorizationLevel.Function, "put",
            Route = "boards/{boardId}/columns/{columnId}/cards/{cardId}")]
        HttpRequest req,
        string boardId, string columnId, string cardId)
    {
        var card = await req.ReadFromJsonAsync<CardModel>();

        if (card == null) return new BadRequestObjectResult("Dados do cartão inválidos");

        if (card.Id != cardId)
            return new BadRequestObjectResult("O ID do cartão no corpo difere da URL.");

        await _service.UpdateCard(boardId, columnId, cardId, card);

        return new OkObjectResult(card);
    }

    [Function("DeleteCard")]
    public async Task<IActionResult> DeleteCard(
        [HttpTrigger(AuthorizationLevel.Function, "delete",
            Route = "boards/{boardId}/columns/{columnId}/cards/{cardId}")]
        HttpRequest req,
        string boardId, string columnId, string cardId)
    {
        await _service.DeleteCard(boardId, columnId, cardId);
        return new NoContentResult();
    }

    [Function("MoveCard")]
    public async Task<IActionResult> MoveCard(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "boards/{boardId}/move-card")]
        HttpRequest req,
        string boardId)
    {
        var moveRequest = await req.ReadFromJsonAsync<MoveCardRequest>();

        if (moveRequest == null)
            return new BadRequestObjectResult("[MoveCard] Invalid data for moving card.");

        await _service.MoveCard(boardId, moveRequest.SourceColumnId, moveRequest.TargetColumnId,
            moveRequest.CardId);

        return new OkResult();
    }
}

public class MoveCardRequest
{
    public string CardId { get; set; }
    public string SourceColumnId { get; set; }
    public string TargetColumnId { get; set; }
}
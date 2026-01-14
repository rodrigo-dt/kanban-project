using Kanban_Functions.Models;
using Kanban_Functions.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Kanban_Functions.Functions;

public class KanbanFunctions
{
    private readonly ILogger<KanbanFunctions> _logger;
    private readonly IKanbanService _service;

    public KanbanFunctions(ILogger<KanbanFunctions> logger, IKanbanService service)
    {
        _logger = logger;
        _service = service;
    }

    [Function("CreateBoard")]
    public async Task<IActionResult> CreateBoard(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "boards")]
        HttpRequest req)
    {
        var board = await req.ReadFromJsonAsync<BoardModel>();

        if (board == null) return new BadRequestObjectResult("[CreateBoard] Invalid board data.");

        await _service.CreateBoard(board);
        return new CreatedResult($"/boards/{board.Id}", board);
    }

    [Function("UpdateBoard")]
    public async Task<IActionResult> UpdateBoard(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "boards/{id}")] HttpRequest req, string id)
    {
        var board = await req.ReadFromJsonAsync<BoardModel>();
        
        if (board == null) return new BadRequestObjectResult("[UpdateBoard] Invalid board data.");

        if (board.Id != id)
        {
            return new BadRequestObjectResult("[UpdateBoard] The informed ID is different from the URL.");
        }

        await _service.UpdateBoard(id, board);
        return new OkObjectResult(board);
    }

    [Function("DeleteBoard")]
    public async Task<IActionResult> DeleteBoard(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "boards/{id}")]
        HttpRequest req, string id)
    {
        await _service.DeleteBoard(id);
        return new NoContentResult();
    }

    [Function("GetBoards")]
    public async Task<IActionResult> GetBoards(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "boards")]
        HttpRequest req)
    {
        var boards = await _service.GetAllBoards();
        return new OkObjectResult(boards);
    }

    [Function("GetBoardById")]
    public async Task<IActionResult> GetBoardById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "boards/{id}")]
        HttpRequest req, string id)
    {
        var board = await _service.GetBoardById(id);
        if (board == null) return new NotFoundResult();

        return new OkObjectResult(board);
    }
}
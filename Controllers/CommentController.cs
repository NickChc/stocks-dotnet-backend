using Api.Dtos.Comment;
using Api.Interfaces;
using Api.Mappers;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController(ICommentRepo commentRepo, IStockRepo stockRepo) : ControllerBase
{
    private readonly ICommentRepo _commentRepo = commentRepo;
    private readonly IStockRepo _stockRepo = stockRepo;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        List<Comment> comments = await _commentRepo.GetAllAsync();

        var commentDtos = comments.Select((c) => c.ToDto());

        return Ok(commentDtos);
    }

    [HttpGet("{id:int}", Name = "GetCommentById")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var comment = await _commentRepo.GetByIdAsync(id);

        if (comment is null)
        {
            return NotFound();
        }

        return Ok(comment.ToDto());
    }

    [HttpPost("{stockId:int}")]
    public async Task<IActionResult> Create(
        [FromRoute()] int stockId,
        [FromBody] CreateCommentDto createCommentDto
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!await _stockRepo.StockExists(stockId))
        {
            return BadRequest("Stock doesn't exist");
        }

        Comment commentModel = createCommentDto.ToComment(stockId);
        await _commentRepo.CreateAsync(commentModel);

        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromBody] UpdateCommentDto updateCommentDto
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Comment? existingComment = await _commentRepo.UpdateAsync(id, updateCommentDto.ToComment());

        if (existingComment is null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Comment? comment = await _commentRepo.DeleteAsync(id);

        if (comment is null)
        {
            return NotFound("Comment doesn't exist");
        }

        return NoContent();
    }
}

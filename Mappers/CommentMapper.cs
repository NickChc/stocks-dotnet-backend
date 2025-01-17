using Api.Dtos.Comment;
using Api.Models;

namespace Api.Mappers;

public static class CommentMapper
{
    public static CommentDto ToDto(this Comment comment)
    {
        return new()
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedOn = comment.CreatedOn,
            StockId = comment.StockId,
            Title = comment.Title,
        };
    }

    public static Comment ToComment(this CreateCommentDto commentDto, int stockId)
    {
        return new()
        {
            Content = commentDto.Content,
            Title = commentDto.Title,
            StockId = stockId,
        };
    }

    public static Comment ToComment(this UpdateCommentDto commentDto)
    {
        return new() { Content = commentDto.Content, Title = commentDto.Title };
    }
}

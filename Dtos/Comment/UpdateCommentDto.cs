using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Comment;

public class UpdateCommentDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be at least 5 characters")]
    [MaxLength(120, ErrorMessage = "Title cannot be over 120 characters")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(5, ErrorMessage = "Content must be at least 5 characters")]
    [MaxLength(280, ErrorMessage = "Content cannot be over 280 characters")]
    public string Content { get; set; } = string.Empty;
}

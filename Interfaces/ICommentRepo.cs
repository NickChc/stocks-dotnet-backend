using Api.Models;

namespace Api.Interfaces;

public interface ICommentRepo
{
    Task<List<Comment>> GetAllAsync();

    Task<Comment?> GetByIdAsync(int id);

    Task<Comment> CreateAsync(Comment commentModel);

    Task<Comment?> UpdateAsync(int id, Comment commentModel);

    Task<Comment?> DeleteAsync(int id);
}

using Api.Data;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class CommentRepo(ApplicationDbContext context) : ICommentRepo
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comment.AsNoTracking().ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        var comment = await _context.Comment.FindAsync(id);

        if (comment is null)
        {
            return null;
        }

        return comment;
    }

    public async Task<Comment> CreateAsync(Comment commentModel)
    {
        await _context.Comment.AddAsync(commentModel);
        await _context.SaveChangesAsync();

        return commentModel;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
    {
        var existingComment = await _context.Comment.FindAsync(id);

        if (existingComment is null)
        {
            return null;
        }

        existingComment.Title = commentModel.Title;
        existingComment.Content = commentModel.Content;

        await _context.SaveChangesAsync();

        return existingComment;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        Comment? commentModel = await _context.Comment.FindAsync(id);

        if (commentModel is null)
        {
            return null;
        }

        _context.Comment.Remove(commentModel);
        await _context.SaveChangesAsync();

        return commentModel;
    }
}

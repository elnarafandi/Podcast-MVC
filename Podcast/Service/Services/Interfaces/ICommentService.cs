using Domain.Entities;
using Service.ViewModels.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface ICommentService
    {
        Task AddCommentAsync(Comment comment);
        Task<IEnumerable<Comment>> GetCommentsByPodcastIdAsync(int podcastId);
        Task<Comment> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<IEnumerable<CommentAdminVM>> GetAllAsync();
    }
}

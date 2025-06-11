using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task AddCommentAsync(Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.Content))
                throw new ArgumentException("Comment content cannot be empty.");
            await _commentRepository.CreateAsync(comment);
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            await _commentRepository.DeleteAsync(comment);
        }

        public async Task<IEnumerable<CommentAdminVM>> GetAllAsync()
        {
            var commentDb=await _commentRepository.GetAllAsync();
            var comment = commentDb.Select(c => new CommentAdminVM
            {
                Id = c.Id,
                Content = c.Content,
                AppUser = c.AppUser,
                Podcast = c.Podcast
            }).ToList();
            return comment;
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            
            return comment;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPodcastIdAsync(int podcastId)
        {
            return await _commentRepository.GetCommentsByPodcastIdAsync(podcastId);
        }
    }
}

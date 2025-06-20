using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.PodcastCategory;
using Service.ViewModels.Podcast;
using Service.ViewModels.TeamMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service.Services
{
    public class PodcastCategoryService : IPodcastCategoryService
    {
        private readonly IPodcastCategoryRepository _podcastCategoryRepository;
        public PodcastCategoryService(IPodcastCategoryRepository podcastCategoryRepository)
        {
            _podcastCategoryRepository = podcastCategoryRepository;
        }
        public async Task CreateAsync(PodcastCategoryCreateVM request)
        {
            if (await _podcastCategoryRepository.ExistsByNameAsync(request.Name))
                throw new InvalidOperationException("A category with the same name already exists.");

            PodcastCategory category = new PodcastCategory
            {
                Name = request.Name
            };

            await _podcastCategoryRepository.CreateAsync(category);
        }

        public async Task DeleteAsync(int id)
        {
            var podcastCategory= await _podcastCategoryRepository.GetByIdAsync(id);
            await _podcastCategoryRepository.DeleteAsync(podcastCategory);
        }

        public async Task EditAsync(int id, PodcastCategoryEditVM request)
        {
            var normalizedNewName = request.Name.Trim().ToLower();

            var allCategories = await _podcastCategoryRepository.GetAllAsync();

            bool exists = allCategories
                .Any(c => c.Id != id && c.Name.Trim().ToLower() == normalizedNewName);

            if (exists)
                throw new InvalidOperationException("A category with the same name already exists.");

            var category = await _podcastCategoryRepository.GetByIdAsync(id);
            category.Name = request.Name;
            await _podcastCategoryRepository.EditAsync(category);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _podcastCategoryRepository.ExistsByNameAsync(name);
        }

        public async Task<List<PodcastCategoryAdminVM>> GetAllAsync()
        {
            var podcastCategories= await _podcastCategoryRepository.GetAllAsync();
            var categories= podcastCategories.Select(pc=>new PodcastCategoryAdminVM
            {
                Id = pc.Id,
                Name = pc.Name,
                Podcasts=pc.Podcasts
            }).ToList();
            return categories;
        }

        public async Task<PodcastCategoryAdminVM> GetByIdAsync(int id)
        {
            var podcastCategory = await _podcastCategoryRepository.GetByIdAsync(id);
            PodcastCategoryAdminVM category = new PodcastCategoryAdminVM
            {
                Id=podcastCategory.Id,
                Name = podcastCategory.Name,
                Podcasts = podcastCategory.Podcasts
            };
            return category;
        }
    }
}

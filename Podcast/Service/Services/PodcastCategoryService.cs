using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.PodcastCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var category = await _podcastCategoryRepository.GetByIdAsync(id);
            category.Name = request.Name;
            await _podcastCategoryRepository.EditAsync(category);
        }

        public async Task<List<PodcastCategoryAdminVM>> GetAllAsync()
        {
            var podcastCategories= await _podcastCategoryRepository.GetAllAsync();
            var categories= podcastCategories.Select(pc=>new PodcastCategoryAdminVM
            {
                Id = pc.Id,
                Name = pc.Name
            }).ToList();
            return categories;
        }

        public async Task<PodcastCategoryAdminVM> GetByIdAsync(int id)
        {
            var podcastCategory = await _podcastCategoryRepository.GetByIdAsync(id);
            PodcastCategoryAdminVM category = new PodcastCategoryAdminVM
            {
                Id=podcastCategory.Id,
                Name = podcastCategory.Name
            };
            return category;
        }
    }
}

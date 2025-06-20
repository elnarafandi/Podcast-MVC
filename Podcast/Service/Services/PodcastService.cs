using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Repository.Repositories.Interfaces;
using Service.Helpers.Extensions;
using Service.Services.Interfaces;
using Service.ViewModels.Guest;
using Service.ViewModels.Podcast;
using Service.ViewModels.TeamMember;
using Service.ViewModels.PodcastCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class PodcastService : IPodcastService
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly IWebHostEnvironment _env;
        public PodcastService(IPodcastRepository podcastRepository,
                              IWebHostEnvironment env)
        {
            _podcastRepository = podcastRepository;
            _env = env;
        }
        public async Task CreateAsync(PodcastCreateVM request)
        {
            if (!request.UploadImage.CheckFileType("image"))
                throw new InvalidOperationException("Only image files are allowed.");

            
            if (!request.UploadImage.CheckFileSize(1024))
                throw new InvalidOperationException("File size should be less than 1 MB.");

            string normalizedTitle = request.Title.Trim().ToLower();

            var allPodcasts = await _podcastRepository.GetAllAsync();

            bool exists = allPodcasts.Any(p => p.Title.Trim().ToLower() == normalizedTitle);

            if (exists)
                throw new InvalidOperationException("A podcast with the same title already exists.");

            string fileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
            string filePath = _env.GenerateFilePath("assets/images/podcasts", fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await request.UploadImage.CopyToAsync(stream);
            }
            Podcast podcast = new Podcast()
            {
                Title = request.Title,
                Description = request.Description,
                Image = fileName,
                TeamMemberId = request.TeamMemberId,
                PodcastCategoryId = request.PodcastCategoryId
            };
            await _podcastRepository.CreateAsync(podcast);
        }

        public async Task DeleteAsync(int id)
        {
            var podcast=await _podcastRepository.GetByIdAsync(id);
            string filePath = _env.GenerateFilePath("assets/images/podcasts", podcast.Image);
            filePath.DeleteFile();
            await _podcastRepository.DeleteAsync(podcast);
        }

        public async Task EditAsync(int id, PodcastEditVM request)
        {
            string normalizedTitle = request.Title.Trim().ToLower();

            var allPodcasts = await _podcastRepository.GetAllAsync();

            bool exists = allPodcasts.Any(p =>
                p.Id != id && p.Title.Trim().ToLower() == normalizedTitle);

            if (exists)
                throw new InvalidOperationException("A podcast with the same title already exists.");

            var podcast = await _podcastRepository.GetByIdAsync(id);
            if (podcast == null) throw new Exception("Podcast not found.");

            if (request.UploadImage != null)
            {
                if (!request.UploadImage.CheckFileType("image"))
                    throw new InvalidOperationException("Only image files are allowed.");

                if (!request.UploadImage.CheckFileSize(1024))
                    throw new InvalidOperationException("File size should be less than 1 MB.");

                string oldFilePath = _env.GenerateFilePath("assets/images/podcasts", podcast.Image);
                oldFilePath.DeleteFile();

                string fileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
                string filePath = _env.GenerateFilePath("assets/images/podcasts", fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.UploadImage.CopyToAsync(stream);
                }

                podcast.Image = fileName;
            }
            podcast.Title = request.Title;
            podcast.Description = request.Description;
            podcast.PodcastCategoryId = request.PodcastCategoryId;
            podcast.TeamMemberId = request.TeamMemberId;
            await _podcastRepository.EditAsync(podcast);
        }

        public async Task<IEnumerable<PodcastAdminVM>> FilterAsync(string searchText, List<int>? categoryIds, List<int>? teamMemberIds)
        {
            var podcastsDb= await _podcastRepository.FilterAsync(searchText, categoryIds, teamMemberIds);
            var podcasts = podcastsDb.Select(p => new PodcastAdminVM
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Image = p.Image,
                TeamMember = p.TeamMember
            }).ToList();
            return podcasts;
        }

        public async Task<IEnumerable<PodcastAdminVM>> FilterByCategoryAsync(string searchText,string categoryName)
        {
            var podcastsDb = await _podcastRepository.GetAllWithConditionAsync(p => p.Title.Trim().ToLower().Contains(searchText.Trim().ToLower()) && p.PodcastCategory.Name==categoryName);
            var podcasts = podcastsDb.Select(p => new PodcastAdminVM
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Image = p.Image,
                TeamMember = p.TeamMember
            }).ToList();
            return podcasts;
        }

        public async Task<List<PodcastAdminVM>> GetAllAsync()
        {
            var podcastsDb=await _podcastRepository.GetAllAsync();
            var podcasts=podcastsDb.Select(m=>new PodcastAdminVM
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                Image = m.Image,
                TeamMember = m.TeamMember,
                PodcastCategory =m.PodcastCategory
            }).ToList();
            return podcasts;
        }

        public async Task<IEnumerable<PodcastAdminVM>> GetAllByCategoryAsync(int categoryId)
        {
            var podcastsDb = await _podcastRepository.GetAllWithConditionAsync(p => p.PodcastCategoryId==categoryId);
            var podcasts = podcastsDb.Select(p => new PodcastAdminVM
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Image = p.Image,
                TeamMember = p.TeamMember
            }).ToList();
            return podcasts;
        }

        public async Task<IEnumerable<PodcastAdminVM>> GetAllByCategoryShowMoreAsync(int categoryId, int skip = 0, int take = 8)
        {
            var podcastsDb= await _podcastRepository.GetPodcastsAsync(categoryId,skip,take);
            var podcasts = podcastsDb.Select(p => new PodcastAdminVM
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Image = p.Image,
                TeamMember = p.TeamMember
            }).ToList();
            return podcasts;
        }

        public async Task<IEnumerable<PodcastAdminVM>> GetAllByCategorySortedByFollowCountAsync(int categoryId)
        {
            var podcastsDb= await _podcastRepository.GetAllByCategorySortedByFollowCountAsync(categoryId);
            var podcasts = podcastsDb.Select(p => new PodcastAdminVM
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Image = p.Image,
                TeamMember = p.TeamMember
            }).ToList();
            return podcasts;
        }

        public async Task<IEnumerable<PodcastAdminVM>> GetAllByCategorySortedByFollowCountShowMoreAsync(int categoryId, int skip = 0, int take = 8)
        {
            var podcastsDb=await _podcastRepository.GetAllByCategorySortedByFollowCountShowMoreAsync(categoryId, skip, take);
            var podcasts = podcastsDb.Select(p => new PodcastAdminVM
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Image = p.Image,
                TeamMember = p.TeamMember
            }).ToList();
            return podcasts;
        }
        public async Task<IEnumerable<PodcastAdminVM>> GetAllByCategorySortedByFollowCountLessShowMoreAsync(int categoryId, int skip = 0, int take = 8)
        {
            var podcastsDb = await _podcastRepository.GetAllByCategorySortedByFollowCountLessShowMoreAsync(categoryId, skip, take);
            var podcasts = podcastsDb.Select(p => new PodcastAdminVM
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Image = p.Image,
                TeamMember = p.TeamMember
            }).ToList();
            return podcasts;
        }

        public async Task<PodcastAdminVM> GetByIdAsync(int id)
        {
            var podcastDb= await _podcastRepository.GetByIdAsync(id);
            PodcastAdminVM podcast = new PodcastAdminVM
            {
                Id = podcastDb.Id,
                Title = podcastDb.Title,
                Description = podcastDb.Description,
                Image = podcastDb.Image,
                TeamMember = podcastDb.TeamMember,
                PodcastCategory = podcastDb.PodcastCategory,
                Episodes =podcastDb.Episodes
            };
            return podcast;
        }

        public async Task<IEnumerable<PodcastAdminVM>> SearchByTitleAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<PodcastAdminVM>();
            var podcastsDb = await _podcastRepository.GetAllWithConditionAsync(p=>p.Title.Trim().ToLower().Contains(searchText.Trim().ToLower()));
            var podcasts=podcastsDb.Select(p=>new PodcastAdminVM
            {
                Id=p.Id,
                Title=p.Title,
                Description=p.Description,
                Image = p.Image,
                TeamMember = p.TeamMember
            }).ToList();
            return podcasts;
        }

        

        public async Task<IEnumerable<PodcastAdminVM>> GetAllByCategorySortedByOldestAsync(int categoryId, int skip, int take)
        {
            var podcastsDb= await _podcastRepository.GetAllByCategorySortedByOldestAsync(categoryId, skip, take);
            var podcasts = podcastsDb.Select(p => new PodcastAdminVM
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Image = p.Image,
                TeamMember = p.TeamMember
            }).ToList();
            return podcasts;
        }




    }
}

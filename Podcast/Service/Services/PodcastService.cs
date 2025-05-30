using Microsoft.AspNetCore.Hosting;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.Podcast;
using Service.ViewModels.TeamMember;
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
        public Task CreateAsync(PodcastCreateVM request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditAsync(int id, PodcastEditVM request)
        {
            throw new NotImplementedException();
        }

        public Task<List<TeamMemberAdminVM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TeamMemberAdminVM> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}

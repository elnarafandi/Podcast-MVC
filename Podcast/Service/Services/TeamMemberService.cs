using Azure.Core;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Repository.Repositories.Interfaces;
using Service.Helpers.Extensions;
using Service.Services.Interfaces;
using Service.ViewModels.TeamMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly IWebHostEnvironment _env;
        public TeamMemberService(ITeamMemberRepository teamMemberRepository,
                                 IWebHostEnvironment env)
        {
            _teamMemberRepository = teamMemberRepository;
            _env = env;
        }
        public async Task CreateAsync(TeamMemberCreateVM request)
        {
            string fileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
            string filePath = _env.GenerateFilePath("assets/images/home", fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await request.UploadImage.CopyToAsync(stream);
            }
            TeamMember member = new TeamMember()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Information = request.Information,
                SocialMedia = request.SocialMedia,
                Image=fileName
            };
            await _teamMemberRepository.CreateAsync(member);
        }

        public async Task DeleteAsync(int id)
        {
            var teamMember= await _teamMemberRepository.GetByIdAsync(id);
            string filePath = _env.GenerateFilePath("assets/images/home", teamMember.Image);
            filePath.DeleteFile();
            await _teamMemberRepository.DeleteAsync(teamMember);
        }

        public async Task EditAsync(int id, TeamMemberEditVM request)
        {
            var member = await _teamMemberRepository.GetByIdAsync(id);
            if (request.UploadImage != null)
            {
                string oldFilePath = _env.GenerateFilePath("assets/images/home", member.Image);
                oldFilePath.DeleteFile();
                string fileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
                string filePath = _env.GenerateFilePath("assets/images/home", fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.UploadImage.CopyToAsync(stream);
                }
                member.Image = fileName;
            }
            member.FirstName= request.FirstName;
            member.LastName= request.LastName;
            member.Information = request.Information;
            member.SocialMedia = request.SocialMedia;
            await _teamMemberRepository.EditAsync(member);
        }

        public async Task<List<TeamMemberAdminVM>> GetAllAsync()
        {
            var teamMembers= await _teamMemberRepository.GetAllAsync();
            var members= teamMembers.Select(tm => new TeamMemberAdminVM
            {
                Id = tm.Id,
                FirstName = tm.FirstName,
                LastName = tm.LastName,
                Information=tm.Information,
                SocialMedia=tm.SocialMedia,
                Image=tm.Image
            }).ToList();
            return members;
        }

        public async Task<TeamMemberAdminVM> GetByIdAsync(int id)
        {
            var teamMember = await _teamMemberRepository.GetByIdAsync(id);
            TeamMemberAdminVM member = new TeamMemberAdminVM()
            {
                Id = teamMember.Id,
                FirstName = teamMember.FirstName,
                LastName = teamMember.LastName,
                Information = teamMember.Information,
                SocialMedia = teamMember.SocialMedia,
                Image = teamMember.Image
            };
            return member;
        }
    }
}

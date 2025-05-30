using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Repository.Repositories;
using Repository.Repositories.Interfaces;
using Service.Helpers.Extensions;
using Service.Services.Interfaces;
using Service.ViewModels.Guest;
using Service.ViewModels.TeamMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository _guestRepository;
        private readonly IWebHostEnvironment _env;
        public GuestService(IGuestRepository guestRepository,
                            IWebHostEnvironment env)
        {
            _guestRepository = guestRepository;
            _env = env;
        }
        public async Task CreateAsync(GuestCreateVM request)
        {
            string fileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
            string filePath = _env.GenerateFilePath("assets/images/home", fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await request.UploadImage.CopyToAsync(stream);
            }
            Guest guest = new Guest()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Information = request.Information,
                SocialMedia = request.SocialMedia,
                Image = fileName
            };
            await _guestRepository.CreateAsync(guest);
        }

        public async Task DeleteAsync(int id)
        {
            var teamMember = await _guestRepository.GetByIdAsync(id);
            string filePath = _env.GenerateFilePath("assets/images/home", teamMember.Image);
            filePath.DeleteFile();
            await _guestRepository.DeleteAsync(teamMember);
        }

        public async Task EditAsync(int id, GuestEditVM request)
        {
            var guest = await _guestRepository.GetByIdAsync(id);
            if (request.UploadImage != null)
            {
                string oldFilePath = _env.GenerateFilePath("assets/images/home", guest.Image);
                oldFilePath.DeleteFile();
                string fileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
                string filePath = _env.GenerateFilePath("assets/images/home", fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.UploadImage.CopyToAsync(stream);
                }
                guest.Image = fileName;
            }
            guest.FirstName = request.FirstName;
            guest.LastName = request.LastName;
            guest.Information = request.Information;
            guest.SocialMedia = request.SocialMedia;
            await _guestRepository.EditAsync(guest);
        }

        public async Task<List<GuestAdminVM>> GetAllAsync()
        {
            var guestsDb = await _guestRepository.GetAllAsync();
            var guests = guestsDb.Select(g => new GuestAdminVM
            {
                Id = g.Id,
                FirstName = g.FirstName,
                LastName = g.LastName,
                Information = g.Information,
                SocialMedia = g.SocialMedia,
                Image = g.Image
            }).ToList();
            return guests;
        }

        public async Task<GuestAdminVM> GetByIdAsync(int id)
        {
            var guestDb = await _guestRepository.GetByIdAsync(id);
            GuestAdminVM guest = new GuestAdminVM()
            {
                Id = guestDb.Id,
                FirstName = guestDb.FirstName,
                LastName = guestDb.LastName,
                Information = guestDb.Information,
                SocialMedia = guestDb.SocialMedia,
                Image = guestDb.Image
            };
            return guest;
        }
    }
}

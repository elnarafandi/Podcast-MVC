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
            // Fayl tipi və ölçü yoxlamaları
            if (!request.UploadImage.CheckFileType("image"))
                throw new InvalidOperationException("Only image files are allowed.");

            if (!request.UploadImage.CheckFileSize(1024)) // 1 MB limit
                throw new InvalidOperationException("Image size should be less than 1 MB.");

            // Email təkrarı yoxlanışı
            var allGuests = await _guestRepository.GetAllAsync();
            bool emailExists = allGuests.Any(g => g.Email.Trim().ToLower() == request.Email.Trim().ToLower());

            if (emailExists)
                throw new InvalidOperationException("A guest with this email already exists.");

            // Şəkil faylının yaddaşa yazılması
            string fileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
            string filePath = _env.GenerateFilePath("assets/images/home", fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await request.UploadImage.CopyToAsync(stream);
            }

            // Yeni qonaq obyektinin yaradılması
            Guest guest = new Guest()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
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
            if (guest == null)
                throw new Exception("Guest not found.");

            // Email təkrarı yoxlanması
            var existingGuests = await _guestRepository.GetAllAsync();
            bool emailExists = existingGuests.Any(g => g.Id != id &&
                g.Email.Trim().ToLower() == request.Email.Trim().ToLower());

            if (emailExists)
                throw new InvalidOperationException("A guest with this email already exists.");

            // Yeni şəkil yüklənibsə, yoxlama və dəyişmə
            if (request.UploadImage != null)
            {
                if (!request.UploadImage.CheckFileType("image"))
                    throw new InvalidOperationException("Only image files are allowed.");

                if (!request.UploadImage.CheckFileSize(1024)) // 1 MB limit
                    throw new InvalidOperationException("Image size must be less than 1 MB.");

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
            guest.Email = request.Email;
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
                Email= g.Email,
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
                Email = guestDb.Email,
                Information = guestDb.Information,
                SocialMedia = guestDb.SocialMedia,
                Image = guestDb.Image
            };
            return guest;
        }
    }
}

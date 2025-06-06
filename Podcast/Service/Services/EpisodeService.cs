using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Repository.Repositories.Interfaces;
using Service.Helpers.Extensions;
using Service.Services.Interfaces;
using Service.ViewModels.Episode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class EpisodeService : IEpisodeService
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IWebHostEnvironment _env;
        public EpisodeService(IEpisodeRepository episodeRepository,
                                 IWebHostEnvironment env)
        {
            _episodeRepository = episodeRepository;
            _env = env;
        }
        public async Task CreateAsync(EpisodeCreateVM request)
        {
            string fileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
            string filePath = _env.GenerateFilePath("assets/images/podcasts", fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await request.UploadImage.CopyToAsync(stream);
            }

            string audioFileName = Guid.NewGuid().ToString() + "-" + request.AudioFile.FileName;
            string audioFilePath = _env.GenerateFilePath("audios", audioFileName);
            using (FileStream stream = new FileStream(audioFilePath, FileMode.Create))
            {
                await request.AudioFile.CopyToAsync(stream);
            }

            Episode episode = new Episode();
            episode.Title = request.Title;
            episode.Description = request.Description;
            episode.PodcastId = request.PodcastId;
            episode.Image = fileName;
            episode.Audio = audioFileName;

            if (request.GuestIds != null && request.GuestIds.Any())
            {
                episode.EpisodeGuests = new List<EpisodeGuest>();

                foreach (var guestId in request.GuestIds)
                {
                    episode.EpisodeGuests.Add(new EpisodeGuest
                    {
                        EpisodeId = episode.Id,
                        GuestId = guestId
                    });
                }
            }

            await _episodeRepository.CreateAsync(episode);

        }

        public async Task DeleteAsync(int id)
        {
            var episode= await _episodeRepository.GetByIdAsync(id);
            string filePath = _env.GenerateFilePath("assets/images/podcasts", episode.Image);
            filePath.DeleteFile();
            string audioFilePath = _env.GenerateFilePath("audios", episode.Audio);
            audioFilePath.DeleteFile();

            await _episodeRepository.DeleteAsync(episode);
        }

        public async Task EditAsync(int id, EpisodeEditVM request)
        {
            var episode = await _episodeRepository.GetByIdAsync(id);
            if (request.UploadImage != null)
            {
                string oldFilePath = _env.GenerateFilePath("assets/images/podcasts", episode.Image);
                oldFilePath.DeleteFile();
                string fileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
                string filePath = _env.GenerateFilePath("assets/images/podcasts", fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.UploadImage.CopyToAsync(stream);
                }
                episode.Image = fileName;
            }

            if (request.AudioFile != null)
            {
                string oldAudioFilePath = _env.GenerateFilePath("audios", episode.Audio);
                oldAudioFilePath.DeleteFile();
                string audioFileName = Guid.NewGuid().ToString() + "-" + request.AudioFile.FileName;
                string audioFilePath = _env.GenerateFilePath("audios", audioFileName);
                using (FileStream stream = new FileStream(audioFilePath, FileMode.Create))
                {
                    await request.AudioFile.CopyToAsync(stream);
                }
                episode.Audio = audioFileName;
            }

            episode.Title = request.Title;
            episode.Description = request.Description;
            episode.PodcastId = request.PodcastId;
            episode.EpisodeGuests.Clear();

            if (request.GuestIds != null && request.GuestIds.Any())
            {
                episode.EpisodeGuests = new List<EpisodeGuest>();

                foreach (var guestId in request.GuestIds)
                {
                    episode.EpisodeGuests.Add(new EpisodeGuest
                    {
                        EpisodeId = episode.Id,
                        GuestId = guestId
                    });
                }
            }

            await _episodeRepository.EditAsync(episode);

        }

        public async Task<List<EpisodeAdminVM>> GetAllAsync()
        {
            var episodeDb= await _episodeRepository.GetAllAsync();
            var episode = episodeDb.Select(m => new EpisodeAdminVM
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                Podcast = m.Podcast,
                Image = m.Image,
                Audio = m.Audio,
                EpisodeGuests = m.EpisodeGuests,
                Likes = m.Likes
            }).ToList();
            return episode;
        }

        public async Task<EpisodeAdminVM> GetByIdAsync(int id)
        {
            var episodeDb= await _episodeRepository.GetByIdAsync(id);
            EpisodeAdminVM episode = new EpisodeAdminVM
            {
                Id = episodeDb.Id,
                Title = episodeDb.Title,
                Description = episodeDb.Description,
                Podcast = episodeDb.Podcast,
                Image = episodeDb.Image,
                Audio = episodeDb.Audio,
                EpisodeGuests = episodeDb.EpisodeGuests,
                Likes= episodeDb.Likes
            };
            return episode;
        }
    }
}

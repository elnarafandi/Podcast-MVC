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
            // Fayl validasiyası: Image
            if (request.UploadImage == null || !request.UploadImage.CheckFileType("image"))
                throw new InvalidOperationException("Only image files are allowed.");

            if (!request.UploadImage.CheckFileSize(1024 * 1024)) // 1 MB
                throw new InvalidOperationException("Image file size should be less than 1 MB.");

            // Fayl validasiyası: Audio
            if (request.AudioFile == null || !request.AudioFile.CheckFileType("audio"))
                throw new InvalidOperationException("Only audio files are allowed.");

            if (!request.AudioFile.CheckFileSize(50 * 1024 * 1024)) // 50 MB
                throw new InvalidOperationException("Audio file size should be less than 50 MB.");

            // Başlıq təkrarı yoxlaması
            string normalizedTitle = request.Title.Trim().ToLower();

            var allEpisodes = await _episodeRepository.GetAllAsync();

            bool exists = allEpisodes.Any(e =>
                e.PodcastId == request.PodcastId &&
                e.Title.Trim().ToLower() == normalizedTitle
            );

            if (exists)
                throw new InvalidOperationException("This podcast already has an episode with the same title.");

            // Image yadda saxla
            string imageFileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
            string imageFilePath = _env.GenerateFilePath("assets/images/podcasts", imageFileName);
            using (FileStream stream = new FileStream(imageFilePath, FileMode.Create))
            {
                await request.UploadImage.CopyToAsync(stream);
            }

            // Audio yadda saxla
            string audioFileName = Guid.NewGuid().ToString() + "-" + request.AudioFile.FileName;
            string audioFilePath = _env.GenerateFilePath("audios", audioFileName);
            using (FileStream stream = new FileStream(audioFilePath, FileMode.Create))
            {
                await request.AudioFile.CopyToAsync(stream);
            }

            // Episode obyektini yarat
            Episode episode = new Episode
            {
                Title = request.Title,
                Description = request.Description,
                PodcastId = request.PodcastId,
                Image = imageFileName,
                Audio = audioFileName,
                EpisodeGuests = new List<EpisodeGuest>()
            };

            if (request.GuestIds != null && request.GuestIds.Any())
            {
                foreach (var guestId in request.GuestIds)
                {
                    episode.EpisodeGuests.Add(new EpisodeGuest
                    {
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
            if (episode == null)
                throw new Exception("Episode not found.");

            // Eyni podkastda eyni adda epizod varmı?
            var allEpisodes = await _episodeRepository.GetAllAsync();
            bool exists = allEpisodes.Any(e => e.Id != id && e.PodcastId == request.PodcastId && e.Title.Trim().ToLower() == request.Title.Trim().ToLower());
            if (exists)
                throw new InvalidOperationException("An episode with the same title already exists in this podcast.");

            // Image yoxlamaları
            if (request.UploadImage != null)
            {
                if (!request.UploadImage.CheckFileType("image"))
                    throw new InvalidOperationException("Only image files are allowed.");

                if (!request.UploadImage.CheckFileSize(1024)) // 1 MB (1024 KB) limit
                    throw new InvalidOperationException("File size should be less than 1 MB.");

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

            // Audio yoxlamaları
            if (request.AudioFile != null)
            {
                if (!request.AudioFile.CheckFileType("audio"))
                    throw new InvalidOperationException("Only audio files are allowed.");

                if (!request.AudioFile.CheckFileSize(50 * 1024 * 1024)) // 50MB limit
                    throw new InvalidOperationException("Audio size should be less than 50 MB.");

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


        public async Task<List<EpisodeAdminVM>> GetAllAsync(int? count = null)
        {
            var episodeDb= await _episodeRepository.GetAllAsync(count);
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

        public async Task<List<EpisodeAdminVM>> GetEpisodesByPodcastIdAsync(int? podcastId)
        {
            var episodeDb= await _episodeRepository.GetEpisodesByPodcastIdAsync(podcastId);
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
    }
}

using Microsoft.EntityFrameworkCore;
using System.Text;
using URL_Shortener.BLL.Abstract;
using URL_Shortener.Models.DTO;
using URL_Shortener.Models.Entities;
using URL_Shortener.Models;
using System.Security.Cryptography;

namespace URL_Shortener.BLL.Services
{
    public class LinkService : ILinkService
    {
        private readonly DatabaseContext _context;

        public LinkService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Status> AddLinkAsync(string originalUrl, string userId)
        {
            var status = new Status();
            try
            {
                var shortenedUrl = GenerateShortenedUrl(originalUrl);

                var link = new Link
                {
                    OriginalUrl = originalUrl,
                    ShortenedUrl = shortenedUrl,
                    CreationTime = DateTime.Now,
                    UserId = userId
                };

                _context.Links.Add(link);
                await _context.SaveChangesAsync();

                status.StatusCode = 1;
                status.Message = "Link added successfully";
            }
            catch (Exception ex)
            {
                status.StatusCode = 0;
                status.Message = "Failed to add link";
            }
            return status;
        }

        public async Task<Status> DeleteLinkAsync(int id)
        {
            var status = new Status();
            try
            {
                var link = await _context.Links.FindAsync(id);
                if (link == null)
                {
                    status.StatusCode = 0;
                    status.Message = "Link not found";
                }
                else
                {
                    _context.Links.Remove(link);
                    await _context.SaveChangesAsync();
                    status.StatusCode = 1;
                    status.Message = "Link deleted successfully";
                }
            }
            catch (Exception ex)
            {
                status.StatusCode = 0;
                status.Message = "Failed to delete link";
            }
            return status;
        }

        public async Task<Status> EditLinkAsync(int id, Link link)
        {
            var status = new Status();
            try
            {
                if (id != link.Id)
                {
                    status.StatusCode = 0;
                    status.Message = "Link ID mismatch";
                }
                else
                {
                    _context.Update(link);
                    await _context.SaveChangesAsync();
                    status.StatusCode = 1;
                    status.Message = "Link updated successfully";
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                status.StatusCode = 0;
                status.Message = "Failed to update link";
            }
            return status;
        }

        public async Task<Link> GetLinkAsync(int id)
        {
            return await _context.Links.FindAsync(id);
        }

        public IQueryable<Link> GetLinksByUserId(string userId)
        {
            return _context.Links.Where(l => l.UserId == userId);
        }

        public async Task<Status> UpdateLinkAsync(int id, string originalUrl)
        {
            var status = new Status();
            var link = await _context.Links.FindAsync(id);
            if (link == null)
            {
                status.StatusCode = 0;
                status.Message = "Link not found";
                return status;
            }

            link.OriginalUrl = originalUrl;
            link.CreationTime = DateTime.Now;

            try
            {
                _context.Links.Update(link);
                await _context.SaveChangesAsync();
                status.StatusCode = 1;
                status.Message = "Link updated successfully";
            }
            catch (Exception ex)
            {
                status.StatusCode = 0;
                status.Message = "Error updating link";
                // Optionally log the exception for debugging purposes
            }

            return status;
        }



        private string GenerateShortenedUrl(string originalUrl)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(originalUrl));
                var shortenedUrl = Convert.ToBase64String(hashBytes)
                    .Replace("/", "")
                    .Replace("+", "")
                    .Substring(0, 6);
                return shortenedUrl;
            }
        }
    }
}

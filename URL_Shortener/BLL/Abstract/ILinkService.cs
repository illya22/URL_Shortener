using URL_Shortener.Models.DTO;
using URL_Shortener.Models.Entities;

namespace URL_Shortener.BLL.Abstract
{
    public interface ILinkService
    {
        Task<Status> AddLinkAsync(string originalUrl, string userId);
        Task<Status> DeleteLinkAsync(int id);
        Task<Status> EditLinkAsync(int id, Link link);
        Task<Link> GetLinkAsync(int id);  
        Task<Status> UpdateLinkAsync(int id, string originalUrl);
        IQueryable<Link> GetLinksByUserId(string userId);
    }
}

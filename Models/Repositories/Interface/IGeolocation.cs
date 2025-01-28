using Report_A_Crime.Models.Entities;
using System.Linq.Expressions;

namespace Report_A_Crime.Models.Repositories.Interface
{
    public interface IGeolocationRepository
    {
        Task<Geolocation> CreateAsync(Geolocation geolocation);
        Task<bool> ExistAsync (Expression<Func<Geolocation, bool>> predicate);
        Task<Geolocation> UpdateAsync(Geolocation geolocation);
        Task<Geolocation> DeleteAsync(Guid geolocationId);
        Task<Geolocation> GetGeolocationAsync(Expression<Func<Geolocation, bool>> expression);
        Task<IEnumerable<Geolocation>> GetByReportIdAsync(Guid reportId);

        Task<IEnumerable<Geolocation>> GetAllAsync();
        Task<IEnumerable<Geolocation>> GetAllByFilterAsync(Expression<Func<Geolocation, bool>> expression);
    }
}

using Report_A_Crime.Models.Entities;
using System.Linq.Expressions;

namespace Report_A_Crime.Models.Repositories.Interface
{
    public interface IGeolocation
    {
        Task<Geolocation> CreateAsync(Geolocation geolocation);
        Task<Geolocation> Update(Geolocation geolocation);
        Task<Geolocation> Delete(Expression<Func<Geolocation, bool>> expression);
        
        Task<Geolocation> GetGeolocationAsync(Expression<Func<Geolocation, bool>> expression);
        Task<IEnumerable<Geolocation>> GetAllAsync();



    }
}

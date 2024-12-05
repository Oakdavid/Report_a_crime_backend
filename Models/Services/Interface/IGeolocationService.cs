using Report_A_Crime.Models.Dtos;
using Report_A_Crime.Models.Entities;
using System.Linq.Expressions;

namespace Report_A_Crime.Models.Services.Interface
{
    public interface IGeolocationService
    {
        Task<bool> GeolocationExistsAsync(Expression<Func<Geolocation, bool>> predicate);
        Task<IEnumerable<GeolocationDto>> GetGeolocations(Expression<Func<Geolocation, bool>> predicate);
        Task<IEnumerable<GeolocationDto>> GetAllFilteredLocations(Expression<Func<Geolocation, bool>> predicate);
        Task<IEnumerable<GeolocationDto>> GetAllGeolocations();
        Task <GeolocationDto> UpdateGeolocationAsync(GeolocationDto geolocation);



    }
}

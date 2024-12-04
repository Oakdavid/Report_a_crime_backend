using Report_A_Crime.Context;
using Report_A_Crime.Models.Repositories.Interface;
using Report_A_Crime.Models.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Report_A_Crime.Models.Repositories.Implementation
{
    public class GeolocationRepository : IGeolocationRepository
    {
        private readonly ReportCrimeDbContext _dbContext;

        public GeolocationRepository(ReportCrimeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Geolocation> CreateAsync(Geolocation geolocation)
        {
            await _dbContext.Geolocations.AddAsync(geolocation);
            return geolocation;
        }

        public async Task<Geolocation> Delete(Guid geolocationId)
        {
           var geolocation = await _dbContext.Geolocations.FindAsync(geolocationId);
            _dbContext.Geolocations.Remove(geolocation);
            return geolocation;

        }

        public async Task<bool> ExistAsync(Expression<Func<Geolocation, bool>> predicate)
        {
            var exist = await _dbContext.Geolocations.AnyAsync(predicate);
            return exist;
        }

        public async Task<IEnumerable<Geolocation>> GetAllAsync()
        {
            var allGeolocation = await _dbContext.Geolocations
                .Include( g => g.Reports)
                .ToListAsync();
            return allGeolocation;
        }

        public async Task<Geolocation> GetGeolocationAsync(Expression<Func<Geolocation, bool>> expression)
        {
            var geolocation = await _dbContext.Geolocations
                .Include ( g => g.Reports)
                .FirstOrDefaultAsync();
            return geolocation;
        }

        public async Task<Geolocation> Update(Geolocation geolocation)
        {
            _dbContext.Geolocations.Update(geolocation);
            return geolocation;
        }
    }
}

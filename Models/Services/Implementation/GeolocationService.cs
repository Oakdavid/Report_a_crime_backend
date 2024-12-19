using Report_A_Crime.Models.Dtos;
using Report_A_Crime.Models.Entities;
using Report_A_Crime.Models.Repositories.Interface;
using Report_A_Crime.Models.Services.Interface;
using System.Linq.Expressions;

namespace Report_A_Crime.Models.Services.Implementation
{
    public class GeolocationService : IGeolocationService
    {
        private readonly IGeolocationRepository _geolocationRepository;
        private readonly ILogger _logger;
        private readonly IReportRepository _reportRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly string _ipinfoToken = "751c3e154c2d6d";

        public GeolocationService(IGeolocationRepository geolocationRepository, ILogger logger, IReportRepository reportRepository, IUnitOfWork unitOfWork, HttpClient httpClient, string ipinfoToken)
        {
            _geolocationRepository = geolocationRepository;
            _logger = logger;
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _ipinfoToken = ipinfoToken;
        }

        public async Task<GeolocationDto> CreateGeolocationAsync(GeolocationRequestModel requestModel, Guid reportId)
        {
            var geolocationExist = await _geolocationRepository.ExistAsync( g => g.Latitude == requestModel.Latitude && g.Longitude == requestModel.Longitude);
            if(geolocationExist)
            {
                return new GeolocationDto
                {
                    Message = " longitude or latitude exist",
                    Status = false,
                    Latitude = requestModel.Latitude.GetValueOrDefault(),
                    Longitude = requestModel.Longitude.GetValueOrDefault(),
                };
            }

            var geolocation = new Geolocation
            {
                Latitude = requestModel.Latitude.GetValueOrDefault(),
                Longitude = requestModel.Longitude.GetValueOrDefault(),
                ReportId = reportId
            };
            await _geolocationRepository.CreateAsync(geolocation);
            await _unitOfWork.SaveChangesAsync();

            return new GeolocationDto
            {
                GeolocationId = geolocation.GeolocationId,
                Latitude = geolocation.Latitude,
                Longitude = geolocation.Longitude,
                ReportId = geolocation.ReportId,
                Status = true,
                Message = "Geolocation created successfully."
            };
        }

        public Task<IEnumerable<GeolocationDto>> GetAllGeolocations()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GeolocationDto>> GetGeolocations(Expression<Func<Geolocation, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<GeolocationDto> UpdateGeolocationAsync(GeolocationDto geolocation)
        {
            throw new NotImplementedException();
        }
    }
}

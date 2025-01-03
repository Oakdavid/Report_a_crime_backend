using Microsoft.AspNetCore.Identity.Data;
using Newtonsoft.Json;
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
        //private readonly ILogger _logger;
        private readonly IReportRepository _reportRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly string _ipInfoToken;

        public GeolocationService(IGeolocationRepository geolocationRepository, IReportRepository reportRepository, IUnitOfWork unitOfWork, HttpClient httpClient, IConfiguration configuration)
        {
            _geolocationRepository = geolocationRepository;
            //_logger = logger;
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _ipInfoToken = configuration["IpInfo:Token"] ?? throw new ArgumentNullException("IpInfo token is not configured.");
        }

        public async Task<GeolocationDto> CreateGeolocationAsync(GeolocationRequestModel requestModel, Guid reportId)
        {
            if(!requestModel.Latitude.HasValue || !requestModel.Longitude.HasValue)
            {
                var ipInfoResponse = await _httpClient.GetAsync($"https://ipinfo.io/json?token={_ipInfoToken}");
                if(!ipInfoResponse.IsSuccessStatusCode)
                {
                    return new GeolocationDto
                    {
                        Message = "Unable to retrieve location information",
                        Status = false,
                    };
                }

                var ipInfoContent = await ipInfoResponse.Content.ReadAsStringAsync();
                var ipInfoData = JsonConvert.DeserializeObject<Geolocation>(ipInfoContent);


                if (ipInfoData == null || ipInfoData.Latitude == 0 || ipInfoData.Longitude == 0)
                {
                    return new GeolocationDto
                    {
                        Message = "No geolocation data available for this request.",
                        Status = false,
                    };
                }

                requestModel.Latitude = ipInfoData.Latitude;
                requestModel.Longitude = ipInfoData.Longitude;
                requestModel.City = ipInfoData.City;
            }

            var geolocationExist = await _geolocationRepository.ExistAsync( g => g.Latitude == requestModel.Latitude && g.Longitude == requestModel.Longitude);
            if(geolocationExist)
            {
                return new GeolocationDto
                {
                    Message = "The specified latitude and longitude already exist.",
                    Status = false,
                    Latitude = requestModel.Latitude.GetValueOrDefault(),
                    Longitude = requestModel.Longitude.GetValueOrDefault(),
                };
            }

            var geolocation = new Geolocation
            {
                Latitude = requestModel.Latitude.GetValueOrDefault(),
                Longitude = requestModel.Longitude.GetValueOrDefault(),
                City = requestModel.City,
                ReportId = reportId
            };
            await _geolocationRepository.CreateAsync(geolocation);
            await _unitOfWork.SaveChangesAsync();

            return new GeolocationDto
            {
                GeolocationId = geolocation.GeolocationId,
                Latitude = geolocation.Latitude,
                Longitude = geolocation.Longitude,
                City = geolocation.City,
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

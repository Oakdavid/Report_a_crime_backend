using Microsoft.AspNetCore.Http;
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
        private readonly IReportRepository _reportRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _ipInfoToken;

        public GeolocationService(IGeolocationRepository geolocationRepository, IReportRepository reportRepository, IUnitOfWork unitOfWork, HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _geolocationRepository = geolocationRepository;
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _ipInfoToken = configuration["IpInfo:Token"] ?? throw new ArgumentNullException("IpInfo token is not configured.");
        }

        public async Task<GeolocationDto> CreateGeolocationAsync(GeolocationRequestModel requestModel, Guid reportId)
        {
            if (requestModel == null)
            {
                return new GeolocationDto
                {
                    Status = false,
                    Message = "Invalid request model"
                };
            }
            var clientIp = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

            if (!requestModel.Latitude.HasValue || !requestModel.Longitude.HasValue)
            {
                var ipInfoResponse = await _httpClient.GetAsync($"https://ipinfo.io/json?token={_ipInfoToken}");
                if (!ipInfoResponse.IsSuccessStatusCode)
                {
                    return new GeolocationDto
                    {
                        Message = "Unable to retrieve location information",
                        Status = false,
                    };
                }

                var ipInfoContent = await ipInfoResponse.Content.ReadAsStringAsync();
                var ipInfoData = JsonConvert.DeserializeObject<dynamic>(ipInfoContent);


                if (ipInfoData == null || ipInfoData.city == null || ipInfoData.country == null)
                {
                    return new GeolocationDto
                    {
                        Message = "No geolocation data available for this request.",
                        Status = false,
                    };
                }

                var locParts = ((string)ipInfoData.loc)?.Split(',');

                if (locParts?.Length != 2 || !double.TryParse(locParts[0], out double lat) || !double.TryParse(locParts[1], out double lng))
                {
                    return new GeolocationDto
                    {
                        Message = "Invalid geolocation data from the external API.",
                        Status = false,
                    };
                }

                requestModel.Latitude = lat;
                requestModel.Longitude = lng;
                requestModel.City = ipInfoData.city;
            }

            if (requestModel.Latitude <= -90 || requestModel.Latitude >= 90 || requestModel.Longitude <= -180 || requestModel.Longitude >= 180)
            {
                return new GeolocationDto
                {
                    Message = "Invalid latitude or longitude values.",
                    Status = false,
                };
            }

            var geolocationExist = await _geolocationRepository.ExistAsync(g => g.Latitude == requestModel.Latitude && g.Longitude == requestModel.Longitude);
            if (geolocationExist)
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
                IpAddress = clientIp,
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
                IpAddress = geolocation.IpAddress,
                Status = true,
                Message = "Geolocation created successfully.",
            };
        }

        public async Task<IEnumerable<GeolocationDto>> GetAllGeolocations()
        {
            var location = await _geolocationRepository.GetAllAsync();
            if (location == null || !location.Any())
            {
                return new List<GeolocationDto>
               {
                   new GeolocationDto
                   {
                       Message = "No geolocation found",
                       Status = false,
                   }
               };
            }

            var allGeolocation = location.Select(g => new GeolocationDto
            {
                GeolocationId = g.GeolocationId,
                Latitude = g.Latitude,
                Longitude = g.Longitude,
                Message = "Geolocation retrieved successfully",
                Status = true
            }).ToList();

            return allGeolocation;
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

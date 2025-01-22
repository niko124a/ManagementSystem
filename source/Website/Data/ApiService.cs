using Blazored.LocalStorage;
using Common.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using WebAPI.Models.User;
using Website.Helpers;

namespace Website.Data
{
    public class ApiService : IApiService
    {
        private IHttpClientFactory httpClientFactory;
        private IConfiguration configuration;
        private ILocalStorageService localStorageService;
        private ApiResponseHelper apiResponseHelper;
        public ApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ApiResponseHelper apiResponseHelper, ILocalStorageService localStorageService)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
            this.apiResponseHelper = apiResponseHelper;
            this.localStorageService = localStorageService;

        }

        public async Task<List<ReservationType>> GetReservationTypesAsync()
        {
            List<ReservationType> ReservationTypes = new List<ReservationType>();
            var httpClient = httpClientFactory.CreateClient("WebApiClient");
            var authToken = configuration.GetValue<string>("APIData:WebsiteApiToken");

            using (var request = new HttpRequestMessage(HttpMethod.Get, "api/reservationtypes"))
            {
                request.Headers.Add("Authorization", $"Bearer {authToken}");
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var requestResponse = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                var result = await apiResponseHelper.HandleApiResponseGetMultipleAsync<ReservationType>(requestResponse, true);

                if (result != null)
                    ReservationTypes = result;
            }
            
            return ReservationTypes;
        }
        #region User
        public async Task<bool> LoginUserAsync(string username, string password)
        {
            bool isLoggedIn = false;
            var httpClient = httpClientFactory.CreateClient("WebApiClient");

            LoginDto loginDto = new LoginDto { Username = username, Password = password };

            try
            {
                using (var memoryContentStream = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(memoryContentStream, loginDto);
                    memoryContentStream.Seek(0, SeekOrigin.Begin); // sets the stream back to the beginning.

                    using (var request = new HttpRequestMessage(HttpMethod.Post, "api/authentication/authenticate"))
                    {
                        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        using (var streamContent = new StreamContent(memoryContentStream))
                        {
                            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            request.Content = streamContent;

                            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                            var result = await apiResponseHelper.HandleApiResponsePostAsync<string>(response, true);
                            if (result == null)
                            {
                                result = string.Empty;
                                isLoggedIn = false;
                            }
                            else
                                isLoggedIn = true;

                            await localStorageService.SetItemAsync("JwtToken", result);

                        }
                    }
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to login the user was canceled with the message: {ocException.Message}");
            }

            return isLoggedIn;
        }

        /// <summary>
        /// Retrieves a new JWT for the website, to communicate with the API.
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="authToken">JWT authentication token for API</param>
        /// <returns>JWT to be used by the website, to communicate with the API</returns>
        public async Task<string> AuthenticateWebsiteAsync(int id, string authToken)
        {
            string token = string.Empty;
            var httpClient = httpClientFactory.CreateClient("WebApiClient");

            try
            {
                using (var memoryContentStream = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(memoryContentStream, id);
                    memoryContentStream.Seek(0, SeekOrigin.Begin); // sets the stream back to the beginning.

                    using (var request = new HttpRequestMessage(HttpMethod.Post, "api/authentication/authenticate/website"))
                    {
                        request.Headers.Add("Authorization", $"Bearer {authToken}");
                        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        using (var streamContent = new StreamContent(memoryContentStream))
                        {
                            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            request.Content = streamContent;

                            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                            token = await apiResponseHelper.HandleApiResponsePostAsync<string>(response, true);
                            if (token == null)
                                token = string.Empty;
                        }
                    }
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to authenticate the website was canceled with the message: {ocException.Message}");
            }

            return token;
        }

        public async Task<User> GetUserAsync(int id, string authToken)
        {
            User user = new User();
            var httpClient = httpClientFactory.CreateClient("WebApiClient");

            try
            {
                using (var userRequest = new HttpRequestMessage(HttpMethod.Get, $"api/users/{id}"))
                {

                    userRequest.Headers.Add("Authorization", $"Bearer {authToken}");
                    userRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                    var userRequestResponse = await httpClient.SendAsync(userRequest, HttpCompletionOption.ResponseHeadersRead);

                    var result = await apiResponseHelper.HandleApiResponseGetSingleAsync<Common.Entities.User>(userRequestResponse, true);
                    if (result != null && result != default)
                        return user = result;
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to get the user was canceled with the message: {ocException.Message}");
            }

            return user;
        }
        /// <summary>
        /// Retrieves a list of a specific users cars.
        /// </summary>
        /// <param name="id">user id</param>
        /// <param name="authToken">JWT token</param>
        /// <returns></returns>
        public async Task<List<Car>> GetUserCarsAsync(int id, string authToken)
        {
            List<Car> cars = new List<Car>();
            var httpClient = httpClientFactory.CreateClient("WebApiClient");

            try
            {
                using (var carsRequest = new HttpRequestMessage(HttpMethod.Get, $"api/cars/user/{id}"))
                {
                    carsRequest.Headers.Add("Authorization", $"Bearer {authToken}");
                    carsRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var carsRequestResponse = await httpClient.SendAsync(carsRequest, HttpCompletionOption.ResponseHeadersRead);

                    var result = await apiResponseHelper.HandleApiResponseGetMultipleAsync<Common.Entities.Car>(carsRequestResponse, true);
                    if (result != null)
                        return cars = result;
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to get the users cars was canceled with the message: {ocException.Message}");
            }

            return cars;
        }

        /// <summary>
        /// Retrieves a list of a specific users active reservations.
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="authToken">JWT token</param>
        /// <returns></returns>
        public async Task<List<Reservation>> GetUserActiveReservationsAsync(int id, string authToken)
        {
            List<Reservation> reservations = new List<Reservation>();
            var httpClient = httpClientFactory.CreateClient("WebApiClient");

            try
            {
                using (var activeReservationsRequest = new HttpRequestMessage(HttpMethod.Get, $"api/reservations/user/{id}/active"))
                {
                    activeReservationsRequest.Headers.Add("Authorization", $"Bearer {authToken}");
                    activeReservationsRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var activeReservationsRequestResponse = await httpClient.SendAsync(activeReservationsRequest, HttpCompletionOption.ResponseHeadersRead);

                    var result = await apiResponseHelper.HandleApiResponseGetMultipleAsync<Common.Entities.Reservation>(activeReservationsRequestResponse, true);
                    if (result != null)
                        return reservations = result;
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to get the users active reservations was canceled with the message: {ocException.Message}");
            }

            return reservations;
        }
        #endregion

    }
}

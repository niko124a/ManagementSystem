using Common.Entities;

namespace Website.Data
{
    public interface IApiService
    {
        Task<List<ReservationType>> GetReservationTypesAsync();
        Task<bool> LoginUserAsync(string username, string password);
        Task<string> AuthenticateWebsiteAsync(int id, string authToken);
        Task<User> GetUserAsync(int id, string authToken);
        Task<List<Car>> GetUserCarsAsync(int id, string authToken);
        Task<List<Reservation>> GetUserActiveReservationsAsync(int id, string authToken);
    }
}

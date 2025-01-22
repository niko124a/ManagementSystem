using Blazored.LocalStorage;
using Common.Entities;
using Common.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Text.Json;
using Website.Data;
using Website.Helpers;

namespace Website.Pages
{
    public partial class Services
    {
        private List<ReservationType> reservationTypes = new List<ReservationType>();
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ReservationService ReservationService { get; set; }
        [Inject] public JsonSerializerOptionsWrapper JsonSerializerOptionsWrapper { get; set; }
        [Inject] public IApiService ApiService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            reservationTypes = await ApiService.GetReservationTypesAsync();
        }

        public void SelectServiceClickEvent(ReservationType reservationType)
        {
            ReservationService.ReservationDto.ReservationType = reservationType;
            NavigationManager.NavigateTo($"createreservation", true);
        }
    }
}

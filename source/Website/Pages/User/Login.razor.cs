using Blazored.LocalStorage;
using Common.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Text.Json;
using WebAPI.Models.User;
using Website.Authentication;
using Website.Data;
using Website.Helpers;

namespace Website.Pages.User
{
    public partial class Login
    {
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public JsonSerializerOptionsWrapper JsonSerializerOptionsWrapper { get; set; }
        [Inject] public IApiService ApiService { get; set; }

        private string username = string.Empty;
        private string password = string.Empty;

        public async Task LoginSubmit()
        {
            bool loginSuccess = false;
            loginSuccess = await ApiService.LoginUserAsync(username, password);
            if (loginSuccess)
                NavigationManager.NavigateTo("/", true);
        }


        bool isShown = false;
        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        private void ShowPassword()
        {
            if (isShown)
            {
                isShown = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                isShown = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

        private async Task EnterLogin(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await LoginSubmit();
            }
        }
    }
}

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Website.Data;
using Exception = System.Exception;

namespace Website.Pages.AdminPages;

public partial class SecuritySettings
{
    [Inject] public IConfiguration Configuration { get; set; }
    [Inject] public IApiService ApiService { get; set; }
    [Inject] public AuthenticationStateProvider CustomAuthenticationStateProvider { get; set; }
    [Inject] public ILocalStorageService LocalStorageService { get; set; }
    
    string _apiKey = string.Empty;
    
    protected override async Task OnInitializedAsync()
    {
        _apiKey = Configuration.GetSection("APIData:WebsiteApiToken").Value ?? string.Empty;
    }

    public async Task UpdateApiKey()
    {
        AuthenticationState authState = await CustomAuthenticationStateProvider.GetAuthenticationStateAsync();

        int id = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);
        var authToken = await LocalStorageService.GetItemAsync<string>("JwtToken");
        
        _apiKey = await GenerateJwt(id, authToken);
        
        
        // Fetch the api key from database and store it in local memory.
        // Everytime the key is used, get it from local memory and make sure it is not expired.
        // If it is expired, call the api with an admin user to create a new key.
        
        
    }
    
    bool _keyVisible = false;
    InputType _keyInput = InputType.Password;
    private void ShowKey(bool toggled)
    {
        if (!toggled)
        {
            _keyVisible = false;
            _keyInput = InputType.Password;
        }
        else
        {
            _keyVisible = true;
            _keyInput = InputType.Text;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">User id</param>
    /// <param name="authToken">JWT authentication token for API</param>
    /// <returns>JWT to be used by the website, to communicate with the API</returns>
    private async Task<string> GenerateJwt(int id, string authToken)
    {
        return await ApiService.AuthenticateWebsiteAsync(id, authToken);
    }
}
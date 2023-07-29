using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Todo.Services.GravatarServices.Client.Models;

namespace Todo.Services.GravatarServices.Client
{
    public class GravatarClient : IGravatarClient
    {
        private readonly HttpClient httpClient;

        public GravatarClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri("https://en.gravatar.com/");
            this.httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<GravatarProfileModel> GetGravatarProfile(string userEmail)
        {
            try
            {
                var response = await this.httpClient.GetFromJsonAsync<GravatarProfileResponse>($"{Gravatar.GetHash(userEmail)}.json?s=30");

                return new GravatarProfileModel
                {
                    DisplayName = response.Entries.FirstOrDefault()?.DisplayName
                };
            }
            catch
            {
                return new GravatarProfileModel();
            }
            
        }
    }
}

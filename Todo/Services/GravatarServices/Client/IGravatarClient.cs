using System.Threading.Tasks;
using Todo.Services.GravatarServices.Client.Models;

namespace Todo.Services.GravatarServices.Client
{
    public interface IGravatarClient
    {
        Task<GravatarProfileModel> GetGravatarProfile(string userEmail);
    }
}

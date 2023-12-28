using RestSharp;
using SharedModels;

namespace TaskTrackerApi.Infrastructure
{
    public class UserServiceGateway : IServiceGateway<MyUserDto>
    {
        string userServiceBaseUrl;

        public UserServiceGateway(string baseUrl)
        {
            userServiceBaseUrl = baseUrl;
        }

        public async Task<MyUserDto> GetAsync(int id)
        {
            RestClient c = new RestClient(userServiceBaseUrl);

            var request = new RestRequest(id.ToString());
            var response = await c.GetAsync<MyUserDto>(request);

            return response;
        }
    }
}

using RestSharp;
using SharedModels;

namespace TaskTrackerApi.Infrastructure
{
    public class TaskServiceGateway
    {
        string taskServiceBaseUrl;

        public TaskServiceGateway(string baseUrl)
        {
            taskServiceBaseUrl = baseUrl;
        }

        public async Task<MyTaskDto> GetAsync(int id)
        {
            RestClient c = new RestClient(taskServiceBaseUrl);

            var request = new RestRequest(id.ToString());
            var response = await c.GetAsync<MyTaskDto>(request);

            return response;
        }
    }
}

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using UserLogNET.Response;

namespace UserLogNET.Client
{
    public class UserLogClient : IUserLogClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _projectName;
        private readonly ILogger<UserLogClient> _logger;

        public UserLogClient(HttpClient client, string projectName, ILogger<UserLogClient>? logger = null)
        {
            _httpClient = client;
            _projectName = projectName;
            _logger = logger ?? new NullLogger<UserLogClient>();
        }
        
        public async Task<bool> Track(UserLogEvent userLogEvent)
        {
            try
            {
                userLogEvent.Project = _projectName;
                var payload = JsonSerializer.Serialize(userLogEvent);

                using var message = new HttpRequestMessage(HttpMethod.Post, UserLogConstants.Endpoints.Log);
                message.Content = new StringContent(payload, Encoding.UTF8, "application/json");
                message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _httpClient.SendAsync(message);

                return response.IsSuccessStatusCode
                    ? await HandleSuccess(response)
                    : await HandleErrorResponse(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UserLog.Track returned an error");
                return false;
            }
        }

        private async Task<bool> HandleSuccess(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                _logger.LogError("UserLog.Track returned empty response");
                return false;
            }

            var responseStatus =
                JsonSerializer.Deserialize<UserLogSuccessResponse>(responseContent);
            if (responseStatus is { Success: true })
                return true;

            _logger.LogError("UserLog.Track returned not ok status: {UserLog_Response_Status}",
                responseStatus?.Message);
            return false;
        }

        private async Task<bool> HandleErrorResponse(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.BadRequest)
            {
                _logger.LogError(
                    "UserLog.Track returned an error: {UserLog_Response_StatusCode} - {UserLog_Response_Reason}",
                    response.StatusCode,
                    response.ReasonPhrase);

                return false;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                _logger.LogError("UserLog.Track returned empty failure response");
                return false;
            }

            var failureResponse =
                JsonSerializer.Deserialize<UserLogFailureResponse>(responseContent) ??
                new UserLogFailureResponse();

            _logger.LogError(
                "UserLog.Track returned {UserLog_Response_StatusCode} - {UserLog_Response_Reason} with error: {@UserLog_Response_Error}",
                (int)response.StatusCode,
                response.ReasonPhrase,
                string.Join("\n", failureResponse.Error.Issues.Select(x => x.Message)));

            return false;
        }
    }
}
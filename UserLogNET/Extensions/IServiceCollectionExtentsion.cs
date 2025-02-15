using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserLogNET.Client;

namespace UserLogNET.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Inject an instance of <see cref="serviceCollection"/>
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="apiKey">UserLog API api_key</param>
        /// <param name="projectName">UserLog project name</param>
        /// <returns></returns>
        /// <exception cref="apiKey"><paramref name="projectName"/> or <paramref name="projectName"/> is empty</exception>
        public static IServiceCollection AddUserLog(this IServiceCollection serviceCollection, string apiKey, string projectName)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException(nameof(apiKey));
            
            if (string.IsNullOrEmpty(projectName))
                throw new ArgumentNullException(nameof(projectName));
            
            serviceCollection.AddHttpClient<IUserLogClient, UserLogClient>((client, sp) =>
            {
                client.BaseAddress = new Uri(UserLogConstants.UserLogBaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                return new UserLogClient(client, projectName, sp.GetService<ILogger<UserLogClient>>());
            });
            
            return serviceCollection;
        }
    }
}
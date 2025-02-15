using System.Threading.Tasks;

namespace UserLogNET.Client
{
    public interface IUserLogClient
    {
        /// <summary>
        /// Publish a new event to UserLog
        /// </summary>
        /// <param name="userLogEvent"></param>
        /// <returns>true if event was pushed successfully</returns>
        Task<bool> Track(UserLogEvent userLogEvent);
    }
}
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;
using SignalR.Backend.Models;

namespace SignalR.Backend
{
    public static class SignalRDemoFunctions
    {
        private const string ConnectionHub = "ConnectionHub";

        public static IList<User> users = new List<User>();
        
        [FunctionName("SendMessage")]
        public static Task SendMessage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]string message,
            [SignalR(HubName = ConnectionHub)]IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "NewMessage",
                    Arguments = new object[] { message }
                });
        }
        
        [FunctionName("ConnectUser")]
        public static Task ConnectUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]string json,
            [SignalR(HubName = ConnectionHub)]IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var user = JsonConvert.DeserializeObject<User>(json);
            var existedUser = users.FirstOrDefault(x => x.UserName == user.UserName);
            
            if (existedUser == null)
                users.Add(user);

            var payload = JsonConvert.SerializeObject(existedUser == null ? true.ToString() : false.ToString());

            GetUsers(json, signalRMessages);

            return signalRMessages.AddAsync(
                    new SignalRMessage
                    {
                        Target = "IsUserConnect",
                        Arguments = new object[] { payload }
                    });
        }

        [FunctionName(nameof(SendRequestToConnectToUser))]
        public static Task SendRequestToConnectToUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] string json,
            [SignalR(HubName = ConnectionHub)] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "RequestReceived",
                    Arguments = new object[] { json }
                });
        }

        [FunctionName(nameof(ConnectToUserAccepted))]
        public static Task ConnectToUserAccepted(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] string userId,
            [SignalR(HubName = ConnectionHub)] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var payload = JsonConvert.SerializeObject(userId);

            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "RequestAccepted",
                    Arguments = new object[] { payload }
                });
        }

        [FunctionName("GetUsers")]
        public static Task GetUsers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] string json,
            [SignalR(HubName = ConnectionHub)] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var activeUsers = new List<User>(users);

            var payload = JsonConvert.SerializeObject(activeUsers);

            return signalRMessages.AddAsync(
                    new SignalRMessage
                    {
                        Target = "SendActiveUsers",
                        Arguments = new object[] { payload }
                    });
        }

        [FunctionName(nameof(SendPictureToUser))]
        public static Task SendPictureToUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] string json,
            [SignalR(HubName = ConnectionHub)] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(
                    new SignalRMessage
                    {
                        Target = "ReceivePictureFromUser",
                        Arguments = new object[] { json }
                    });
        }

        [FunctionName(nameof(DisconnectUser))]
        public static Task DisconnectUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] string json,
            [SignalR(HubName = ConnectionHub)] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var user = JsonConvert.DeserializeObject<User>(json);
            var serverUser = users.FirstOrDefault(x => x.UserName == user.UserName);
            users.Remove(serverUser);

            return signalRMessages.AddAsync(
                    new SignalRMessage
                    {
                        Target = "IsUserConnect",
                        Arguments = new object[] { json }
                    });
        }

        [FunctionName("Negotiate")]
        public static SignalRConnectionInfo Negotiate(
        [HttpTrigger(AuthorizationLevel.Anonymous)]HttpRequest req,
        [SignalRConnectionInfo(HubName = ConnectionHub)] SignalRConnectionInfo connectionInfo)
        {
            // connectionInfo contains an access key token with a name identifier claim set to the authenticated user
            return connectionInfo;
        }
    }
}

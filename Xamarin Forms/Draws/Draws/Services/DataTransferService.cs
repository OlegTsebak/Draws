using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Draws.Helpers;
using Draws.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace Draws.Services
{
    public class DataTransferService : IDataTransferService
    {
        public User CurrentUser { get; set; } = new User();
        
        public User ConnectedUser { get; set; } = new User();
        
        private readonly HttpClient _httpClient;
        
        public HubConnection _connection;

        public DataTransferService()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(Constants.BackendUrl)
                .Build();
            _httpClient = new HttpClient();
        }

        public event Action<IEnumerable<User>> ActiveUsersReceived;
        
        public event Action<User> RequestReceived;
        
        public event Action<string> RequestAccepted;
        
        public event Action<Picture> PictureReceived;
        
        public async Task<bool> Connect()
        {
            if (_connection.State == HubConnectionState.Connected) return true;

            _connection.On<string>("NewMessage", messageString =>
            {
                var message = JsonConvert.DeserializeObject<Message>(messageString);
            });
            
            _connection.On<string>("IsUserConnect", messageString  =>
            {
                bool.TryParse(messageString, out var isUserConnect);
            });
            
            _connection.On<string>("SendActiveUsers", messageString  =>
            {
                var allActiveUsers = JsonConvert.DeserializeObject<List<User>>(messageString);
                allActiveUsers?.RemoveAll(x => x.UserName == CurrentUser.UserName);
                ActiveUsersReceived?.Invoke(allActiveUsers);
            });
            
            _connection.On<string>("RequestReceived", messageString  =>
            {
                var user = JsonConvert.DeserializeObject<SpareRequest>(messageString);
                if(user?.Receiver.Id == CurrentUser.Id)
                    RequestReceived?.Invoke(user?.Owner);
            });
            
            _connection.On<string>("RequestAccepted", messageString  =>
            {
                var userId = JsonConvert.DeserializeObject<string>(messageString);
                if (userId == CurrentUser.Id)
                    RequestAccepted?.Invoke(messageString);
            });
            
            _connection.On<string>("ReceivePictureFromUser", messageString  =>
            {
                var picture = JsonConvert.DeserializeObject<Picture>(messageString);
                if (picture.Receiver.Id == CurrentUser.Id)
                    PictureReceived?.Invoke(picture);
            });

            try
            {
                await _connection.StartAsync();
                CurrentUser.Id = _connection.ConnectionId;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        
        public void Send(Message message)
        {
            var serializedPayload = JsonConvert.SerializeObject(message);

            Task.Run(async () =>
            {
                await _httpClient
                    .PostAsync($"{Constants.BackendUrl}/SendMessage", 
                        new StringContent(serializedPayload));
            });
        }
        
        public async Task GetUsers()
        {
            var serializedPayload = JsonConvert.SerializeObject(CurrentUser);
            
            await _httpClient
                .PostAsync($"{Constants.BackendUrl}/GetUsers", 
                    new StringContent(serializedPayload));
        }
        
        public async Task ConnectUser()
        {
            var serializedPayload = JsonConvert.SerializeObject(CurrentUser);
            
            await _httpClient
                .PostAsync($"{Constants.BackendUrl}/ConnectUser", 
                    new StringContent(serializedPayload));
        }
        
        public async Task ConnectToUser(SpareRequest spareRequest)
        {
            var serializedPayload = JsonConvert.SerializeObject(spareRequest);
            
            await _httpClient
                .PostAsync($"{Constants.BackendUrl}/SendRequestToConnectToUser", 
                    new StringContent(serializedPayload));
        }
        
        public async Task ConnectToUserAccepted(string connectionUserId)
        {
            await _httpClient
                .PostAsync($"{Constants.BackendUrl}/ConnectToUserAccepted", 
                    new StringContent(connectionUserId));
        }
        
        public async Task SendPictureToUser(Picture picture)
        {
            picture.Receiver = ConnectedUser;
            var serializedPayload = JsonConvert.SerializeObject(picture);
            
            await _httpClient
                .PostAsync($"{Constants.BackendUrl}/SendPictureToUser", 
                    new StringContent(serializedPayload));
        }
        
        public async Task DisconnectUser()
        {
            var serializedPayload = JsonConvert.SerializeObject(CurrentUser);
            await _httpClient
                .PostAsync($"{Constants.BackendUrl}/SendPictureToUser", 
                    new StringContent(serializedPayload));
        }
    }
}
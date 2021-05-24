using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Draws.Models;

namespace Draws.Services
{
    public interface IDataTransferService
    {
        User CurrentUser { get; set; }
        
        User ConnectedUser { get; set; }
        
        event Action<IEnumerable<User>> ActiveUsersReceived;
        
        event Action<User> RequestReceived;
        
        event Action<string> RequestAccepted;
        
        event Action<Picture> PictureReceived;
        
        Task ConnectUser();
        
        Task ConnectToUser(SpareRequest request);
        
        Task ConnectToUserAccepted(string userId);

        Task DisconnectUser();
        
        Task GetUsers();
        
        void Send(Message message);

        Task SendPictureToUser(Picture picture);

        Task<bool> Connect();
    }
}
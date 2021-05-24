using System;
namespace SignalR.Backend.Models
{
    public class SpareRequest
    {
        public User Owner { get; set; }

        public User Receiver { get; set; }
    }
}

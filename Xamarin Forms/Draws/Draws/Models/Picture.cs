namespace Draws.Models
{
    public class Picture
    {
        public User Receiver { get; set; }
        
        public string ImageInBase64 { get; set; }
    }
}
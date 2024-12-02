using Microsoft.AspNetCore.Identity;

namespace dreamStayHotel.Entities
{
    public class Room
    {
        public string? Name { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public string[]? Extras { get; set; }
        public int Rating { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace dreamStayHotel.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string[]? Extras { get; set; }
        public decimal PricePerNight { get; set; }
        public int MaxAdults { get; set; }
        public int MaxChildren { get; set; }
        public string? Img_Src { get; set; }
        public string? Description { get; set; }
        public int Rating { get; set; }
        public DateTime? CheckInDate { get; set; } = null;

    }
}

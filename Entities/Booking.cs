namespace dreamStayHotel.Entities
{
    public class Booking
    {
        public int id { get; set; }
        public string userId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; } = "Jóváhagyásra vár";
        public virtual Room Room { get; set; }
    }
}

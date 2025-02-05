namespace dreamStayHotel.DTO
{
    public class CreateBookingDTO
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int roomId { get; set; }
    }
}

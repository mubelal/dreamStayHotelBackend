namespace dreamStayHotel.DTO
{
    public class SortByDTO
    {
        public int MaxAdults { get; set; }
        public int MaxChildren { get; set; }
        public DateTime? CheckInDate { get; set; } = null;
    }
}

namespace PoMad.Models
{
    public class DailyData
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public int Calories { get; set; }
        public double Weight { get; set; }
        public bool DidOMAD { get; set; }
    }
}

namespace GoldenRaspberryAwards.Api.Application.Models
{
    public class Interval
    {
        public int IntervalValue { get; set; }
        public int PreviousWin { get; set; }
        public int FollowingWin { get; set; }
    }
}

using Airport.Data.Model;

namespace Airport.Client.Models
{
    public class LiveUpdateViewModel
    {
        public List<LiveUpdate> LiveUpdateList { get; set; }
        public bool IsLastPage { get; set; }
        public bool IsFirstPage { get; set; }
        public int CurrentPage { get; set; }
    }
}

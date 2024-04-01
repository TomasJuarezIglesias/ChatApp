namespace ChatApp_API.DTOs
{
    public class Pagination
    {
        public Pagination(int page, int limit)
        {
            Page = page;
            Limit = limit;
        }

        public int Page { get; set; } = 1;
        public int Quantity { get; set; }
        private int _limit;
        public int Limit
        {
            get
            {
                return _limit;
            }
            set
            {
                _limit = value < 50 ? value : 50;
            }
        }
        public int Total { get; set; }
    }
}

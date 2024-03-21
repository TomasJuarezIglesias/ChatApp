namespace ChatApp_API.DTOs
{
    public class Pagination
    {
        public Pagination(int page, int quantity)
        {
            Page = page;
            Quantity = quantity;
        }

        public int Page { get; set; } = 1;
        public int Quantity {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value < Limit ? value : Limit;
            }
        }
        private int _quantity;
        public int Limit { get; private set; } = 50;
        public int Total { get; set; }
    }
}

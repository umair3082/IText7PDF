namespace IText7PdfPOC
{
    public class Customer
    {
        public int customerId { get; set; }
        public string customerName { get; set; } = string.Empty;
        public string customerEmail { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Age { get; set; }
        public List<Cartitems>? Items { get; set; }
    }
    public class Cartitems
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Version { get; set; }
        public string Color { get; set; }
        public string ImageURL { get; set; }
    }
}

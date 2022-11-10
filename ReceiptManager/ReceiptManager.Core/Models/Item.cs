namespace ReceiptManager.Core.Models
{
    public class Item : Entity
    {
        public Item()
        { }

        public Item(string productName)
        {
            ProductName = productName;
        }

        public string ProductName { get; set; }
    }
}

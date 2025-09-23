namespace Grocery.Core.Models
{
    public class Product : Model
    {
        public int Stock { get; set; }
        public Product(int id, string name, int stock) : base(id, name)
        {
            Stock = stock;
        }
    }
}

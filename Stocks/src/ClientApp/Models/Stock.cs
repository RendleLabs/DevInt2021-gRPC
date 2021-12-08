namespace ClientApp.Models;

public class Stock
{
    public Stock(string code, string name, int quantityHeld)
    {
        Code = code;
        Name = name;
        QuantityHeld = quantityHeld;
        Price = 0f;
    }

    public string Code { get; set; }
    public string Name { get; set; }
    public int QuantityHeld { get; set; }
    public float Price { get; set; }
}
using System;

namespace ECommerceSystem
{
    public class Product
    {
        // 1. Product Structure
        public int Id { get; set; }        // int Id
        public string Name { get; set; }   // string Name
        public double Price { get; set; }  // double Price
        public int Quantity { get; set; }  // int Quantity

        public Product(int id, string name, double price, int quantity)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        // 9. String Interpolation
        public void Display()
        {
            Console.WriteLine($"ID: {Id} | Name: {Name} | Price: {Price:C} | Stock: {Quantity}");
        }
    }
}
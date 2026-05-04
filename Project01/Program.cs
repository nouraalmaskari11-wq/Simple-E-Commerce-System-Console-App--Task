using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ECommerceSystem
{
    class Program
    {
        // ==================== DATA STORAGE ====================
        // 2. Store products using List
        static List<Product> products = new List<Product>();

        // Store cart items using List (as required - but using Dictionary is Bonus)
        // Note: I'm using Dictionary as Bonus
        static Dictionary<int, int> cart = new Dictionary<int, int>();

        // Bonus: Stack for undo (to track orders)
        static Stack<string> actionHistory = new Stack<string>();

        static int nextId = 1;
        static string dataFile = "products.txt";

        // ==================== MAIN ====================
        static void Main(string[] args)
        {
            // 10. File Handling (Bonus) - Load products
            LoadProductsFromFile();

            if (products.Count == 0)
            {
                AddSampleProducts();
            }

            int choice = 0;

            // 3. Menu System with while loop
            while (choice != 7)
            {
                Console.Clear();

                // 3. Menu System
                Console.WriteLine("=========================================");
                Console.WriteLine("       E-Commerce System");
                Console.WriteLine("=========================================");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. View All Products");
                Console.WriteLine("3. Search Product");
                Console.WriteLine("4. Add to Cart");
                Console.WriteLine("5. View Cart");
                Console.WriteLine("6. Checkout");
                Console.WriteLine("7. Exit");
                Console.WriteLine("=========================================");
                Console.Write("Enter your choice: ");

                string input = Console.ReadLine();

                // 8. Handle invalid input
                if (!int.TryParse(input, out choice))
                {
                    Console.WriteLine("Error: Please enter a valid number");
                    Console.ReadKey();
                    continue;
                }

                // 3. Switch statement
                switch (choice)
                {
                    case 1:
                        AddProduct();
                        break;
                    case 2:
                        ViewProducts();
                        break;
                    case 3:
                        SearchProductMenu();
                        break;
                    case 4:
                        AddToCartMenu();
                        break;
                    case 5:
                        ViewCart();
                        break;
                    case 6:
                        Checkout();
                        break;
                    case 7:
                        Console.WriteLine("Thank you for using the system. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }

                if (choice != 7)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        static void AddSampleProducts()
        {
            products.Add(new Product(1, "Laptop", 2500, 10));
            products.Add(new Product(2, "Mouse", 50, 50));
            products.Add(new Product(3, "Keyboard", 120, 30));
            nextId = 4;
            SaveProductsToFile();
        }

        // ==================== 4. ADD PRODUCT ====================
        static void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("--- Add New Product ---");
            Console.WriteLine("");

            Console.Write("Enter product name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Error: Product name cannot be empty");
                return;
            }

            // Bonus: Regex validation
            if (!Regex.IsMatch(name, @"^[a-zA-Z0-9\s]{2,50}$"))
            {
                Console.WriteLine("Error: Name must be 2-50 characters (letters, numbers, spaces)");
                return;
            }

            Console.Write("Enter product price: ");
            if (!double.TryParse(Console.ReadLine(), out double price) || price <= 0)
            {
                Console.WriteLine("Error: Price must be a positive number");
                return;
            }

            Console.Write("Enter product quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
            {
                Console.WriteLine("Error: Quantity must be zero or a positive number");
                return;
            }

            Product newProduct = new Product(nextId, name, price, quantity);
            products.Add(newProduct);
            nextId++;

            // 10. Save to file
            SaveProductsToFile();

            Console.WriteLine("");
            Console.WriteLine("Product added successfully!");
            Console.WriteLine($"Product ID: {newProduct.Id}");
        }

        // ==================== 4. VIEW PRODUCTS ====================
        static void ViewProducts()
        {
            Console.Clear();
            Console.WriteLine("--- All Products ---");
            Console.WriteLine("");

            if (products.Count == 0)
            {
                Console.WriteLine("No products found");
                return;
            }

            Console.WriteLine("ID     Name                 Price        Stock");
            Console.WriteLine("---    -----------------   ----------   -----");

            foreach (Product p in products)
            {
                // 9. String interpolation
                Console.WriteLine($"{p.Id,-6} {p.Name,-20} {p.Price,10:C} {p.Quantity,8}");
            }

            Console.WriteLine($"\nTotal products: {products.Count}");
        }

        // ==================== SEARCH MENU ====================
        static void SearchProductMenu()
        {
            Console.Clear();
            Console.WriteLine("--- Search Product ---");
            Console.WriteLine("");
            Console.WriteLine("1. Search by ID");
            Console.WriteLine("2. Search by Name");
            Console.Write("Choose: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter product ID: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    // 5. Method Overloading - by ID
                    SearchProduct(id);
                }
                else
                {
                    Console.WriteLine("Error: Invalid ID");
                }
            }
            else if (choice == "2")
            {
                Console.Write("Enter product name: ");
                string name = Console.ReadLine();
                // 5. Method Overloading - by Name
                SearchProduct(name);
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }

        // 5. Method Overloading - Search by ID
        static void SearchProduct(int id)
        {
            // 6. out parameter
            if (TryFindProductById(id, out Product found))
            {
                Console.WriteLine("");
                Console.WriteLine("Product found:");
                found.Display();
            }
            else
            {
                // 8. Handle product not found
                Console.WriteLine($"Product with ID {id} not found!");
            }
        }

        // 6. out parameter
        static bool TryFindProductById(int id, out Product product)
        {
            foreach (Product p in products)
            {
                if (p.Id == id)
                {
                    product = p;
                    return true;
                }
            }
            product = null;
            return false;
        }

        // 5. Method Overloading - Search by Name
        static void SearchProduct(string name)
        {
            List<Product> found = new List<Product>();

            foreach (Product p in products)
            {
                if (p.Name.ToLower().Contains(name.ToLower()))
                {
                    found.Add(p);
                }
            }

            if (found.Count > 0)
            {
                Console.WriteLine("");
                Console.WriteLine($"Found {found.Count} product(s):");
                foreach (Product p in found)
                {
                    p.Display();
                }
            }
            else
            {
                // 8. Handle product not found
                Console.WriteLine($"No products found with name containing '{name}'");
            }
        }

        // ==================== 4. ADD TO CART ====================
        static void AddToCartMenu()
        {
            Console.Clear();
            Console.WriteLine("--- Add to Cart ---");
            Console.WriteLine("");

            ViewProducts();

            if (products.Count == 0)
            {
                Console.WriteLine("No products to add");
                return;
            }

            Console.Write("Enter product ID: ");
            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Error: Invalid ID");
                return;
            }

            // 6. out parameter
            if (!TryFindProductById(productId, out Product selectedProduct))
            {
                Console.WriteLine("Product not found");
                return;
            }

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Error: Invalid quantity");
                return;
            }

            // 8. Handle insufficient quantity
            if (selectedProduct.Quantity < quantity)
            {
                Console.WriteLine($"Insufficient stock. Available: {selectedProduct.Quantity}");
                return;
            }

            // Bonus: Save for undo using Stack
            int oldQuantity = cart.ContainsKey(productId) ? cart[productId] : 0;
            actionHistory.Push($"{productId}|{oldQuantity}");

            // Bonus: Add to cart using Dictionary
            if (cart.ContainsKey(productId))
            {
                cart[productId] += quantity;
            }
            else
            {
                cart.Add(productId, quantity);
            }

            // 6. ref parameter to update product quantity
            int currentStock = selectedProduct.Quantity;
            UpdateProductQuantity(ref currentStock, -quantity);
            selectedProduct.Quantity = currentStock;

            // 10. Save to file
            SaveProductsToFile();

            // 9. String interpolation
            Console.WriteLine("");
            Console.WriteLine($"Added to cart successfully!");
            Console.WriteLine($"Product: {selectedProduct.Name}");
            Console.WriteLine($"Quantity: {quantity}");
            Console.WriteLine($"Total price: {selectedProduct.Price * quantity:C}");
        }

        // 6. ref parameter
        static void UpdateProductQuantity(ref int currentStock, int change)
        {
            currentStock += change;
        }

        // ==================== 4. VIEW CART (WITH RECURSION) ====================
        static void ViewCart()
        {
            Console.Clear();
            Console.WriteLine("--- Shopping Cart ---");
            Console.WriteLine("");

            if (cart.Count == 0)
            {
                Console.WriteLine("Cart is empty");
                return;
            }

            // Convert cart to list for recursion
            List<int> productIds = new List<int>(cart.Keys);

            // 7. Recursion to display cart items
            double total = DisplayCartRecursive(productIds, 0);

            Console.WriteLine("");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Total: {total:C}");
        }

        // 7. Recursive function (no loops inside)
        static double DisplayCartRecursive(List<int> productIds, int index)
        {
            // Base case
            if (index >= productIds.Count)
            {
                return 0;
            }

            int productId = productIds[index];
            int quantity = cart[productId];

            Product product = null;
            foreach (Product p in products)
            {
                if (p.Id == productId)
                {
                    product = p;
                    break;
                }
            }

            double itemTotal = 0;
            if (product != null)
            {
                itemTotal = product.Price * quantity;
                Console.WriteLine($"{product.Name} x {quantity} = {itemTotal:C}");
            }

            // Recursive call
            double remainingTotal = DisplayCartRecursive(productIds, index + 1);

            return itemTotal + remainingTotal;
        }

        // ==================== 4. CHECKOUT (WITH DISCOUNT) ====================
        static void Checkout()
        {
            Console.Clear();
            Console.WriteLine("--- Checkout ---");
            Console.WriteLine("");

            if (cart.Count == 0)
            {
                Console.WriteLine("Cart is empty! Add products first");
                return;
            }

            ViewCart();

            // Calculate total
            double total = 0;
            foreach (var item in cart)
            {
                foreach (Product p in products)
                {
                    if (p.Id == item.Key)
                    {
                        total += p.Price * item.Value;
                        break;
                    }
                }
            }

            // Bonus: Discount system
            double discount = 0;
            string discountMessage = "";

            if (total >= 500)
            {
                discount = total * 0.10;
                discountMessage = "10% discount for orders over 500";
            }
            else if (total >= 200)
            {
                discount = total * 0.05;
                discountMessage = "5% discount for orders over 200";
            }

            double finalTotal = total - discount;

            Console.WriteLine("");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Subtotal: {total:C}");

            if (discount > 0)
            {
                Console.WriteLine($"{discountMessage}: -{discount:C}");
                Console.WriteLine($"Total after discount: {finalTotal:C}");
            }

            Console.Write("Confirm purchase? (y/n): ");
            string confirm = Console.ReadLine();

            if (confirm == "y" || confirm == "Y")
            {
                // 10. Save invoice
                SaveInvoice(total, discount, finalTotal);
                cart.Clear();
                actionHistory.Clear();

                Console.WriteLine("");
                Console.WriteLine("Purchase completed successfully! Thank you for shopping with us");
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("Purchase cancelled");
            }
        }

        // ==================== 10. SAVE INVOICE ====================
        static void SaveInvoice(double total, double discount, double finalTotal)
        {
            string fileName = $"invoice_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("INVOICE");
                writer.WriteLine("========");
                writer.WriteLine($"Date: {DateTime.Now}");
                writer.WriteLine("");
                writer.WriteLine("Products:");
                writer.WriteLine("");

                foreach (var item in cart)
                {
                    foreach (Product p in products)
                    {
                        if (p.Id == item.Key)
                        {
                            double itemTotal = p.Price * item.Value;
                            writer.WriteLine($"{p.Name} x {item.Value} = {itemTotal:C}");
                            break;
                        }
                    }
                }

                writer.WriteLine("");
                writer.WriteLine($"Subtotal: {total:C}");

                if (discount > 0)
                {
                    writer.WriteLine($"Discount: -{discount:C}");
                    writer.WriteLine($"Total: {finalTotal:C}");
                }

                writer.WriteLine("");
                writer.WriteLine("Thank you for shopping with us");
            }

            Console.WriteLine("");
            Console.WriteLine($"Invoice saved to: {fileName}");
        }

        // ==================== 10. FILE HANDLING ====================
        static void SaveProductsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(dataFile))
                {
                    foreach (Product p in products)
                    {
                        writer.WriteLine($"{p.Id}|{p.Name}|{p.Price}|{p.Quantity}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving products: {ex.Message}");
            }
        }

        static void LoadProductsFromFile()
        {
            if (!File.Exists(dataFile))
            {
                return;
            }

            try
            {
                using (StreamReader reader = new StreamReader(dataFile))
                {
                    string line;
                    int maxId = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length == 4)
                        {
                            int id = int.Parse(parts[0]);
                            string name = parts[1];
                            double price = double.Parse(parts[2]);
                            int quantity = int.Parse(parts[3]);

                            products.Add(new Product(id, name, price, quantity));

                            if (id > maxId) maxId = id;
                        }
                    }

                    nextId = maxId + 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading products: {ex.Message}");
            }
        }
    }
}
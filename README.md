
# 🛒 Simple E-Commerce Console App

A C# console-based simulation of an online shopping system that allows users to manage products, handle a shopping cart, and process checkout — all while demonstrating key C# concepts like OOP, method overloading, `ref`/`out`, recursion, exception handling, and optional file I/O.



##  Features

###  Core Requirements
- **Product Management** – Add, view, and search products (by ID or Name)
- **Shopping Cart** – Add items, view cart, checkout
- **Menu System** – Loop-driven console interface with `switch` statements
- **Method Overloading** – `SearchProduct(int id)` and `SearchProduct(string name)`
- **ref / out**  
  - `ref` to update product quantity after adding to cart  
  - `out` to return a found product in search methods
- **Recursion** – Display cart items recursively instead of loops
- **Exception Handling** – Handles invalid input, missing products, insufficient stock
- **String Interpolation** – Clean, readable output (`$"Product: {name}"`)

###  Bonus Features
- **File Handling** – Save/load products to/from a text file (`products.txt`) using `StreamWriter` / `StreamReader`
- **Discount System** – Apply percentage discount on total price
- **Order Stack** – Track completed orders using `Stack<Order>`
- **Input Validation** – Validate IDs, quantities, and names using `Regex`

---

##  Project Structure
SimpleEcommerce/
├── Program.cs # Entry point & menu loop
├── Models/
│ └── Product.cs # Product entity
├── Services/
│ ├── ProductManager.cs # CRUD + search + file ops
│ ├── CartManager.cs # Cart operations + recursion
│ └── OrderManager.cs # Checkout & discount
└── Utils/
└── InputValidator.cs # Regex validation


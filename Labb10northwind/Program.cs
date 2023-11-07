using Labb10northwind.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
class Program
{
    static void Main(string[] args)
    {
        using (var context = new NorthwindContext())
        {
            while (true)
            {
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1. List all customers");
                Console.WriteLine("2. View customer details");
                Console.WriteLine("3. Add a new customer");
                Console.WriteLine("4. Exit");

                switch (Console.ReadLine())
                {
                    case "1":
                        ListCustomers(context);
                        break;
                    case "2":
                        ViewCustomerDetails(context);
                        break;
                    case "3":
                        AddCustomer(context);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }
            }
        }
    }

    static void AddCustomer(NorthwindContext context)
    {
        var customer = new Customer
        {
            CustomerId = GenerateRandomCustomerID(),
            CompanyName = PromptForInput("Enter company name: ", false),
            ContactName = PromptForInput("Enter contact name: ", true),
            ContactTitle = PromptForInput("Enter contact title: ", true),
            Address = PromptForInput("Enter address: ", true),
            City = PromptForInput("Enter city: ", true),
            Region = PromptForInput("Enter region: ", true),
            PostalCode = PromptForInput("Enter postal code: ", true),
            Country = PromptForInput("Enter country: ", true),
            Phone = PromptForInput("Enter phone number: ", true),
        };

        context.Customers.Add(customer);
        context.SaveChanges();
        Console.WriteLine($"New customer added with ID: {customer.CustomerId}");
    }

    static string PromptForInput(string prompt, bool allowNull)
    {
        string input;
        do
        {
            Console.WriteLine(prompt);
            input = Console.ReadLine();
            if (allowNull)
            {
                return string.IsNullOrWhiteSpace(input) ? null : input;
            }
        } while (string.IsNullOrWhiteSpace(input));
        return input;
    }

    static string GenerateRandomCustomerID()
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 5)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    static void ListCustomers(NorthwindContext context)
    {
        Console.WriteLine("Sort by company name in (A)scending or (D)escending order?");
        var sortOrder = Console.ReadLine();

        var customersQuery = context.Customers
            .Select(c => new
            {
                c.CompanyName,
                c.Country,
                c.Region,
                c.Phone,
                OrderCount = c.Orders.Count
            });

        var customers = sortOrder.ToUpper() == "A"
            ? customersQuery.OrderBy(c => c.CompanyName).ToList()
            : customersQuery.OrderByDescending(c => c.CompanyName).ToList();

        foreach (var customer in customers)
        {
            Console.WriteLine($"Company: {customer.CompanyName}, Country: {customer.Country}, " +
                              $"Region: {customer.Region}, Phone: {customer.Phone}, " +
                              $"Orders: {customer.OrderCount}");
        }
    }

    static void ViewCustomerDetails(NorthwindContext context)
    {
        Console.WriteLine("Enter the ID of the customer you wish to view:");
        var customerId = Console.ReadLine();

        var customerDetails = context.Customers
            .Where(c => c.CustomerId == customerId)
            .Select(c => new
            {
                c.CompanyName,
                c.ContactName,
                c.ContactTitle,
                c.Address,
                c.City,
                c.Region,
                c.PostalCode,
                c.Country,
                c.Phone,
                Orders = c.Orders.Select(o => new { o.OrderId, o.OrderDate, o.ShippedDate }).ToList()
            })
            .FirstOrDefault();

        if (customerDetails == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        Console.WriteLine($"Company: {customerDetails.CompanyName}");
        Console.WriteLine($"Contact: {customerDetails.ContactName}, {customerDetails.ContactTitle}");
        Console.WriteLine($"Address: {customerDetails.Address}");
        Console.WriteLine($"City: {customerDetails.City}, {customerDetails.Region}, {customerDetails.PostalCode}");
        Console.WriteLine($"Country: {customerDetails.Country}");
        Console.WriteLine($"Phone: {customerDetails.Phone}");
        Console.WriteLine("Orders:");
        foreach (var order in customerDetails.Orders)
        {
            Console.WriteLine($"  Order ID: {order.OrderId}, Order Date: {order.OrderDate}, Shipped Date: {(order.ShippedDate.HasValue ? order.ShippedDate.ToString() : "Not shipped yet")}");
        }
    }

}

using System;
using System.Collections.Generic;

class Program
{
    // Both lists' indices correlate
    private static string[] itemList = { "Banana", "Book", "Ibuprofen", "Shelving Unit",
            "Milk", "Donuts", "Wireless Router", "Ethernet Cable", "Laptop"};

    // Both lists' indices correlate
    private static double[] priceList = { 3.25, 12.50, 9.99, 32.50, 
        3.99, 8.25, 129.99, 13.0, 1200};

    private static List<Entry> shoppingCart;

    static void Main(string[] args)
    {
        shoppingCart = new List<Entry>();

        DisplayInitialPrompt();

        bool done = false;
        while (!done)
        {
            // Dont display cart if it is empty
            if (shoppingCart.Count != 0)
                DisplayCurrentCart();

            Console.Write("Enter an item name: ");

            string input = Console.ReadLine();
            if (input.Equals("done"))
            {
                done = true;
            }
            else if (input.Equals("items"))
            {
                DisplayItemList();
            }
            else
            {
                AddToCart(input);
            }
        }
        DisplayFinalCartTotal();
    }

    public static void DisplayInitialPrompt()
    {
        DisplayItemList();
        Console.Write("To add an item to the cart, first enter the name of the item\n");
        Console.Write("Example input: \t Milk\n");
        Console.Write("To view the list of items again, enter: \"items\"\n");
        Console.Write("To exit app, enter: \"done\"\n");
    }

    public static void DisplayItemList()
    {
        string[] items = { "Banana (per 6)", "Book", "Ibuprofen (per bottle)", "Shelving Unit",
            "Milk (buy 2 get one free)", "Donuts (per dozen)", "Wireless Router",
            "Ethernet Cable", "Laptop"};

        double[] prices = { 3.25, 12.50, 9.99, 32.50, 3.99, 8.25, 129.99, 13.00, 1200 };

        Console.Write("Items: \n");
        for (int i = 0; i < items.Length; i++)
        {
            Console.Write("\t$" + prices[i] + "\t" + items[i] + "\n");
        }
    }

    public static void AddToCart(string input)
    {
        // Validate name of item, then quantity
        if (MatchInputName(input))
        {
            int quantity = GetInputQuantity();

            // Ensure Banana and Donuts quantities are of proper size
            if(input.Equals("Banana") || input.Equals("Donuts"))
                quantity = HandleBananaAndDonutQuantity(input, quantity);

            if (quantity > 0)
                AddEntryToCart(input, quantity);
        }
    }

    public static void DisplayCurrentCart()
    {
        Console.Write("Cart contains: ");
        if(shoppingCart != null)
        {
            // Print each entry with form: name(quantity)
            foreach (Entry entry in shoppingCart)
            {
                Console.Write(entry.getName() + "(" + entry.getQuantity() + ")  ");
            }
        }
        Console.Write("\n");
    }

    public static void AddEntryToCart(string name, int quantity)
    {
        if(shoppingCart == null)
            return;

        foreach (Entry entry in shoppingCart)
        {
            // If entry exists already, just add to the quantity
            if (entry.getName().Equals(name))
            {
                entry.setQuantity(entry.getQuantity() + quantity);
                return;
            }
        }

        double price = 0;

        for(int i = 0; i < itemList.Length; i++)
        {
            if(name.Equals(itemList[i]))
            {
                price = priceList[i];
            }
        }

        shoppingCart.Add(new Entry(name, quantity, price));
    }

    public static bool MatchInputName(string name)
    {
        foreach(string item in itemList)
        {
            if(item.Equals(name))
            {
                return true;
            }
        }
        NameErrorPrompt();
        return false;
    }

    public static int GetInputQuantity()
    {
        Console.Write("Enter Quantity: ");
        string input = Console.ReadLine();

        try
        {
            long quantity = Int64.Parse(input);
            if (quantity > 0 && quantity < Int32.MaxValue)
                    

                return (int)quantity;
            else
                QuantityErrorPrompt();
        }
        catch (FormatException)
        {
            QuantityErrorPrompt();
        }
        return -1;
    }

    public static int HandleBananaAndDonutQuantity(string nameInput, int quantityInput)
    {
        int correctInput = quantityInput;

        // If the item is a Banana and the quantity is not already a multiple of 6
        if(nameInput.Equals("Banana") && ((quantityInput % 6) != 0))
        {
            Console.Write("Please enter a multiple of 6.\n");
            correctInput = WaitForCorrectInput(6);
        }
        // If the item is Donuts and the quantity is not already a multiple of 12
        else if (nameInput.Equals("Donuts") && ((quantityInput % 12) != 0))
        {
            Console.Write("Please enter a multiple of 12.\n");
            correctInput = WaitForCorrectInput(12);

        }
        return correctInput;
    }

    public static int WaitForCorrectInput(int correctInput)
    {
        bool done = false;
        int quantity = 0;

        // Wait for user to input a quantity of proper size as specified by the parameter
        while(!done)
        {
            quantity = GetInputQuantity();

            if ((quantity % correctInput) == 0)
                done = true;
        }
        return quantity;
    }

    public static void NameErrorPrompt()
    {
        Console.Write("Input not recognized. Use format: [Name of Item] [Quantity]\n");
    }

    public static void QuantityErrorPrompt()
    {
        Console.Write("Invalid Quantity. Must enter number between 1 and 2,147,483,647\n");
    }

    public static void DisplayFinalCartTotal()
    {
        PrintEachItem();

        double subtotal = 0;
        double taxTotal = 0;
        double endTotal = 0;

        // Add the total price to the subtotal per entry as well as calculate tax
        foreach(Entry entry in shoppingCart)
        {
            // No tax on food items
            if(CheckFood(entry)) {
                subtotal += entry.getTotalPrice();
            }
            // Medical items taxed at 2.5%
            else if(CheckMedical(entry))
            {
                subtotal += entry.getTotalPrice();
                taxTotal += (entry.getTotalPrice() * 0.025);
            }
            // Everything else taxed at 5.5%
            else
            {
                subtotal += entry.getTotalPrice();
                taxTotal += (entry.getTotalPrice() * 0.055);
            }
        }

        double discount = FindDiscounts(subtotal);


        endTotal = subtotal - discount + taxTotal;

        // Round to get nice looking numbers
        subtotal = Math.Round(subtotal, 2);
        taxTotal = Math.Round(taxTotal, 2);
        endTotal = Math.Round(endTotal, 2);

        Console.Write("Subtotal = $" + subtotal + "\n");
        Console.Write("Tax = $" + taxTotal + "\n");
        Console.Write("Discount = $" + discount + "\n");
        Console.Write("Total = $" + endTotal + "\n");
    }

    public static bool CheckFood(Entry entry)
    {
        string[] foodItems = { "Milk", "Banana", "Donuts"};

        for (int i = 0; i < foodItems.Length; i++)
            if (foodItems[i].Equals(entry.getName()))
                return true;
            
        return false;
    }
        
    public static bool CheckMedical(Entry entry)
    {
        if(entry.getName().Equals("Ibuprofen"))
            return true;

        return false;
    }

    public static double FindDiscounts(double subtotal)
    {
        double discount = 0;

        // discount @ $150
        if (subtotal > 150)
            discount += 5;
        // discount @ $1000
        if (subtotal > 1000)
            discount += 15;

        return discount;
    }

    public static void PrintEachItem()
    {
        int index = 1;
        foreach(Entry entry in shoppingCart)
        {
            Console.Write(index + ". Item: " + entry.getName() + ", Quantity: " 
                + entry.getQuantity() + " = $" + Math.Round(entry.getTotalPrice(), 2) + "\n");
            index++;
        }
    }
}

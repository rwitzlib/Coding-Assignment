using System;
using System.Collections.Generic;
using System.Text;

class Entry
{
    private string name;
    private int quantity;
    private double pricePerItem;

    public Entry(string name, int quantity, double price)
    {
        this.name = name;
        this.quantity = quantity;
        this.pricePerItem = price;
    }

    public string getName()
    {
        return name;
    }

    public int getQuantity()
    {
        return quantity;
    }

    public void setQuantity(int newQuantity)
    {
        this.quantity = newQuantity;
    }

    public double getTotalPrice()
    {
        // Priced by the half-dozen
        if (this.name.Equals("Banana"))
            return 3.25 * (this.quantity / 6);

        // Priced by the dozen
        else if (this.name.Equals("Donuts"))
            return 8.25 * (this.quantity / 12);

        // Buy one get one free
        else if (this.name.Equals("Milk"))
            return 3.99 * (this.quantity - (this.quantity / 3));

        return this.quantity * this.pricePerItem;
    }


}

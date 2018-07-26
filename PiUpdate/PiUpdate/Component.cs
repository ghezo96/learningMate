﻿using System;
using System.IO;
using System.Text.RegularExpressions;

public class Component
{
    private string name;
    private string GPIO;
    private string currentCharge;
    public string previousCharge;



    //Default constructor
    public Component()
	{
        previousCharge = "0";
	}

    public Component(string name, string GPIO, string currentCharge)
    {
        this.name = name;
        this.GPIO = GPIO;
        this.currentCharge = currentCharge;
        this.previousCharge = this.currentCharge;
    }

    public string getGPIO()
    {
        return this.GPIO;
    }
    public string getName()
    {
        return this.name;
    }

    //Setters and getters
    public void setCurrentCharge(string currentCharge)
    {
        this.currentCharge = currentCharge;
    }

    public void setPreviousCharge(string previousCharge)
    {
        this.previousCharge = previousCharge;
    }

    //Methods
    public bool isChanged()
    {
        if(this.currentCharge != this.previousCharge)
        {
           return true;
        }
        return false;
    }

    public string getJson()
    {
        return "{\"name\": \"" + this.name + "\", \"state\": " + this.currentCharge + "}";
    }

    public string getPinNumber()
    {
        int index = this.GPIO.Length - 1;
        char pinNumber = this.GPIO[index];
        return pinNumber.ToString();
    }

    public void update()
    {
        this.previousCharge = this.currentCharge;
        this.currentCharge = Regex.Replace(File.ReadAllText("/sys/class/gpio/" + this.GPIO + "/value"), @"\t|\n|\r", "");

    }
}
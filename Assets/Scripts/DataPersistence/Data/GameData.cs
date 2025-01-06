using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int tattoosCompleted;
    public int currentTimeElapsed = 0;
    public Inventory inventory;
    public List<TattooClientData> tattooClients;
    public List<TattooClientBookingData> currentBookingList;
    public TattooClientBookingData currentBookedClient;

    public GameData()
    {
        tattoosCompleted = 0;
        currentTimeElapsed = 0;
        inventory = new Inventory();
        tattooClients = new List<TattooClientData>();
        currentBookingList = new List<TattooClientBookingData>();
        currentBookedClient = null;
    }
}

[System.Serializable]
public class Inventory
{
    public int totalCash;
    public int numCandy;
    public int numLidocaneSpray;
    public int numNumbingCream;
    public int machineType;

    public Inventory()
    {
        totalCash = 0;
        numCandy = 0;
        numLidocaneSpray = 0;
        numNumbingCream = 0;
        machineType = 1;
    }
}
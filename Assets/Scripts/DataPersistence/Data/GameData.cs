using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int tattoosCompleted;
    public Inventory inventory;
    public List<TattooClientData> tattooClients;
    public List<TattooClientBookingData> currentBookingList;

    public GameData()
    {
        tattoosCompleted = 0;
        inventory = new Inventory();
        tattooClients = new List<TattooClientData>();
        currentBookingList = new List<TattooClientBookingData>();
    }
}

[System.Serializable]
public class Inventory
{
    public int numCandy;
    public int numLidocaneSpray;
    public int numNumbingCream;
    public int machineType;

    public Inventory()
    {
        numCandy = 0;
        numLidocaneSpray = 0;
        numNumbingCream = 0;
        machineType = 1;
    }
}
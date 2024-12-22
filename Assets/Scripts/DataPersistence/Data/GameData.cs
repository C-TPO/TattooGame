using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int tattoosCompleted;
    public Inventory inventory;
    public List<TattooClient> clients;

    public GameData()
    {
        tattoosCompleted = 0;
        inventory = new Inventory();
        clients = new List<TattooClient>();
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
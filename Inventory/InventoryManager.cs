using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryManager : Resource
{
    Dictionary<String, String> inventoryManager = new Dictionary<String, String>();
    private String[] equippedItem = new String[2];
    public void CreateInventoryManager(){
        inventoryManager.Add("BasicObject1", "TempObject");

    }
    public void CreateEquippedItem(String[] equippedItem){
        this.equippedItem = equippedItem;
    }
    public void PrepareAction(String action, PlayerInfoManager playerInfoManager){
        switch(action){
            case "BasicObject1":
                BasicInventory(playerInfoManager);
                break;
        }
    }
    public void DoAction(String action, PlayerInfoManager pim){
        switch (action){
            case "BasicObject1":
				pim.AnimateCharacter("Attack");
                pim.StartTimer();
                break;
        }
    }
    public void BasicInventory(PlayerInfoManager playerInfoManager){ 
        playerInfoManager.EndSelectMove();
    }

    public String[] EquippedItem{
        get => equippedItem;
        set => equippedItem = value;
    }
    public Dictionary<String, String> InventoryManagerDictionary{
        get => inventoryManager;
        set => inventoryManager = value;
    }
}

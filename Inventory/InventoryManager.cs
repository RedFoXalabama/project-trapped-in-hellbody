using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryManager : Resource
{
    //Variabili
    //inventario che contiene gli oggetti utilizzabili durante il combattimento
    //Da modificare in base a come verrà implementato l'inventario poichè il TValue (ex.TempObject) non è utilizzato
    Dictionary<String, String> inventoryManager = new Dictionary<String, String>();
    private String[] equippedItem = new String[2];
    public void CreateInventoryManager(){
        inventoryManager.Add("BasicObject1", "TempObject");

    }
    public void CreateEquippedItem(String[] equippedItem){ //lista oggetti equipaggiati
        this.equippedItem = equippedItem;
    }
    //FUNZIONI AZIONI
    //prepara la funzione da eseguire
    public void PrepareAction(String action, PlayerInfoManager playerInfoManager){
        switch(action){
            case "BasicObject1":
                BasicInventory(playerInfoManager);
                break;
        }
    }
    //esegue la funzione
    public void DoAction(String action, PlayerInfoManager pim){
        switch (action){
            case "BasicObject1":
				pim.AnimateCharacter("Attack");
                pim.StartTimer();
                break;
        }
    }
    //FUNZIONI OGGETTI
    public void BasicInventory(PlayerInfoManager playerInfoManager){ 
        playerInfoManager.EndSelectMove();
    }

    //GETTER E SETTER
    public String[] EquippedItem{
        get => equippedItem;
        set => equippedItem = value;
    }
    public Dictionary<String, String> InventoryManagerDictionary{
        get => inventoryManager;
        set => inventoryManager = value;
    }
}

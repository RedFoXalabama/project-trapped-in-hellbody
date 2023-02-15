using Godot;
using System;
using System.Collections.Generic;

public class AllyManager : Resource
{
    //DATABASE ALLEATI
    Dictionary<String, String> dataBase = new Dictionary<String, String>();
    public void CreateDataBase(){
        dataBase.Add("Demo_Ally", "res://Characters/Ally/Demo_Ally/Demo_Ally.tscn");
    }

    //ALLEATI EQUIPAGGIATI
    private String[] equippedAlly = new String[4];

    public String[] EquippedAlly{
        get => equippedAlly;
    }
    public void EquipAlly(String allyName, int position){
        EquippedAlly[position] = allyName;
    }
    public String EqAllyPath(int position){
        if (EquippedAlly[position] != null){
            return dataBase[EquippedAlly[position]];
        }  
        return ""; //da come chiave niente se non c'è nessun ally eq in una posizione dell'array,
        //il database non trova nulla e passa "" che nella if della funzione createAllyPS salta la riga
    }
}
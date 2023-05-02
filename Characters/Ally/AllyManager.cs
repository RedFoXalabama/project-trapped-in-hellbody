using Godot;
using System;
using System.Collections.Generic;

public partial class AllyManager : Resource
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
     
    //FUNZIONI PER IL BATTLE MENU
    Dictionary<String, String[]> AllySkillManager = new Dictionary<String, String[]>();
    public void CreateAllySkillManager(){
        AllySkillManager.Add("Ally1", new String[]{"BasicAttack1", "BasicAttack2"});
    }
    public void PrepareAction(string action, AllyInfoManager aim){
        switch(action){
            case "BasicAttack1":
                BasicAttack1(aim);
                break;
            case "BasicAttack2":
                BasicAttack2(aim);
                break;
        }
    }
    public void DoAction(string action, AllyInfoManager aim){
        switch (action){
            case "BasicAttack1":
                aim.AnimateCharacter("Attack");
                aim.SelectedEnemy.AnimateCharacter("Damage");
                aim.StartTimer();
                break;
            case "BasicAttack2":
                aim.AnimateCharacter("Attack");
                aim.SelectedEnemy.AnimateCharacter("Damage");
                aim.StartTimer();
                break;
        }
    }
    //ELENCO MOSSE
    public void BasicAttack1(AllyInfoManager aim){
        aim.SelectEnemy();
    }
    public void BasicAttack2(AllyInfoManager aim){
        aim.SelectEnemy();
    }
    //GETTER AND SETTER
    public Dictionary<String, String[]> AllySkillManagerDictionary{
        get => AllySkillManager;
        set => AllySkillManager = value;
    }
}
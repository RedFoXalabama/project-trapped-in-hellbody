using Godot;
using System;
using System.Collections.Generic;

public partial class AllyManager : Resource
{
    //DATABASE ALLEATI
    Dictionary<String, String> dataBase = new Dictionary<String, String>();
    public void CreateDataBase(){
        dataBase.Add("Ally1", "res://Characters/Ally/Ally/Ally.tscn");
        dataBase.Add("Ally2", "res://Characters/Ally/Ally2/Ally2.tscn");
        dataBase.Add("", "");
    }

    //ALLEATI EQUIPAGGIATI
    private String[] equippedAlly = new String[4];

    public String[] EquippedAlly{
        get => equippedAlly;
        set => equippedAlly = value;
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
    TTBCScript tTBCScript;
    PlayerInfoManager pim;
    int EvokeAllyNumber;
    Boolean previouslyEvoked = false;
    public void CreateAllySkillManager(){
        AllySkillManager.Add("Ally1", new String[]{"BasicAttack1", "BasicAttack2"});
        AllySkillManager.Add("Ally2", new String[]{"BasicAttack1", "BasicAttack2"});
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
    //FUNZIONI PER EVOCARE L'ALLEATO
    public void SelectAlly(string selectedAlly, PlayerInfoManager pim, Dictionary<String, AllyInfoManager> dal, int allyNumber){
        this.pim = pim;
        AllyInfoManager aim = pim.GetAllyInfoManager(selectedAlly);
        tTBCScript = pim.TTBCScript;
        var allyList = tTBCScript.AllyList;
        EvokeAllyNumber = allyNumber;
        //se è null non è presente nella lista degli alleati in campo, quindi di default è in campo
        switch (aim){
            case null: //NON è presente nella lista degli alleati in campo
                if (!dal.ContainsKey(selectedAlly) && !allyList.ContainsKey(selectedAlly)){ //se non è ancora stato evocato
                    //FUNZIONI PER EVOCARLO
                    String[] option = new String[]{"Evoca"};
                    pim.AllyOptionMenu.OverrideButton(option);
                    if (!previouslyEvoked){ //controllo per evitare di aggiungere più volte lo stesso segnale
                        pim.AllyOptionMenu.GetButton(0).Pressed += PrepareAllySummon;
                    } 
                    pim.AllyOptionMenu.ShowUp();
                    previouslyEvoked = true;  
                } else { //se è già stato evocato
                    previouslyEvoked = false;
                    GD.Print("Ally already summoned");
                }
                break;
            default: //È presente nella lista degli alleati in campo
                //funzioni per gestire alleato
                previouslyEvoked = false;
                break;
        }
        
    }
    public void PrepareAllySummon(){
        pim.EndSelectMove();
    }
    public void SummonAlly(){
        tTBCScript.SpawnAlly(tTBCScript.AllyPS[EvokeAllyNumber]);
        pim.StartTimer();
        //poichè non posso rimettere a null l'EvokeAllyNumber non lo modifico, se le funzioni si eseguono correttamente verrà sempre sovrascritto
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
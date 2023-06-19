using Godot;
using System;
using System.Collections.Generic;

public partial class SkillManager : Resource
{   
    //<"Nome Attacco", "Nome funzione">
    //è un dizionario contenente gli attacchi disponibili e le relative funzioni da chiamare
    Dictionary<String, String> skillManager = new Dictionary<String, String>();
    private String[] equippedSkill = new String[2]; //è un array contenente gli attacchi equipaggiati
    //crea il dizionario degli attacchi disponibili, sostituire i valori cosi HD con un file json
    public void CreateSkillManager(){
        skillManager.Add("BasicAttack1", "TempAttack");
        skillManager.Add("BasicAttack2", "TempAttack2");

    }

    public void CreateEquippedSkill(String[] equippedSkill){ //crea l'array degli attacchi equipaggiati
        this.equippedSkill = equippedSkill;
    }

    //FORMULA DEL DANNO
    //calcola il danno inflitto ad un nemico in base agli attributi del personaggio e del nemico
    MagicProp magicProp = ResourceLoader.Load<MagicProp>("res://Combat System/MagicProp.tres") as MagicProp;
    public int DamageFormula(PlayerInfoManager pim, EnemyInfoManager eim){
        return (int)((pim.Attack) - (eim.Defense/magicProp.CalcMagicEffect(pim, eim)));
    }
    //FUNZIONI AZIONI
    //preparazione della mossa con relative funzioni preliminari
    public void PrepareAction(String action, PlayerInfoManager playerInfoManager){
        switch(action){
            case "BasicAttack1":
                BasicAttack1(playerInfoManager);
                break;
            case "BasicPosition1":
                BasicPotion1(playerInfoManager);
                break;
        }
    }
    //esecuzione della mossa con relative funzioni e danni
    public void DoAction(String action, PlayerInfoManager pim){
		switch (action){
			case "BasicAttack1":
				//funzione da creare per l'attacco base
				pim.AnimateCharacter("Attack");
				pim.SelectedEnemy.AnimateCharacter("Damage");
                pim.SelectedEnemy.TakeDamage(DamageFormula(pim, pim.SelectedEnemy));
                pim.StartTimer();
				break;
			case "BasicAttack2":
				break;
		}
    }
    
    //FUNZIONI AZIONI/MOSSE
    public void BasicAttack1(PlayerInfoManager playerInfoManager){//attacco base 1 di prova
		playerInfoManager.SelectEnemy();
		//si esce dalla select move quando si sceglie il nemico
	}
	public void BasicPotion1(PlayerInfoManager playerInfoManager){
		playerInfoManager.EndSelectMove();
	}

    public String[] EquippedSkill{
        get => equippedSkill;
        set => equippedSkill = value;
    }
    public Dictionary<String, String> SkillManagerDictionary{
        get => skillManager;
        set => skillManager = value;
    }
}

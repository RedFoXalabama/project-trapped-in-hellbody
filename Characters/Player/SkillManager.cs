using Godot;
using System;
using System.Collections.Generic;

public partial class SkillManager : Resource
{   
    //<"Nome Attacco", "Nome funzione">
    Dictionary<String, String> skillManager = new Dictionary<String, String>();
    private String[] equippedSkill = new String[2];
    public void CreateSkillManager(){
        skillManager.Add("BasicAttack1", "TempAttack");
        skillManager.Add("BasicAttack2", "TempAttack2");

    }
    public void CreateEquippedSkill(String[] equippedSkill){
        this.equippedSkill = equippedSkill;
    }
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
    public void DoAction(String action, PlayerInfoManager pim){
		switch (action){
			case "BasicAttack1":
				//funzione da creare per l'attacco base
				pim.AnimateCharacter("Attack");
				pim.SelectedEnemy.AnimateCharacter("Damage");
				pim.StartTimer();
				break;
			case "BasicAttack2":
				break;
		}
    }
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

using Godot;
using System;
using System.Collections.Generic;

public partial class SkillManager : Resource
{   
    Dictionary<String, String> skillManager = new Dictionary<String, String>();
    private String[] equippedSkill = new String[4];
    public void CreateSkillManager(){
        skillManager.Add("BasicAttack", "TempAttack");
        skillManager.Add("BasicAttack2", "TempAttack2");

        skillManager.Add("BasicInvetory", "TempInventory");
    }
    
}

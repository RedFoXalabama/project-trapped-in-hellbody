using Godot;
using System;
using System.Collections.Generic;

public partial class SkillManager : Resource
{   
    Dictionary<int, String> skillManager = new Dictionary<int, String>();
    private String[] equippedSkill = new String[4];
    public void CreateSkillManager(){
        skillManager.Add(0, "BasicAttack1");
    }
    
}

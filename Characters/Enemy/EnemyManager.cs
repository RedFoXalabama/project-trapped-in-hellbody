using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyManager : Resource
{
    //DATABASE NEMICI
    Dictionary<string,string> dataBase = new Dictionary<string,string>();
    public void CreateDatabase(){
        dataBase.Add("Demo_Enemy1", "res://Characters/Enemy/Demo_Enemy1/Demo_Enemy1.tscn");
        dataBase.Add("Demo_Enemy2", "res://Characters/Enemy/Demo_Enemy2/Demo_Enemy2.tscn");
        dataBase.Add("Demo_Enemy3", "res://Characters/Enemy/Demo_Enemy3/Demo_Enemy3.tscn");
    }
    public String EnemyPath(String enemyName){
        return dataBase[enemyName];
    }

}

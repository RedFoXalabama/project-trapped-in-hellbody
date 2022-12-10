using Godot;
using System;

public class Demo_Enemy_LifeBar : ProgressBar{
    public void ChangeValue(int value){
        this.Value = value;
    }
}

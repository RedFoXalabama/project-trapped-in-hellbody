using Godot;
using System;

public partial class GameBar : ProgressBar{
    public override void _Ready(){
        var bar_background = GetNode<Sprite2D>("Bar_Background");
        var rect_size = bar_background.Texture2D.GetSize();
        rect_size.y -= rect_size.y/5;
        this.Size = rect_size;
        
    }

    public void ChangeValue(int value){
        this.Value = value;
    }
    public void ChangeMaxValue(int maxValue){
        this.MaxValue = maxValue;
    }
}

using Godot;
using System;

public class GameBar : ProgressBar{
    public override void _Ready(){
        var bar_background = GetNode<Sprite>("Bar_Background");
        var rect_size = bar_background.Texture.GetSize();
        rect_size.y -= rect_size.y/5;
        this.RectSize = rect_size;
        
    }

    public void ChangeValue(int value){
        this.Value = value;
    }
    public void ChangeMaxValue(int maxValue){
        this.MaxValue = maxValue;
    }
}

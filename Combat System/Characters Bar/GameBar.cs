using Godot;
using System;

public partial class GameBar : ProgressBar{
	public override void _Ready(){
		var bar_background = GetNode<Sprite2D>("Bar_Background");
		var rect_size = bar_background.Texture.GetSize();
		rect_size.Y -= rect_size.Y/5;
		SetDeferred("size", rect_size);
		
	}

	public void ChangeValue(int value){
		this.Value = value;
	}
	public void ChangeMaxValue(int maxValue){
		this.MaxValue = maxValue;
	}
}

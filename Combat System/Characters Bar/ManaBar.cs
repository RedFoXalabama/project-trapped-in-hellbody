using Godot;
using System;

public partial class ManaBar : HBoxContainer
{
	int mana;
	int maxMana;

	public override void _Ready()
	{
		switch(GetParent().Name){
			case "PlayerInfoManager":

				break;
			case "EnemyInfoManager":

				break;
			case "AllyInfoManager":

				break;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

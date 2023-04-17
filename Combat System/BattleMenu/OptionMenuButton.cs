using Godot;
using System;

public partial class OptionMenuButton : Button
{
	private int id;
	private Boolean ofPointer;
	[Signal] public delegate void OfPointerSignalEventHandler(int id);
	public override void _Ready(){
		FocusEntered += FocusEnteredButton;
	}
	public void FocusEnteredButton(){
		if(ofPointer){
			EmitSignal(SignalName.OfPointerSignal, id);
		}
		//modificare l'id_Button_focused del OptionMenu nella funzione che chiama la OfPointerSignal
	}
	public int Id{
		get => id;
		set => id = value;
	}
	public Boolean OfPointer{
		get => ofPointer;
		set => ofPointer = value;
	}
}

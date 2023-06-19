using Godot;
using System;

public partial class OptionMenuButton : Button
{
	//Questo speciale Button, utilizzato nell'OptionMenu, permette di emettere un segnale quando il bottone viene focusato
	//questo segnale serve per aggiornare l'id del bottone focusato nell'OptionMenu
	//permettendo cosi di capire che bottone è stato focusato e di conseguenza eseguire la funzione corretta
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

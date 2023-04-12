using Godot;
using System;

public partial class OptionMenu : Control
{
	private Control control;
	private Sprite2D sprite;
	private VBoxContainer vBoxContainer;
	[Export] private String[] option;
	[Export] private Boolean subMenu;
	public override void _Ready(){ //prende array di elementi che contengono le opzioni del menù
		var optionMenuButtonPS = GD.Load<PackedScene>("res://Combat System/BattleMenu/OptionMenuButton.tscn");
		//GETNODE
		control = this;
		sprite = GetNode<Sprite2D>("Sprite2D");
		vBoxContainer = GetNode<VBoxContainer>("VBoxContainer");
		//RIDIMENSIONAMENTO ELEMENTI
		control.SetSize(sprite.Texture.GetSize());
		vBoxContainer.SetSize(sprite.Texture.GetSize());
		//ISTANZIA BUTTON
		for (int i = 0; i < option.Length; i++){
			vBoxContainer.AddChild(optionMenuButtonPS.Instantiate());
			GetButton(i).Text = option[i];
			GetButton(i).Id = i;
			GetButton(i).FocusExited += CloseMenu;
		}
		//CREAZIONE ORDINE FOCUS BUTTON
		SetFocusNeighbourTopBottom();
		Hide();
	}
	public void OverrideButton(String[] optionElement){ //funzione per creare i pulsanti/opzioni del menu
		var optionMenuButtonPS = GD.Load<PackedScene>("res://Combat System/BattleMenu/OptionMenuButton.tscn");
	//Modifico il primo pulsante precreato per non dare errore ed aggiungo i successivi -> nel for parto da 1
		option = optionElement;
		if (vBoxContainer.GetChildCount() <= option.Length){ //I BUTTON AUMENTANO o sono costanti
			for (int i = 0; i < vBoxContainer.GetChildCount(); i++){ //button esistenti sovrascritti
				GetButton(i).Text = option[i];
			}
			for (int i = vBoxContainer.GetChildCount(); i < option.Length; i++){//crea button nuovi
				vBoxContainer.AddChild(optionMenuButtonPS.Instantiate());
				GetButton(i).Text = option[i];
				GetButton(i).Id = i;
				GetButton(i).FocusExited += CloseMenu;
			}
		} else if (vBoxContainer.GetChildCount() > option.Length){  //I BUTTON DIMINUSICONO
			for (int i = 0; i < option.Length; i++){ //button esistenti sovrascritti
				GetButton(i).Text = option[i];
			}
			for (int i = option.Length; i < vBoxContainer.GetChildCount(); i++){//elimina button non utilizzati
				GetButton(i).QueueFree();
			}
		}
		SetFocusNeighbourTopBottom();
	}
	public void SetFocusNeighbourTopBottom(){
		for (int i = 0; i < option.Length; i++){
			if( i == 0){ //BOTTONE SOPRA
				GetButton(0).FocusNeighborTop = GetButton(option.Length-1).GetPath();
			} else {
				GetButton(i).FocusNeighborTop = GetButton(i-1).GetPath();
			}
			if ( i == option.Length - 1){ //BOTTONE SOTTO
				GetButton(option.Length - 1).FocusNeighborBottom = GetButton(0).GetPath();
			} else {
				GetButton(i).FocusNeighborBottom = GetButton(i+1).GetPath();
			}	
		}
	}
	public void SetFocusPrevioustTo(OptionMenu to){
		for (int i = 0; i < option.Length; i++){ //BATTLE MENU
			this.GetButton(i).FocusPrevious = to.GetButton(0).GetPath();
		}
	}
	public OptionMenuButton GetButton(int i){
		return vBoxContainer.GetChild<OptionMenuButton>(i);
	}
	public void ShowUp(){
		Visible = true;
		vBoxContainer.GetChild<OptionMenuButton>(0).GrabFocus();
	}
	public void CloseMenu(){
		if (subMenu && Input.IsActionJustPressed("ui_focus_prev")){
			Hide();
		}
	}
}

using Godot;
using System;

public partial class OptionMenu : Control
{
	private Control control;
	private Sprite2D sprite;
	private VBoxContainer vBoxContainer;
	[Export] private String[] Option;
	[Export] private Boolean subMenu;

	public override void _Ready(){ //prende array di elementi che contengono le opzioni del menù
		//VARIABILE TEMPORANEA
		//GETNODE
		control = this;
		sprite = GetNode<Sprite2D>("Sprite2D");
		vBoxContainer = GetNode<VBoxContainer>("VBoxContainer");
		//RIDIMENSIONAMENTO ELEMENTI
		control.SetSize(sprite.Texture.GetSize());
		vBoxContainer.SetSize(sprite.Texture.GetSize());
		//ISTANZIA BUTTON
		for (int i = 0; i < Option.Length; i++){
			vBoxContainer.AddChild(new Button());
			vBoxContainer.GetChild<Button>(i).Text = Option[i];
			vBoxContainer.GetChild<Button>(i).FocusExited += CloseMenu;
		}
		//CREAZIONE ORDINE FOCUS BUTTON
		for (int i = 0; i < Option.Length; i++){
			if( i == 0){ //BOTTONE SOPRA
				vBoxContainer.GetChild<Button>(0).FocusNeighborTop = vBoxContainer.GetChild<Button>(Option.Length-1).GetPath();
			} else {
				vBoxContainer.GetChild<Button>(i).FocusNeighborTop = vBoxContainer.GetChild<Button>(i-1).GetPath();
			}
			if ( i == Option.Length - 1){ //BOTTONE SOTTO
				vBoxContainer.GetChild<Button>(Option.Length - 1).FocusNeighborBottom = vBoxContainer.GetChild<Button>(0).GetPath();
			} else {
				vBoxContainer.GetChild<Button>(i).FocusNeighborBottom = vBoxContainer.GetChild<Button>(i+1).GetPath();
			}	
		}
		Hide();
		
	}
	public Button GetButton(int i){
		return vBoxContainer.GetChild<Button>(i);
	}
	public void ShowUp(){
		Visible = true;
		vBoxContainer.GetChild<Button>(0).GrabFocus();
	}
	public void CloseMenu(){
		if (subMenu && Input.IsActionJustPressed("ui_focus_prev")){
			Hide();
		}
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
}

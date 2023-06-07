using Godot;
using System;

public partial class OptionMenu : Control
{
	// Riferimenti ai componenti del menu
	private Control control;
	private Sprite2D sprite;
	private VBoxContainer vBoxContainer;
	private int id_ButtonFocused;
	// Opzioni del menu
	[Export] private String[] option;

	// Indica se il menu è un sottomenu o meno
	[Export] private Boolean subMenu;

	/**
	 * Metodo invocato quando l'oggetto viene caricato e istanziato.
	 * In questo metodo, vengono inizializzati i riferimenti ai componenti del menu,
	 * le opzioni del menu vengono caricate e configurati i pulsanti per le opzioni del menu.
	 * I pulsanti vengono aggiunti come figli di un contenitore verticale (VBoxContainer).
	 * Viene inoltre configurato l'evento di FocusExited sui pulsanti,
	 * che viene chiamato quando il pulsante perde il focus.
	 */
	public override void _Ready(){
		// Carica la scena del pulsante del menu
		var optionMenuButtonPS = GD.Load<PackedScene>("res://Combat System/BattleMenu/OptionMenuButton.tscn");

		// Inizializza i riferimenti ai componenti del menu
		control = this;
		sprite = GetNode<Sprite2D>("Sprite2D");
		vBoxContainer = GetNode<VBoxContainer>("VBoxContainer");

		// Ridimensiona gli elementi del menu
		control.SetSize(sprite.Texture.GetSize());
		vBoxContainer.SetSize(sprite.Texture.GetSize());

		// Configura i pulsanti per le opzioni del menu
		for (int i = 0; i < option.Length; i++){
			vBoxContainer.AddChild(optionMenuButtonPS.Instantiate());
			GetButton(i).Text = option[i];
			GetButton(i).Id = i;
			GetButton(i).FocusExited += CloseMenu;
		}

		// Imposta i vicini di focus per ogni pulsante
		SetFocusNeighbour();

		// Nasconde il menu
		Hide();
	}

	/**
	 * Metodo per creare i pulsanti per le opzioni del menu.
	 * Questo metodo accetta un array di stringhe optionElement, che viene usato
	 * per sovrascrivere le opzioni del menu correnti.
	 * Vengono creati pulsanti se necessario e quelli inutilizzati vengono rimossi.
	 */
	public int OverrideButton(String[] optionElement){
		// Carica la scena del pulsante del menu
		var optionMenuButtonPS = GD.Load<PackedScene>("res://Combat System/BattleMenu/OptionMenuButton.tscn");
		var nButtonNew = 0;
		// Modifica il primo pulsante precreato per non dare errore ed aggiunge i successivi
		option = optionElement;
		if (vBoxContainer.GetChildCount() <= option.Length){
			for (int i = 0; i < vBoxContainer.GetChildCount(); i++){
				GetButton(i).Text = option[i];
			}
			for (int i = vBoxContainer.GetChildCount(); i < option.Length; i++){
				vBoxContainer.AddChild(optionMenuButtonPS.Instantiate());
				GetButton(i).Text = option[i];
				GetButton(i).Id = i;
				GetButton(i).FocusExited += CloseMenu;
				nButtonNew++;
			}
		} else if (vBoxContainer.GetChildCount() > option.Length){
			for (int i = 0; i < option.Length; i++){
				GetButton(i).Text = option[i];
			}
			for (int i = option.Length; i < vBoxContainer.GetChildCount(); i++){
				GetButton(i).QueueFree();
			}
		}

		// Imposta i vicini di focus per ogni pulsante
		SetFocusNeighbour();
		return nButtonNew;
	}

	public void SetPressed(String[] array,Boolean ofPointer ,OptionMenuButton.OfPointerSignalEventHandler ButtonFocused, Action ButtonPressed){
		for (int i = 0; i < array.Length; i++){
			GetButton(i).OfPointer = ofPointer;
			GetButton(i).OfPointerSignal += ButtonFocused;
			GetButton(i).Pressed += ButtonPressed;
		}

	}
	public void SetPressed(String[] array, int start ,Boolean ofPointer ,OptionMenuButton.OfPointerSignalEventHandler ButtonFocused, Action ButtonPressed){
		for (int i = start; i < array.Length; i++){
			GetButton(i).OfPointer = ofPointer;
			GetButton(i).OfPointerSignal += ButtonFocused;
			GetButton(i).Pressed += ButtonPressed;
		}
	}
	// Imposta i vicini di focus per ogni pulsante
	public void SetFocusNeighbour(){
		//imposta i focus sopra e sotto per navigare verticalmente
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
			//imposta i focus a destra e sinistra per EVITARE DI NAVIGARE ORIZZONTALMENTE
			GetButton(i).FocusNeighborLeft = GetButton(i).GetPath();
			GetButton(i).FocusNeighborRight = GetButton(i).GetPath();
		}
	}

	/**
	 * Metodo per impostare il focus sul pulsante precedente.
	 * Viene utilizzato quando il menu principale viene chiuso e il focus
	 * deve essere impostato sul pulsante precedente nel menu principale.
	 */
	public void SetFocusPrevioustTo(OptionMenu to){
		for (int i = 0; i < option.Length; i++){
			this.GetButton(i).FocusPrevious = to.GetButton(0).GetPath();
		}
	}

	// Restituisce il bottone in posizione "i"
	public OptionMenuButton GetButton(int i){
		return vBoxContainer.GetChild<OptionMenuButton>(i);
	}

	// Mostra il menu
	public void ShowUp(){
		Visible = true;
		vBoxContainer.GetChild<OptionMenuButton>(0).GrabFocus();
	}

	// Chiude il menu
	public void CloseMenu(){
		if (subMenu && Input.IsActionJustPressed("ui_focus_prev")){
			Hide();
		}
	}
	public int Id_ButtonFocused{
		get => id_ButtonFocused;
		set => id_ButtonFocused = value;
	}
}


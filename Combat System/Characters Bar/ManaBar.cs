using Godot;
using System;

public partial class ManaBar : Node
{
	//barra del mana che è formata da un timer che ogni tot tempo aggiunge un mana e un hBoxContainer che contiene le pepite (sprite)
	//Variabili
	private int mana = 0;
	private int maxMana = 0;
	private int manaVelocity;
	private Sprite2D indicator;
	private Texture2D manaTexture = GD.Load<Texture2D>("res://Combat System/BattleMenu/Mana Perl.png");//INSERIRE IL MANA SPRITE
	private Timer timer;
	private HBoxContainer hBoxContainer;
	public override void _Ready()
	{
		indicator = GetNode<Sprite2D>("Indicator");
		timer = GetNode<Timer>("Timer");
		hBoxContainer = GetNode<HBoxContainer>("HBoxContainer");
	}
	//dato un mana iniziale, un manamax e una velocità di mana, crea la barra del mana
	public void Set_StartManaBar(int mana, int maxMana, int manaVelocity){
		//Settiamo i valori
		this.mana = mana;
		this.maxMana = maxMana;
		this.manaVelocity = manaVelocity;
		timer.WaitTime = manaVelocity;
		//Settiamo la grandezza del container
		hBoxContainer.SetSize(new Vector2(maxMana*manaTexture.GetSize().X , manaTexture.GetSize().Y));
		//Creiamo le pepite di mana aggiungnedo uno sprite per ogni mana modificando texture e posizione
		for (int i = 0; i < maxMana; i++)
		{
			hBoxContainer.AddChild(new Sprite2D());
			GetManaPerl(i).Texture = manaTexture;
			GetManaPerl(i).Position = new Vector2(i*manaTexture.GetSize().X , manaTexture.GetSize().Y);
		}
		//versione coi pannelli (non funziona)
		/*for (int i = 0; i < maxMana; i++)
		{
			hBoxContainer.AddChild(new PanelContainer());
			hBoxContainer.GetChild<PanelContainer>(i).AddChild(new Sprite2D());
			GetManaPerl(i).Texture = manaTexture;
			var temp = manaTexture.GetSize();
			var temp2 = new Vector2(i*manaTexture.GetSize().X , manaTexture.GetSize().Y);
			hBoxContainer.GetChild<PanelContainer>(i).Size = manaTexture.GetSize();
			hBoxContainer.GetChild<PanelContainer>(i).Position = new Vector2(i*manaTexture.GetSize().X , manaTexture.GetSize().Y);
		}*/
		ChangeManaVisibility();
		//Startiamo il timer
	}
	public void StartManaBar(){
		timer.Start();
	}
	//Segnale timer
	public void _on_timer_timeout(){ //quando il timer finisce aggiunge un mana se non è al massimo
		if (mana < maxMana)
		{
			mana++;
			ChangeManaVisibility();
		}
	}
	public void UseMana(int value){ //quando si usa il mana, diminuisce il mana e cambia la visibilità
		mana = mana - value;
		ChangeManaVisibility();
	}
	public void ChangeMaxMana(int maxValue){ //cambia il massimo mana e cambia la visibilità
		maxMana = maxValue;
	}
	public void ChangeManaVisibility(){//Serve a cambiare la visibilità dei mana in base al quantitativo
		for (int i = 0; i < mana; i++){ //per ogni mana inferiore al mana attuale, lo rende visibile
			GetManaPerl(i).Visible = true;
		}
		for (int i = mana; i < maxMana; i++){ //per ogni mana superiore al mana attuale, lo rende invisibile
			GetManaPerl(i).Visible = false;
		}
		indicator.Frame = mana; //aggiorna il numero dell'indicatore
	}
	public Sprite2D GetManaPerl(int mana){ //ritorna il mana in base al numero
		return hBoxContainer.GetChild<Sprite2D>(mana);
	}
}

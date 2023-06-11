using Godot;
using System;

public partial class ManaBar : Node
{
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
	public void Set_StartManaBar(int mana, int maxMana, int manaVelocity){
		//Settiamo i valori
		this.mana = mana;
		this.maxMana = maxMana;
		this.manaVelocity = manaVelocity;
		timer.WaitTime = manaVelocity;
		hBoxContainer.SetSize(new Vector2(maxMana*manaTexture.GetSize().X , manaTexture.GetSize().Y));
		//Creiamo le pepite di mana
		for (int i = 0; i < maxMana; i++)
		{
			hBoxContainer.AddChild(new Sprite2D());
			GetManaPerl(i).Texture = manaTexture;
			var temp = manaTexture.GetSize();
			var temp2 = new Vector2(i*manaTexture.GetSize().X , manaTexture.GetSize().Y);
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
	public void _on_timer_timeout(){
		if (mana < maxMana)
		{
			mana++;
			ChangeManaVisibility();
		}
	}
	public void UseMana(int value){
		mana = mana - value;
		ChangeManaVisibility();
	}
	public void ChangeMaxMana(int maxValue){
		maxMana = maxValue;
	}
	public void ChangeManaVisibility(){//Serve a cambiare la visibilità dei mana in base al quantitativo
		for (int i = 0; i < mana; i++)
		{
			GetManaPerl(i).Visible = true;
		}
		for (int i = mana; i < maxMana; i++)
		{
			GetManaPerl(i).Visible = false;
		}
		indicator.Frame = mana;
	}
	public Sprite2D GetManaPerl(int mana){
		return hBoxContainer.GetChild<Sprite2D>(mana);
	}
}

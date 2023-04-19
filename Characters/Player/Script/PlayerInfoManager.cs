using Godot;
using System;

public partial class PlayerInfoManager : Node2D , BaseMoves
{
	//PROPRIETIES
	[Export] private String cname;
	[Export] private int life;
	[Export] private int mana;
	[Export] private int attack;
	[Export] private int defense;
	[Export] private int velocity;
	[Export] private Boolean free = true; //VARIA TRAMITE ANIMAZIONE CHE MODIFICA IL VALORE
	private Timer timer;
	private AnimationNodeStateMachinePlayback animationState;
	private GameBar lifeBar;
	private GameBar manaBar;
	private NameBar nameBar;
	private OptionMenu battleMenu;
	private OptionMenu skillBattleMenu;
	private OptionMenu allyManagerMenu;
	private OptionMenu inventoryBattleMenu;
	private TTBCScript tTBCScript;
	//VARIABILI PER LA SCELTA MOSSA
	private SkillManager skillManager;
	private EnemyInfoManager selectedEnemy;
	private AllyInfoManager selectedAlly;
	private String selectedAction;

	public override void _Ready(){
		//Timer
		timer = GetNode<Timer>("BattleTimer");
		//Animazioni
		animationState = (AnimationNodeStateMachinePlayback)GetNode<Sprite2D>("BattleAnimation").GetNode<AnimationTree>("AnimationTree").Get("parameters/playback");
		GetNode<Sprite2D>("BattleAnimation").GetNode<AnimationTree>("AnimationTree").Active = true;
		//All Bars
		lifeBar = GetNode<GameBar>("LifeBar");
		manaBar = GetNode<GameBar>("ManaBar");
		nameBar = GetNode<NameBar>("NameBar");
		//Impostazione Bars
		nameBar.SetNameBar(Cname);
		lifeBar.ChangeMaxValue(Life); //al momento la vita non avendo un valore esplicito è zero
		manaBar.ChangeMaxValue(Mana); //al momento il mana non avendo un valore esplicito è zero
		//ALL Menu
		battleMenu = GetNode<OptionMenu>("BattleMenu");
		skillBattleMenu = battleMenu.GetNode<OptionMenu>("SkillBattleMenu");
		allyManagerMenu = battleMenu.GetNode<OptionMenu>("AllyManagerMenu");
		inventoryBattleMenu = battleMenu.GetNode<OptionMenu>("InventoryBattleMenu");
		AllBattleMenu_CreateSignals();//crea i segnali dei vari pulsanti 
		//TTBCSRIPT
		tTBCScript = GetParent().GetParent<TTBCScript>();
		
	}

	//FUNZIONI INTERFACCIA BASEMOVES
	public void GetDamage(int damage){
		//cambia stato FREE nell'animazione
		Life -= damage;
		lifeBar.ChangeValue(Life);
		AnimateCharacter("Damage");
	}
	public void StartTimer(){
		timer.Start();
	}
		//ANIMAZIONE
	public void AnimateCharacter(String animation){
		animationState.Travel(animation);
	}
	public void BackToIdle(){
		//DA MODIFICARE IN BASE ALLO STATO DELLA VITA
		AnimateCharacter("Idle_FullLife");
	}

	public void _on_BattleTimer_timeout(){ //METTE IN CODA DI ATTESA DEL TIMER
		tTBCScript.WaitingQueue.Enqueue(GetParent<Marker2D>()); //viene messo in coda alla SelectMove
		tTBCScript.UpdateSelectMoveQueue(); //aggiorna la selectmove, nel caso essa sia vuota cosi da poter scegliere la mossa
	}

	//SELEZIONE MOSSA
	public void SelectMove(){
		battleMenu.ShowUp();
	}
	public void EndSelectMove(){ //da inserire quando si scelie una mossa
		//si libera la SelectMoveQueue
		skillBattleMenu.Hide();
		allyManagerMenu.Hide();
		inventoryBattleMenu.Hide();
		battleMenu.Hide();
		tTBCScript.UpdateMoveQueue(tTBCScript.SelectMoveQueue);
	}
	//FUNZIONI CREZIONE BOTTONI
	public void AllBattleMenu_CreateSignals(){
		battleMenu.GetButton(0).Pressed += AttackPressed;
		battleMenu.GetButton(1).Pressed += AllyManagerPressed;
		battleMenu.GetButton(2).Pressed += InventoryPressed;
		battleMenu.GetButton(3).Pressed += EscapePressed;
		skillBattleMenu.GetButton(0).Pressed += BasicAttack1;
		inventoryBattleMenu.GetButton(0).Pressed += BasicPotion1;
		//aggiungere gli altri menu
		//SISTEMA FOCUS PRECEDENTI
		battleMenu.SetFocusPrevioustTo(battleMenu); 
		skillBattleMenu.SetFocusPrevioustTo(battleMenu);
		allyManagerMenu.SetFocusPrevioustTo(battleMenu);
		inventoryBattleMenu.SetFocusPrevioustTo(battleMenu);
	}
	public void AttackPressed(){
		skillBattleMenu.ShowUp();
		if (allyManagerMenu.Visible || inventoryBattleMenu.Visible){
			allyManagerMenu.Visible = false;
			inventoryBattleMenu.Visible = false;
		}
		//bisogna fermare il tempo e dare focus
	}
	public void AllyManagerPressed(){
		allyManagerMenu.ShowUp();
		if (skillBattleMenu.Visible || inventoryBattleMenu.Visible){
			skillBattleMenu.Visible = false;
			inventoryBattleMenu.Visible = false;
		}
	}
	public void InventoryPressed(){
		inventoryBattleMenu.ShowUp();
		if (skillBattleMenu.Visible || allyManagerMenu.Visible){
			skillBattleMenu.Visible = false;
			allyManagerMenu.Visible = false;
		}
	}
	public void EscapePressed(){

	}
	//SELEZIONARE UN NEMICO + relativi segnali
	public void SelectEnemy(){ //popupa il menu di scelta nemici e quando il segnale è inviato dal popmn il nemico viene selezionato
		//funzioni per decidere il nemico
		tTBCScript.CreateEnemyListOptionMenu();
		//il nemico si seleziona nel TTBCScript
	}

	public void BasicAttack1(){//attacco base 1 di prova
		/*HD*/selectedAction = "BasicAttack1";
		SelectEnemy();
		//si esce dalla select move quando si sceglie il nemico
	}
	public void BasicPotion1(){
		selectedAction = "BasicPotion1";
		EndSelectMove();
	}
	public void DoAction(){ //Data il nome della mossa memorizzata, prende la funzione dal dictionary ed esegue l'azione
		//vai in stato notFree dall'animazione
		//esecuzione mossa
		switch (selectedAction){
			case "BasicAttack1":
				//funzione da creare per l'attacco base
				AnimateCharacter("Attack");
				SelectedEnemy.AnimateCharacter("Damage");
				StartTimer();
				break;
			case "BasicAttack2":
				break;
			case "BasicPotion1":
				AnimateCharacter("Attack");
				break;

		}
		//pulizia variabili
		CleanSelectedMove();
	}
	public void CleanSelectedMove(){//da far eseguire ogni volta che si termina la mossa per evitare che si conservi la scelta
		selectedAction = null;
		selectedEnemy = null;
		selectedAlly = null;
	}
	public Boolean IsTargetFree(){ //controlla se il target è libero
		//se è libero ritorna true se non è libero ritorna false
		Boolean status = false;
		if (selectedEnemy != null){
			status = selectedEnemy.FreeForFight;
		} else if (selectedAlly != null){
			status = selectedAlly.FreeForFight;
		} else {
			status = true;
		}
		return status;
	}


	//GESTIONE ALLEATI


	//GESTIONE INVENTARIO


	//GETTER AND SETTER
	public String Cname {
		get => cname; 
		set => cname = value;
	}
	public int Life {
		get => life;
		set => life = value;
	}
	public int Mana {
		get => mana;
		set => mana = value;
	}
	public int Attack {
		get => attack;
		set => attack = value;
	}
	public int Defense {
		get => defense;
		set => defense = value;
	}
	public int Velocity {
		get => velocity;
		set => velocity = value;
	}
	public OptionMenu SkillBattleMenu{
		get => skillBattleMenu;
		set => skillBattleMenu = value;
	}
	public EnemyInfoManager SelectedEnemy{
		get => selectedEnemy;
		set => selectedEnemy = value;
	}
	public Boolean FreeForFight{
		get => free;
		set => free = value;
	}
}


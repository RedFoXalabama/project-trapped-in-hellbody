using Godot;
using System;

public partial class PlayerInfoManager : Node2D , BaseMoves
{
	//PROPRIETIES, [EXPORT] modificate tramite engine
	[Export] private String cname;
	[Export] private int life;
	[Export] private int mana;
	[Export] private int attack;
	[Export] private int defense;
	[Export] private int velocity;
	[Export] private int manaStart;
	[Export] private int maxMana;
	[Export] private int manaVelocity;
	[Export] private MagicProp.MagicType magicType;
	[Export] private float magicPower;
	[Export] private Boolean free = true; //VARIA TRAMITE ANIMAZIONE CHE MODIFICA IL VALORE
	private Timer timer;
	private AnimationNodeStateMachinePlayback animationState;
	private GameBar lifeBar;
	private ManaBar manaBar;
	private NameBar nameBar;
	private OptionMenu battleMenu;
	private OptionMenu skillBattleMenu;
	private OptionMenu allyManagerMenu;
	private OptionMenu allyOptionMenu;
	private OptionMenu inventoryBattleMenu;
	private TTBCScript tTBCScript;
	//VARIABILI PER LA SCELTA MOSSA
	SkillManager skillManager = ResourceLoader.Load<SkillManager>("res://Characters/Player/SkillManager.tres") as SkillManager;
	InventoryManager inventoryManager = ResourceLoader.Load<InventoryManager>("res://Inventory/Inventory.tres") as InventoryManager;
	AllyManager allyManager = ResourceLoader.Load<AllyManager>("res://Characters/Ally/AllyManager.tres") as AllyManager;
	MagicProp magicProp = ResourceLoader.Load<MagicProp>("res://Combat System/MagicProp.tres") as MagicProp;
	private EnemyInfoManager selectedEnemy;
	private AllyInfoManager selectedAlly;
	private String selectedAction;
	private String choosedMove;

	public override void _Ready(){
		//Timer
		timer = GetNode<Timer>("BattleTimer");
		//Animazioni
		animationState = (AnimationNodeStateMachinePlayback)GetNode<Sprite2D>("BattleAnimation").GetNode<AnimationTree>("AnimationTree").Get("parameters/playback");
		GetNode<Sprite2D>("BattleAnimation").GetNode<AnimationTree>("AnimationTree").Active = true;
		//All Bars
		lifeBar = GetNode<GameBar>("LifeBar");
		manaBar = GetNode<ManaBar>("ManaBar");
		nameBar = GetNode<NameBar>("NameBar");
		//Impostazione Bars
		nameBar.SetNameBar(Cname);
		lifeBar.ChangeMaxValue(Life); //al momento la vita non avendo un valore esplicito è zero
		manaBar.Set_StartManaBar(manaStart, maxMana, manaVelocity);
		//ALL Menu
		battleMenu = GetNode<OptionMenu>("BattleMenu");
		skillBattleMenu = battleMenu.GetNode<OptionMenu>("SkillBattleMenu");
		allyManagerMenu = battleMenu.GetNode<OptionMenu>("AllyManagerMenu");
		inventoryBattleMenu = battleMenu.GetNode<OptionMenu>("InventoryBattleMenu");
		allyOptionMenu = allyManagerMenu.GetNode<OptionMenu>("AllyOptionMenu");
		//TTBCSRIPT
		tTBCScript = GetParent().GetParent<TTBCScript>();
		//SKILL MANAGER
		skillManager.CreateSkillManager();
		/*HD*/skillManager.CreateEquippedSkill(new String[2] {"BasicAttack1","BasicAttack2"});
		inventoryManager.CreateInventoryManager();
		/*HD*/inventoryManager.CreateEquippedItem(new String[1] {"BasicObject1"});
		//PROPENSIONE MAGICA
		magicProp = new MagicProp(magicType, magicPower);
		AllBattleMenu_CreateSignals();//crea i segnali dei vari pulsanti 
	}

	//FUNZIONI INTERFACCIA BASEMOVES
	public void TakeDamage(int damage){
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
	public void SelectMove(){//mostra il menu di selezione delle mosse
		battleMenu.ShowUp();
	}
	public void EndSelectMove(){ //da inserire quando si scelie una mossa, per terminare la scelta
		//si libera la SelectMoveQueue
		skillBattleMenu.Hide();
		allyManagerMenu.Hide();
		allyOptionMenu.Hide();
		inventoryBattleMenu.Hide();
		battleMenu.Hide();
		tTBCScript.UpdateMoveQueue(tTBCScript.SelectMoveQueue);
	}
	//FUNZIONI CREZIONE BOTTONI
	public void AllBattleMenu_CreateSignals(){//crezione dei segnali dei vari pulsanti
		battleMenu.GetButton(0).Pressed += AttackPressed;
		battleMenu.GetButton(1).Pressed += AllyManagerPressed;
		battleMenu.GetButton(2).Pressed += InventoryPressed;
		battleMenu.GetButton(3).Pressed += EscapePressed;
		//ovveride dei bottoni per riempierli con le mosse e gli oggetti equipaggiati
		skillBattleMenu.OverrideButton(skillManager.EquippedSkill);
		skillBattleMenu.SetPressed(skillManager.EquippedSkill, true, SkillBattleMenu_ButtonFocused, SkillBattleMenu_ButtonPressed);

		inventoryBattleMenu.OverrideButton(inventoryManager.EquippedItem);
		inventoryBattleMenu.SetPressed(inventoryManager.EquippedItem, true, InventoryBattleMenu_ButtonFocused, InventoryBattleMenu_ButtonPressed);
		
		/*HD*/allyManager.EquipAlly("Ally1", 0); //funzione da spostare nel menu della gestione alleati
		/*HD*/allyManager.EquipAlly("Ally2", 1); //funzione da spostare nel menu della gestione alleati
		/*HD*/allyManager.EquipAlly("Ally3", 2); //funzione da spostare nel menu della gestione alleati
		/*HD*/allyManager.EquipAlly("", 3); //funzione da spostare nel menu della gestione alleati
		allyManagerMenu.OverrideButton(allyManager.EquippedAlly);
		allyManagerMenu.SetPressed(allyManager.EquippedAlly, true, AllyManagerMenu_ButtonFocused, AllyManagerMenu_ButtonPressed);
		//aggiungere gli altri menu
		//SISTEMA FOCUS PRECEDENTI per tornare al menu precedente col tasto z (impostato su engine come focusprevious)
		battleMenu.SetFocusPrevioustTo(battleMenu); 
		skillBattleMenu.SetFocusPrevioustTo(battleMenu);
		allyManagerMenu.SetFocusPrevioustTo(battleMenu);
		allyOptionMenu.SetFocusPrevioustTo(allyManagerMenu);
		inventoryBattleMenu.SetFocusPrevioustTo(battleMenu);
	}
	//FUNZIONI PRESSED DEI MENU
	//servono per nascondere gli altri menù quando uno di essi viene mostrato
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
	//per ora non fa nulla, serve per scappare dalla battaglia
	public void EscapePressed(){

	}
	//funzioni per i bottoni dei menu FOCUSED e PRESSED
	//La funzione focused serve per identificare il bottone che si sta puntando
	//la funzione pressed serve per identificare il bottone che si sta premendo (l'ultimo bottone puntato)
	public void SkillBattleMenu_ButtonFocused(int id){
		skillBattleMenu.Id_ButtonFocused = id;
	}
	public void SkillBattleMenu_ButtonPressed(){
		choosedMove = "Skill";
		selectedAction = skillManager.EquippedSkill[skillBattleMenu.Id_ButtonFocused];
		skillManager.PrepareAction(selectedAction, this);
	}
	public void InventoryBattleMenu_ButtonFocused(int id){
		inventoryBattleMenu.Id_ButtonFocused = id;
	}
	public 	void InventoryBattleMenu_ButtonPressed(){
		choosedMove = "Inventory";
		selectedAction = inventoryManager.EquippedItem[inventoryBattleMenu.Id_ButtonFocused];
		inventoryManager.PrepareAction(selectedAction, this);
	}
	public void AllyManagerMenu_ButtonFocused(int id){
		allyManagerMenu.Id_ButtonFocused = id;
	}
	public void AllyManagerMenu_ButtonPressed(){
		choosedMove = "AllyManager";
		selectedAction = allyManager.EquippedAlly[allyManagerMenu.Id_ButtonFocused];
		allyManager.SelectAlly(selectedAction, this, tTBCScript.DiedAllyList, allyManagerMenu.Id_ButtonFocused) ;
	}
	//SELEZIONARE UN NEMICO + relativi segnali
	public void SelectEnemy(){ //popupa il menu di scelta nemici e quando il segnale è inviato dal popmn il nemico viene selezionato
		//funzioni per decidere il nemico
		tTBCScript.CreateEnemyListOptionMenu(false, null);
		//il nemico si seleziona nel TTBCScript
	}
	public void DoAction(){ //Data il nome della mossa memorizzata, prende la funzione dal dictionary ed esegue l'azione
		//vai in stato notFree dall'animazione, nell'animationPlayer
		//esecuzione mossa
		switch (choosedMove){
			case "Skill":
				skillManager.DoAction(selectedAction, this);
				break;
			case "Inventory":
				inventoryManager.DoAction(selectedAction, this);
				break;
			case "AllyManager":
				allyManager.SummonAlly();
				break;
			default:
				break;
		}	
		//pulizia variabili
		CleanSelectedMove();
	}
	public void CleanSelectedMove(){//da far eseguire ogni volta che si termina la mossa per evitare che si conservi la scelta
		choosedMove = null;
		selectedAction = null;
		selectedEnemy = null;
		selectedAlly = null;
	}
	public Boolean IsTargetFree(){ //controlla se il target è libero, serve per capire se si può essere bersaglio o bersagliere
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
	public String SelectedAction{
		get => selectedAction;
		set => selectedAction = value;
	}
	public OptionMenu AllyManagerMenu{
		get => allyManagerMenu;
		set => allyManagerMenu = value;
	}
	public AllyInfoManager GetAllyInfoManager(String allyName){
		//prende tramite nome l'AllyInfoManager solo quelli presenti in campo
		if (tTBCScript.AllyList.ContainsKey(allyName)){
			return tTBCScript.AllyList[allyName];
		} else { //se non è presente restituisce null
			return null;
		}
	}
	public OptionMenu AllyOptionMenu{
		get => allyOptionMenu;
		set => allyOptionMenu = value;
	}
	public TTBCScript TTBCScript{
		get => tTBCScript;
		set => tTBCScript = value;
	}
	public ManaBar ManaBar{
		get => manaBar;
		set => manaBar = value;
	}
	public MagicProp MagicProp{
		get => magicProp;
		set => magicProp = value;
	}
}


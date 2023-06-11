using Godot;
using System;

public partial class AllyInfoManager : Node2D , BaseMoves
{
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
	private Boolean inBattle = false;
	private Timer timer;
	private AnimationNodeStateMachinePlayback animationState;
	private GameBar lifeBar;
	private ManaBar manaBar;
	private NameBar nameBar;
	private TTBCScript tTBCScript;
	MagicProp magicProp = ResourceLoader.Load<MagicProp>("res://Combat System/MagicProp.tres") as MagicProp;
	AllyManager allyManager = ResourceLoader.Load<AllyManager>("res://Characters/Ally/AllyManager.tres") as AllyManager;
	private EnemyInfoManager selectedEnemy;
	private AllyInfoManager selectedAlly;
	private String selectedAction;
	private OptionMenu battleMenu;

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
		manaBar.Set_StartManaBar(manaStart, maxMana, manaVelocity); //al momento il mana non avendo un valore esplicito è zero
		//TTBCSRIPT
		tTBCScript = GetParent().GetParent<TTBCScript>();
		//PROPENSIONE MAGICA
		magicProp = new MagicProp(magicType, magicPower);
		//BATTLE MENU
		battleMenu = GetNode<OptionMenu>("BattleMenu");
		AllBattleMenu_CreateSignals();
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
	public void _on_BattleTimer_timeout(){ //METTE IN CODA DI ATTESA DEL TIMER
		tTBCScript.WaitingQueue.Enqueue(GetParent<Marker2D>()); //viene messo in coda alla SelectMove
		tTBCScript.UpdateSelectMoveQueue(); //aggiorna la selectmove, nel caso essa sia vuota cosi da poter scegliere la mossa
	}
	public void AnimateCharacter(String animation){
		animationState.Travel(animation);
	}
	public void BackToIdle(){
		AnimateCharacter("Idle");
	}
	public void SelectMove(){
		battleMenu.ShowUp();
	}
	public void EndSelectMove(){
		battleMenu.Hide();
		tTBCScript.UpdateMoveQueue(tTBCScript.SelectMoveQueue);
	}
	public void DoAction(){
		//Data il nome della mossa memorizzata, prende la funzione dal dictionary ed esegue l'azione
		//vai in stato notFree dall'animazione
		//esecuzione mossa
		allyManager.DoAction(selectedAction, this);
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
	//FUNZIONI PER IL BATTLE MENU
	public void AllBattleMenu_CreateSignals(){
		var skills = allyManager.AllySkillManagerDictionary[cname];
		battleMenu.OverrideButton(skills);
		battleMenu.SetPressed(skills, true, BattleMenu_ButtonFocused, BattleMenu_ButtonPressed);
		//FOCUS
		battleMenu.SetFocusPrevioustTo(battleMenu);
	}
	public void BattleMenu_ButtonFocused(int id){
		battleMenu.Id_ButtonFocused = id;
	}
	public void BattleMenu_ButtonPressed(){
		selectedAction = allyManager.AllySkillManagerDictionary[cname][battleMenu.Id_ButtonFocused];
		allyManager.PrepareAction(selectedAction, this);
	}
	public void SelectEnemy(){ //popupa il menu di scelta nemici e quando il segnale è inviato dal popmn il nemico viene selezionato
		//funzioni per decidere il nemico
		tTBCScript.CreateEnemyListOptionMenu(true, this);
		//il nemico si seleziona nel TTBCScript
	}
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
	public Boolean FreeForFight{
		get => free;
		set => free = value;
	}
	public EnemyInfoManager SelectedEnemy{
		get => selectedEnemy;
		set => selectedEnemy = value;
	}
	public Boolean InBattle{
		get => inBattle;
		set => inBattle = value;
	}
	public OptionMenu BattleMenu{
		get => battleMenu;
		set => battleMenu = value;
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


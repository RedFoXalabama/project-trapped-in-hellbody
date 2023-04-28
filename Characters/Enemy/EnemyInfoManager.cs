using Godot;
using System;

public partial class EnemyInfoManager : Node2D, BaseMoves
{
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
	private NameBar nameBar;
	private TTBCScript tTBCScript;
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
		nameBar = GetNode<NameBar>("NameBar");
		//Impostazione Bars
		nameBar.SetNameBar(Cname);
		lifeBar.ChangeMaxValue(Life); //al momento la vita non avendo un valore esplicito è zero
		//TTBCSRIPT
		tTBCScript = GetParent().GetParent<TTBCScript>();
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
		tTBCScript.EnemyWaitingQueue.Enqueue(GetParent<Marker2D>());//viene messo in coda alla EnemySelectMove
		tTBCScript.UpdateEnemySelectMoveQueue(); //aggiorna la Enemyselectmove, nel caso essa sia vuota cosi da poter scegliere la mossa
	}
	public void AnimateCharacter(String animation){
		animationState.Travel(animation);
	}
	public void BackToIdle(){
		AnimateCharacter("Idle");
	}
	public void SelectMove(){}
	public void DoAction(){

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
}

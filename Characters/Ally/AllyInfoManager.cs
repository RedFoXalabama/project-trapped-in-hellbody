using Godot;
using System;

public class AllyInfoManager : Node2D , BaseMoves
{
    [Export] private String cname;
    [Export] private int life;
    [Export] private int mana;
    [Export] private int attack;
    [Export] private int defense;
    [Export] private int velocity;
    private Timer timer;
    private Boolean free = true;
    private AnimationNodeStateMachinePlayback animationState;
    private GameBar lifeBar;
    private GameBar manaBar;
    private NameBar nameBar;
    private TTBCScript tTBCScript;


    public override void _Ready(){
        //Timer
        timer = GetNode<Timer>("BattleTimer");
        //Animazioni
        animationState = (AnimationNodeStateMachinePlayback)GetNode<Sprite>("BattleAnimation").GetNode<AnimationTree>("AnimationTree").Get("parameters/playback");
        GetNode<Sprite>("BattleAnimation").GetNode<AnimationTree>("AnimationTree").Active = true;
        //All Bars
        lifeBar = GetNode<GameBar>("LifeBar");
        manaBar = GetNode<GameBar>("ManaBar");
        nameBar = GetNode<NameBar>("NameBar");
        //Impostazione Bars
        nameBar.SetNameBar(Cname);
        lifeBar.ChangeMaxValue(Life); //al momento la vita non avendo un valore esplicito è zero
        manaBar.ChangeMaxValue(Mana); //al momento il mana non avendo un valore esplicito è zero
        //TTBCSRIPT
        tTBCScript = GetParent().GetParent<TTBCScript>();
    }

    //FUNZIONI INTERFACCIA BASEMOVES
    public void GetDamage(int damage){
        Life -= damage;
        lifeBar.ChangeValue(Life);
        AnimateCharacter("Damage");
    }
    public void StartTimer(){
        timer.Start();
    }
    public void _on_BattleTimer_timeout(){ //METTE IN CODA DI ATTESA DEL TIMER
        tTBCScript.WaitingQueue.Enqueue(GetParent<Position2D>()); //viene messo in coda alla SelectMove
        tTBCScript.UpdateSelectMoveQueue(); //aggiorna la selectmove, nel caso essa sia vuota cosi da poter scegliere la mossa
    }
    public void AnimateCharacter(String animation){
        animationState.Travel(animation);
    }
    public void SelectMove(){}
    public void ChangeStatusFree(Boolean free){
        this.free = free;
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
}


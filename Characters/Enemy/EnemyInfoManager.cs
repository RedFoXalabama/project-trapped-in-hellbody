using Godot;
using System;

public class EnemyInfoManager : Node2D
{
    [Export] private String cname;
    [Export] private int life;
    [Export] private int mana;
    [Export] private int attack;
    [Export] private int defense;
    [Export] private int velocity;
    [Export] private Timer timer;
    private AnimationNodeStateMachinePlayback animationState;
    private GameBar lifeBar;
    private GameBar manaBar;
    //private LineEdit nameBar;


    public override void _Ready(){
        //Timer
        timer = GetNode<Timer>("BattleTimer");
        //Animazioni
        animationState = (AnimationNodeStateMachinePlayback)GetNode<Sprite>("BattleAnimation").GetNode<AnimationTree>("AnimationTree").Get("parameters/playback");
        GetNode<Sprite>("BattleAnimation").GetNode<AnimationTree>("AnimationTree").Active = true;
        //All Bars
        lifeBar = GetNode<GameBar>("LifeBar");
        //nameBar = GetNode<LineEdit>("NameBar");
    }

    //FUNZIONI INTERFACCIA BASEMOVES
    public void GetDamage(int damage){
        Life -= damage;
        lifeBar.ChangeValue(Life);
        AnimateCharacter("Damage");
    }
    public void StartTimer(Timer timer){
        timer.Start();
    }
    public void AnimateCharacter(String animation){
        animationState.Travel(animation);
    }
    public void SelectMove(){}




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

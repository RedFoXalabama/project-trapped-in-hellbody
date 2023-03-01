using Godot;
using System;

public class PlayerInfoManager : Node2D , BaseMoves
{
    //PROPRIETIES
    [Export] private String cname;
    [Export] private int life;
    [Export] private int mana;
    [Export] private int attack;
    [Export] private int defense;
    [Export] private int velocity;
    private Boolean free = true;
    private Timer timer;
    private AnimationNodeStateMachinePlayback animationState;
    private GameBar lifeBar;
    private GameBar manaBar;
    private NameBar nameBar;
    private PopupMenu battleMenu;
    private PopupMenu skillBattleMenu;
    private PopupMenu allyManagerMenu;
    private PopupMenu inventoryBattleMenu;
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
        //ALL Menu
        battleMenu = GetNode<PopupMenu>("BattleMenu");
        skillBattleMenu = battleMenu.GetNode<PopupMenu>("SkillBattleMenu");
        allyManagerMenu = battleMenu.GetNode<PopupMenu>("AllyManagerMenu");
        inventoryBattleMenu = battleMenu.GetNode<PopupMenu>("InventoryBattleMenu");
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
    public void AnimateCharacter(String animation){
        animationState.Travel(animation);
    }
    public void _on_BattleTimer_timeout(){ //METTE IN CODA DI ATTESA DEL TIMER
        tTBCScript.WaitingQueue.Enqueue(GetParent<Position2D>()); //viene messo in coda alla SelectMove
        tTBCScript.UpdateSelectMoveQueue(); //aggiorna la selectmove, nel caso essa sia vuota cosi da poter scegliere la mossa
    }
    public void SelectMove(){
        battleMenu.Popup_();


        //fine 
    }
    public void EndSelectMove(){ //da inserire quando si scelie una mossa
        //si libera la SelectMoveQueue
        tTBCScript.UpdateMoveQueue(tTBCScript.SelectMoveQueue);
    }
    //test di prova
    public void _on_BattleMenu_id_pressed(int id){ //INPUT ACCETTATI: space bar o Invio
        //selezione mossa con lo switch
        switch(id){
            case 0:
                SkillBattleMenuPopup();
                break;
            case 1:
                AllyManagerMenuPopup();
                break;
            case 2:
                InventoryBattleMenuPopup();
                break;
            case 3:
                //FUNZIONI ESCAPE
                break;
        }
        battleMenu.Hide();
        EndSelectMove();
    }

    public void SkillBattleMenuPopup(){
        skillBattleMenu.Popup_();
    }
    public void AllyManagerMenuPopup(){
        allyManagerMenu.Popup_();
    }
    public void InventoryBattleMenuPopup(){
        inventoryBattleMenu.Popup_();
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

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
    private PopupMenu battleMenu;
    private PopupMenu skillBattleMenu;
    private PopupMenu allyManagerMenu;
    private PopupMenu inventoryBattleMenu;
    private TTBCScript tTBCScript;
    //VARIABILI PER LA SCELTA MOSSA
    private EnemyInfoManager selectedEnemy;
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
        battleMenu = GetNode<PopupMenu>("BattleMenu");
        skillBattleMenu = battleMenu.GetNode<PopupMenu>("SkillBattleMenu");
        allyManagerMenu = battleMenu.GetNode<PopupMenu>("AllyManagerMenu");
        inventoryBattleMenu = battleMenu.GetNode<PopupMenu>("InventoryBattleMenu");
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
        battleMenu.Show();
    }
    public void EndSelectMove(){ //da inserire quando si scelie una mossa
        //si libera la SelectMoveQueue
        tTBCScript.UpdateMoveQueue(tTBCScript.SelectMoveQueue);
    }
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
    }
    public void SkillBattleMenuPopup(){
        skillBattleMenu.Popup();
    }
    public void _on_SkillBattleMenu_id_pressed(int id){
        switch(id){ //Qui ci sarà tutto l'elenco delle mosse base equipaggiate e possibili da fare
            case 0: //ATTACCO 1
                BasicAttack1();
                break;
            case 1: //ATTACCO 2
                break;
        }
    }
    //SELEZIONARE UN NEMICO + relativi segnali
    public void SelectEnemy(){ //popupa il menu di scelta nemici e quando il segnale è inviato dal popmn il nemico viene selezionato
        //funzioni per decidere il nemico
        tTBCScript.EnemyListMenu.Position = (skillBattleMenu.Position);
        tTBCScript.EnemyListMenu.Popup();
    }
    //SEGNALE DEL POPUPMENU SELEZIONE NEMICI
    public void _on_EnemyListMenu_id_pressed(int id){
        switch(id){ //si esce dalla select move quando si sceglie il nemico
            case 0:
                selectedEnemy = tTBCScript.EnemyList[0];
                EndSelectMove(); //esci dalla select move
                break;
            case 1:
                selectedEnemy = tTBCScript.EnemyList[1];
                EndSelectMove(); //esci dalla select move
                break;
            case 2:
                selectedEnemy = tTBCScript.EnemyList[2];
                EndSelectMove(); //esci dalla select move
                break;    
        }        
    }
    public void _on_EnemyListMenu_id_focused(int id){
        tTBCScript.EnemyListMenu.GetNode<Sprite2D>("Pointer").GlobalPosition = tTBCScript.EnemiesPosition[id].Position; 
        tTBCScript.EnemyListMenu.GetNode<Sprite2D>("Pointer").Visible = true;
    }
    public void BasicAttack1(){//attacco base 1 di prova
        selectedAction = "BasicAttack1";
        SelectEnemy();
        //si esce dalla select move quando si sceglie il nemico
    }
    public void DoAction(){ //Data il nome della mossa memorizzata, prende la funzione dal dictionary ed esegue l'azione
        //vai in stato notFree dall'animazione
        //esecuzione mossa
        switch (selectedAction){
            case "BasicAttack1":
                AnimateCharacter("Attack");
                //funzione da creare per l'attacco base
                break;
            case "BasicAttack2":
                break;
        }
        //pulizia variabili
        CleanSelectedMove();
    }
    public void CleanSelectedMove(){//da far eseguire ogni volta che si termina la mossa per evitare che si conservi la scelta
        selectedAction = null;
        selectedEnemy = null;
    }

    //GESTIONE ALLEATI
    public void AllyManagerMenuPopup(){
        allyManagerMenu.Popup();
    }

    //GESTIONE INVENTARIO
    public void InventoryBattleMenuPopup(){
        inventoryBattleMenu.Popup();
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

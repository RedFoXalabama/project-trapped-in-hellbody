using Godot;
using System;
using System.Collections.Generic;

public partial class TTBCScript : Node
{
	//PLAYER VARIABLES
	private Marker2D playerPosition;
	private PlayerInfoManager playerInfoManager;
	private Boolean wasPlayerTurn = true;

	//ALLY VARIABLES
	private Marker2D ally1Position;
	private Marker2D ally2Position;
	private AllyInfoManager ally1;
	private AllyInfoManager ally2;
	private AllyInfoManager attackingAlly;
	AllyManager allyManager = ResourceLoader.Load("res://Characters/Ally/AllyManager.tres") as AllyManager;
	private PackedScene[] allyPS = new PackedScene[4];
	private Dictionary<String,AllyInfoManager> allyList = new Dictionary<string, AllyInfoManager>();
	private Dictionary<String,AllyInfoManager> diedAllyList = new Dictionary<string, AllyInfoManager>();
	
	//ENEMY VARIABLES
	private Marker2D enemy1Position;
	private Marker2D enemy2Position;
	private Marker2D enemy3Position;
	private Marker2D[] enemiesPosition;
	private EnemyInfoManager enemy1;
	private EnemyInfoManager enemy2;
	private EnemyInfoManager enemy3;
	private EnemyInfoManager[] enemyList; //lista degli enemy vivi
	private OptionMenu enemyListOption; //menu per la scelta tra gli enemy vivi da attaccare
	EnemyManager enemyManager = ResourceLoader.Load("res://Characters/Enemy/EnemyManager.tres") as EnemyManager;
	private PackedScene[] enemyPS /*HD*/= new PackedScene[3];// in modo tale da non dare errore

	//TTBC VARIABLES
	private Queue<Marker2D> waitingQueue = new Queue<Marker2D>(); //per il giocatore CODA TIMER
	private Queue<Marker2D> enemyWaitingQueue = new Queue<Marker2D>(); //per gli enemy CODA TIMER
	private Queue<Marker2D> selectMoveQueue = new Queue<Marker2D>(); //Per il giocatore CODA SCEGLI MOSSA
	private Queue<Marker2D> enemySelectMoveQueue = new Queue<Marker2D>(); //Per gli enemy CODA SCEGLI MOSSA
	private Queue<Marker2D> moveQueue = new Queue<Marker2D>(); //Per tutti CODA ESEGUI MOSSA
	private Boolean battleStated = false;

	public override void _Ready(){
		//PLAYER STARTING
		playerPosition = GetNode<Marker2D>("PlayerPosition");
		playerInfoManager = playerPosition.GetNode<PlayerInfoManager>("PlayerInfoManager");
		//ALLY STARTING (creazione del database degli alleati, del packedScene[] e del database delle skill)
		/*HD*/allyManager.CreateDataBase(); //funzione da inserire fuori dal combattimento cosi da non essere creato ogni volta	
		createAllyPS();
		allyManager.CreateAllySkillManager(); //funzione da inserire fuori dal combattimento cosi da non essere creato ogni volta
		///*hd*/SpawnAlly(allyPS[0]);
		//ENEMY STARTING
		/*HD*/enemyManager.CreateDatabase(); //funzione da inserire fuori dal combattimento cosi da non essere creato ogni volta	
		//enemyPS = PackedScene[] caricato dal nemico, io lo hardcodo per farlo funzionare
		//una volta che si avrà il packedScene[] già creato, non ci sarà bisogno di crearlo qui
		/*HD*/enemyPS[0] = GD.Load<PackedScene>(enemyManager.EnemyPath("Demo_Enemy1"));
		/*HD*/enemyPS[1] = GD.Load<PackedScene>(enemyManager.EnemyPath("Demo_Enemy2"));
		/*HD*/enemyPS[2] = GD.Load<PackedScene>(enemyManager.EnemyPath("Demo_Enemy3"));
		spawnEnemy(enemyPS);
		enemyListOption = GetNode<OptionMenu>("EnemyListMenu/EnemyListOption");

		//BATTLE START BUTTON
		GetNode<BaseButton>("PlayButton").GrabFocus();
		
	}
	public override void _Process(double delta){
		CheckBattleEnd(); //controlla se la battaglia è finita
		FightUpdate(); //aggiorna la battaglia
	}
	//INIZIO BATTAGLIA
	public void _on_play_button_pressed(){ //fa partire la battaglia
		GetNode<BaseButton>("PlayButton").Hide();
		BattleStart();
	}
	//ALLY FUNCTIONS
	public void createAllyPS(){ //funzione che crea il packedScene[] degli alleati equipaggiati
		///*HD*/allyManager.EquipAlly("Demo_Ally", 0); //funzione da spostare nel menu della gestione alleati
		///*HD*/allyManager.EquipAlly("", 1); //funzione da spostare nel menu della gestione alleati
		for(int i = 0; i < allyManager.EquippedAlly.Length; i++){
			if (allyManager.EqAllyPath(i) != null && !(allyManager.EqAllyPath(i).Equals(""))){
				allyPS[i] = GD.Load<PackedScene>(allyManager.EqAllyPath(i));
			}	
		}
	}
	public void SpawnAlly(PackedScene ally){ //funzione che spawna un alleato
		ally1Position = GetNode<Marker2D>("Ally1Position");
		ally2Position = GetNode<Marker2D>("Ally2Position");
		if (ally1Position.GetChildCount() > 0 == false){ //se la prima posizione è vuota riempi
			ally1Position.AddChild(ally.Instantiate());
			ally1 = ally1Position.GetChild<AllyInfoManager>(0);
			allyList.Add(ally1.Cname, ally1); //aggiungo alla lista degli alleati vivi in campo
			ally1.StartTimer(); //avviamo il timer per metterlo in coda
			ally1.ManaBar.StartManaBar(); //start del mana bar
		} else if (ally2Position.GetChildCount() > 0 == false){ // se la seconda posizione è vuota riempi
			ally2Position.AddChild(ally.Instantiate()); //genera un fracasso di errori
			ally2 = ally2Position.GetChild<AllyInfoManager>(0);
			allyList.Add(ally2.Cname, ally2); //aggiungo alla lista degli alleati vivi in campo
			ally2.StartTimer();//avviamo il timer per metterlo in coda
			ally2.ManaBar.StartManaBar(); //start del mana bar
		}
		//non fa nulla se entrambi gli slot alleati sono pieni
	}
	//ENEMY FUNCTIONS
	public void spawnEnemy(PackedScene[] enemy){ //funzione che spawna i nemici dato un packedscene
		enemy1Position = GetNode<Marker2D>("Enemy1Position");
		enemy2Position = GetNode<Marker2D>("Enemy2Position");
		enemy3Position = GetNode<Marker2D>("Enemy3Position");
		EnemiesPosition = new Marker2D[] {enemy1Position, enemy2Position, enemy3Position};
		for (int i = 0; i < enemy.Length; i++){
			enemiesPosition[i].AddChild(enemy[i].Instantiate());
		}
	}
	//FUNZIONI TTBC
	public void BattleStart(){ //funzioni per inizio battaglia
		//CONTROLLO VELOCITÀ E SELEZIONE DI CHI INIZIA
		var playerVelocity = playerInfoManager.Velocity;
		EnemyList = new EnemyInfoManager[enemyPS.Length];
		//controllo chi inizia prendendo le velocità dei nemici in campo
		var enemy1Velocity = 0;
		var enemy2Velocity = 0;
		var enemy3Velocity = 0;
		if (enemy1Position.GetChildCount() == 1){ //se c'è un nemico nello slot 1
			enemy1 = enemy1Position.GetChild<EnemyInfoManager>(0);
			EnemyList[0] = enemy1;
			enemy1Velocity = enemy1.Velocity; //prendo la velocità del nemico
		}
		if (enemy2Position.GetChildCount() == 1){ //se c'è un nemico nello slot 2
			enemy2 = enemy2Position.GetChild<EnemyInfoManager>(0);
			EnemyList[1] = enemy2;
			enemy2Velocity = enemy2.Velocity; //prendo la velocità del nemico
		}
		if (enemy3Position.GetChildCount() == 1){ //se c'è un nemico nello slot 3
			enemy3 = enemy3Position.GetChild<EnemyInfoManager>(0);
			EnemyList[2] = enemy3;
			enemy3Velocity = enemy3.Velocity; //prendo la velocità del nemico
		}
		
		//Creo per la prima volta la lista dei nemici
		String[] enemyListName = new String[EnemyList.Length];
		for (int i = 0; i < EnemyList.Length; i ++){
			enemyListName[i] = EnemyList[i].Cname;
		}
		//imposto il menu per la selezione del nemico da attaccare
		var nButtonNew = enemyListOption.OverrideButton(enemyListName);
		enemyListOption.SetFocusPrevioustTo(playerInfoManager.SkillBattleMenu);
		EnemyListOption.SetPressed(enemyListName, true, EnemyListOption_ButtonFocused, EnemyListOption_ButtonPressed);
		
		//controllo velocità per decidere chi inizia
		if (playerVelocity > ((enemy1Velocity + enemy2Velocity + enemy3Velocity)/3)-10){ //formula calcolo velocità complessiva da aggiustare
			//il player è più veloce
			SelectMoveQueue.Enqueue(playerPosition); //messo in coda il player
			playerInfoManager.SelectMove();//subito il player scelgie la mossa
			///*HD*/ally1.StartTimer();
			enemy1.StartTimer();
			enemy2.StartTimer();
			enemy3.StartTimer();
		} else {
			//i nemici sono più veloci, il nemico più veloce inizia, gli altri fanno paritre il timer
			playerInfoManager.StartTimer();
			var enemyVelocityMax = enemy1; 
			for (int i = 0; i < EnemyList.Length; i++){ //cerchiamo il nemico più veloce
				if(EnemyList[i].Velocity > enemyVelocityMax.Velocity){
					enemyVelocityMax = EnemyList[i];
				} else {
					EnemyList[i].StartTimer();
				}
			}
			enemyWaitingQueue.Enqueue(enemyVelocityMax.GetParent<Marker2D>());
		}
		//START MANA BARS
		//player
		playerInfoManager.ManaBar.StartManaBar();
		//enemies
		for (int i = 0; i < enemyList.Length; i++){
			enemyList[i].ManaBar.StartManaBar();
		}
		battleStated = true;
	}

	//FUNZIONI PER LA GESTIONE DELLE CODE
	public void UpdateSelectMoveQueue(){ //utilizzata dall'infoManager quando finisce di selezionare la mossa e si sposta nella MoveQueue e la SelectMove si libera
		//PASSAGGIO DA WAITINGQUEUE -> SELECTMOVEQUEUE player
		if(SelectMoveQueue.Count == 0 && WaitingQueue.Count != 0){ //il personaggio viene spostato e gli si chiede la mossa
			SelectMoveQueue.Enqueue(WaitingQueue.Dequeue()); //aggiunge il primo elemento della waiting alla select e lo rimuove dalla waiting
			//SCELTA MOSSA
			if(SelectMoveQueue.Peek().Name.Equals("PlayerPosition")){ //controllo se il nodo posizione è del player altrimenti è l'alleto
				playerInfoManager.SelectMove(); //sappiamo che è il giocatore
			} else {
				SelectMoveQueue.Peek().GetChild<AllyInfoManager>(0).SelectMove(); //non sappiamo quale alleato sia
			}		
		}
	}
	public void UpdateEnemySelectMoveQueue(){ //sposta il primo nemico della waiting alla select e lo rimuove dalla waiting
		//PASSAGGIO DA WAITING -> SELECT enemy
		if(EnemySelectMoveQueue.Count == 0 && EnemyWaitingQueue.Count != 0){
			EnemySelectMoveQueue.Enqueue(EnemyWaitingQueue.Dequeue());
			//SCELTA MOSSA (da programmare ancora)
			EnemySelectMoveQueue.Peek().GetChild<EnemyInfoManager>(0).SelectMove();
		}
	}
	public void UpdateMoveQueue(Queue<Marker2D> smq){ //data una selectedqueue sposta il primo nella movequeue
		//PASSAGGIO DA SELECT -> MOVEQUEUE 		UNIVERSALE
		MoveQueue.Enqueue(smq.Dequeue());//si libera la selectMove e la si fa aggiuornare
		if (smq.Equals(SelectMoveQueue)){
			UpdateSelectMoveQueue();
		} else if(smq.Equals(EnemySelectMoveQueue)){
			UpdateEnemySelectMoveQueue();
		}
	}

	public void FightUpdate(){ //funzione che permette di eseguire la mossa del primo personaggio della movequeue
		//la mosssa viene eseguita solo se il personaggio sul picco è libero e il target è libero
		//PASSAGGIO MOVEQUEUE -> ESECUZIONE MOSSA
		if(MoveQueue.Count != 0){
			var peek = MoveQueue.Peek();
			switch (peek.GetChild(0).Name){ //controlla il nome del nodo, DELLE CLASSE, del figlio per capire se è un player o un nemico o un alleato
				case "PlayerInfoManager":
					if(peek.GetChild<PlayerInfoManager>(0).FreeForFight && peek.GetChild<PlayerInfoManager>(0).IsTargetFree()){
						peek.GetChild<PlayerInfoManager>(0).DoAction();
						MoveQueue.Dequeue();
					}
					break;
				case "AllyInfoManager":
					if(peek.GetChild<AllyInfoManager>(0).FreeForFight && peek.GetChild<AllyInfoManager>(0).IsTargetFree()){
						peek.GetChild<AllyInfoManager>(0).DoAction();
						MoveQueue.Dequeue();
					}
					break;
				case "EnemyInfoManager":
					if(peek.GetChild<EnemyInfoManager>(0).FreeForFight && peek.GetChild<EnemyInfoManager>(0).IsTargetFree()){
						peek.GetChild<EnemyInfoManager>(0).DoAction();
						MoveQueue.Dequeue();
					}
					break;
			}
			CheckEnemyList();//controllerà i nemici ad fine di ogni esecuzione mossa, se si muore per altre cause ex veleno non si viene considerati
			CheckAllyList();//controllerà gli alleati e nel caso li sposterà nella diedAllyList
		}
		//CheckEnemyList() ogni frame controlli se un nemico è morto
	}
	//CONTROLLO AVANZAMENTO BATTAGLIA
	public void CheckBattleEnd(){ //Controllo se la battaglia è finita
		if (battleStated){
			if (EnemyList.Length == 0){ //se non ci sono più nemici
			//VITTORIA
			GD.Print("VITTORIA");
			} else if (playerInfoManager.Life <= 0){ //se il player è morto
			//SCONFITTA
			GD.Print("SCONFITTA");
			}
		}
	}

	//CREAZIONE MENU NEMICI + AGGIORNAMENTO POINTER
	public void CreateEnemyListOptionMenu(Boolean ally, AllyInfoManager aim){
		//funzione che crea il menu per la scelta dei nemici da attaccare, sia per il player che per l'alleato
		//CREA IL MENU DA CUI SELEZIONARE I NEMICI
		String[] enemyListName = new String[EnemyList.Length]; //creo un array di stringhe con i nomi dei nemici
		for (int i = 0; i < EnemyList.Length; i ++){ //riempio l'array con i nomi dei nemici
			enemyListName[i] = EnemyList[i].Cname;
		}	
		var nButtonNew = enemyListOption.OverrideButton(enemyListName); //aggiorno i button e mi salvo quanti nuovi ne ho creati
		//CONTROLLO SE È IL TURNO DEL PLAYER O DELL'ALLEATO
		//in base al caso aggiorno i bottoni vecchi e nuovi modificando i segnali a cui devono fare riferimento
		//questo perchè i segnali del player sono diversi da quelli dell'alleato
		//serve per evitare che si riassegni nuovamente lo stesso segnale a un button 
		//o per evitare che si chiami il segnale sbagliato
		//se era il turno del giocatore aggiorno wasPlayerTurn = true, altrimenti se era dell'alleato wasPlayerTurn = false
		switch (wasPlayerTurn){
			//È IL TURNO DEL PLAYER ED ERA IL TURNO DEL PLAYER
    		case true when !ally:
				enemyListOption.SetPressed(enemyListName, (enemyListName.Length - nButtonNew), true, EnemyListOption_ButtonFocused, EnemyListOption_ButtonPressed);
        		enemyListOption.SetFocusPrevioustTo(playerInfoManager.SkillBattleMenu);
				break;
			//È IL TURNO DELL'ALLEATO ED ERA IL TURNO DEL PLAYER
			case true when ally:
        		for (int i = (enemyListName.Length - nButtonNew); i < enemyListName.Length; i++){//aggiorna solo i nuovi button
            		enemyListOption.GetButton(i).OfPointer = true;
            		enemyListOption.GetButton(i).OfPointerSignal += EnemyListOption_ButtonFocused;
        		}
        		for (int i = 0; i < enemyListName.Length; i++){
					//non dovrebbe essere necessario disconettere prima se si usa +=
					enemyListOption.GetButton(i).Pressed -= EnemyListOption_ButtonPressed;
					enemyListOption.GetButton(i).Pressed += AllyEnemyListOption_ButtonPressed;
        		}
				enemyListOption.SetFocusPrevioustTo(aim.BattleMenu);
        		wasPlayerTurn = false;
        		attackingAlly = aim;
        		break;
			//È IL TURNO DELL'ALLEATO ED ERA IL TURNO DELL'ALLEATO
			case false when ally:
				enemyListOption.SetPressed(enemyListName, (enemyListName.Length - nButtonNew), true, EnemyListOption_ButtonFocused, AllyEnemyListOption_ButtonPressed);
				enemyListOption.SetFocusPrevioustTo(aim.BattleMenu);
				wasPlayerTurn = false;
        		attackingAlly = aim;
        		break;
			//È IL TURNO DEL PLAYER ED ERA IL TURNO DELL'ALLEATO
   			case false when !ally:
        		for (int i = (enemyListName.Length - nButtonNew); i < enemyListName.Length; i++){//aggiorna solo i nuovi button
            	enemyListOption.GetButton(i).OfPointer = true;
            	enemyListOption.GetButton(i).OfPointerSignal += EnemyListOption_ButtonFocused;
        		}
        		for (int i = 0; i < enemyListName.Length; i++){
					//non dovrebbe essere necessario disconettere prima se si usa +=
					enemyListOption.GetButton(i).Pressed -= AllyEnemyListOption_ButtonPressed;
            		enemyListOption.GetButton(i).Pressed += EnemyListOption_ButtonPressed;
				}
				enemyListOption.SetFocusPrevioustTo(playerInfoManager.SkillBattleMenu);
        		wasPlayerTurn = true;
        	break;
		}
		//mostro il menu
		enemyListOption.GetParent<Node2D>().Show();
		enemyListOption.ShowUp();
	}

	//FUNZIONI PER I SEGNALI DEI BUTTON
	//funzioni focus, servono per aggioranre la posizione del pointer (capire quale button è focussato)
	public void EnemyListOption_ButtonFocused(int id){
		EnemyListOption.GetNode<Sprite2D>("../Pointer").GlobalPosition = EnemiesPosition[id].Position; 
		EnemyListOption.GetNode<Sprite2D>("../Pointer").Show();
		EnemyListOption.Id_ButtonFocused = id;
	}
	public void EnemyListOption_ButtonPressed(){
		playerInfoManager.SelectedEnemy = EnemyList[enemyListOption.Id_ButtonFocused];
		enemyListOption.GetParent<Node2D>().Hide();
		playerInfoManager.EndSelectMove();
	}
	public void AllyEnemyListOption_ButtonPressed(){
		attackingAlly.SelectedEnemy = EnemyList[enemyListOption.Id_ButtonFocused];
		enemyListOption.GetParent<Node2D>().Hide();
		attackingAlly.EndSelectMove();
		attackingAlly = null;
	}

	//FUNZIONI CHECK ENEMYLIST E ALLYLIST
	public void CheckEnemyList(){
		//funzione che serve ad aggiornare la EnemyList cosi da ridurla eliminando gli spazi vuoti (nemici morti)
		//bisogna aggiornare anche l'array dell'enemyPosition cosi da non far sfasare il puntatore
		/*HD*/EnemyInfoManager[] temp = new EnemyInfoManager[3]; //3 Vlore massimo dei nemici
		/*HD*/Marker2D[] tempPosition = new Marker2D[3];
		int j = 0;
		for(int i = 0; i < enemyList.Length; i++){
			if(enemyList[i].Life > 0){
				temp[j] = enemyList[i];
				tempPosition[j] = enemiesPosition[i];
				j++;
			}
		}
		Array.Resize(ref temp, j);
		Array.Resize(ref tempPosition, j);
		enemyList = temp;
		enemiesPosition = tempPosition;
	}
	public void CheckAllyList(){ //aggiorna la lista degli alleati vivi presenti in campo e la lista degli alleati morti
		//serve per capire quali alleati sono morti e quali no, cosi da evitare di rievocarli
		for (int i = 0; i < allyManager.EquippedAlly.Length; i++){
			if(allyList.ContainsKey(allyManager.EquippedAlly[i])){
				if(allyList[allyManager.EquippedAlly[i]].Life <= 0){
					diedAllyList.Add(allyManager.EquippedAlly[i], allyList[allyManager.EquippedAlly[i]]);
					allyList.Remove(allyManager.EquippedAlly[i]);
				}
			}
		}
	}
	//GETTER E SETTER
	public Queue<Marker2D> WaitingQueue{
		get => waitingQueue;
		set => waitingQueue = value;
	}
	public Queue<Marker2D> EnemyWaitingQueue{
		get => enemyWaitingQueue;
		set => enemyWaitingQueue = value;
	}
	public Queue<Marker2D> SelectMoveQueue{
		get => selectMoveQueue;
		set => selectMoveQueue = value;
	}
	public Queue<Marker2D> EnemySelectMoveQueue{
		get => enemySelectMoveQueue;
		set => enemySelectMoveQueue = value;
	}
	public Queue<Marker2D> MoveQueue{
		get => moveQueue;
		set => moveQueue = value;
	}
	public EnemyInfoManager[] EnemyList{
		get => enemyList;
		set => enemyList = value;
	}
	public Marker2D[] EnemiesPosition{
		get => enemiesPosition;
		set => enemiesPosition = value;
	}
	public OptionMenu EnemyListOption{
		get => enemyListOption;
		set => enemyListOption = value;
	}
	public Dictionary<String, AllyInfoManager> AllyList{
		get => allyList;
		set => allyList = value;
	}
	public Dictionary<String, AllyInfoManager> DiedAllyList{
		get => diedAllyList;
		set => diedAllyList = value;
	}
	public PackedScene[] AllyPS{
		get => allyPS;
		set => allyPS = value;
	}
}	

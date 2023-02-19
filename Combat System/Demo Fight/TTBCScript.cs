using Godot;
using System;
using System.Collections.Generic;

public class TTBCScript : Node
{
	//PLAYER VARIABLES
	private Position2D playerPosition;
	private PlayerInfoManager playerInfoManager;

	//ALLY VARIABLES
	private Position2D ally1Position;
	private Position2D ally2Position;
	private AllyInfoManager ally1;
	private AllyInfoManager ally2;
	AllyManager allyManager = ResourceLoader.Load("res://Characters/Ally/AllyManager.tres") as AllyManager;
	private PackedScene[] allyPS = new PackedScene[4];

	//ENEMY VARIABLES
	private Position2D enemy1Position;
	private Position2D enemy2Position;
	private Position2D enemy3Position;
	private EnemyInfoManager enemy1;
	private EnemyInfoManager enemy2;
	private EnemyInfoManager enemy3;
	EnemyManager enemyManager = ResourceLoader.Load("res://Characters/Enemy/EnemyManager.tres") as EnemyManager;
	private PackedScene[] enemyPS = new PackedScene[3];

	//TTBC VARIABLES
	private Queue<Position2D> waitingQueue = new Queue<Position2D>(); //per il giocatore CODA TIMER
	private Queue<Position2D> enemyWaitingQueue = new Queue<Position2D>(); //per gli enemy CODA TIMER
	private Queue<Position2D> selectMoveQueue = new Queue<Position2D>(); //Per il giocatore CODA SCEGLI MOSSA
	private Queue<Position2D> enemySelectMoveQueue = new Queue<Position2D>(); //Per gli enemy CODA SCEGLI MOSSA
	private Queue<Position2D> moveQueue = new Queue<Position2D>(); //Per tutti CODA ESEGUI MOSSA
	private Boolean winned = false;

	public override void _Ready(){
		//PLAYER STARTING
		playerPosition = GetNode<Position2D>("PlayerPosition");
		playerInfoManager = playerPosition.GetNode<PlayerInfoManager>("PlayerInfoManager");
		//ALLY STARTING
		/*HD*/allyManager.CreateDataBase(); //funzione da inserire fuori dal combattimento cosi da non essere creato ogni volta	
		createAllyPS();
		spawnAlly(allyPS[0]);
		//ENEMY STARTING
		/*HD*/enemyManager.CreateDatabase(); //funzione da inserire fuori dal combattimento cosi da non essere creato ogni volta	
		//enemyPS = PackedScene[] caricato dal nemico, io lo hardcodo per farlo funzionare
		/*HD*/enemyPS[0] = GD.Load<PackedScene>(enemyManager.EnemyPath("Demo_Enemy1"));
		/*HD*/enemyPS[1] = GD.Load<PackedScene>(enemyManager.EnemyPath("Demo_Enemy2"));
		/*HD*/enemyPS[2] = GD.Load<PackedScene>(enemyManager.EnemyPath("Demo_Enemy3"));
		spawnEnemy(enemyPS);
		//INIZIO BATTAGLIA
		BattleStart();
	}
	public override void _Process(float delta){
		//BattleUpdate();
	}
	//ALLY FUNCTIONS
	public void createAllyPS(){
		/*HD*/allyManager.EquipAlly("Demo_Ally", 0); //funzione da spostare nel menu della gestione alleati
		for(int i = 0; i < allyManager.EquippedAlly.Length; i++){
			if (allyManager.EqAllyPath(i) != null && !(allyManager.EqAllyPath(i).Equals(""))){
				allyPS[i] = GD.Load<PackedScene>(allyManager.EqAllyPath(i));
			}	
		}
	}
	public void spawnAlly(PackedScene ally){
		ally1Position = GetNode<Position2D>("Ally1Position");
		ally2Position = GetNode<Position2D>("Ally2Position");
		if (ally1Position.HasNode("Ally1Position/Ally1") == false){
			ally1Position.AddChild(ally.Instance());
			ally1 = ally1Position.GetChild<AllyInfoManager>(0);
		} else {
			ally2Position.AddChild(ally.Instance());
			ally2 = ally2Position.GetChild<AllyInfoManager>(0);
		}

	}
	//ENEMY FUNCTIONS
	public void spawnEnemy(PackedScene[] enemy){
		enemy1Position = GetNode<Position2D>("Enemy1Position");
		enemy2Position = GetNode<Position2D>("Enemy2Position");
		enemy3Position = GetNode<Position2D>("Enemy3Position");
		Position2D[] enemiesPosition = {enemy1Position, enemy2Position, enemy3Position};
		for (int i = 0; i < enemy.Length; i++){
			enemiesPosition[i].AddChild(enemy[i].Instance());
		}
	}
	//FUNZIONI TTBC
	public void BattleStart(){
		//CONTROLLO VELOCITÀ E SELEZIONE DI CHI INIZIA
		var playerVelocity = playerInfoManager.Velocity;
		enemy1 = enemy1Position.GetChild<EnemyInfoManager>(0);
		enemy2 = enemy2Position.GetChild<EnemyInfoManager>(0);
		enemy3 = enemy3Position.GetChild<EnemyInfoManager>(0);
		var enemy1Velocity = enemy1.Velocity;
		var enemy2Velocity = enemy2.Velocity;
		var enemy3Velocity = enemy3.Velocity;
		if (playerVelocity > ((enemy1Velocity + enemy2Velocity + enemy3Velocity)/3)-10){ //formula calcolo velocità complessiva da aggiustare
			SelectMoveQueue.Enqueue(playerPosition); //messo in coda il player
			playerInfoManager.SelectMove();//subito il player scelgie la mossa
			enemy1.StartTimer();
			enemy2.StartTimer();
			enemy3.StartTimer();
		} else {
			playerInfoManager.StartTimer();
			EnemyInfoManager[] enemyList = {enemy1, enemy2, enemy3};
			var enemyVelocityMax = enemy1; 
			for (int i = 0; i < enemyList.Length; i++){ //cerchiamo il nemico più veloce
				if(enemyList[i].Velocity > enemyVelocityMax.Velocity){
					enemyVelocityMax = enemyList[i];
				} else {
					enemyList[i].StartTimer();
				}
			}
			enemyWaitingQueue.Enqueue(enemyVelocityMax.GetParent<Position2D>());
		}
	}

	public void BattleUpdate(){
		
		while(winned == false){
			
			
			//PASSAGGIO DA SELECT -> MOVEQUEUE

			//PASAGGIO MOVEQUEUE -> ESECUZIONE MOSSA
		}
	}

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
	public void UpdateEnemySelectMoveQueue(){
		//PASSAGGIO DA WAITING -> SELECT enemy
		if(EnemySelectMoveQueue.Count == 0 && EnemyWaitingQueue.Count != 0){
			EnemySelectMoveQueue.Enqueue(EnemyWaitingQueue.Dequeue());
			//SCELTA MOSSA
			EnemySelectMoveQueue.Peek().GetChild<EnemyInfoManager>(0).SelectMove();
		}
	}
	public void UpdateMoveQueue(Queue<Position2D> smq){ //data una selectedqueue sposta il primo nella movequeue
		//PASSAGGIO DA SELECT -> MOVEQUEUE 		UNIVERSALE
		MoveQueue.Enqueue(smq.Dequeue());//si libera la selectMove e la si fa aggiuornare
		if (smq.Equals(SelectMoveQueue)){
			UpdateSelectMoveQueue();
		} else if(smq.Equals(EnemySelectMoveQueue)){
			UpdateEnemySelectMoveQueue();
		}
	}
	public Queue<Position2D> WaitingQueue{
		get => waitingQueue;
		set => waitingQueue = value;
	}
	public Queue<Position2D> EnemyWaitingQueue{
		get => enemyWaitingQueue;
		set => enemyWaitingQueue = value;
	}
	public Queue<Position2D> SelectMoveQueue{
		get => selectMoveQueue;
		set => selectMoveQueue = value;
	}
	public Queue<Position2D> EnemySelectMoveQueue{
		get => enemySelectMoveQueue;
		set => enemySelectMoveQueue = value;
	}
	public Queue<Position2D> MoveQueue{
		get => moveQueue;
		set => moveQueue = value;
	}
}	
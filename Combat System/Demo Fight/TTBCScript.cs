using Godot;
using System;

public class TTBCScript : Node
{
	//PLAYER VARIABLES
	private Position2D playerPosition;
	private PlayerInfoManager playerInfoManager;
	//ALLY VARIABLES
	private Position2D ally1Position;
	private Position2D ally2Position;
	AllyManager allyManager = ResourceLoader.Load("res://Characters/Ally/AllyManager.tres") as AllyManager;
	private PackedScene[] allyPS = new PackedScene[4];
	//ENEMY VARIABLES
	private Position2D enemy1Position;
	private Position2D enemy2Position;
	private Position2D enemy3Position;
	EnemyManager enemyManager = ResourceLoader.Load("res://Characters/Enemy/EnemyManager.tres") as EnemyManager;
	private PackedScene[] enemyPS = new PackedScene[3];
	public override void _Ready(){
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
		} else {
			ally2Position.AddChild(ally.Instance());
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
}

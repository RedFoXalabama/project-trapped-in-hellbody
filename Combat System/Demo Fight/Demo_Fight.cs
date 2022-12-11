using Godot;
using System;

public class Demo_Fight : Node2D
{   
	private Boolean is_focused;
	private int id_selected;
	public override void _Ready(){
		var combat_menu = GetNode<PopupMenu>("PopupMenu");
		var combat_menu_Rect = new Rect2(combat_menu.RectPosition, combat_menu.RectSize);
		combat_menu.Popup_(combat_menu_Rect);
	}
	public override void _Process(float delta){
		var combat_menu = GetNode<PopupMenu>("PopupMenu");
		var combat_menu_Rect = new Rect2(combat_menu.RectPosition, combat_menu.RectSize);
		if (combat_menu.Visible){
			if(is_focused && Input.IsActionJustPressed("ui_selection")){
				switch(id_selected){
					case 0: //Attack //funzioni da mettere nei relativi menu con singole funzioni da chiamare
					is_focused = false;
					var skill_PopupMenu = GetNode<PopupMenu>("PopupMenu/Skill_PopupMenu");
					var skill_PopupMenu_Rect = new Rect2(skill_PopupMenu.RectPosition, skill_PopupMenu.RectSize);
					skill_PopupMenu_Rect.Position += combat_menu_Rect.Position;
					skill_PopupMenu.Popup_(skill_PopupMenu_Rect);
					var cEnemy_LifeBar = GetNode<GameBar>("../GameBar/CEnemy_LifeBar");
					cEnemy_LifeBar.ChangeValue(80); // cambio vita da mettere al posto giusto
					break;
				}
			}
		}
	}

	public void _on_PopupMenu_id_focused(int id){
	   is_focused = true;
	   id_selected = id;
	}
}

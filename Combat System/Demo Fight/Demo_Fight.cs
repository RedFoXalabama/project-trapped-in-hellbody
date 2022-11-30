using Godot;
using System;

public class Demo_Fight : Node2D
{
    public override void _Ready(){
        var combat_menu = GetNode<PopupMenu>("PopupMenu");
        var combat_menu_Rect = new Rect2(combat_menu.RectPosition, combat_menu.RectSize);
        combat_menu.Popup_(combat_menu_Rect);
    }
}

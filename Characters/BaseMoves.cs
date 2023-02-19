using Godot;
using System;

interface BaseMoves
{
    void GetDamage(int damage);
    void StartTimer();
    void _on_BattleTimer_timeout();
    void SelectMove(/*tipo di mossa*/);
    void AnimateCharacter(String animation);

}

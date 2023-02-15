using Godot;
using System;

interface BaseMoves
{
    void GetDamage(int damage);
    void StartTimer(Timer timer);
    void SelectMove(/*tipo di mossa*/);
    void AnimateCharacter(String animation);

}

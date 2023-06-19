using Godot;
using System;

interface BaseMoves
{
	void TakeDamage(int damage); //serve a far subire danno al personaggio
	void StartTimer(); //serve a far partite il timer
	void _on_BattleTimer_timeout(); //serve a mettere in coda di attesa una volta scattato il timer
	void SelectMove(/*tipo di mossa*/);
	void DoAction(/*esegue la mossa tramite switch*/);
	void AnimateCharacter(String animation); //serve a far partire l'animazione
	void BackToIdle(); //serve a far tornare il personaggio in idle

}

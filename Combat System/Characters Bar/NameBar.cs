using Godot;
using System;

public partial class NameBar : LineEdit
{
    //barra del nome che si aggiorna in base al nome del personaggio
    //serve a impostare il nome tramite la proprietà dell'infomanager, usato nel ready dell'infomanager
    public void SetNameBar(String name){
        this.Text = name;
    }

}

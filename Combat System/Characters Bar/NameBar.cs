using Godot;
using System;

public partial class NameBar : LineEdit
{
    public void SetNameBar(String name){ //serve a impostare il nome tramite la proprietà dell'infomanager, usato nel ready dell'infomanager
        this.Text = name;
    }

}

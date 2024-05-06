using System;
using Godot;

public partial class Main : Node
{
    private void OnSnekHeadEnd()
    {
        var label = this.GetNode<Label>("Label");
        label.Visible = true;
    }
}

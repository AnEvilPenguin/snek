using System;
using Godot;

public partial class Main : Node
{
    private void OnSnekHeadEnd(string type)
    {
        var label = this.GetNode<Label>("Label");
        label.Visible = true;

        label.Text = $"Game Over\n({type})";
    }
}

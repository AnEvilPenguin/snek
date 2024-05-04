using Godot;
using System;

public partial class BodyPart : Area2D
{
    [Export]
    protected int _tileSize = 64;

    protected SnekBody _next;

    public SnekBody NextBody
    {
        get { return this._next; }
        set { this._next = value; }
    }
}

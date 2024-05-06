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

    public Vector2 CalculateMovement(Vector2 direction)
    {
        Vector2 movement = direction * this._tileSize;
        return new Vector2(movement.X, movement.Y);
    }
}

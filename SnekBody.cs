using Godot;
using System;
using System.Collections.Generic;
using static Godot.Control;

public partial class SnekBody : BodyPart
{
    [Export]
    public bool IsTail = true;

    private Queue<Vector2> _movements;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // When spawned initial move is to stay still
        this._movements.Enqueue(Vector2.Zero);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void AddMovement(Vector2 movement)
    {
        this._movements.Enqueue(movement);
    }

    public void OnTimerTimeout()
    {
        Vector2 nextDirection = this._movements.Dequeue();
        this.Position += nextDirection * this._tileSize;

        if (this._next != null)
        {
            this._next.AddMovement(nextDirection);
        }
    }
}

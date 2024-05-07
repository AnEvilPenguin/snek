using Godot;
using System;
using System.Collections.Generic;

public partial class BodyPart : Area2D
{
    [Export]
    protected int _tileSize = 64;

    protected SnekBody _next;

    public enum Turn
    {
        Clockwise,
        CounterClockwise,
        Stright,
    }

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

    protected void RotateSprite(Vector2 direction, Node2D sprite)
    {
        if (direction == Vector2.Right)
        {
            sprite.RotationDegrees = 90;
        }
        else if (direction == Vector2.Down)
        {
            sprite.RotationDegrees = 180;
        }
        else if (direction == Vector2.Left)
        {
            sprite.RotationDegrees = 270;
        }
        else
        {
            sprite.RotationDegrees = 0;
        }
    }

    protected void EndGame()
    {
        Hide(); // Player disappears after being hit.
        // Must be deferred as we can't change physics properties on a physics callback.
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);

        if (_next == null)
        {
            return;
        }

        _next.EndGame();
    }

    public List<Vector2> ListPositions()
    {
        var list = new List<Vector2>();

        return ListPositions(list, Vector2.Zero);
    }

    public List<Vector2> ListPositions(List<Vector2> accumulator, Vector2 relative)
    {
        var myPosition = this.Position + relative;
        accumulator.Add(myPosition);

        if (_next == null)
        {
            return accumulator;
        }

        return _next.ListPositions(accumulator, myPosition);
    }
}

using System;
using System.Collections.Generic;
using Godot;
using static Godot.Control;

public partial class SnekBody : BodyPart
{
    [Export]
    public bool IsTail = true;

    private Queue<Vector2> _movement = new Queue<Vector2>();

    private AnimatedSprite2D _sprite;

    private Turn _direction;

    // private Vector2 _previousPosition = Vector2.Zero;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // We spawn under last body segment
        this.Visible = false;

        _sprite = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void turnSprite(Turn turn)
    {
        if (this.IsTail)
        {
            return;
        }

        if (turn == Turn.Stright)
        {
            _sprite.Play("body");
            return;
        }

        _sprite.Play("corner");

        _sprite.FlipH = turn == Turn.Clockwise;
    }

    public void AddMovement(Vector2 movement, Turn directon)
    {
        this._movement.Enqueue(new Vector2(movement.X, movement.Y));
        this._direction = directon;
    }

    public void AddBody(SnekBody body)
    {
        this.IsTail = false;
        this._sprite.Play("body");

        this.NextBody = body;
        this.AddChild(body);
    }

    public void ExecuteMovement()
    {
        if (this._movement.Count == 0)
        {
            return;
        }

        Vector2 nextDirection = this._movement.Dequeue();
        turnSprite(_direction);
        RotateSprite(nextDirection, _sprite);

        if (nextDirection == Vector2.Zero)
        {
            return;
        }

        if (this.Visible == false)
        {
            this.Visible = true;
        }

        Vector2 movement = (nextDirection * -1) * this._tileSize;

        this.Position = movement;

        // this.Rotate(Mathf.Pi / 2);

        if (this.NextBody != null)
        {
            this.NextBody.ExecuteMovement();
            this.NextBody.AddMovement(nextDirection, _direction);
        }
    }
}

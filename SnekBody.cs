using System;
using System.Collections.Generic;
using Godot;
using static Godot.Control;

public partial class SnekBody : Area2D
{
    [Export]
    public bool IsTail = true;

    [Export]
    protected int _tileSize = 64;

    public SnekBody _next;
    public SnekBody NextBody
    {
        get { return this._next; }
        set { this._next = value; }
    }

    private Vector2 initialPosition;

    private Queue<Vector2> _movement = new Queue<Vector2>();

    // private Vector2 _previousPosition = Vector2.Zero;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // When spawned initial move is to stay still
        // this._movement.Enqueue(Vector2.Zero);
        GD.Print("Spawning body at " + this.Position.ToString());
        
        this.initialPosition = this.Position;
        this.Visible = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void AddMovement(Vector2 movement)
    {
        this._movement.Enqueue(new Vector2(movement.X, movement.Y));
    }

    public void AddBody(SnekBody body)
    {
        body.Position = Vector2.Left * this._tileSize;
        this.IsTail = false;
        this.NextBody = body;
        this.AddChild(body);
    }

    public void ExecuteMovement()
    {
        // TODO unfuck movement
        // TODO keep track of movement better (Go back to queue?)
        if (this._movement.Count == 0)
        {
            return;
        }
        Vector2 nextDirection = this._movement.Dequeue();

        if (nextDirection == Vector2.Zero)
        {
            return;
        }

        if (this.Visible == false)
        {
            this.Visible = true;
        }

        GD.Print("Body Position: " + this.Position.ToString());

        Vector2 movement = (nextDirection * -1) * this._tileSize;

        GD.Print("Body Executing movement " + movement.ToString());

        this.Position = movement;

        GD.Print("Body New Position: " + this.Position.ToString());

        // this.Rotate(Mathf.Pi / 2);

        if (this.NextBody != null)
        {
            this.NextBody.ExecuteMovement();
            this.NextBody.AddMovement(nextDirection);
        }
    }
}

using System;
using Godot;
using static Godot.Control;

/// <summary>
/// The head of our snake. Processes all user controls and 'drives' the snake.
/// </summary>
public partial class snek_head : Area2D
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


    /// <summary>
    /// The next direction our snake will travel in.
    /// </summary>
    private Vector2 _nextDirection = Vector2.Zero;

    /// <summary>
    /// The previous direction our snake travelled in (used to prevent backtracking).
    /// </summary>
    private Vector2 _previousDirection = Vector2.Zero;

    /// <summary>
    /// The last unit of snake body (tip of the tail).
    /// </summary>
    private SnekBody _lastBody;

    [Export]
    private int _initialBodyLength = 2;

    /// <summary>
    /// Gets or Sets a Body Scene to spawn body parts from.
    /// </summary>
    [Export]
    public PackedScene BodyScene { get; set; }

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        // Snapped allows us to round the position to the nearest element
        this.Position = this.Position.Snapped(Vector2.One * this._tileSize);

        // This centers the player on the tile
        this.Position += Vector2.One * this._tileSize / 2;

        GD.Print("Spawning head at " + this.Position.ToString());

        for (int i = 0; i < this._initialBodyLength; i++)
        {
            this.AddBody();
        }
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    /// <param name="delta">The elapsed time since the previous frame.</param>
    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("ui_left"))
        {
            this._nextDirection = Vector2.Left;
        }

        if (Input.IsActionPressed("ui_right"))
        {
            this._nextDirection = Vector2.Right;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            this._nextDirection = Vector2.Down;
        }

        if (Input.IsActionPressed("ui_up"))
        {
            this._nextDirection = Vector2.Up;
        }
    }

    /// <summary>
    /// Called on timer timeout signal.
    /// </summary>
    public void OnTimerTimeout()
    {
        if (this._nextDirection == Vector2.Zero)
        {
            return;
        }

        GD.Print("Head Position: " + this.Position.ToString());

        Vector2 movement = this.CalculateMovement(this._nextDirection);

        GD.Print("Head Executing movement " + movement.ToString());

        this.Position = movement + this.Position;

        GD.Print("Head new position: " + this.Position.ToString());

        if (this.NextBody != null)
        {
            this.NextBody.AddMovement(this._nextDirection);
            this.NextBody.ExecuteMovement();
        }

        this._previousDirection = this._nextDirection;
    }

    /// <summary>
    /// Used to spawn a new body segment.
    /// </summary>
    private void AddBody()
    {
        Vector2 spawnLocation = this._lastBody != null ?
            this._lastBody.Position :
            this.Position;

        SnekBody body = this.BodyScene.Instantiate<SnekBody>();

        body.Position = new Vector2(spawnLocation.X, spawnLocation.Y);
        GD.Print("Moving body to " + body.Position.ToString());

        if (this.NextBody == null)
        {
            body.Position = Vector2.Left * this._tileSize;
            this.NextBody = body;
            this.AddChild(body);
        }

        if (this._lastBody == null)
        {
            this._lastBody = body;
        }
        else
        {
            this._lastBody.AddBody(body);
            this._lastBody = body;
        }
    }
}

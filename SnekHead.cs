using System;
using System.Linq;
using Godot;
using static Godot.Control;

/// <summary>
/// The head of our snake. Processes all user controls and 'drives' the snake.
/// </summary>
public partial class SnekHead : BodyPart
{
    [Signal]
    public delegate void EndEventHandler(string kind);

    [Signal]
    public delegate void AteMouseEventHandler();

    private Sprite2D _sprite;

    private Boolean _hasEaten = false;

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

    private Turn _turn;

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

        _sprite = this.GetNode<Sprite2D>("Sprite2D");

        for (int i = 0; i < this._initialBodyLength; i++)
        {
            this.AddBody();
        }
    }

    private void ProcessOverlap()
    {
        var areas = this.GetOverlappingAreas();

        foreach (Area2D area in areas)
        {
            if (area.Name == "SnekBody")
            {
                this.EndGame();
                EmitSignal(SignalName.End, "Ouroboros");
            }

            if (area.Name == "OutOfBounds")
            {
                this.EndGame();
                EmitSignal(SignalName.End, "Out of Bounds");
            }

            if (area.Name == "Mouse" && !_hasEaten)
            {
                EmitSignal(SignalName.AteMouse);
                this.AddBody();

                _hasEaten = true;
            }
        }
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    /// <param name="delta">The elapsed time since the previous frame.</param>
    public override void _Process(double delta)
    {
        if (this.HasOverlappingAreas())
        {
            this.ProcessOverlap();
        }

        if (this._previousDirection + this._nextDirection == Vector2.Zero)
        {
            this._nextDirection = this._previousDirection;
        }

        if (Input.IsActionPressed("ui_left") && this._previousDirection != Vector2.Right)
        {
            this._nextDirection = Vector2.Left;
        }

        if (Input.IsActionPressed("ui_right") && this._previousDirection != Vector2.Left)
        {
            this._nextDirection = Vector2.Right;
        }

        if (Input.IsActionPressed("ui_down") && this._previousDirection != Vector2.Up)
        {
            this._nextDirection = Vector2.Down;
        }

        if (Input.IsActionPressed("ui_up") && this._previousDirection != Vector2.Down)
        {
            this._nextDirection = Vector2.Up;
        }
    }

    public Turn GetTurnType()
    {
        int x = (int)(_nextDirection.X - _previousDirection.X);
        int y = (int)(_nextDirection.Y + _previousDirection.Y);
        int turn = x * y;

        switch (turn)
        {
            case 1: return Turn.CounterClockwise;
            case -1: return Turn.Clockwise;
            default: return Turn.Stright;
        }
    }

    /// <summary>
    /// Called on timer timeout signal.
    /// </summary>
    public void OnTimerTimeout()
    {
        _hasEaten = false;

        if (this._nextDirection == Vector2.Zero)
        {
            return;
        }

        Vector2 movement = this.CalculateMovement(this._nextDirection);

        Turn direction = GetTurnType();
        RotateSprite(this._nextDirection, _sprite);

        this.Position = movement + this.Position;

        if (this.NextBody != null)
        {
            this.NextBody.AddMovement(this._nextDirection, direction);
            this.NextBody.ExecuteMovement();
        }

        this._previousDirection = this._nextDirection;

        if (this.HasOverlappingAreas())
        {
            ProcessOverlap();
        }
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

        if (this.NextBody == null)
        {

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

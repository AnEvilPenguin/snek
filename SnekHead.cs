using System;
using Godot;
using static Godot.Control;

/// <summary>
/// The head of our snake. Processes all user controls and 'drives' the snake.
/// </summary>
public partial class SnekHead : BodyPart
{
    /// <summary>
    /// The next direction our snake will travel in.
    /// </summary>
    private Vector2 _nextDirection = Vector2.Zero;

    /// <summary>
    /// The previous direction our snake travelled in (used to prevent backtracking).
    /// </summary>
    private Vector2 _previousDirection = Vector2.Zero;

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        // Snapped allows us to round the position to the nearest element
        this.Position = this.Position.Snapped(Vector2.One * this._tileSize);

        // This centers the player on the tile
        this.Position += Vector2.One * this._tileSize / 2;
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
        this.Position += this._nextDirection * this._tileSize;

        if (this._next != null)
        {
            this._next.AddMovement(this._nextDirection);
        }

        this._previousDirection = this._nextDirection;
    }
}

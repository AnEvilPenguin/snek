using Godot;
using System;
using static Godot.TextServer;

public partial class Player : Area2D
{
	private int tileSize = 64;

	Vector2 nextDirection { get; set; }

	bool isHead { get; set; }
	bool isIndependent { get; set; }

	Player nextPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		// Snapped allows us to round the position to the nearest element
		Position = Position.Snapped(Vector2.One * tileSize);
		// This centers the player on the tile
		Position += Vector2.One * tileSize / 2;

		nextDirection = Vector2.Zero;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_left"))
		{
			nextDirection = Vector2.Left;
		}

		if (Input.IsActionPressed("ui_right"))
		{
			nextDirection = Vector2.Right;
		}

		if (Input.IsActionPressed("ui_down"))
		{
			nextDirection = Vector2.Down;
		}

		if (Input.IsActionPressed("ui_up"))
		{
			nextDirection = Vector2.Up;
		}
	}

	public void OnTimerTimeout()
	{
        Position += nextDirection * tileSize;
    }
}

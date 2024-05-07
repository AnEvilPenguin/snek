using System;
using System.Collections.Generic;
using Godot;
using Snek;

public partial class Main : Node
{
    [Export]
    public PackedScene MouseScene { get; set; }

    private int _score;

    private Mouse _currentMouse;

    private Vector2 _windowSize;

    private Random _random = new Random();

    private SnekHead _head;

    private Timer _timer;

    private void OnSnekHeadEnd(string type)
    {
        var label = this.GetNode<Label>("Label");
        label.Visible = true;

        label.Text = $"Game Over\n({type})";

        _currentMouse.Visible = false;

    }

    private Vector2 newMouseLocation() =>
        newMouseLocation(0, _head.ListPositions());

    private Vector2 newMouseLocation(int depth, List<Vector2> snakePositions)
    {
        if (depth > 10)
        {
            GD.PrintErr("Failed to get new mouse location");
        }

        var x = _random.Next((int)_windowSize.X - 66) + 1;
        var y = _random.Next((int)_windowSize.Y - 66) + 1;

        var position = Util.GetSnappedPosition(x, y);

        if (depth > 10)
        {
            // FIXME probably need a better way of handling this.
            GD.PrintErr("Failed to get new mouse location");

            return position;
        }

        if (snakePositions.Contains(position))
        {
            return newMouseLocation(depth + 1, snakePositions);
        }

        if (position == _currentMouse.Position)
        {
            return newMouseLocation(depth + 1, snakePositions);
        }

        return position;
    }

    private void OnSnekHeadAteMouseEvent()
    {
        _currentMouse.Position = newMouseLocation();
        _score++;

        if (_score % 5 == 0 && _timer.WaitTime > 0.1)
        {
            _timer.WaitTime = _timer.WaitTime - 0.1;
        }

        // If overlapping move it?
        // Could also just check for Snek positioning and re-roll if that fails?
    }

    private void SpawnMouse()
    {
        _currentMouse = this.MouseScene.Instantiate<Mouse>();
        _currentMouse.Position = newMouseLocation();

        this.AddChild(_currentMouse);
    }

    public override void _Process(double delta)
    {
    }

    public override void _Ready()
    {
        _windowSize = GetTree().Root.Size;

        _head = this.GetNode<SnekHead>("SnekHead");
        _timer = this.GetNode<Timer>("Timer");

        SpawnMouse();
    }
}

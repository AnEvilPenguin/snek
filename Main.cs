using System;
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

    private void OnSnekHeadEnd(string type)
    {
        var label = this.GetNode<Label>("Label");
        label.Visible = true;

        label.Text = $"Game Over\n({type})";

        _currentMouse.Visible = false;

    }

    private Vector2 newMouseLocation() =>
        newMouseLocation(0);

    private Vector2 newMouseLocation(int depth)
    {
        if (depth > 10)
        {
            GD.PrintErr("Failed to get new mouse location");
        }

        var x = _random.Next((int)_windowSize.X - 1);
        var y = _random.Next((int)_windowSize.X - 1);

        var position = Util.GetSnappedPosition(x, y);

        if (position == _currentMouse.Position)
        {
            return newMouseLocation(depth + 1);
        }

        if (position == _head.Position)
        {
            return newMouseLocation(depth + 1);
        }

        return position;
    }

    private void OnSnekHeadAteMouseEvent()
    {
        _currentMouse.Position = newMouseLocation();
        _score++;

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

        SpawnMouse();
    }
}

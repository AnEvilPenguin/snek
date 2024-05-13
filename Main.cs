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

    private Label _scoreLabel;

    private Label _gameOverLabel;

    private Button _startButton;

    private List<AudioStreamPlayer> _eatEffects = new List<AudioStreamPlayer>();

    private List<AudioStreamPlayer> _deathEffects = new List<AudioStreamPlayer>();

    private SaveManager _save = new SaveManager();

    public override void _Process(double delta)
    {
    }

    public override void _Ready()
    {
        this._windowSize = this.GetTree().Root.Size;

        this._head = this.GetNode<SnekHead>("SnekHead");
        this._timer = this.GetNode<Timer>("Timer");

        this._scoreLabel = this.GetNode<Label>("Score");
        this._gameOverLabel = this.GetNode<Label>("Label");

        this._startButton = this.GetNode<Button>("StartButton");

        this._eatEffects.Add(this.GetNode<AudioStreamPlayer>("Eat1"));
        this._eatEffects.Add(this.GetNode<AudioStreamPlayer>("Eat2"));
        this._eatEffects.Add(this.GetNode<AudioStreamPlayer>("Eat3"));

        this._deathEffects.Add(this.GetNode<AudioStreamPlayer>("Death1"));
        this._deathEffects.Add(this.GetNode<AudioStreamPlayer>("Death2"));
        this._deathEffects.Add(this.GetNode<AudioStreamPlayer>("Death3"));

        this.SpawnMouse();
    }

    private void OnStartButtonPressed()
    {
        this._gameOverLabel.Hide();
        this._scoreLabel.Show();

        this._score = 0;
        this.UpdateScoreLabel();

        this._head.Start();

        this._timer.WaitTime = 1;
        this._timer.Start();

        this.newMouseLocation();
        this._currentMouse.Show();

        this._startButton.Hide();
    }

    private void OnSnekHeadEnd(string type)
    {
        int effectNumber = this._random.Next(0, this._deathEffects.Count);
        this._deathEffects[effectNumber].Play();

        this._gameOverLabel.Text = $"Game Over\n({type})";
        this._gameOverLabel.Show();

        this._currentMouse.Visible = false;

        this._head.Stop();

        this._timer.Stop();

        this._currentMouse.Hide();

        this._startButton.Show();

        this._save.AddScore(this._score, type);
        this._save.getScores();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            this._save.Persist();
            GetTree().Quit(); // default behavior
        }
    }

    private Vector2 newMouseLocation() =>
        this.newMouseLocation(0, this._head.ListPositions());

    private Vector2 newMouseLocation(int depth, List<Vector2> snakePositions)
    {
        if (depth > 10)
        {
            GD.PrintErr("Failed to get new mouse location");
        }

        var x = this._random.Next((int)this._windowSize.X - 66) + 1;
        var y = this._random.Next((int)this._windowSize.Y - 66) + 1;

        var position = Util.GetSnappedPosition(x, y);

        if (depth > 10)
        {
            // FIXME probably need a better way of handling this.
            GD.PrintErr("Failed to get new mouse location");

            return position;
        }

        if (snakePositions.Contains(position))
        {
            return this.newMouseLocation(depth + 1, snakePositions);
        }

        if (position == this._currentMouse.Position)
        {
            return this.newMouseLocation(depth + 1, snakePositions);
        }

        return position;
    }

    private void OnSnekHeadAteMouseEvent()
    {
        int effectNumber = this._random.Next(0, this._eatEffects.Count);
        this._eatEffects[effectNumber].Play();

        this._currentMouse.Position = this.newMouseLocation();
        this._currentMouse.RotateRandom();

        this._score++;
        this.UpdateScoreLabel();

        if (this._score % 5 == 0 && this._timer.WaitTime > 0.1)
        {
            this._timer.WaitTime = this._timer.WaitTime - 0.1;
        }
    }

    private void UpdateScoreLabel() =>
        this._scoreLabel.Text = $"Score: {this._score}";

    private void SpawnMouse()
    {
        this._currentMouse = this.MouseScene.Instantiate<Mouse>();
        this._currentMouse.Position = this.newMouseLocation();
        this._currentMouse.Hide();

        this.AddChild(this._currentMouse);
    }
}

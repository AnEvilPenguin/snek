using Godot;
using System;

public partial class Mouse : Area2D
{

    private Sprite2D _sprite;

    private Random _random = new Random();

    public override void _Ready()
    {
        this._sprite = this.GetNode<Sprite2D>("Sprite2D");
    }

    public void RotateRandom()
    {
        int degrees = this._random.Next(0, 360);

        this._sprite.RotationDegrees = degrees;
    }
}

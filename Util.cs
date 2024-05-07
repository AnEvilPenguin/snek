namespace Snek
{
    using System;
    using Godot;

    internal class Util
    {
        internal static Vector2 GetSnappedPosition(int x, int y, int tileSize = 64)
        {
            Vector2 position = new Vector2(x, y);

            position = position.Snapped(Vector2.One * tileSize);

            position += Vector2.One * tileSize / 2;

            return position;
        }
    }
}

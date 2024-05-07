namespace Snek
{
    using System;
    using Godot;

    internal class Util
    {
        internal static Vector2 GetSnappedPosition(int x, int y, int tileSize = 64)
        {
            Vector2 position = new Vector2(x, y);

            // Snapped allows us to round the position to the nearest element
            position = position.Snapped(Vector2.One * tileSize);

            // This centers the player on the tile
            position += Vector2.One * tileSize / 2;

            return position;
        }

        internal static Vector2 GetSnappedPosition(Vector2 position, int tileSize = 64) =>
            GetSnappedPosition((int)position.X, (int)position.Y, tileSize);
    }
}

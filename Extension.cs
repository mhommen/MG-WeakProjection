using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace test3d
{
    public static class Extension
    {
        private static Texture2D _texture;
        private static Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _texture.SetData(new[] { Color.White });
            }

            return _texture;
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }

        public static void DrawRect(this SpriteBatch spriteBatch, Rectangle rect, Color color, int width = 1)
        {
            spriteBatch.DrawLine(new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top), color, width);
            spriteBatch.DrawLine(new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom), color, width);
            spriteBatch.DrawLine(new Vector2(rect.Right, rect.Bottom), new Vector2(rect.Left, rect.Bottom), color, width);
            spriteBatch.DrawLine(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Left, rect.Top), color, width);
        }

        public static void DrawX(this SpriteBatch spriteBatch, Vector2 p, Color col, float len = 3)
        {
            spriteBatch.DrawLine(p - new Vector2(len, len), p + new Vector2(len, len), col);
            spriteBatch.DrawLine(p + new Vector2(-len, len), p - new Vector2(-len, len), col);
        }

        public static float AngleTo(this Vector2 v1, Vector2 v2)
        {
            return (float)Math.Atan2(v2.Y - v1.Y, v2.X - v1.X);
        }

        public static Vector2 ToProjection(this Vector3 v)
        {
            return new Vector2(v.X / v.Z, v.Y / v.Z);
        }
    }
}

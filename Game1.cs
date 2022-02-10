using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace test3d
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private Vector4[] _points;
        private Vector2 _origin;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _points = new Vector4[8];
            _points[0] = new Vector4(-50, -50, 1, 1);
            _points[1] = new Vector4(50, -50, 1, 1);
            _points[2] = new Vector4(50, 50, 1, 1);
            _points[3] = new Vector4(-50, 50, 1, 1);
            _points[4] = new Vector4(-50, -50, 1.2f, 1);
            _points[5] = new Vector4(50, -50, 1.2f, 1);
            _points[6] = new Vector4(50, 50, 1.2f, 1);
            _points[7] = new Vector4(-50, 50, 1.2f, 1);

            _origin = new Vector2(_graphics.PreferredBackBufferWidth / 2f, _graphics.PreferredBackBufferHeight / 2f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().GetPressedKeys().Length == 0)
                return;

            var m = new Matrix(
                new Vector4(1, 0, 0, 0),
                new Vector4(0, 1, 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(0, 0, 0, 1)
            );

            var speed = 3;

            if (Keyboard.GetState().IsKeyDown(Keys.A)) // left
                m.M14 = -speed;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) // right
                m.M14 = speed;
            if (Keyboard.GetState().IsKeyDown(Keys.W)) // up
                m.M24 = -speed;
            if (Keyboard.GetState().IsKeyDown(Keys.S)) // down
                m.M24 = speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                var theta = .05f;
                var cosTheta = (float)Math.Cos(theta);
                var sinTheta = (float)Math.Sin(theta);

                m.M11 = cosTheta;
                m.M12 = sinTheta;
                m.M21 = -sinTheta;
                m.M33 = cosTheta;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                var theta = .05f;
                var cosTheta = (float)Math.Cos(theta);
                var sinTheta = (float)Math.Sin(theta);

                m.M11 = cosTheta;
                m.M12 = -sinTheta;
                m.M21 = sinTheta;
                m.M33 = cosTheta;
            }

            for (int i = 0; i < _points.Length; i++)
                _points[i] = MatrixMultipl(_points[i], m);

            base.Update(gameTime);
        }

        private Vector4 MatrixMultipl(Vector4 p, Matrix m)
        {
            return new Vector4(
                (m.M11 * p.X) + (m.M12 * p.Y) + (m.M13 * p.Z) + (m.M14 * p.W),
                (m.M21 * p.X) + (m.M22 * p.Y) + (m.M23 * p.Z) + (m.M24 * p.W),
                (m.M31 * p.X) + (m.M32 * p.Y) + (m.M33 * p.Z) + (m.M34 * p.W),
                (m.M41 * p.X) + (m.M42 * p.Y) + (m.M43 * p.Z) + (m.M44 * p.W)
            );
        }

        private void DrawRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color color, float thickness = 1f)
        {
            _spriteBatch.DrawLine(p1 + _origin, p2 + _origin, color, thickness);
            _spriteBatch.DrawLine(p2 + _origin, p3 + _origin, color, thickness);
            _spriteBatch.DrawLine(p3 + _origin, p4 + _origin, color, thickness);
            _spriteBatch.DrawLine(p4 + _origin, p1 + _origin, color, thickness);
        }

        private void DrawCube(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Vector3 p5, Vector3 p6, Vector3 p7, Vector3 p8, Color color, float thickness = 1f)
        {
            DrawRect(p5.ToProjection(), p6.ToProjection(), p7.ToProjection(), p8.ToProjection(), color, thickness);
            _spriteBatch.DrawLine(p1.ToProjection() + _origin, p5.ToProjection() + _origin, color, thickness);
            _spriteBatch.DrawLine(p2.ToProjection() + _origin, p6.ToProjection() + _origin, color, thickness);
            _spriteBatch.DrawLine(p3.ToProjection() + _origin, p7.ToProjection() + _origin, color, thickness);
            _spriteBatch.DrawLine(p4.ToProjection() + _origin, p8.ToProjection() + _origin, color, thickness);
            DrawRect(p1.ToProjection(), p2.ToProjection(), p3.ToProjection(), p4.ToProjection(), color, thickness);
        }
        
        private void DrawCube(Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4, Vector4 p5, Vector4 p6, Vector4 p7, Vector4 p8, Color color, float thickness = 1f)
        {
            DrawCube(
                new Vector3(p1.X, p1.Y, p1.Z),
                new Vector3(p2.X, p2.Y, p2.Z),
                new Vector3(p3.X, p3.Y, p3.Z),
                new Vector3(p4.X, p4.Y, p4.Z),
                new Vector3(p5.X, p5.Y, p5.Z),
                new Vector3(p6.X, p6.Y, p6.Z),
                new Vector3(p7.X, p7.Y, p7.Z),
                new Vector3(p8.X, p8.Y, p8.Z),
                color, thickness);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            DrawCube(_points[0], _points[1], _points[2], _points[3], _points[4], _points[5], _points[6], _points[7], Color.Red, 2);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

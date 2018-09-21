using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TU_NAMESPACE
{
    public class Camera
    {
    
        private Point lim;
        public Point Lim
        {
            set { this.lim = value; }
            get { return lim; }
        }

        private Matrix transform;
        public Matrix Transform
        {
            get
            {
                return transform;
            }
        }

        private Vector2 centro;
        private Viewport viewport;

        public Camera(Viewport viewport, int anchoEsc, int altoEsc)
        {
            this.viewport = viewport;
            this.lim = new Point(anchoEsc, altoEsc);
        }

        public void Update(Vector2 posicionPersonaje)
        {
            if (posicionPersonaje.X < viewport.Width / 2)
                centro.X = viewport.Width / 2;
            else if (posicionPersonaje.X > lim.X - (viewport.Width / 2))
                centro.X = lim.X - (viewport.Width / 2);
            else
                centro.X = posicionPersonaje.X;

            if (posicionPersonaje.Y > viewport.Height / 2)
                centro.Y = viewport.Height / 2;
            else if (posicionPersonaje.Y < -lim.Y + (viewport.Height / 2))
                centro.Y = -lim.Y   + (viewport.Height / 2);
            else
                centro.Y = posicionPersonaje.Y;

            transform = Matrix.CreateTranslation(
                new Vector3(-centro.X + (viewport.Width / 2),
                            -centro.Y + (viewport.Height / 2), 0));
        }
    }
}

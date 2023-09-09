using SFML.Graphics;
using SFML.System;

namespace Critters.GraphHelpers
{/// <summary>
 /// Allows to render a line with a thickness.
 /// </summary>
    public class Line : Drawable
    {
        /// <summary>
        /// Sets an <see cref="SFML.Graphics.Color"/> for this line.
        /// </summary>
        public Color Color
        {
            set
            {
                for (var i = 0; i < _vertices.Length; i++)
                {
                    _vertices[i].Color = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets this line's thickness in pixels.
        /// </summary>
        public float Thickness { get { return _thickness; } set { _thickness = value; Update(); } }

        /// <summary>
        /// Gets or sets this line's starting point.
        /// </summary>
        public Vector2f Start { get { return _start; } set { SetPoints(value, _end); } }

        /// <summary>
        /// Gets or sets this line's end point.
        /// </summary>
        public Vector2f End { get { return _end; } set { SetPoints(_start, value); } }

        /// <summary>
        /// Sets start and end points for this line.
        /// </summary>
        public void SetPoints(Vector2f start, Vector2f end)
        {
            _start = start;
            _end = end;
            Update();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_vertices, PrimitiveType.TriangleStrip, states);
        }

        private void Update()
        {
            var direction = _end - _start;
            var unitDirection = direction / (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            var unitPerpendicular = new Vector2f(-unitDirection.Y, unitDirection.X);
            var offset = (_thickness / 2.0f) * unitPerpendicular;

            _vertices[0].Position = _start + offset;
            _vertices[1].Position = _start - offset;
            _vertices[2].Position = _end + offset;
            _vertices[3].Position = _end - offset;
        }

        private Vector2f _start = new Vector2f(0, 0);
        private Vector2f _end = new Vector2f(0, 0);
        private float _thickness = 0;
        private Vertex[] _vertices = new Vertex[4];
    };
}

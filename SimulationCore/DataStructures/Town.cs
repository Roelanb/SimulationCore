using SimulationCore.Factories;
using SimulationCore.Helpers;
using SFML.Graphics;
using SFML.System;

namespace SimulationCore.DataStructures
{
    public class Town
    {
        public Town(Vector2f position, Texture texture)
        {
            this.Position = position;

            this.Shape = new RectangleShape(new Vector2f(300 * Configuration.Scale, 300 * Configuration.Scale))
            {
                Texture = texture,
                Origin = new Vector2f(150*Configuration.Scale, 150 * Configuration.Scale),
                Position = position,
            };

            Id = TownHelper.TownId;
        }

        public Vector2f Position { get; set; }

        public RectangleShape Shape { get; set; }

        public int Id { get; set; }

        public void Draw(RenderWindow window)
        {
            window.Draw(Shape);
        }
    }
}

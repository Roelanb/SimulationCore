using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Critters.Object
{
    public class Obstacle : BaseItem
    {
        public RectangleShape Shape { get; set; }

        public Obstacle(World world)
        {
            Type = ItemTypes.Obstacle;

            // set position to random value in world
            Position = new Vector2f(Application.random.Next(0, (int)Configuration.Width), Application.random.Next(0, (int)Configuration.Height));

            Shape = new RectangleShape(new Vector2f(10, 10))
            {
                FillColor = SFML.Graphics.Color.Yellow,
                Origin = new Vector2f(5, 5),
                Position = Position,
            };
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(Shape);
        }
    }
}

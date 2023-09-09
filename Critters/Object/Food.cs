using Critters.ExtensionMethods;
using Critters.GraphHelpers;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Critters.Object;

public class Food : BaseItem
{
    public RectangleShape Shape { get; set; }
    public FontText Info;
    public bool Active { get; set; }
    public Food(World world)
    {
        Type = ItemTypes.Food;
        Active = true;

        // set position to random value in world
        Position = new Vector2f(Application.random.Next(0, (int)Configuration.Width), Application.random.Next(0, (int)Configuration.Height));

        Shape = new RectangleShape(new Vector2f(10, 10))
        {
            FillColor = Color.Green,
            Origin = new Vector2f(5, 5),
            Position = Position,
        };

        Info = new FontText($"", Color.Black, 0.5f);


    }

    public void Update()
    {
        Info.StringText = Info();
    }


    public void Draw(RenderWindow window)
    {
        Shape.Position = Position;

        if (Active)
            Shape.FillColor = new Color(5, 5, 5);
        else
            Shape.FillColor = Color.Green;

        window.Draw(Shape);


        // draw the position x and y on the screen
        window.DrawString(Info, Shape.Position);
    }
}

using Critters.ExtensionMethods;
using Critters.SFML_Text;
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

public class Creature : BaseItem
{
    public NeuralNetwork Brain { get; set; }
    public RectangleShape Shape { get; set; }
    private FontText Info;

    public int Frame;
    public double ParentChance { get; set; }

    private World _world;

    public Creature(World world)
    {
        Type = ItemTypes.Creature;

        _world = world;

        // set brain to random neural network
        Brain = new NeuralNetwork(0.0, 4, 250, 3);
        Brain.Randomize();

        // set position to random value in world
        Position = new Vector2f(Application.random.Next(0, (int)Configuration.Width), Application.random.Next(0, (int)Configuration.Height));

        Shape = new RectangleShape(new Vector2f(10, 10))
        {
            FillColor = Color.Red,
            Origin = new Vector2f(5, 5),
            Position = Position,
        };

        // set life to 100
        Life = 100;

        // set fitness to 0
        Fitness = 0;

        // set angle to random value
        Angle = Application.random.Next(0, 360);

        Info = new FontText($"", Color.Black, 0.5f);

    }

    public void Reset()
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());
        //this.Position = new Vector2(random.Next(Bounds.Left, Bounds.Right), random.Next(Bounds.Top, Bounds.Bottom));
        Angle = random.Next(0, 360);
        Frame = random.Next(0, 8);
        Fitness = 0;
        Life = 100;
    }

    public void Draw(RenderWindow window)
    {
        if (Fitness > 0)
        {
            Shape.Rotation = (float)Angle;
            Shape.Position = Position;

            window.Draw(Shape);

            // draw the position x and y on the screen
            window.DrawString(Info, Shape.Position);
        }
    }


    public void Update(List<Food> foods, List<Obstacle> obstacle)
    {
        if (Life <= 0)
        {
            Fitness = 0;
            return;
        }

        Frame++;
        if (Frame > 7)
            Frame = 0;

        double[] input = null;
        double[] output = null;

        input = new double[4];

        var closestFood = GetClosestFood(foods, Position);

        Vector2f Origin = new Vector2f(32, 32);
        Vector2f LeftSensor = ExtendedPoint(Position, (float)(Angle - 45 - 90), 100);
        Vector2f RightSensor = ExtendedPoint(Position, (float)(Angle + 45 - 90), 100);
        
        double ClosestFoodLeft = GetDistance(closestFood?.Position, LeftSensor);
        double ClosestFoodRight = GetDistance(closestFood?.Position, RightSensor);
        double ClosestObstacleLeft = GetDistance(GetClosestObstacle(obstacle, Position).Position, LeftSensor);
        double ClosestObstacleRight = GetDistance(GetClosestObstacle(obstacle, Position).Position, RightSensor);
        double CenterDistance = GetDistance(closestFood?.Position, Position);
        double CenterDistanceObstacle = GetDistance(GetClosestObstacle(obstacle, Position).Position, Position);

        if (CenterDistance < 30 && closestFood != null)
        {
            Life += 30;
            Fitness += 10;
            Random random = new Random(Guid.NewGuid().GetHashCode());
            closestFood.Position = new Vector2f(Application.random.Next(0, (int)Configuration.Width), Application.random.Next(0, (int)Configuration.Height));
            closestFood.Active = false;
        }

        Life -= 0.1;
        if (CenterDistance < CenterDistanceObstacle)
        {
            if (ClosestFoodLeft > ClosestFoodRight)
            {
                input[0] = 1;
                input[1] = -1;
            }
            else
            {
                input[0] = -1;
                input[1] = 1;
            }

            input[2] = 0;
            input[3] = 0;
        }
        else
        {
            input[0] = 0;
            input[1] = 0;

            if (ClosestObstacleLeft > ClosestObstacleRight)
            {
                input[2] = 1;
                input[3] = -1;
            }
            else
            {
                input[2] = -1;
                input[3] = 1;
            }

        }

        /*if (CenterDistance > CenterDistanceObstacle)
        {
            input[4] = 1;
        }
        else
        {
            input[4] = -1;
        }*/

        /*input[0] = ClosestFoodLeft;
        input[1] = ClosestFoodRight;
        input[2] = ClosestObstacleLeft;
        input[3] = ClosestObstacleRight;*/

        output = Brain.FeedForward(input);

        if (output[0] > output[1])
        {
            Angle += output[0] * 4;
        }
        else
        {
            Angle -= output[1] * 4;
        }

        double Speed = output[2] * 2;
        float Radians = (float)((Angle - 90) * Math.PI / 180);
        Vector2f OldPos = Position;

        Position += new Vector2f((float)(Math.Cos(Radians) * Speed), (float)(Math.Sin(Radians) * Speed));

        double ClosestObstacle = 32000000;
        foreach (Obstacle o in obstacle)
        {
            var distance = GetDistance(o.Position - Origin, Position - Origin);

            if (distance < ClosestObstacle) ClosestObstacle = distance;

        }

        if (ClosestObstacle < 30)
        {
            Life -= 1;
            Fitness -= 10;
            if (Fitness < 0)
                Fitness = 0;
            Position = OldPos;
        }

        if (Position.X < 0.0f) Position = new Vector2f(0.0f, Position.Y);
        if (Position.X > Configuration.Width) Position = new Vector2f(Configuration.Width, Position.Y);

        if (Position.Y < 0.0f) Position = new Vector2f(Position.X, 0.0f);
        if (Position.Y > Configuration.Height) Position = new Vector2f(Position.X, Configuration.Height);


        // set info text
        Info.StringText = Info();
    }


    public Vector2f ExtendedPoint(Vector2f center, float directionangle, int length)
    {
        float Radians = (float)(directionangle * Math.PI / 180);
        Vector2f resultposition;
        resultposition.X = (float)(center.X + (Math.Cos(Radians) * length));
        resultposition.Y = (float)(center.Y + (Math.Sin(Radians) * length));
        return resultposition;
    }

    public double GetDistance(Vector2f? Start, Vector2f? End)
    {
        if (Start == null) return 999;
        
        Vector2f Diff = Start.Value - End.Value;
        return Math.Sqrt(Diff.X * Diff.X + Diff.Y * Diff.Y);
    }

    public Food? GetClosestFood(List<Food> foods, Vector2f Start)
    {
        Food? closestFood = null;
        double Closest = 320000;
        foreach (Food f in foods)
        {
            var distance = GetDistance(Start, f.Position);
            if (distance < Closest)
            {
                Closest = distance;
                closestFood = f;
            }

        }
        return closestFood;
    }

    public Obstacle GetClosestObstacle(List<Obstacle> obstacle, Vector2f Start)
    {
        Obstacle ClosestObstacle = new Obstacle(_world);
        double Closest = 32000;
        foreach (Obstacle o in obstacle)
        {
            if (GetDistance(Start, o.Position) < Closest)
            {
                Closest = GetDistance(Start, o.Position);
                ClosestObstacle = o;
            }
        }
        return ClosestObstacle;
    }

    public Creature Reproduce()
    {
        var child = new Creature(_world);

        child.Brain = Brain.Clone();

        return child;
    }

    public void Mutate()
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());

        int MutationPoint = random.Next(0, Brain.GetNrOfDendrites());
        double[] Weights = Brain.GetWeights();
        Weights[MutationPoint] = random.NextDouble();
        Brain.SetWeights(Weights);
    }
}

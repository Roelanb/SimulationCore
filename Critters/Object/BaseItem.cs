using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Critters.Object
{
    public enum ItemTypes
    {
        Creature = 0,
        Food = 1,
        Obstacle = 2
    }

    public class BaseItem
    {
        public Vector2f Position { get; set; }
        public double Angle { get; set; }
        public double Life { get; set; }
        public double Fitness { get; set; }

        public ItemTypes Type { get; set; }


        public BaseItem? Hit(Vector2i checkPosition)
        {
            // check if checkPosition is close to Position
            if (Math.Abs(checkPosition.X - Position.X) < 5 && Math.Abs(checkPosition.Y - Position.Y) < 5)
            {
                return this;
            }

            return null;
        }

        public override string ToString()
        {
            return $"Life: {Life:00.0}  Pos: {Position.X:0000},{Position.Y:0000}";
        }

        public string Info()
        {
            if (Type== ItemTypes.Creature) return $"Life: {Life:00.0} {Fitness:00.0}";
            if (Type == ItemTypes.Food) return $"Food";
            if (Type == ItemTypes.Obstacle) return $"Obstacle";
            


            return "";
            
        }
    }
}

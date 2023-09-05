using SimulationCore.DataStructures;
using SimulationCore.Helpers;
using SFML.Graphics;
using System.Collections.Generic;

namespace SimulationCore.Factories
{
    public static class TownFactory
    {
        public static List<Town> GetTowns()
        {
            var towns = new List<Town>();

            for(int i = 0; i < TownHelper.TownPositions.Count; i++)
            {
                var townPosition = TownHelper.TownPositions[i];
                // Create a new town that has a single textured quad.
                towns.Add(new Town(townPosition, new Texture(GetTownTexture(i))));
            }

            return towns;
        }

        private static string GetTownTexture(int i)
        {
            if (Configuration.UseRandomTowns)
            {
                return $"../../../Resources/Town_{Application.random.Next(1, 10)}.png";
            }
            else
            {
                return $"../../../Resources/Town_{i + 1}.png";
            }
        }
    }
}

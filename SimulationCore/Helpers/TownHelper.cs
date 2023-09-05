using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using SimulationCore.ExtensionMethods;

namespace SimulationCore.Helpers;

public static class TownHelper
{

    public static List<ConvexShape> GetTownSequencePath(List<int> townSequence)
    {
        var paths = new List<ConvexShape>();

        for (int i = 1; i < townSequence.Count; i++)
        {
            // Get the two towns that our line will be joining
            var fromTown = TownPositions[townSequence[i - 1]];
            var toTown = TownPositions[townSequence[i]];

            // Get the normalized vector in the direction of fromTown to toTown
            var directionVector = (toTown - fromTown).Normalize();

            // Now that we have the vector pointing from fromTown to toTown, we can traverse it to give our towns
            // some space around them when we draw our line.
            var startingPoint = fromTown + (directionVector * Configuration.PathOffsetFromTown);
            var endingPoint = toTown - (directionVector * Configuration.PathOffsetFromTown);

            // We want to fade the lines from black - grey to show the direction of the path
            var lumination = Convert.ToByte((200.0 / TownPositions.Count) * (i - 1));

            // Convert the points we have into a 'ConvexShape' :( damn SFML.
            paths.Add(SFMLGraphicsHelper.GetLine(startingPoint, endingPoint, Configuration.Linethickness, new Color(lumination, lumination, lumination)));
        }

        return paths;
    }

    private static int townId { get; set; }

    public static int TownId
    {
        get
        {
            return townId++;
        }
    }

    public static void PopulateTowns()
    {
        if (Configuration.UseRandomTowns)
        {
            for (int i = 0; i < Configuration.TownCount; i++)
            {
                TownPositions.Add(GeneratRandomTownPosition());
            }
        }
        else
        {
            TownPositions.AddRange(townPositions);
        }
    }

    private static Vector2f GeneratRandomTownPosition()
    {
        return new Vector2f
        {
            X = 100 + ((float)Application.random.NextDouble() * (Configuration.Width - 100)),
            Y = 100 + ((float)Application.random.NextDouble() * (Configuration.Height - 100))
        };
    }


    public static List<Vector2f> TownPositions = new List<Vector2f>();

    public static List<Vector2f> townPositions = new List<Vector2f>()
    {
        new Vector2f(3060 * Configuration.Scale, 1300 * Configuration.Scale), 
        new Vector2f(1050 * Configuration.Scale, 450 * Configuration.Scale),
        new Vector2f(450 * Configuration.Scale, 750 * Configuration.Scale),
        new Vector2f(690 * Configuration.Scale, 1890 * Configuration.Scale),
        new Vector2f(1410 * Configuration.Scale, 1830 * Configuration.Scale),
        new Vector2f(2070 * Configuration.Scale, 1560 * Configuration.Scale),
        new Vector2f(1725 * Configuration.Scale, 1080 * Configuration.Scale),
        new Vector2f(3360 * Configuration.Scale, 810 * Configuration.Scale),
        new Vector2f(3450 * Configuration.Scale, 1770 * Configuration.Scale),
        new Vector2f(2460 * Configuration.Scale, 240 * Configuration.Scale),
    };
}

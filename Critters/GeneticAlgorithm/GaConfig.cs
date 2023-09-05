using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Critters.GeneticAlgorithm;

public static class GaConfig
{
    public static int PopulationSize => 500;

    public static int FoodCount => 50;

    public static int ObstacleCount => 10;

    public static int MaxGenerations { get; set; } = 40;

    public static double MutationRate => 0.05;

}

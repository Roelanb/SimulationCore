using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Critters.GeneticAlgorithm;

public static class GaConfig
{
    public static int CreaturesCount => 30;

    public static int FoodCount => 25;

    public static int ObstacleCount => 0;

    public static int MaxGenerations { get; set; } = 40;

    public static double MutationRate => 0.05;
                                  
    public static int FoodRange => 60;

}

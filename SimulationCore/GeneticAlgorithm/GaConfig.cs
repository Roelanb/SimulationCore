using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationCore.GeneticAlgorithm
{
    public static class GaConfig
    {
        public static int PopulationCount => 100;

        public static int MaxGenerations { get; set; } = 40;

        public static double MutationRate => 0.05;

    }
}

using SimulationCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulationCore;


/// <summary>
/// Represents a neighbour solution in a genetic algorithm simulation.
/// </summary>
public class Individual
{
    private static Random random = new Random();

    /// <summary>
    /// Gets or sets the sequence of towns representing this neighbour's solution.
    /// </summary>
    public List<int> Sequence { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Individual"/> class with the specified sequence of towns.
    /// </summary>
    /// <param name="sequence">The sequence of towns representing this neighbour's solution.</param>
    public Individual(List<int> sequence)
    {
        this.Sequence = sequence;
    }

    /// <summary>
    /// Calculates and returns the fitness score of this neighbour solution.
    /// </summary>
    /// <returns>The fitness score of this neighbour solution.</returns>
    public double GetFitness()
    {
        var totalDistance = 0.0;

        for (int i = 0; i < Sequence.Count - 1; i++)
        {
            var from = TownHelper.TownPositions[Sequence[i]];
            var to = TownHelper.TownPositions[Sequence[i + 1]];

            var x = from.X - to.X;
            var y = from.Y - to.Y;

            totalDistance += x * x + y * y;

        }

        return totalDistance;
    }
}

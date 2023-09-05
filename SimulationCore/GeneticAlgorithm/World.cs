using SimulationCore.ExtensionMethods;
using SimulationCore.GeneticAlgorithm;
using SimulationCore.SFML_Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulationCore;

public class World
{
    private static List<double> cumulativeProportions = new List<double>();

    public List<Individual> Population { get; set; }
    public int GenerationCount { get; private set; } = 0;

    public List<double> FitnessOverTime { get; private set; } = new List<double>();

    public World()
    {
        Population = new List<Individual>();
        cumulativeProportions = new List<double>();
    }

    public void Spawn()
    {
        // Generate {PopulationCount} neighbours
        for (int i = 0; i < GaConfig.PopulationCount; i++)
        {
            this.Population.Add(GenerateNeighbour());
        }
    }

    private Individual GenerateNeighbour()
    {
        // Generate a list of numbers [0, 1, 2, 3... 9]
        var townCount = Configuration.UseRandomTowns ? Configuration.TownCount : 10;
        var sequence = Enumerable.Range(0, townCount).ToList();

        // Randomly shuffle the list [3, 1, 5, 9... 4]
        sequence.Shuffle();

        // Create a new neighbour with our random sequence
        return new Individual(sequence);
    }

    public void DoGeneration()
    {
        GenerationCount++; 

        UpdateCumulativeProportions();

        var offspring = new List<Individual>();

        while(offspring.Count < GaConfig.PopulationCount)
        {
       
            var parent1 = GetParent();
            var parent2 = GetParent();

            while (parent1 == parent2)
            {
                parent2 = GetParent();
            }

            var (child1, child2) = GetOffspring(parent1, parent2);

            // Mutate
            (parent1, parent2) = Mutate(parent1, parent2);


            offspring.Add(child1);
            offspring.Add(child2);

        }

        Population.AddRange(offspring);

        // get the best {PopulationCount} neighbours
        Population = Population.OrderBy(x => x.GetFitness()).Take(GaConfig.PopulationCount).ToList();

        var bestFitness = Population.First().GetFitness();

        FitnessOverTime.Add(bestFitness);   

    }

    private (Individual parent1, Individual parent2) Mutate(Individual parent1, Individual parent2)
    {
        var newParent1 = new Individual(parent1.Sequence);
        var newParent2 = new Individual(parent2.Sequence);

        if (Application.random.NextDouble() < GaConfig.MutationRate)
        {
            newParent1=Mutate(parent1);
        }

        if (Application.random.NextDouble() < GaConfig.MutationRate)
        {
            newParent2 = Mutate(parent2);
        }

        return (newParent1, newParent2);
    }

    private Individual Mutate(Individual parent)
    {
        if (Application.random.NextDouble() < 0.5)
        {
            // swap mutation
            return SwapMutation(parent);
        }
        else
        {
            // rotate mutation
            return RotateMutate(parent);
        }
     
    }

    private Individual RotateMutate(Individual individual)
    {
        var sequence = individual.Sequence.ToList();

        var index1 = Application.random.Next(0, sequence.Count);
        var index2 = Application.random.Next(0, sequence.Count);

        while (index1 == index2)
        {
            index2 = Application.random.Next(0, sequence.Count);
        }

        var firstIndex = Math.Min(index1, index2);
        var secondIndex = Math.Max(index1, index2);

        var newSequence = sequence.Take(firstIndex).ToList();
        var middleSequence = sequence.Skip(firstIndex).Take(secondIndex - firstIndex).Reverse().ToList();
        var lastSequence = sequence.Skip(secondIndex).ToList();

        newSequence.AddRange(middleSequence);
        newSequence.AddRange(lastSequence);


        return new Individual(newSequence);
    }

    private Individual SwapMutation(Individual parent)
    {
        var sequence = parent.Sequence.ToList();

        var index1 = Application.random.Next(0, sequence.Count);
        var index2 = Application.random.Next(0, sequence.Count);

        while (index1 == index2)
        {
            index2 = Application.random.Next(0, sequence.Count);
        }

        sequence.SwapInPlace(index1, index2);

        return new Individual(sequence);
    }

    private (Individual, Individual) GetOffspring(Individual individualA, Individual individualB)
    {
        var offspringA = DoCrossover(individualA, individualB);
        var offspringB = DoCrossover(individualB, individualA);

        return (offspringA, offspringB);
    }

    private Individual DoCrossover(Individual individualA, Individual individualB)
    {
        var crossoverPosition = Application.random.Next(1, individualA.Sequence.Count-1);

        var offspringSequence = individualA.Sequence.Take(crossoverPosition).ToList();

        var appeared = offspringSequence.ToHashSet();

        foreach (var gene in individualB.Sequence)
        {
            if (!appeared.Contains(gene))
            {
                offspringSequence.Add(gene);
            }
        }

        return new Individual(offspringSequence);
    }

    private (Individual,Individual) Crossover(Individual parent1, Individual parent2)
    {
        return (parent1, parent2);
    }

    private Individual GetParent()
    {
        if (Application.random.NextDouble() > 0.5)
        {
            // Tournament selection
            return TournamentSelection();
        }
        else
        {
            // Biased selection
            return BiasedSelection();
        }


    }

    private Individual TournamentSelection()
    {
        var candidate1 = Population[Application.random.Next(0, Population.Count)];
        var candidate2 = Population[Application.random.Next(0, Population.Count)];

        while (candidate1 == candidate2)
        {
            candidate2 = Population[Application.random.Next(0, Population.Count)];
        }

        if (candidate1.GetFitness() < candidate2.GetFitness())
        {
            return candidate1;
        }
        else
        {
            return candidate2;
        }
    }

    private Individual BiasedSelection()
    {


        var selectedValue = Application.random.NextDouble();

        for (int i = 0; i < cumulativeProportions.Count; i++)
        {
            var value = cumulativeProportions[i];

            if (value >= selectedValue)
            {
                // Return the neighbour that is at this index.
                return Population[i];
            }
        }

        throw new Exception("This should never happen");
    }

    public Individual GetBestNeighbour()
    {
        return Population.OrderBy(n => n.GetFitness()).FirstOrDefault();
        
    }
    public void UpdateCumulativeProportions()
    {
        var sum = Population.Sum(n => n.GetFitness());
        var proportions = Population.Select(n => sum / n.GetFitness());
        var proportionSum = proportions.Sum();

        var normalizedProportions = proportions.Select(p => p / proportionSum);

        var cumulativeTotal = 0.0;

        foreach (var proportion in normalizedProportions)
        {
            cumulativeTotal += proportion;
            cumulativeProportions.Add(cumulativeTotal);
        }
    }
}

using Critters.Object;
using MethodTimer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Critters.GeneticAlgorithm;

public class Algo
{
    public int Generation;
    public double ElitismChance;
    public double CrossOverChance;
    public double MutationChance;
    public double AverageFitness;
    public double HighestFitness;

    public List<Creature> NextGeneration;

    private World _world;
    public Algo(World world, int crossOverChance, int mutationChance)
    {
        _world = world;

        CrossOverChance = crossOverChance;
        ElitismChance = 100 - CrossOverChance;
        MutationChance = mutationChance;
    }

    [Time]
    public double Evolve(List<Creature> creatures, World world)
    {
        NextGeneration = new List<Creature>();

        CalculateFitness(creatures);
        Elitism(creatures);
        CrossOver(creatures, world);
        Mutation();
        CopyCreatures(creatures);

        Generation++;

        NextGeneration.Clear();

        return AverageFitness;
    }


    [Time]
    public void CalculateFitness(List<Creature> creature)
    {
        HighestFitness = 0;
        AverageFitness = 0;
        foreach (Creature c in creature)
        {
            AverageFitness += c.Fitness;
            if (c.Fitness > HighestFitness)
                HighestFitness = c.Fitness;
        }
        AverageFitness /= creature.Count();

        foreach (Creature c in creature)
        {
            if (HighestFitness == 0)
            {
                c.ParentChance = 100;
            }
            else
            {
                c.ParentChance = (c.Fitness / HighestFitness) * 100;
            }
        }
    }

    [Time]
    public void Elitism(List<Creature> creature)
    {
        creature = creature.OrderByDescending(Creature => Creature.Fitness).ToList();
        int NrOfElites = (int)(creature.Count() * (double)(ElitismChance / 100));
        for (int i = 0; i < NrOfElites; i++)
            NextGeneration.Add(creature[i]);
    }

 
    public Creature Selection()
    {
        NextGeneration = NextGeneration.OrderBy(Creature => Guid.NewGuid()).ToList();
        Random random = new Random(Guid.NewGuid().GetHashCode());
        int ParentTreshold = random.Next(0, 100);
        foreach (Creature c in NextGeneration)
        {
            if (c.ParentChance > ParentTreshold)
            {
                return c;
            }
        }
        return null;
    }

    [Time]
    public void CrossOver(List<Creature> creature, World world)
    {
        int NrOfCrossOver = (int)(creature.Count() * (double)(CrossOverChance / 100));
        for (int j = 0; j < NrOfCrossOver; j++)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            Creature ParentA = Selection();
            Creature ParentB = Selection();

            double[] ParentAWeights = ParentA.Brain.GetWeights();
            double[] ParentBWeights = ParentB.Brain.GetWeights();

            double[] ChildWeights = new double[ParentAWeights.Length];

            int CrossOverPoint = random.Next(0, ParentAWeights.Length);

            for (int i = 0; i < CrossOverPoint; i++)
            {
                ChildWeights[i] = ParentAWeights[i];
            }
            for (int i = CrossOverPoint; i < ParentAWeights.Length; i++)
            {
                ChildWeights[i] = ParentBWeights[i];
            }
            Creature Child = new Creature(_world);
            Child.Brain.SetWeights(ChildWeights);
            NextGeneration.Add(Child);
        }
    }

    [Time]
    public void Mutation()
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());
        foreach (Creature c in NextGeneration)
        {
            if (random.Next(0, 100) < MutationChance)
            {
                int MutationPoint = random.Next(0, c.Brain.GetNrOfDendrites());
                double[] Weights = c.Brain.GetWeights();
                Weights[MutationPoint] = random.NextDouble();
                c.Brain.SetWeights(Weights);
            }
        }
    }

    [Time]
    public void CopyCreatures(List<Creature> creature)
    {
        for (int i = 0; i < creature.Count(); i++)
        {
            creature[i] = NextGeneration[i];
            creature[i].Reset();
        }
    }
}

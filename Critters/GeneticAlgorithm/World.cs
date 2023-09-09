
using Critters.GeneticAlgorithm;
using Critters.GraphHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Critters.Object;
using SFML.Audio;
using SFML.System;
using System.ComponentModel;

namespace Critters;

public class World
{
    public Algo GA { get; set; }
    private static List<double> cumulativeProportions = new List<double>();

    public List<Creature> Creatures { get; set; }
    public List<Food> Foods { get; set; }
    public List<Obstacle> Obstacles { get; set; }

    public Creature SelectedCreature { get; set; }

    public int CreatureCount => Creatures.Count;

    public int GenerationCount { get; private set; } = 0;
    public int Ticks;
    public int Evolutions;
    public int NrOfDeaths;
    private double[] GraphValue;

    public List<double> FitnessOverTime { get; private set; } = new List<double>();

    public World()
    {
        Creatures = new List<Creature>();
        Foods = new List<Food>();
        Obstacles = new List<Obstacle>();

        cumulativeProportions = new List<double>();
        
        GA = new Algo(this, 60, 1);
        NrOfDeaths = 0;
        Ticks = 0;
        Evolutions = 0;
        GraphValue = new double[32000];

        InitWorld();

    }

    public void InitWorld()
    {
        for (int i = 0; i < GaConfig.CreaturesCount; i++)
        {
            Creatures.Add(new Creature(this));
        }

        for (int i = 0; i < GaConfig.FoodCount; i++)
        {
            Foods.Add(new Food(this));
        }

        for (int i = 0; i < GaConfig.ObstacleCount; i++)
        {
            Obstacles.Add(new Obstacle(this));
        }
    }

    public void Update()
    {
        // reset the nrof deaths
        NrOfDeaths = 0;

        var parents = new List<Creature>();

        foreach (Creature creature in Creatures)
        {
            // update the creature with the active foods
            creature.Update(Foods.Where(p => p.Active).ToList(), Obstacles);

            if (creature.Life <= 0)
                NrOfDeaths++;


            // reproduce
            if (creature.Fitness > 80)
            {

                parents.Add(creature);
            }
        }

        foreach (Creature creature in parents)
        {
            // reset the parents fitness

            creature.Fitness = 0;
            Creatures.Add(creature.Reproduce());
        }


        foreach (Food food in Foods)
        {
            food.Update();
        }

        Ticks++;
        //if (Ticks == 500 || NrOfDeaths == Creatures.Count())
        //{
        //    Ticks = 0;
        //    Evolutions++;
        //    GraphValue[GA.Generation] = GA.Evolve(Creatures, this);
        //}

    }

    public BaseItem? Hit(Vector2i checkPosition)
    {
        foreach (var bi in Creatures)
        {
            if (bi.Hit(checkPosition) != null) return bi;
        }

        foreach (var bi in Foods)
        {
            if (bi.Hit(checkPosition) != null) return bi;
        }
        foreach (var bi in Obstacles)
        {
            if (bi.Hit(checkPosition) != null) return bi;
        }



        return null;
    }

    public void Spawn()
    {

    }


    public void DoGeneration()
    {


    }

}

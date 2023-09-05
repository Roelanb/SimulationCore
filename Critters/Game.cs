using Critters.GeneticAlgorithm;
using Critters.Screens;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Critters;

public class Game
{

    private readonly RenderWindow window;

    private readonly Clock clock;
    private readonly Clock generationClock;
    private readonly float generationTime = 0.2f;
    private readonly SimulationScreen screen;

    public World world { get; }

    private int generation = 0;

    public Game()
    {
        window = new RenderWindow(new VideoMode(Configuration.Width, Configuration.Height), "Simulation");
        window.SetFramerateLimit(60);

        window.Closed += OnClose;
        window.Resized += OnResize;

        // Create our world
        world = new World();

        // Create a simulation screen. Note that screens can be stacked on top of one another
        // in the screen manager that has been omitted from this specific repository.
        screen = new SimulationScreen(window, world, Configuration.SinglePlayer);


        // Clock to track frame time. If we dont do this, and instead assume we are hitting 60fps, we can get stutters 
        // in our camera movement.
        clock = new Clock();

        // Create a new generation clock to only DoGeneration once per second.
        generationClock = new Clock();
    }

    public void Run()
    {
        world.Spawn();

        generationClock.Restart();

        while (window.IsOpen)
        {
            // Get the amount of time that has passed since we drew the last frame.
            float deltaT = clock.Restart().AsMicroseconds() / 1000000f;

            // Clear the previous frame
            window.Clear(Configuration.Background);

            // Process events
            window.DispatchEvents();

            // Draw the paths of the current best individual
            screen.UpdateSequence();

            // Update all the screen components that are unrelated to our sequence.
            screen.Update(deltaT);

            screen.Draw();

            // Update the window
            window.Display();

            // If one second has passed, breed the next generation
            if (generationClock.ElapsedTime.AsSeconds() > generationTime && world.GenerationCount <= GaConfig.MaxGenerations)
            {
                // Do one generation of breeding & culling.
                world.DoGeneration();

                // Restart our generation timer
                generationClock.Restart();
            }

            if (world.GenerationCount == GaConfig.MaxGenerations)
            {
                screen.SetGaCompleted();

                // copy the fitness to clipboard
              //  Clipboard.SetText(string.Join<double>(",",world.FitnessOverTime));
            }

           


            // Check to see if the user has pressed the quit key.
            if (Keyboard.IsKeyPressed(Configuration.QuitKey) || Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                return;
            }
        }
    }

    private void OnResize(object sender, SizeEventArgs e)
    {
        var window = (RenderWindow)sender;
        screen.Camera.ScaleToWindow(window.Size.X, window.Size.Y);
    }

    private void OnClose(object sender, EventArgs e)
    {
        // Close the window when OnClose event is received
        RenderWindow window = (RenderWindow)sender;
        window.Close();
    }
}

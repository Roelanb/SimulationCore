using SimulationCore.Factories;
using SimulationCore.Helpers;
using SimulationCore.SFML_Text;
using SFML.Graphics;
using SFML.Window;
using SimulationCore.Screens;
using System.Collections.Generic;
using System.IO;
using SFML.System;
using SimulationCore.ExtensionMethods;
using SimulationCore.ViewTools;

namespace SimulationCore.Screens
{
    public class SimulationScreen : Screen
    {
        private FontText TotalDistance;
        private FontText QuitString;

        public FontText GenerationString { get; private set; }

        private List<RectangleShape> townVisuals;

        private RenderTexture texture;

        private int frame = 0;

        public SimulationScreen(
            RenderWindow window,
            FloatRect configuration)
            : base(window, configuration)
        {
            this.townVisuals = new List<RectangleShape>();
            this.pathLines = new List<ConvexShape>();

            // Populate the towns, either hard coded or randomly. This is congifurable in the Configuration.cs.
            TownHelper.PopulateTowns();

            // Grab the images that are used to represent our towns and insert them into quads so they can be shown.
            // For now our town positions are hard coded, but theres no reason we cant expand this in future to randomly place them.
            foreach (var town in TownFactory.GetTowns())
            {
                this.townVisuals.Add(town.Shape);
            }
            // Create a camera instance to handle the changing of window sizes
            Camera = new Camera(Configuration.SinglePlayer);

           


            TotalDistance = new FontText(new Font(@"Resources/font.ttf"), "test", Color.Black, 1);
            GenerationString = new FontText(new Font(@"Resources/font.ttf"), $"Generation: {0}", Color.Black, 1);
            QuitString = new FontText(new Font(@"Resources/font.ttf"), "Press 'Q' to quit.", Color.Black, 1);

        }

        List<ConvexShape> pathLines;

        public void UpdateSequence(Individual neighbour)
        {
            // Convert our sequence of ints to the 2D line representations to be drawn on the screen.
            pathLines.Clear();
            pathLines.AddRange(TownHelper.GetTownSequencePath(neighbour.Sequence));

            // Convert the fitness into a format that is easily digestable and update the value on screen
            // Format: 1234.56
            TotalDistance.StringText = neighbour.GetFitness().ToString("#.##");

        }

        /// <summary>
        /// Update - Update each of the components that are time dependent.
        /// </summary>
        /// <param name="deltaT"></param>
        public override void Update(float deltaT)
        {
            base.Update(deltaT);

            // Checks user input and modifies the cameras position / rotation.
            Camera.Update(deltaT);
        }

        /// <summary>
        /// Draw - Here we don't update any of the components, only draw them in their current state to the screen.
        /// </summary>
        public void Draw()
        {
            // Clear the previous frame off the screen
            window.Clear(Configuration.Background);

            // Update the current view based off the cameras location/rotation
            window.SetView(Camera.GetView());

            // Draw each of the line segment for the path we are currently displaying
            foreach (var pathLine in pathLines)
            {
                window.Draw(pathLine);
            }

            // Draw all of our towns
            foreach (var town in townVisuals)
            {
                window.Draw(town);
            }

            // Draw the updated distance to the screen
            window.DrawString(TotalDistance, new Vector2f(Configuration.Width / 2, 50));

            // Display the current generation
            window.DrawString(GenerationString, new Vector2f(50 * Configuration.Scale, 50));

            // Draw the 'Press Q to quit' message
            window.DrawString(QuitString, new Vector2f(450 * Configuration.Scale, (Configuration.Height - 100) * Configuration.Scale));
        }

        public void SetGaCompleted()
        {
            TotalDistance.TextColour = Color.Green;
        }

    }
}

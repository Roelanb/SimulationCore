
using Critters.SFML_Text;
using SFML.Graphics;
using SFML.Window;
using Critters.Screens;
using System.Collections.Generic;
using System.IO;
using SFML.System;

using Critters.ViewTools;
using Critters.ExtensionMethods;
using Critters.Object;

namespace Critters.Screens
{
    public class SimulationScreen : Screen
    {
        private FontText NrOfDeathString;
        private FontText TickCount;

        private FontText MousePosition;

        public World World { get; set; }

        private RenderTexture texture;

        private int frame = 0;

        public SimulationScreen(
            RenderWindow window,
            World world,
            FloatRect configuration)
            : base(window, configuration)
        {
            World = world;
            // Create a camera instance to handle the changing of window sizes
            Camera = new Camera(Configuration.SinglePlayer);

           


            NrOfDeathString = new FontText( "test", Color.Black, 0.7f);
            TickCount = new FontText("P", Color.Black, 0.7f);

            MousePosition = new FontText($"", Color.Black, 0.6f);
        }

        List<ConvexShape> pathLines;

        public void UpdateSequence()
        {
           

        }

        /// <summary>
        /// Update - Update each of the components that are time dependent.
        /// </summary>
        /// <param name="deltaT"></param>
        public override void Update(float deltaT)
        {
            base.Update(deltaT);

            World.Update();

            // Checks user input and modifies the cameras position / rotation.
            Camera.Update(deltaT);

            // Format: 1234.56
            NrOfDeathString.StringText = $"Nr of deaths: {World.NrOfDeaths}";

            TickCount.StringText = $"Ticks: {World.Ticks} Evolutions: {World.Evolutions}";

            // check for mouse overlap
            var bi = World.Hit(Mouse.GetPosition(window));

            if (bi!=null)
            {
                MousePosition.StringText = ((BaseItem)bi).ToString();
            }
            else
            {
                MousePosition.StringText = "";
            }


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

            foreach (var creature in World.Creatures)
            {
                creature.Draw(window);
            }   

            // draw all the foods
            foreach (var food in World.Foods)
            {
                food.Draw(window);
            }

            // draw all the obstacles
            foreach (var obstacle in World.Obstacles)
            {
                obstacle.Draw(window);
            }


            // Draw the updated distance to the screen
            window.DrawString(NrOfDeathString, new Vector2f(Configuration.Width / 2, 25));
            window.DrawString(TickCount, new Vector2f(Configuration.Width / 4, 20));


            // get the mouse position
            var mousePos = Mouse.GetPosition(window);

            // draw the position x and y on the screen
            window.DrawString(MousePosition, new Vector2f(mousePos.X, mousePos.Y));
        }

        public void SetGaCompleted()
        {
            //TotalDistance.TextColour = Color.Green;
        }

    }
}

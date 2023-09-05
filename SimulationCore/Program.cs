global using SFML.Graphics;
global using SFML.Window;
using SimulationCore;


public static class Application
{
    public static Random random = new Random();
    public static void Main()
    {
        new Game().Run();
    }

}



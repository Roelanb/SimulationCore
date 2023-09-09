using SFML.Graphics;

namespace Critters.GraphHelpers;


public class FontText
{
    public FontText(string stringText, Color textColour, float scale = 1)
    {
        var fontBytes = File.ReadAllBytes(@"F:\Projects\GA\SimulationCore\Critters\Resources\font.ttf");

        this.Font = new Font( fontBytes);
        this.StringText = stringText;
        this.TextColour = textColour;
        this.Scale = scale;
    }

    public Font Font;

    public string StringText;

    public Color TextColour;

    public float Scale;
}

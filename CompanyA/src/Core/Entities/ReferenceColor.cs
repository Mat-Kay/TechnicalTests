namespace Core.Entities
{
    using System.Drawing;

    public class ReferenceColor
    {
        public ReferenceColor(string name, Color color)
        {
            Name = name;
            Color = color;
        }

        public string Name { get; }

        public Color Color { get; }
    }
}

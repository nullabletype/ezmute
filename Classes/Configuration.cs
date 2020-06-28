namespace ezmute.Classes
{
    public class Configuration
    {
        public bool ShowToolTipsOnChange { get; set; }
        public bool AlwaysOntop { get; set; }
        public XY StartingPosition { get; set; } = new XY();
        public XY StartingSize { get; set; } = new XY();
        public KeyBind Key { get; set; } = new KeyBind();
        public KeyBind ModifierKey { get; set; } = new KeyBind();
        public string BackgroundColour { get; set; }
        public string MutedColour { get; set; }
        public string UnmutedColour { get; set; }
        public int FontSize { get; set; }
    }

    public class XY
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class KeyBind
    {
        public string Key { get; set; }
    }
}

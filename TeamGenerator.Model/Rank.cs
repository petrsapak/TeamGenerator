namespace TeamGenerator.Model
{
    public class Rank
    {
        public string Name { get; private set; }
        public double Value { get; private set; }

        public Rank(string name, double value)
        {
            Name = name;
            Value = value;
        }
    }
}

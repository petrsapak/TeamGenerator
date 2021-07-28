using TeamGenerator.Model.Interfaces;

namespace TeamGenerator.Model
{
    public class Rank : IRank
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

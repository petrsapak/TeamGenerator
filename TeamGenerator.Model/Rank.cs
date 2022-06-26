using TeamGenerator.Model.Validators;

namespace TeamGenerator.Model
{
    public class Rank
    {
        public string Name { get; }
        public double Value { get; }

        public Rank(string name, double value)
        {
            Validator.ValidateString(name);
            Name = name;
            Validator.ValidateDouble(value);
            Value = value;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

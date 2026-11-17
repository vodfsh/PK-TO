namespace Lab3.Sim;

public interface IVector
{
    double Abs();
    double[] GetComponents();

    string ToCartesianStr()
    {
        var components = GetComponents();
        return $"({components[0]}, {components[1]})";
    }
}

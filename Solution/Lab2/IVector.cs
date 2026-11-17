namespace Lab2;

public interface IVector
{
    public double Abs();

    public double CDot(IVector other)
    {
        var c1 = GetComponents();
        var c2 = other.GetComponents();

        var x1 = c1[0];
        var y1 = c1[1];
        var z1 = c1.Length > 2 ? c1[2] : 0.0;

        var x2 = c2[0];
        var y2 = c2[1];
        var z2 = c2.Length > 2 ? c2[2] : 0.0;

        return x1 * x2 + y1 * y2 + z1 * z2;
    }

    public double[] GetComponents();
    public IVector Cross(IVector other);

    public string ToCartesianStr()
    {
        var comp = GetComponents();

        return comp.Length > 2
            ? $"({comp[0]}, {comp[1]}, {comp[2]})"
            : $"({comp[0]}, {comp[1]})";
    }
}
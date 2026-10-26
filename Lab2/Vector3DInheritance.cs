namespace Lab2;

public class Vector3DInheritance : Vector2D
{
    private readonly double z;

    public Vector3DInheritance(double x, double y, double z) : base(x, y)
    {
        this.z = z;
    }

    public override double Abs()
    {
        var c = GetComponents();
        var x = c[0];
        var y = c[1];
        return Math.Sqrt(x * x + y * y + z * z);
    }

    public override double[] GetComponents()
    {
        var c = base.GetComponents();
        var x = c[0];
        var y = c[1];
        return new[] { x, y, z };
    }

    public override IVector Cross(IVector other)
    {
        var c = GetComponents();
        var x = c[0];
        var y = c[1];

        var c2 = other.GetComponents();
        var x2 = c2[0];
        var y2 = c2[1];
        var z2 = c2.Length > 2 ? c2[2] : 0.0;

        var cx = y * z2 - z * y2;
        var cy = z * x2 - x * z2;
        var cz = x * y2 - y * x2;

        return new Vector3DInheritance(cx, cy, cz);
    }

    public override string ToString()
    {
        var comp = GetComponents();
        return $"({comp[0]}, {comp[1]}, {comp[2]})";
    }

    public IVector GetSrcV()
    {
        return this;
    }
}
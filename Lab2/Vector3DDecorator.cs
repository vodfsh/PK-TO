namespace Lab2;

public class Vector3DDecorator : IVector
{
    private readonly IVector srcVector;
    private readonly double z;

    public Vector3DDecorator(IVector src, double z)
    {
        srcVector = src;
        this.z = z;
    }

    public double Abs()
    {
        var baseAbs = srcVector.Abs();
        return Math.Sqrt(baseAbs * baseAbs + z * z);
    }

    public double CDot(IVector other)
    {
        return ((IVector)this).CDot(other);
    }

    public double[] GetComponents()
    {
        var baseComp = srcVector.GetComponents();
        return new[] { baseComp[0], baseComp[1], z };
    }

    public IVector Cross(IVector other)
    {
        var c1 = GetComponents();
        var c2 = other.GetComponents();
        var x2 = c2[0];
        var y2 = c2[1];
        var z2 = c2.Length > 2 ? c2[2] : 0.0;

        var cx = c1[1] * z2 - c1[2] * y2;
        var cy = c1[2] * x2 - c1[0] * z2;
        var cz = c1[0] * y2 - c1[1] * x2;

        return new Vector3DDecorator(new Vector2D(cx, cy), cz);
    }

    public IVector GetSrcV()
    {
        return srcVector;
    }

    public override string ToString()
    {
        var comp = GetComponents();
        return $"({comp[0]}, {comp[1]}, {comp[2]})";
    }
}
namespace Lab2;

public class Vector2D : IVector
{
    private readonly double x;
    private readonly double y;

    public Vector2D(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public double CDot(IVector other)
    {
        return ((IVector)this).CDot(other);
    }

    public virtual double[] GetComponents()
    {
        return new[] { x, y };
    }

    public virtual double Abs()
    {
        return Math.Sqrt(x * x + y * y);
    }

    public virtual IVector Cross(IVector other)
    {
        var c2 = other.GetComponents();
        var x2 = c2[0];
        var y2 = c2[1];
        var z2 = c2.Length > 2 ? c2[2] : 0.0;

        var cx = y * z2;
        var cy = -x * z2;
        var cz = x * y2 - y * x2;

        return new Vector3DDecorator(new Vector2D(cx, cy), cz);
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }
}
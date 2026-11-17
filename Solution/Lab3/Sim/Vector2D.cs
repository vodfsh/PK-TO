namespace Lab3.Sim;

public class Vector2D : IVector
{
    public Vector2D() : this(0, 0)
    {
    }

    public Vector2D(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; set; }
    public double Y { get; set; }

    public virtual double Abs()
    {
        return Math.Sqrt(X * X + Y * Y);
    }

    public virtual double[] GetComponents()
    {
        return new[] { X, Y };
    }

    public double DistanceSquared(Vector2D other)
    {
        var dx = X - other.X;
        var dy = Y - other.Y;
        return dx * dx + dy * dy;
    }

    public static Vector2D operator +(Vector2D a, Vector2D b)
    {
        return new Vector2D(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2D operator -(Vector2D a, Vector2D b)
    {
        return new Vector2D(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2D operator *(Vector2D vector, double scalar)
    {
        return new Vector2D(vector.X * scalar, vector.Y * scalar);
    }

    public static Vector2D operator *(double scalar, Vector2D vector)
    {
        return new Vector2D(vector.X * scalar, vector.Y * scalar);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}
namespace Lab2;

public class Vector2D:IVector
{
    private double x;
    private double y;

    public Vector2D(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }
    
    public double Abs()
    {
        throw new NotImplementedException();
    }

    public double CDot(IVector other)
    {
        throw new NotImplementedException();
    }

    public double[] GetComponents()
    {
        throw new NotImplementedException();
    }

    public IVector Cross(IVector other)
    {
        throw new NotImplementedException();
    }
}
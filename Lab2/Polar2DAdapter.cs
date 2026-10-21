namespace Lab2;

public class Polar2DAdapter: IPolar2D, IVector
{
    private Vector2D srcVector;
    public double GetAngle()
    {
        throw new NotImplementedException();
    }

    double IPolar2D.Abs()
    {
        return srcVector.Abs();
    }

    public double CDot(IVector other)
    {
        return srcVector.CDot(other);
    }

    public double[] GetComponents()
    {
        return srcVector.GetComponents();
    }

    public IVector Cross(IVector other)
    {
        throw new NotImplementedException();
    }

    double IVector.Abs()
    {
        return srcVector.Abs();
    }
}
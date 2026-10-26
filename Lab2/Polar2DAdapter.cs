namespace Lab2;

public class Polar2DAdapter : IPolar2D, IVector
{
    private readonly Vector2D srcVector;

    public Polar2DAdapter(Vector2D srcVector)
    {
        this.srcVector = srcVector;
    }

    public double GetAngle()
    {
        var comp = srcVector.GetComponents();
        return Math.Atan2(comp[1], comp[0]);
    }

    public double Abs()
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
        return srcVector.Cross(other);
    }

    public override string ToString()
    {
        return $"(r = {Abs():F2},  {GetAngle():F2} rad, {GetAngle() * (180.0 / Math.PI):F2}*)";
    }
}
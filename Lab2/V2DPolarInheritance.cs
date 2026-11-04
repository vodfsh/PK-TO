namespace Lab2;

public class V2DPolarInheritance : Vector2D
{
    public V2DPolarInheritance(double x, double y) : base(x, y)
    {
    }

    public double GetAngle()
    {
        var c = GetComponents();
        var x = c[0];
        var y = c[1];
        return Math.Atan2(y, x);
    }

    public override string ToString()
    {
        return $"(r = {Abs():F2},  {GetAngle():F2} rad, {GetAngle() * (180.0 / Math.PI):F2}*)";
    }
}
namespace Lab3.Sim;

public class V2DPolarInheritance : Vector2D, IPolar2D
{
    public V2DPolarInheritance()
    {
    }

    public V2DPolarInheritance(double x, double y) : base(x, y)
    {
    }

    public double GetAngle()
    {
        return Math.Atan2(Y, X);
    }

    public double Angle
    {
        get => Math.Atan2(Y, X);
        set
        {
            var radius = Abs();
            if (radius < 1e-12)
            {
                X = Math.Cos(value);
                Y = Math.Sin(value);
                return;
            }

            X = radius * Math.Cos(value);
            Y = radius * Math.Sin(value);
        }
    }

    public double Radius
    {
        get => Abs();
        set
        {
            var angle = Angle;
            X = value * Math.Cos(angle);
            Y = value * Math.Sin(angle);
        }
    }

    public void Rotate(double delta)
    {
        Angle += delta;
    }

    public static V2DPolarInheritance FromPolar(double angle, double radius)
    {
        return new V2DPolarInheritance(radius * Math.Cos(angle), radius * Math.Sin(angle));
    }

    public override string ToString()
    {
        return FormattableString.Invariant(
            $"(r = {Abs():F2},  {Angle:F2} rad, {Angle * (180.0 / Math.PI):F2}*)");
    }
}
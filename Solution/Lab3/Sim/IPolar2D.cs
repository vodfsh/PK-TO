namespace Lab3.Sim;

public interface IPolar2D
{
    double GetAngle();
    double Abs();

    double Angle { get; set; }
    double Radius { get; set; }
    void Rotate(double delta);
}

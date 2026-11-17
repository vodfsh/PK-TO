namespace Lab3.Sim;

public interface IPolar2D
{
    double Angle { get; set; }
    double Radius { get; set; }
    double GetAngle();
    double Abs();
    void Rotate(double delta);
}
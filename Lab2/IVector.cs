namespace Lab2;

public interface IVector
{
    public double Abs();
    public double CDot(IVector other);
    public double[] GetComponents();
    public IVector Cross(IVector other);
}
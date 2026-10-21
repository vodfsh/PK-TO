namespace Lab2;

public class Vector3DDecorator:IVector
{
    private IVector srcVector;
    private double z;

    public Vector3DDecorator(IVector src, double z)
    {
        this.srcVector = src;
        this.z = z;
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
        throw new NotImplementedException();
    }

    public IVector GetSrcV()
    {
        return srcVector;
    }
}
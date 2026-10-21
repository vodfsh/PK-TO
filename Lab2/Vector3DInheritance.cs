namespace Lab2;

public class Vector3DInheritance:Vector2D
{
    private double z;

    public Vector3DInheritance(double x, double y, double z) : base(x,y)
    {
        this.z = z;
    }

    public Vector3DInheritance Cross(IVector other)
    {
        throw new NotImplementedException();
    }

    public IVector GetSrcV()
    {
        return this;
    }
    
}
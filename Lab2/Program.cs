namespace Lab2;

internal class Program
{
    private static void Main(string[] args)
    {
        var vectors = new IVector[]
        {
            new Vector2D(3.0, 4.0),
            new Vector3DDecorator(new Vector2D(1.0, 2.0), 2.0),
            new V2DPolarInheritance(5.0, 12.0)
        };

        Console.WriteLine("wektory:");
        for (var i = 0; i < vectors.Length; i++)
        {
            var comp = vectors[i].GetComponents();

            var polarAdapter = new Polar2DAdapter(new Vector2D(comp[0], comp[1]));

            Console.WriteLine($"Vector {i + 1}:");
            Console.WriteLine($"  Kartezjańskie: {vectors[i].ToCartesianStr()}");
            Console.WriteLine($"  Biegunowe: {polarAdapter}");
            Console.WriteLine($"  Długość: {vectors[i].Abs():F2}\n");
        }

        Console.WriteLine("Iloczyn skalarny");
        for (var i = 0; i < vectors.Length; i++)
        for (var j = 0; j < vectors.Length; j++)
        {
            var dotResult = vectors[i].CDot(vectors[j]);
            Console.WriteLine($"{vectors[i].ToCartesianStr()} · {vectors[j].ToCartesianStr()} = {dotResult}");
        }

        Console.WriteLine("\nIloczyn wektorowy");
        for (var i = 0; i < vectors.Length; i++)
        for (var j = 0; j < vectors.Length; j++)
        {
            var crossResult = vectors[i].Cross(vectors[j]);
            Console.WriteLine($"{vectors[i].ToCartesianStr()} x {vectors[j].ToCartesianStr()} = {crossResult}");
        }
    }
}
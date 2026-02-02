using Godot;

public static class VectorCalculator
{
    public static Vector3 Normalise(Vector3 vector)
    {
        return vector / vector.Length();
    }

    public static Vector3 Orthonomal1(Vector3 vector)
    {
        if (vector.X == 0 & vector.Y == 0)
        {
            return new(1, 0, 0);
        }
        else
        {
            return Normalise(new(vector.Y, -vector.X, 0));
        }
    }

    public static Vector3 Orthonomal2(Vector3 vector)
    {
        if (vector.X == 0 & vector.Y == 0)
        {
            return new(0, 1, 0);
        }
        else
        {
            return Normalise(new(vector.X * vector.Z, -vector.Y * vector.Z, -vector.X * vector.X - vector.Y * vector.Y));
        }
    }
}
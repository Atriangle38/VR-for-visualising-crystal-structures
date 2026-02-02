// A static class for vector calculations.

using Godot;

public static class VectorCalculator
{
    // Normalises the input vector to unit length:
    public static Vector3 Normalise(Vector3 vector)
    {
        return vector / vector.Length();
    }

    // Returns a unit vector orthogonal to the input vector:
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

    // Returns a unit vector orthogonal to both the input vector and the vector returned by the Orthonormal1 function of the inout vector:
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

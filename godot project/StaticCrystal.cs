using System.Collections.Generic;
using Godot;

public static class StaticCrystal
{
    public const float CYLINDER_WIDTH = 0.02f;

	public class Atom(Vector3 relativePos, Color colour)
	{
		public Vector3 relativePos = relativePos;
        public Color colour = colour;
    }

	public class Basis()
	{
        public List<MeshInstance3D> spheres = [];

        public Basis(Node parentNode, Vector3 latticeSite, List<Atom> basisAtoms): this()
        {
            for (int i = 0; i < basisAtoms.Count; i++)
            {
                spheres.Add((MeshInstance3D)GD.Load<PackedScene>("res://subscenes/sphere.tscn").Instantiate());
                parentNode.AddChild(spheres[i]);
                spheres[i].Position = latticeSite + basisAtoms[i].relativePos;
                StandardMaterial3D material = new()
                {
                    AlbedoColor = basisAtoms[i].colour
                };
                spheres[i].SetSurfaceOverrideMaterial(0, material);
            }
            StandardMaterial3D material2 = new()
            {
                AlbedoColor = new(0, 1, 1)
            };
        }
    }

    public static void CreateGridLine(Node parentNode, Vector3 pos, Vector3 vector)
    {
        MeshInstance3D cylinder = (MeshInstance3D)GD.Load<PackedScene>("res://subscenes/cylinder.tscn").Instantiate();
        parentNode.AddChild(cylinder);
        cylinder.Position = pos;
        cylinder.Basis = new( CYLINDER_WIDTH * VectorCalculator.Orthonomal1(vector), vector, CYLINDER_WIDTH * VectorCalculator.Orthonomal2(vector));
    }

    public static void CreateCrystal(Node parentNode, int size, Vector3 a1, Vector3 a2, Vector3 a3, List<Atom> basisAtoms)
    {
		for (int n1 = -size; n1 <= size; n1++)
		{
			for (int n2 = -size; n2 <= size; n2++)
			{
				for (int n3 = -size; n3 <= size; n3++)
				{
					new Basis(parentNode, n1 * a1 + n2 * a2 + n3 * a3, basisAtoms);
				}
			}
		}

        for (int n1 = -size; n1 <= size; n1++)
        {
            for (int n2 = -size; n2 <= size; n2++)
            {
                CreateGridLine(parentNode, n1 * a1 + n2 * a2, size * a3);
            }
        }
        for (int n2 = -size; n2 <= size; n2++)
        {
            for (int n3 = -size; n3 <= size; n3++)
            {
                CreateGridLine(parentNode, n2 * a2 + n3 * a3, size * a1);
            }
        }
        for (int n1 = -size; n1 <= size; n1++)
        {
            for (int n3 = -size; n3 <= size; n3++)
            {
                CreateGridLine(parentNode, n1 * a1 + n3 * a3, size * a2);
            }
        }
    }
}

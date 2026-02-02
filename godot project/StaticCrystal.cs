// A static class that contains most of this project's code that is not specific to a node.

using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

public static class StaticCrystal
{
    public const float CYLINDER_WIDTH = 0.02f; // The diameter of the grid lines in metres (with atoms having a thickness of 0.2 metres).

	public class Atom(Vector3 relativePos, Color colour)
	{
		public Vector3 relativePos = relativePos; // The relative position of atoms in each instance of the basis.
        public Color colour = colour; // The colours of atoms in each instance of the basis.
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
        cylinder.Basis = new(CYLINDER_WIDTH * VectorCalculator.Orthonomal1(vector), vector, CYLINDER_WIDTH * VectorCalculator.Orthonomal2(vector));
    }

    public static void CreateCrystal(Node parentNode, int size, Vector3 a1, Vector3 a2, Vector3 a3, List<Atom> basisAtoms)
    {
        // Places the atoms:
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

        // Builds the grid:
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

    // Loads the data from the input path and creates the crystal structure as a child of the parentNode:
    public static void LoadData(Node parentNode, string path)
    {
        {
            string structureJSON = File.ReadAllText(path);
            SaveFile structureFile = JsonSerializer.Deserialize<SaveFile>(structureJSON);
            List<Atom> basisAtoms = [];
            for (int i = 0; i < structureFile.BasisAtoms.Count; i++)
            {
                basisAtoms.Add(new(
                    new(x: structureFile.BasisAtoms[i].RelativePos[0], y: structureFile.BasisAtoms[i].RelativePos[1], z: structureFile.BasisAtoms[i].RelativePos[2]),
                    new(r: structureFile.BasisAtoms[i].Colour[0], g: structureFile.BasisAtoms[i].Colour[1], b: structureFile.BasisAtoms[i].Colour[2])
                ));
            }
            CreateCrystal(parentNode, structureFile.Size,
                new(structureFile.A1[0], structureFile.A1[1], structureFile.A1[2]),
                new(structureFile.A2[0], structureFile.A2[1], structureFile.A2[2]),
                new(structureFile.A3[0], structureFile.A3[1], structureFile.A3[2]),
                basisAtoms
            );
        }       
    }
}

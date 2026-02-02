// A class used to store saved structure data in its simplest form to be converted to or from JSON.

using System.Collections.Generic;

public class SaveFile
{
    public int Size { get; set; } // Grid will have 2 * Size + 1 lattice points in each direction.

    // The three basis vectors:
    public float[] A1 { get; set; }
    public float[] A2 { get; set; }
    public float[] A3 { get; set; }

    public List<SaveAtom> BasisAtoms { get; set; }

    // A class used to store saved StaticCrystal.Atom type object data in its simplest form to be converted to or from JSON.
    public class SaveAtom
    {
        public float[] RelativePos { get; set; }
        public float[] Colour { get; set; }
    }
}

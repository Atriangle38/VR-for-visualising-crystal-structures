from vpython import *
import numpy as np

class atom:
    def __init__(self, relative_pos, colour):
        self.relative_pos = relative_pos
        self.colour = colour

class basis:
    def __init__(self, lattice_site):
        self.spheres = []
        for a in basis_atoms:
            self.spheres.append(sphere(radius = atom_size, pos = lattice_site + a.relative_pos, color = a.colour))

atom_size = 0.1 # Radius of each atom
grid_size = 1 # Grid will have 2 * grid_size + 1 lattice points in each direction

# The three basis vectors:
a1 = vector(1, 0, 0)
a2 = vector(1/2, np.sqrt(3)/2, 0)
a3 = vector(0, 0, 2)

# The relative positions and colours of atoms in each instance of the basis:
basis_atoms = [\
    atom(vector(0.0, 0.0, 0.0), color.white),\
    atom(vector(1/2, np.sqrt(3)/6, 0), color.red),\
]

# Builds the grid:
grid_lines = []
for n1 in range(-grid_size, grid_size + 1):
    for n2 in range(-grid_size, grid_size + 1):
        grid_lines.append(curve(pos = [n1 * a1 + n2 * a2 + -grid_size * a3, n1 * a1 + n2 * a2 + grid_size * a3]))
for n1 in range(-grid_size, grid_size + 1):
    for n3 in range(-grid_size, grid_size + 1):
        grid_lines.append(curve(pos = [n1 * a1 + n3 * a3 + -grid_size * a2, n1 * a1 + n3 * a3 + grid_size * a2]))
for n2 in range(-grid_size, grid_size + 1):
    for n3 in range(-grid_size, grid_size + 1):
        grid_lines.append(curve(pos = [n2 * a2 + n3 * a3 + -grid_size * a1, n2 * a2 + n3 * a3 + grid_size * a1]))

# Places the atoms:
for n1 in range(-grid_size, grid_size + 1):
    for n2 in range(-grid_size, grid_size + 1):
        for n3 in range(-grid_size, grid_size + 1):
            basis(n1 * a1 + n2 * a2 + n3 * a3)

input("Enter to close window:")

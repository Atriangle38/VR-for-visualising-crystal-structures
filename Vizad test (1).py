import viz
import vizshape
import json
import os
import math
import vizconnect

viz.setMultiSample(4)

vizconnect.go('ere.py')

# Load JSON (same folder)
JSON_FILE = 'structure.json'
json_path = os.path.join(os.path.dirname(__file__), JSON_FILE)

with open(json_path, 'r', encoding='utf-8') as f:
    data = json.load(f)

atoms = data.get('atoms', [])
grid_lines = data.get('grid_lines', [])
visual_scale = float(data.get('visualScale', 1.0))

WORLD_SCALE = 0.3   
Z_FLIP = 1.0        

root = viz.addGroup()
root.setPosition([1.3, 1.6, 0])  # average human eye height

# Vector helpers
def v_sub(a, b): return [a[0]-b[0], a[1]-b[1], a[2]-b[2]]
def v_add(a, b): return [a[0]+b[0], a[1]+b[1], a[2]+b[2]]
def v_mul(a, s): return [a[0]*s, a[1]*s, a[2]*s]
def v_len(a): return math.sqrt(a[0]*a[0] + a[1]*a[1] + a[2]*a[2])
def v_dot(a, b): return a[0]*b[0] + a[1]*b[1] + a[2]*b[2]
def v_cross(a, b):
    return [a[1]*b[2]-a[2]*b[1], a[2]*b[0]-a[0]*b[2], a[0]*b[1]-a[1]*b[0]]
def clamp(x, lo, hi): return max(lo, min(hi, x))

def to_world_pos(p):
    x, y, z = float(p[0]), float(p[1]), float(p[2])
    return [x * visual_scale * WORLD_SCALE,
            y * visual_scale * WORLD_SCALE,
            z * visual_scale * WORLD_SCALE * Z_FLIP]

def add_cylinder_between(p1, p2, radius, color_rgb):
    d = v_sub(p2, p1)
    L = v_len(d)
    if L <= 1e-9:
        return

    mid = v_mul(v_add(p1, p2), 0.5)

    cyl = vizshape.addCylinder(height=L, radius=radius, axis=vizshape.AXIS_Y)
    cyl.setParent(root)
    cyl.setPosition(mid)
    cyl.color(color_rgb)

    # rotate +Y to direction
    y_axis = [0.0, 1.0, 0.0]
    dirn = v_mul(d, 1.0 / L)

    dotv = clamp(v_dot(y_axis, dirn), -1.0, 1.0)
    angle_deg = math.degrees(math.acos(dotv))

    axis = v_cross(y_axis, dirn)
    axis_len = v_len(axis)

    if axis_len < 1e-9:
        cyl.setAxisAngle([1, 0, 0, 0 if dotv > 0 else 180])
    else:
        axis = v_mul(axis, 1.0 / axis_len)
        cyl.setAxisAngle([axis[0], axis[1], axis[2], angle_deg])

# Render atoms
for a in atoms:
    p = to_world_pos(a['pos'])
    r = float(a.get('radius', 0.1)) * visual_scale * WORLD_SCALE
    col = a.get('color', [1, 1, 1])

    s = vizshape.addSphere(radius=r, axis=vizshape.AXIS_Y)
    s.setParent(root)
    s.setPosition(p)
    s.color(col)

print("Loaded atoms:", len(atoms))

# Render grid lines (this matches your VPython "curve" grid)
for seg in grid_lines:
    p1 = to_world_pos(seg['p1'])
    p2 = to_world_pos(seg['p2'])
    rad = float(seg.get('radius', 0.01)) * visual_scale * WORLD_SCALE
    col = seg.get('color', [0.3, 0.3, 0.3])

    add_cylinder_between(p1, p2, radius=rad, color_rgb=col)

print("Grid segments:", len(grid_lines))

# Camera
viz.MainView.setPosition([0, 0, 0])
viz.MainView.lookAt([0, 0, 0])

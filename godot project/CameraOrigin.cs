using Godot;

public partial class CameraOrigin : Node3D // CameraOrigin is a node for the camera's origin position that it faces towards, moves and rotates alongside and zooms in to and out from, not the camera itself.
{
	private const float SPEED = 0.1f; // How many metres the camera's origin moves per tick when a movement key is pressed.
	private Vector3 ANTICLOCKWISE_SPEED = new(0, 1, 0); // The vector the camera and its origin rotate around when moving anticlockwise and how fast they rotate in degrees per tick.
	private const float ZOOM_OUT_SPEED = 1.1f; // The factor the displacement of the camera from its origin is multiplied/divided by when zooming out/in.
	private Camera3D camera;

	public override void _Ready() // Runs on scene start.
	{
		camera = GetChild<Camera3D>(0); // Gets the Camera3D node, which is a child of CameraOrigin, and gives it the identifier "camera" which is used when zooming in and out.
	}

	public override void _PhysicsProcess(double delta) // Runs every tick where one second is 60 ticks by default and can be changed in Project → Project Settings → General → Physics → Common.
	{
		// These actions and the keys associated with them are defined in Project → Project Settings → Input Map.

		// Moving the camera's origin. This causes its child (the camera) to move along with it it. Directions X, Y and Z are local coordinates, relative to the origin's rotation:
		if (Input.IsActionPressed("move forwards")) // W
		{
			Position += SPEED * Basis.X;
		}
		if (Input.IsActionPressed("move backwards")) // S
		{
			Position -= SPEED * Basis.X;
		}
		if (Input.IsActionPressed("move up")) // Space
		{
			Position += SPEED * Basis.Y;
		}
		if (Input.IsActionPressed("move down")) // Shift or Ctrl
		{
			Position -= SPEED * Basis.Y;
		}
		if (Input.IsActionPressed("move right")) // D
		{
			Position += SPEED * Basis.Z;
		}
		if (Input.IsActionPressed("move left")) // A
		{
			Position -= SPEED * Basis.Z;
		}

		// Rotating the camera's origin. This causes its child (the camera) to rotate around it:
		if (Input.IsActionPressed("rotate anticlockwise")) // Q
		{
			RotationDegrees += ANTICLOCKWISE_SPEED;
		}
		if (Input.IsActionPressed("rotate clockwise")) // E
		{
			RotationDegrees -= ANTICLOCKWISE_SPEED;
		}

		// Zooming the camera towards or away from its origin. The attribute camera.Position is relative to the camera's origin:
		if (Input.IsActionJustReleased("zoom out")) // Mouse Wheel Down
		{
			camera.Position *= ZOOM_OUT_SPEED;
		}
		if (Input.IsActionJustReleased("zoom in")) // Mouse Wheel Up
		{
			camera.Position /= ZOOM_OUT_SPEED;
		}
	}
}

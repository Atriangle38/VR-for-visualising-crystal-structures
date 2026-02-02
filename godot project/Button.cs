public partial class Button : Godot.Button
{
	private void ButtonPressedSignal()
	{
		StaticCrystal.CreateCrystal(GetTree().Root.GetChild(0).GetChild(0), 1, new(1, 0, 0), new(0, 1, 0), new(0, 0, 1),
			[
				new(new(0.0f, 0.0f, 0.0f), new(1, 0, 0)),
				new(new(0.5f, 0.5f, 0.5f), new(0, 1, 1))
			]
		);
		Hide();
	}
}

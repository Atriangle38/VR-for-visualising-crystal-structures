using Godot;

public partial class OpenFileWindow : FileDialog
{
	private void ShowOpenFileWindowSignal()
	{
		Show(); // Makes the open file window appear when the open file button is pressed.
	}

	private void FileSelectedSignal(string path)
	{
		GetTree().CallGroup("structure", "queue_free"); // Removes all atoms and grid lines.
		StaticCrystal.LoadData(GetTree().Root.GetChild(0).GetChild(0), path); // Opens the selected file.
	}
}

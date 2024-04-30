using Godot;

namespace Pinnuckle.addons.ScenicRoute;

[GlobalClass, Tool]
public partial class SceneListItem : HBoxContainer
{
    [Export] public string LabelText;

    public override void _Ready()
    {
        _CreateItemParts();
    }

    private void _CreateItemParts()
    {
        Label label = new Label();
        label.Text = LabelText;
        Panel labelPanel = _CreateLabelPanel();
        labelPanel.AddChild(label);
        Button removeButton = new Button();
        removeButton.Text = "-";

        AddChild(labelPanel);
        AddChild(removeButton);
    }

    private Panel _CreateLabelPanel()
    {
        Panel panel = new Panel();
        panel.SizeFlagsHorizontal = SizeFlags.Expand;
        panel.SizeFlagsStretchRatio = 10;
        return panel;
    }
}
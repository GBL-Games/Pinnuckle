using Godot;

namespace Pinnuckle.Scripts
{
    public partial class MeldListItem : RichTextLabel
    {
        [Export] public MeldData MeldListItemData;

        private Theme _theme = GD.Load<Theme>("res://Resources/PinnuckleTheme.tres");

        public override void _Ready()
        {
            base._Ready();

            Theme = _theme;

            AutowrapMode = TextServer.AutowrapMode.Off;
            FitContent = true;
            Text = MeldListItemData.Name;
            TextDirection = TextDirection.Rtl;
        }
    }
}
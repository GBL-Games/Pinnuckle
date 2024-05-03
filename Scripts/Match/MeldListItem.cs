using Godot;

namespace Pinnuckle.Scripts.Match
{
    public partial class MeldListItem : RichTextLabel
    {
        [Export] public Match.MeldData MeldListItemData;
        [Export] public string MeldListOwner;

        private Theme _theme = GD.Load<Theme>("res://Resources/PinnuckleTheme.tres");

        public override void _Ready()
        {
            base._Ready();

            Theme = _theme;

            AutowrapMode = TextServer.AutowrapMode.Off;
            MouseFilter = MouseFilterEnum.Ignore;
            FitContent = true;
            Text = MeldListItemData.Name;
            TextDirection = MeldListOwner == "player" ? TextDirection.Ltr : TextDirection.Rtl;
        }
    }
}
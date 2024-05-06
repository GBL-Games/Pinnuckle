using Godot;
using Godot.Collections;
using Pinnuckle.Scripts.Archetype;
using Pinnuckle.Scripts.Player;

namespace Pinnuckle.Scripts.Managers;

public partial class PlayerManager : Node
{
    private PlayerData _playerData;

    private int _health;
    private int _block;

    private Array<EnhanceType> _enhancements = [];
    private Array<ImpairType> _impairments = [];
    private Array<ArchetypeSkill> _skills = [];

    private Control _playerHud;

    public void InitializePlayer(PlayerData playerData, Control playerHud)
    {
        _playerData = playerData;
        _playerHud = playerHud;

        _skills = _playerData.PlayerArchetype.Skills;

        GD.Print($"Initializing player with the archetype: {_playerData.PlayerArchetype.Name}");

        _InitializePlayerHud();
    }

    private void _InitializePlayerHud()
    {
        GD.Print(_playerHud.ToString());

        for (int i = 0; i < _skills.Count; i++)
        {
            AtlasTexture atlas = new AtlasTexture();
            atlas.Atlas = (Texture2D)ResourceLoader.Load("res://Assets/ui/fantasy_epicweapons_pack1_nobg.png");
            atlas.Region = new Rect2(_skills[i].SkillIconPos, new Vector2(32, 32));
            _playerHud.GetNode<TextureButton>($"Skill{(i + 1).ToString()}").TextureNormal = atlas;
        }
    }
}
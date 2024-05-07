using Godot;
using Godot.Collections;
using Pinnuckle.addons.ScenicRoute;
using Pinnuckle.Scripts.Archetype;
using Pinnuckle.Scripts.Autoload;
using Pinnuckle.Scripts.Player;

namespace Pinnuckle.Scripts.Managers;

public partial class PlayerManager : Control
{
    private PlayerData _playerData;

    private float _health;
    private int _atk;
    private float _maxHealth;
    private float _block;

    private Array<EnhanceType> _enhancements = [];
    private Array<ImpairType> _impairments = [];
    private Array<ArchetypeSkill> _skills = [];

    private Control _playerHud;
    private SignalBus _signalBus;
    private ScenicRoute _scenicRoute;
    private GameState _gameState;

    public override void _Ready()
    {
        _scenicRoute = GetNodeOrNull<ScenicRoute>("/root/ScenicRoute");
        _gameState = (GameState)_scenicRoute.GetCurrentGameState();

        if (_gameState == null)
        {
            GD.PrintErr("Couldn't load game state");
            return;
        }

        _playerData = _gameState.CurrentPlayer;
        _signalBus = GetNode<SignalBus>("/root/SignalBus");
        _skills = _playerData.PlayerArchetype.Skills;

        GD.Print($"Initializing player with the archetype: {_playerData.PlayerArchetype.Name}");

        _ConnectSignals();
        _InitializePlayerHud();
    }

    #region Initialization

    private void _ConnectSignals()
    {
        _signalBus.PlayerHpChange += _UpdatePlayerHealth;
    }

    // TODO: Update sprites to be their own individual sprite sheets for more control.  
    private void _InitializePlayerHud()
    {
        _health = _playerData.Health;
        _maxHealth = _playerData.Health;

        for (int i = 0; i < _skills.Count; i++)
        {
            AtlasTexture atlas = new AtlasTexture();
            atlas.Atlas = (Texture2D)ResourceLoader.Load("res://Assets/ui/fantasy_weapons_pack1_noglow.png");
            atlas.Region = new Rect2(_skills[i].SkillIconPos, new Vector2(32, 32));
            GetNode<TextureButton>($"Skill{(i + 1).ToString()}").TextureNormal = atlas;
        }
    }

    #endregion

    #region HUD Updates

    private void _UpdatePlayerHealth(int amount)
    {
        _health += amount;
        float healthPercentage = float.Round(_health / _maxHealth * 100);
        GetNode<TextureProgressBar>("HealthBar").Value = healthPercentage;
        GetNode<TextureProgressBar>("Heart").Value = healthPercentage;
    }

    private void _UpdatedPlayerAtk(int amount)
    {
        _atk += amount;
    }

    private void _UpdatedPlayerDef(int amount)
    {
        _block += amount;
    }

    #endregion
}
using DefaultEcs;
using Microsoft.Xna.Framework.Content;
using MonoDreams.Renderer;
using MonoDreams.Scale.Level.Levels;

namespace MonoDreams.Scale.Level;

public class LevelLoader
{
    private World _world;
    private readonly ContentManager _content;
    private readonly ResolutionIndependentRenderer _renderer;

    public LevelLoader(World world, ContentManager content, ResolutionIndependentRenderer renderer)
    {
        _world = world;
        _content = content;
        _renderer = renderer;
    }

    public int CurrentLevel { get; private set; }

    public void LoadLevel(int index)
    {
        CurrentLevel = index;
        ILevel level = index switch
        {
            0 => new Level0(_content, _renderer),
            1 => new Level1(_content, _renderer),
            _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
        };
        level.Load(_world);
    }

    public void ReloadLevel(World world)
    {
        _world = world;
        LoadLevel(CurrentLevel);
    }

    public void LoadNextLevel(World world)
    {
        _world = world;
        LoadLevel(CurrentLevel + 1);
    }
}
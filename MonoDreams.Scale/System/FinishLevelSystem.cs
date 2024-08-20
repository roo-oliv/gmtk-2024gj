using DefaultEcs;
using DefaultEcs.System;
using MonoDreams.Scale.Message;
using MonoDreams.State;

namespace MonoDreams.Scale.System;

public class FinishLevelSystem : ISystem<GameState>
{
    private readonly Action _finishLevel;
    private readonly List<FinishLevelMessage> _finishLevelMessages;

    public FinishLevelSystem(World world, Action finishLevel)
    {
        _finishLevel = finishLevel;
        world.Subscribe(this);
        _finishLevelMessages = [];
    }

    [Subscribe]
    private void On(in FinishLevelMessage message) => _finishLevelMessages.Add(message);

    public void Update(GameState state)
    {
        if (_finishLevelMessages.Count == 0) return;
        _finishLevelMessages.Clear();
        _finishLevel();
    }

    public bool IsEnabled { get; set; } = true;
    
    public void Dispose()
    {
        _finishLevelMessages.Clear();
    }
}
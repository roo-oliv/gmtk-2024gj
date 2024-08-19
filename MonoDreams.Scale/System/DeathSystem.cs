using DefaultEcs;
using DefaultEcs.System;
using MonoDreams.Scale.Message;
using MonoDreams.State;

namespace MonoDreams.Scale.System;

public class DeathSystem : ISystem<GameState>
{
    private readonly List<DeathMessage> _deathMessages;
    private readonly Action _reload;

    public DeathSystem(World world, Action reload)
    {
        _reload = reload;
        world.Subscribe(this);
        _deathMessages = [];
    }

    [Subscribe]
    private void On(in DeathMessage message) => _deathMessages.Add(message);
        
    public bool IsEnabled { get; set; } = true;
        
    public void Dispose()
    {
        _deathMessages.Clear();
    }

    public void Update(GameState state)
    {
        if (_deathMessages.Count == 0)  return;

        _deathMessages.Clear();
        _reload();
    }
}
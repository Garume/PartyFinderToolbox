namespace PartyFinderToolbox.Shared.Services.Task.YieldInstruction;

public class WaitWile(Func<bool> predicate) : IYieldInstruction
{
    public bool KeepWaiting => predicate();
}
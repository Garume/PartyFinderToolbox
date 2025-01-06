namespace PartyFinderToolbox.Shared.Services.Task.YieldInstruction;

public class WaitUntil(Func<bool> predicate) : IYieldInstruction
{
    public bool KeepWaiting => !predicate();
}
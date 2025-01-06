using System.Diagnostics;

namespace PartyFinderToolbox.Shared.Services.Task.YieldInstruction;

public class WaitForSeconds(int second) : IYieldInstruction
{
    private long _startTimeStamp;

    public bool KeepWaiting
    {
        get
        {
            if (_startTimeStamp == 0) _startTimeStamp = Stopwatch.GetTimestamp();

            var elapsed = Stopwatch.GetElapsedTime(_startTimeStamp);
            Logger.Warning($"Elapsed: {elapsed.Seconds} bool:{elapsed.Seconds < second}",true);
            return elapsed.Seconds < second;
        }
    }
}
using System.Collections;
using System.Diagnostics;
using Dalamud.IoC;
using Dalamud.Plugin.Services;
using PartyFinderToolbox.Shared.Services;

namespace PartyFinderToolbox.Core.Services;

[LoadService]
public class TaskService : Service<TaskService>
{
    private static readonly Queue<Task> Tasks = new();
    private static CoroutineManager _coroutineManager = new();
    private Task? _currentTask;
    [PluginService] private static IFramework Framework { get; set; } = null!;

    protected override void Initialize()
    {
        Framework.Update += Update;
        Disposables.Add(() => Framework.Update -= Update);

        Framework.Update += CoroutineUpdate;
        Disposables.Add(() => Framework.Update -= CoroutineUpdate);
    }

    private static void CoroutineUpdate(IFramework framework)
    {
        _coroutineManager.Update();
    }

    private void Update(IFramework framework)
    {
        if (_currentTask == null && Tasks.Count > 0)
        {
            _currentTask = Tasks.Dequeue();
            _currentTask.StartTime = Stopwatch.GetTimestamp();
        }

        if (_currentTask != null)
        {
            var elapsed = Stopwatch.GetElapsedTime(_currentTask.StartTime);
            if (elapsed.Milliseconds >= _currentTask.Duration)
            {
                _currentTask.IsCompleted = true;
                try
                {
                    _currentTask.Action();
                }
                catch (Exception e)
                {
                    Logger.Error(e.ToString());
                }
                finally
                {
                    _currentTask = null;
                }
            }
        }
    }

    private static void AddTask(Task task)
    {
        Tasks.Enqueue(task);
    }

    public static void AddTask(string name, int duration, Action action)
    {
        AddTask(new Task
        {
            Name = name,
            Duration = duration,
            Action = action
        });
    }

    public static void StartCoroutine(IEnumerator routine)
    {
        _coroutineManager.Start(routine);
    }

    public static void StopCoroutine(IEnumerator routine)
    {
        _coroutineManager.Stop(routine);
    }
}

public record Task
{
    public string Name { get; init; }
    public long StartTime { get; set; }
    public int Duration { get; init; }
    public Action Action { get; init; }
    public bool IsCompleted { get; set; }
}
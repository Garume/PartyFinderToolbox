using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.IoC;
using Dalamud.Plugin.Services;

namespace PartyFiltering.Shared.Services;

[LoadService(Priority = 1000)]
public class Logger : Service<Logger>
{
    [PluginService] private static IPluginLog PluginLog { get; set; } = null!;
    [PluginService] private static IChatGui ChatGui { get; set; } = null!;

    private static string Text(string message)
    {
        return $"[PartyFiltering] {message}";
    }


    private static void Log(string message, bool shouldSendChat, ushort colorKey)
    {
        Assert();
        var text = Text(message);
        PluginLog.Debug(text);
        if (shouldSendChat)
            ChatGui.Print(
                new XivChatEntry
                {
                    Message = new SeStringBuilder().AddUiForeground(text, colorKey).Build(),
                    Type = PluginInterface.GeneralChatType
                }
            );
    }

    public static void Information(string message, bool shouldSendChat = false)
    {
        Log(message, shouldSendChat, 3);
    }

    public static void Debug(string message, bool shouldSendChat = false)
    {
        Log(message, shouldSendChat, 4);
    }

    public static void Verbose(string message, bool shouldSendChat = false)
    {
        Log(message, shouldSendChat, 5);
    }

    public static void Warning(string message, bool shouldSendChat = false)
    {
        Log(message, shouldSendChat, 540);
    }

    public static void Error(string message, bool shouldSendChat = false)
    {
        Log(message, shouldSendChat, 17);
    }

    public static void Fatal(string message, bool shouldSendChat = false)
    {
        Log(message, shouldSendChat, 19);
    }

    private static void Assert()
    {
        if (Initialized) return;
        throw new InvalidOperationException();
    }
}
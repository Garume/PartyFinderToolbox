using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace PartyFiltering.Services;

public class Logger : IIinitializable
{
    [PluginService] private static IPluginLog PluginLog { get; set; } = null!;
    [PluginService] private static IChatGui ChatGui { get; set; } = null!;
    [PluginService] private static IDalamudPluginInterface PluginInterface { get; set; } = null!;


    public static bool Initialized { get; private set; }

    public void Init(IDalamudPluginInterface pluginInterface)
    {
        if (Initialized) PluginLog.Debug("Services already initialized, skipping");
        Initialized = true;
        try
        {
            pluginInterface.Create<Logger>();
        }
        catch (Exception ex)
        {
            PluginLog.Error(ex.Message);
        }
    }

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
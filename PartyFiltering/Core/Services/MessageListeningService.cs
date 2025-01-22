using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.IoC;
using Dalamud.Plugin.Services;
using PartyFinderToolbox.Shared.Services;

namespace PartyFinderToolbox.Core.Services;

[LoadService]
public class MessageListeningService : Service<MessageListeningService>
{
    public delegate void OnMessage(string message);

    [PluginService] private static IChatGui ChatService { get; set; } = null!;
    public event OnMessage? MessageReceived;

    protected override void Initialize()
    {
        ChatService.ChatMessage += OnChatMessage;
        Disposables.Add(() => ChatService.ChatMessage -= OnChatMessage);
        Disposables.Add(() => MessageReceived = null!);
    }

    private void OnChatMessage(XivChatType type, int timestamp, ref SeString sender, ref SeString message,
        ref bool isHandled)
    {
        MessageReceived?.Invoke(message.TextValue);

        if (!ConfigurationConfigService.TryGetConfig(out var config)) return;
        
        Logger.Debug($"Message received: {message.TextValue}");
        
        if (config.EnableAutoReloadParty && message.TextValue.Contains(config.AutoReloadPartyMessage))
            PartyService.ReloadParty();
    }
}
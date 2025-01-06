/*
https://git.annaclemens.io/ascclemens/XivCommon/src/branch/main/XivCommon/Functions/Chat.cs
MIT License
Copyright (c) 2021 Anna Clemens
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Runtime.InteropServices;
using System.Text;
using Dalamud.Game;
using Dalamud.IoC;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.System.Memory;
using FFXIVClientStructs.FFXIV.Client.System.String;
using PartyFinderToolbox.Shared.Services;

namespace PartyFinderToolbox.Core.Services;

[LoadService]
public class ChatService : Service<ChatService>
{
    private static unsafe delegate* unmanaged<Utf8String*, int, IntPtr, void> _sanitiseString = null!;

    [PluginService] private static ISigScanner SigScanner { get; set; } = null!;

    private static ProcessChatBoxDelegate ProcessChatBox { get; set; }

    protected override unsafe void Initialize()
    {
        if (SigScanner.TryScanText(Signatures.SendChat, out var processChatBoxPtr))
            ProcessChatBox = Marshal.GetDelegateForFunctionPointer<ProcessChatBoxDelegate>(processChatBoxPtr);
        if (SigScanner.TryScanText(Signatures.SanitiseString, out var sanitiseStringPtr))
            _sanitiseString = (delegate* unmanaged<Utf8String*, int, IntPtr, void>)sanitiseStringPtr;
    }

    public static void ExecuteCommand(string message)
    {
        if (!message.StartsWith("/"))
            throw new InvalidOperationException(
                $"Attempted to execute command but was not prefixed with a slash: {message}");
        SendMessage(message);
    }

    public static void SendMessage(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        if (bytes.Length == 0) throw new ArgumentException("message is empty", nameof(message));
        if (bytes.Length > 500) throw new ArgumentException("message is longer than 500 bytes", nameof(message));
        if (message.Length != SanitiseText(message).Length)
            throw new ArgumentException("message contained invalid characters", nameof(message));
#pragma warning disable CS0618 // Type or member is obsolete
        SendMessageUnsafe(bytes);
#pragma warning restore CS0618 // Type or member is obsolete
    }

    public static unsafe string SanitiseText(string text)
    {
        if (_sanitiseString == null)
            throw new InvalidOperationException("Could not find signature for chat sanitisation");
        var uText = Utf8String.FromString(text);
        _sanitiseString(uText, 0x27F, IntPtr.Zero);
        var sanitised = uText->ToString();
        uText->Dtor();
        IMemorySpace.Free(uText);
        return sanitised;
    }

    public static unsafe void SendMessageUnsafe(byte[] message)
    {
        if (ProcessChatBox == null) throw new InvalidOperationException("Could not find signature for chat sending");
        var uiModule = (IntPtr)Framework.Instance()->GetUIModule();
        using var payload = new ChatPayload(message);
        var mem1 = Marshal.AllocHGlobal(400);
        Marshal.StructureToPtr(payload, mem1, false);
        ProcessChatBox(uiModule, mem1, IntPtr.Zero, 0);
        Marshal.FreeHGlobal(mem1);
    }

    private static class Signatures
    {
        internal const string SendChat = "48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 48 8B F2 48 8B F9 45 84 C9";
        internal const string SanitiseString = "E8 ?? ?? ?? ?? EB 0A 48 8D 4C 24 ?? E8 ?? ?? ?? ?? 48 8D AE";
    }

    private delegate void ProcessChatBoxDelegate(IntPtr uiModule, IntPtr message, IntPtr unused, byte a4);

    [StructLayout(LayoutKind.Explicit)]
    private readonly struct ChatPayload : IDisposable
    {
        [FieldOffset(0)] private readonly IntPtr textPtr;
        [FieldOffset(16)] private readonly ulong textLen;
        [FieldOffset(8)] private readonly ulong unk1;
        [FieldOffset(24)] private readonly ulong unk2;

        internal ChatPayload(byte[] stringBytes)
        {
            textPtr = Marshal.AllocHGlobal(stringBytes.Length + 30);
            Marshal.Copy(stringBytes, 0, textPtr, stringBytes.Length);
            Marshal.WriteByte(textPtr + stringBytes.Length, 0);
            textLen = (ulong)(stringBytes.Length + 1);
            unk1 = 64;
            unk2 = 0;
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(textPtr);
        }
    }
}
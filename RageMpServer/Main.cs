using System;
using GTANetworkAPI;
using RageMpServer.Repository;

namespace RageMpServer
{
    public class Main : Script
    {
        private Chat _chat = null;

        [ServerEvent(Event.ResourceStart)]
        public void OnResurcesStart()
        {
            _chat = new Chat();
            NAPI.Server.SetGlobalServerChat(false);

            NAPI.Util.ConsoleOutput("=========== Successfuly Started ==========");
        }

        [ServerEvent(Event.ChatMessage)]
        public void OnChatMessage(Player player, string message)
        {
            _chat.CustomChat(player, $"{player.Name} сказал: {message}", 20, "!{#556680}");
        }

        [ServerEvent(Event.PlayerConnected)]
        public void OnPlayerConnected(Player player)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, "ShowAuthCef", true);
        }
    }
}

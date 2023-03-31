using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using RageMpServer.Contexts;
using RageMpServer.DatabaseEntities;
using RageMpServer.Extensions;
using RageMpServer.Models;
using RageMpServer.Repository;
using RageMpServer.Services;

namespace RageMpServer
{
    public class Main : Script
    {
        private ChatService _chat = null;
        private BlipService _blip = null;
        private MainService _main = null;


        [ServerEvent(Event.ResourceStart)]
        public void OnResurcesStart()
        {
           
            _blip = new BlipService();
            _chat = new ChatService();
            _main = new MainService();

            NAPI.Server.SetGlobalServerChat(false);
            NAPI.Server.SetAutoRespawnAfterDeath(false);

            _blip.CreateBlip(162, new Vector3(-1017, -2695, 13), 2, 1f, "Аренда", false);
            
            var position = new Vector3(-1019.85, -2694.38, 9.98);

            NAPI.Checkpoint.CreateCheckpoint(CheckpointType.Cyclinder, position, new Vector3(0, 0, 0), scale: 1.5f, new Color(255, 0, 0, 100), NAPI.GlobalDimension);

            NAPI.Util.ConsoleOutput("=========== Successfuly Started ==========");
        }

        [ServerEvent(Event.ChatMessage)]
        public async Task OnChatMessage(Player player, string message)
        {
            NAPI.Task.Run(() => _chat.CustomChat(player, $"{player.Name} сказал: {message}", 20, "!{#556680}"));
        }

        [ServerEvent(Event.PlayerConnected)]
        public void OnPlayerConnected(Player player)
        {
            _main.PlayerConnected(player);
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            NAPI.Task.Run(async () => await _main.PlayerDisconnected(player, type, reason));
        }
    }
}

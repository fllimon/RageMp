using System.Linq;
using AutoMapper;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using RageMpServer.Contexts;
using RageMpServer.Extensions;
using RageMpServer.Repository;

namespace RageMpServer
{
    public class Main : Script
    {
        private Chat _chat = null;
        private RageContext _db = null;
        private IMapper _mapper = null;


        [ServerEvent(Event.ResourceStart)]
        public void OnResurcesStart()
        {
            _db = DbInitializer.GetInstance();
            _mapper = MapperInitializer.GetInstance();
            
            _chat = new Chat();
            NAPI.Server.SetGlobalServerChat(false);
            NAPI.Server.SetAutoRespawnAfterDeath(false);

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

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            bool hasData = player.HasData(Entity.Player.PLayerData);

            if (hasData)
            {
                var playerData = player.GetData<Entity.Player>(Entity.Player.PLayerData);
                playerData.Position = player.Position;
                playerData.Rotation = player.Rotation;
                playerData.Health = player.Health;
                playerData.Armor = player.Armor;

                var mapedPlayer = _mapper.Map<Models.Player>(playerData);

                var existingEntity = _db.Players
                    .Include(x => x.Position)
                    .FirstOrDefault(x => x.FirstName == mapedPlayer.FirstName && x.LastName == mapedPlayer.LastName);

                mapedPlayer.Id = existingEntity.Id;
                mapedPlayer.UserId = existingEntity.UserId;
                mapedPlayer.CreatedDate = existingEntity.CreatedDate;
                mapedPlayer.Position.Id = existingEntity.Position.Id;
                mapedPlayer.Position.PlayerId = existingEntity.Position.PlayerId;

                _db.Entry(existingEntity.Position).CurrentValues.SetValues(mapedPlayer.Position);
                _db.Entry(existingEntity).CurrentValues.SetValues(mapedPlayer);

                _db.Players.Update(existingEntity);
                _db.SaveChanges();
            }
        }
    }
}

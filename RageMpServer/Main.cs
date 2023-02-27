using System.Linq;
using System.Threading.Tasks;
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
        private Repository.Blip _blip = null;


        [ServerEvent(Event.ResourceStart)]
        public void OnResurcesStart()
        {
            _db = DbInitializer.GetInstance();
            _mapper = MapperInitializer.GetInstance();
            _blip = new Repository.Blip();

            _chat = new Chat();
            NAPI.Server.SetGlobalServerChat(false);
            NAPI.Server.SetAutoRespawnAfterDeath(false);

            _blip.CreateBlip(162, new Vector3(-1017, -2695, 13), 2, 1f, "Аренда", false);

            for (int i = 0; i < 1; i++)
            {
                var position = new Vector3(-1019.85, -2694.38, 9.98);
                NAPI.Vehicle.CreateVehicle(VehicleHash.Surge, position, 150f, 0, 0, "", 255, false, false, NAPI.GlobalDimension);
            }

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
            NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT:ShowAuthCef", true);
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public async Task OnPlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            bool hasData = player.HasData(Entity.Player.PLayerData);

            if (hasData)
            {
                var playerData = player.GetData<Entity.Player>(Entity.Player.PLayerData);
                playerData.Position = player.Position;
                playerData.Rotation = player.Rotation;
                playerData.Health = player.Health;
                playerData.Armor = player.Armor;

                var mapedPlayer = _mapper.Map<Models.CustomPlayer>(playerData);

                var existingEntity = await _db.Players
                    .Include(x => x.Position)
                    .FirstOrDefaultAsync(x => x.FirstName == mapedPlayer.FirstName && x.LastName == mapedPlayer.LastName);

                mapedPlayer.Id = existingEntity.Id;
                mapedPlayer.UserId = existingEntity.UserId;
                mapedPlayer.CreatedDate = existingEntity.CreatedDate;
                mapedPlayer.Position.Id = existingEntity.Position.Id;
                mapedPlayer.Position.PlayerId = existingEntity.Position.PlayerId;

                _db.Entry(existingEntity.Position).CurrentValues.SetValues(mapedPlayer.Position);
                _db.Entry(existingEntity).CurrentValues.SetValues(mapedPlayer);

                await _db.SaveChangesAsync();
            }
        }
    }
}

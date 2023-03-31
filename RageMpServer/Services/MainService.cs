using AutoMapper;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using RageMpServer.Contexts;
using RageMpServer.DatabaseEntities;
using RageMpServer.Extensions;
using RageMpServer.Models;
using RageMpServer.Repository;
using System.Threading.Tasks;

namespace RageMpServer.Services
{
    public class MainService : Script
    {
        private readonly IMapper _mapper = null;
        private readonly PlayerRepository _playerRepository;

        public MainService()
        {
            _mapper = MapperInitializer.GetInstance();
            _playerRepository = new PlayerRepository();
        }

        public async Task PlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            bool hasData = player.HasData(PlayerModel.PLayerData);

            if (hasData)
            {
                var playerData = player.GetData<PlayerModel>(PlayerModel.PLayerData);
                playerData.Position = player.Position;
                playerData.Rotation = player.Rotation;
                playerData.Health = player.Health;
                playerData.Armor = player.Armor;

                var mapedPlayer = _mapper.Map<CustomPlayer>(playerData);

                var existingEntity = await _playerRepository.GetPlayerByFirstLastNames(mapedPlayer.FirstName, mapedPlayer.LastName);

                mapedPlayer.Id = existingEntity.Id;
                mapedPlayer.UserId = existingEntity.UserId;
                mapedPlayer.CreatedDate = existingEntity.CreatedDate;
                mapedPlayer.Position.Id = existingEntity.Position.Id;
                mapedPlayer.Position.PlayerId = existingEntity.Position.PlayerId;

                await _playerRepository.UpdatePlayerPosition(existingEntity, mapedPlayer);
            }
        }

        public void PlayerConnected(Player player)
        {
            NAPI.Player.FreezePlayerTime(player, true);
            NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT:ShowAuthCef", true);
        }
    }
}

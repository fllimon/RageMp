using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using RageMpServer.Contexts;
using RageMpServer.DatabaseEntities;
using RageMpServer.Extensions;
using RageMpServer.Models;

namespace RageMpServer.Repository
{
    class LoginRepository : Script
    {
        private RageContext _db = null;
        private IMapper _mapper = null;

        public LoginRepository()
        {
            _db = DbInitializer.GetInstance();
            _mapper = MapperInitializer.GetInstance();
        }

        [RemoteEvent("CLIENT:SERVER:SendLoginInfo")]
        public void LoginInfoFromClient(Player player, string login, string password)
        {
            NAPI.Task.Run(async () => await LoginPlayer(player, login, password)); 
        }

        private async Task LoginPlayer(Player player, string login, string password)
        {
            var user = await GetUserByLogin(login);

            if (user == null)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT:AuthError");

                return;
            }

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                var entity = await GetPlayerByUserId(user.Id);

                if (entity == null)
                {
                    NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT:ShowAuthCef", false);
                    NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT:ShowCreatePlayerForm", true, user.Id);
                    NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT:SetUserId", user.Id);

                    return;
                }

                NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT:ShowAuthCef", false);

                var playerData = _mapper.Map<PlayerModel>(entity);
                var hasData = player.HasData(PlayerModel.PLayerData);

                if (!hasData)
                {
                    player.SetData(PlayerModel.PLayerData, playerData);
                    player.Name = playerData.FirstName + " " + playerData.LastName;
                    player.Position = playerData.Position;
                    player.Health = playerData.Health;
                    player.Armor = playerData.Armor;
                    player.Dimension = NAPI.GlobalDimension;
                }

                NAPI.Player.FreezePlayerTime(player, false);
            }
        }

        private async Task<CustomPlayer> GetPlayerByUserId(Guid id)
        {
            return await _db.Players
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.UserId == id);
        }

        private async Task<User> GetUserByLogin(string login)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Login == login);
        }
    }
}

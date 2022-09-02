using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using RageMpServer.Contexts;
using RageMpServer.Extensions;
using RageMpServer.Models;

namespace RageMpServer.Repository
{
    class Login : Script
    {
        private RageContext _db = null;
        private IMapper _mapper = null;

        public Login()
        {
            _db = DbInitializer.GetInstance();
            _mapper = MapperInitializer.GetInstance();
        }

        [RemoteEvent("LoginInfoFromClientEvent")]
        public void LoginInfoFromClient(GTANetworkAPI.Player player, string login, string password)
        {
            var user = GetUserByLogin(login);

            if (user == null)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "AuthError");

                return;
            }

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                var entity = GetPlayerByUserId(user.Id);

                if (entity == null)
                {
                    NAPI.ClientEvent.TriggerClientEvent(player, "ShowAuthCef", false);
                    NAPI.ClientEvent.TriggerClientEvent(player, "ShowCreatePlayerForm", true, user.Id);
                    NAPI.ClientEvent.TriggerClientEvent(player, "SetUserId", user.Id);

                    return;
                }

                NAPI.ClientEvent.TriggerClientEvent(player, "ShowAuthCef", false);

                var playerData = _mapper.Map<Entity.Player>(entity);
                var hasData = player.HasData(Entity.Player.PLayerData);

                if (!hasData)
                {
                    player.SetData(Entity.Player.PLayerData, playerData);
                    player.Name = playerData.FirstName + " " + playerData.LastName;
                    player.Position = playerData.Position;
                    player.Health = playerData.Health;
                    player.Armor = playerData.Armor;
                }
            }
        }

        private Models.Player GetPlayerByUserId(Guid id)
        {
            return _db.Players
                .Include(x => x.Position)
                .FirstOrDefault(x => x.UserId == id);
        }

        private User GetUserByLogin(string login)
        {
            return _db.Users.FirstOrDefault(x => x.Login == login);
        }
    }
}

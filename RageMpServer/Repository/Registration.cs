using System;
using GTANetworkAPI;
using RageMpServer.Contexts;
using System.Linq;
using RageMpServer.Models;
using RageMpServer.Extensions;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RageMpServer.Repository
{
    class Registration : Script
    {
        private RageContext _db = null;
        private IMapper _mapper = null;

        public Registration()
        {
            _db = DbInitializer.GetInstance();
            _mapper = MapperInitializer.GetInstance();
        }

        [RemoteEvent("CLIENT:SERVER:RegisterUser")]
        public async Task RegistrationInfoFromClient(Player player, string login, string email, string password)
        {
            if (IsNullOrEmpty(login) || IsNullOrEmpty(email) || IsNullOrEmpty(password))
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "AuthError");

                return;
            }

            if (!await IsExist(login, email, player.SocialClubName))
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "AuthError");

                return;
            }

            var user = new User()
            {
                Login = login,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                RegistryDate = DateTime.UtcNow,
                SocialClubName = player.SocialClubName,
            };

            AddUser(user);
            await SaveChangesAsync();

            NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT:ShowAuthCef", false);
            NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT:ShowCreatePlayerForm", true, user.Id);
            NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT:SetUserId", user.Id);
        }

        [RemoteEvent("CLIENT:SERVER:CreatePlayer")]
        public async Task CreatePlayer(Player player, string firstName, string lastName, string userId)
        {
            Guid id = Guid.Parse(userId);

            if (!await IsPlayerExistByUserId(id) && !await IsPlayerExist(firstName, lastName))
            {
                return;
            }

            var newPlayer = new Models.CustomPlayer()
            {
                FirstName = firstName,
                LastName = lastName,
                UserId = id,
                Position = new PlayerPosition(),
                CreatedDate = DateTime.UtcNow
            };

            AddPlayer(newPlayer);
            await SaveChangesAsync();

            NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT:ShowCreatePlayerForm", false);

            var playerData = _mapper.Map<Entity.Player>(newPlayer);
            var hasData = player.HasData(Entity.Player.PLayerData);

            if (!hasData)
            {
                player.SetData(Entity.Player.PLayerData, playerData);
                player.Name = playerData.FirstName + " " + playerData.LastName;
                player.Position = playerData.Position;
            }
        }

        private void AddPlayer(CustomPlayer player)
        {
            _db.Players.Add(player);
        }

        private void AddUser(User user)
        {
            _db.Users.Add(user);
        }

        private async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        private bool IsNullOrEmpty(string value)
        {
            bool isNull = false;

            if (value?.Any() == false)
            {
                isNull = true;
            }

            return isNull;
        }

        private async Task<bool> IsExist(string login, string email, string socialName)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Login == login || x.Email == email || x.SocialClubName == socialName) == null;
        }

        private async Task<bool> IsPlayerExistByUserId(Guid id)
        {
            return await _db.Players.FirstOrDefaultAsync(x => x.UserId == id) == null;
        }

        private async Task<bool> IsPlayerExist(string firstName, string lastName)
        {
            return await _db.Players.FirstOrDefaultAsync(x => x.FirstName == firstName && x.LastName == lastName) == null;
        }
    }
}

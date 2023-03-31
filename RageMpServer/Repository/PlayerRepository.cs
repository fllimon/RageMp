using Microsoft.EntityFrameworkCore;
using RageMpServer.Contexts;
using RageMpServer.DatabaseEntities;
using RageMpServer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RageMpServer.Repository
{
    public class PlayerRepository
    {
        private readonly RageContext _db = null;

        public PlayerRepository()
        {
            _db = DbInitializer.GetInstance();
        }

        public async Task<CustomPlayer> GetPlayerByFirstLastNames(string firstName, string lastName)
        {
            return await _db.Players
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.FirstName == firstName && x.LastName == lastName);
        }

        public async Task UpdatePlayerPosition(CustomPlayer existingEntity, CustomPlayer currentPlayer)
        {
            _db.Entry(existingEntity.Position).CurrentValues.SetValues(currentPlayer.Position);
            _db.Entry(existingEntity).CurrentValues.SetValues(currentPlayer);

            await _db.SaveChangesAsync();
        }
    }
}

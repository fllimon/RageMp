using System;
using System.ComponentModel.DataAnnotations;

namespace RageMpServer.DatabaseEntities
{
    public class CustomPlayer
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public PlayerPosition Position { get; set; }

        public int Expirience { get; set; } = 0;

        public int Lvl { get; set; } = 0;

        public decimal Money { get; set; } = 2000;

        public int Health { get; set; } = 100;

        public int Armor { get; set; } = 0;

        public DateTime CreatedDate { get; set; }

        public CustomPlayer()
        {
            Id = Guid.NewGuid();
        }
    }
}

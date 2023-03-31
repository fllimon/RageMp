using System;
using System.ComponentModel.DataAnnotations;

namespace RageMpServer.DatabaseEntities
{
    public class User
    {
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string Login { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        public string Password { get; set; }

        [MaxLength(50)]
        public string SocialClubName { get; set; }

        public DateTime RegistryDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        public User()
        {
            Id = Guid.NewGuid();
        }
    }
}

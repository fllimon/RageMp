using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RageMpServer.Models
{
    class User
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

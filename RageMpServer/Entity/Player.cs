using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageMpServer.Entity
{
    class Player
    {
        public static readonly string PLayerData = "Player_Info";

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }

        public decimal Money { get; set; }

        public int Health { get; set; }

        public int Armor { get; set; } = 0;

        public int Expirience { get; set; }

        public int Lvl { get; set; } = 0;
    }
}

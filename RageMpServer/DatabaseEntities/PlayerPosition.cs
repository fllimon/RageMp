using System;
using System.Collections.Generic;
using System.Text;

namespace RageMpServer.DatabaseEntities
{
    public class PlayerPosition
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public CustomPlayer Player { get; set; }
        public float PositionX { get; set; } = -1042.7578f;
        public float PositionY { get; set; } = -2746.206f;
        public float PositionZ { get; set; } = 21.35938f;
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }

        public PlayerPosition()
        {
            Id = Guid.NewGuid();
        }
    }
}

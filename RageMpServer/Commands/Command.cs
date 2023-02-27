using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageMpServer.Commands
{
    internal class Command : Script
    {
        [RemoteEvent("CLIENT:SERVER:GetCoordinate")]
        public void GetCoordinates(Player player)
        {
            player.SendChatMessage($"POSITION: - X: {player.Position.X} - Y: {player.Position.Y} - Z: {player.Position.Z}");
        }
    }
}

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
            player.SendChatMessage($"POSITION: - X: {player.Position.X} - Z: {player.Position.X} - Z: {player.Position.Z}");
        }
    }
}

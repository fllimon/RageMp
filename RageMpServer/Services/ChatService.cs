using System.Collections.Generic;
using GTANetworkAPI;

namespace RageMpServer.Services
{
    public class ChatService
    {
        public void CustomChat(Player player, string message, double radius, string color)
        {
            List<Player> players = NAPI.Player.GetPlayersInRadiusOfPlayer(radius, player);

            foreach (var plr in players)
            {
                if (plr.Dimension != player.Dimension)
                {
                    continue;
                }
                else
                {
                    plr.SendChatMessage($"{color}{message}");
                }
            }
        }
    }
}

using GTANetworkAPI;

namespace RageMpServer.Services
{
    public class BlipService : Script
    {
        public void CreateBlip(int sprite, Vector3 position, byte color, float scale, string name, bool isShortRange)
        {
            NAPI.Blip.CreateBlip(sprite, position, scale, color, name, alpha: 255, 0, isShortRange);
        }
    }
}

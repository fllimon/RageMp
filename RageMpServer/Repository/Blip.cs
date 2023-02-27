using GTANetworkAPI;
using RageMpServer.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageMpServer.Repository
{
    public class Blip : Script
    {
        public void CreateBlip(int sprite, Vector3 position, byte color, float scale, string name, bool isShortRange)
        {
            NAPI.Blip.CreateBlip(sprite, position, scale, color, name, alpha: 255, 0, isShortRange);
        }
    }
}

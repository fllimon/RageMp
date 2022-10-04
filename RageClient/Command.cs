using RAGE;
using RAGE.Ui;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageClient
{
    internal class Command : Events.Script
    {
        public Command()
        {
            Input.Bind(VirtualKeys.P, down:true, GetCoordinate);
        }

        private void GetCoordinate()
        {
            Events.CallRemote("CLIENT:SERVER:GetCoordinate");
        }
    }
}

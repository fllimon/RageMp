using RAGE;
using RAGE.Ui;

namespace RageClient
{
    internal class Command : Events.Script
    {
        public Command()
        {
            Input.Bind(VirtualKeys.P, down:true, GetCoordinate);
            Input.Bind(VirtualKeys.F2, down:true, ShowCursor);
        }

        private void ShowCursor()
        {
            Cursor.ShowCursor(false, true);
        }

        private void GetCoordinate()
        {
            Events.CallRemote("CLIENT:SERVER:GetCoordinate");
        }
    }
}

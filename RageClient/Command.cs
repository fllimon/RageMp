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
            if (!Cursor.Visible)
            {
                Cursor.ShowCursor(false, false);
            }

            Cursor.ShowCursor(false, true);
        }

        private void GetCoordinate()
        {
            Events.CallRemote("CLIENT:SERVER:GetCoordinate");
        }
    }
}

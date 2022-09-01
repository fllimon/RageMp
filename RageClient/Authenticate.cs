using RAGE;
using RAGE.Elements;
using RAGE.Ui;
using System;

namespace RageClient
{
    public class Authenticate : Events.Script
    {
        private HtmlWindow _loginRegistrationForm = null;
        private HtmlWindow _createPlayerForm = null;
        private Guid _playerId = Guid.Empty;

        public Authenticate()
        {
            _loginRegistrationForm = new HtmlWindow("package://auth/LoginRegistration/index.html");
            _createPlayerForm = new HtmlWindow("package://auth/CreatePlayer/index.html");
            _createPlayerForm.Active = false;

            Events.Add("ShowAuthCef", ShowAuthCef);
            Events.Add("LoginInfoToClientEvent", LoginInfoToClientEvent);
            Events.Add("RegistrationInfoToClientEvent", RegistrationInfoToClientEvent);
            Events.Add("AuthError", LoginRegisrationError);

            Events.Add("ShowCreatePlayerForm", ShowCreatePlayerCef);
            Events.Add("CreatePlayerInfoToClientEvent", CreatePlayer);
            Events.Add("SetUserId", SetUserId);
        }

        private void SetUserId(object[] args)
        {
            _createPlayerForm.Call("SetUserId", (string)args[0]);
        }

        private void ShowCreatePlayerCef(object[] args)
        {
            ShowCEF(_createPlayerForm, (bool)args[0]);

            if ((bool)args[0] == false)
            {
                _createPlayerForm.Destroy();
                _createPlayerForm = null;
            }
        }

        private void CreatePlayer(object[] args)
        {
            Events.CallRemote("CreatePlayerInfoFromClientEvent", (string)args[0], (string)args[1], (string)args[2]);
        }

        private void LoginRegisrationError(object[] args)
        {
            _loginRegistrationForm.ExecuteJs("InvalidPassword()");
        }

        private void ShowAuthCef(object[] args)
        {
            ShowCEF(_loginRegistrationForm, (bool)args[0]);

            if ((bool)args[0] == false)
            {
                _loginRegistrationForm.Destroy();
                _loginRegistrationForm = null;
            }
        }

        private void ShowCEF(HtmlWindow window, bool flag)
        {
            window.Active = flag;
            RAGE.Ui.Console.Log(ConsoleVerbosity.Info, $"Window: {window.Active}");
            Cursor.Visible = flag;
            Player.LocalPlayer.FreezePosition(flag);
        }

        private void RegistrationInfoToClientEvent(object[] args)
        {
            Events.CallRemote("RegistrationInfoFromClientEvent", (string)args[0], (string)args[1], (string)args[2]);
        }

        private void LoginInfoToClientEvent(object[] args)
        {
            Events.CallRemote("LoginInfoFromClientEvent", (string)args[0], (string)args[1]);
        }
    }
}

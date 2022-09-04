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

        public Authenticate()
        {
            _loginRegistrationForm = new HtmlWindow("package://auth/LoginRegistration/index.html");
            _createPlayerForm = new HtmlWindow("package://auth/CreatePlayer/index.html");
            _createPlayerForm.Active = false;

            Events.Add("SERVER:CLIENT:ShowAuthCef", ShowAuthCef);
            Events.Add("CEF:CLIENT:SetLoginInfo", SendLoginInfo);
            Events.Add("CEF:CLIENT:Registration", RegisterUser);
            Events.Add("AuthError", LoginRegisrationError);

            Events.Add("SERVER:CLIENT:ShowCreatePlayerForm", ShowCreatePlayerCef);
            Events.Add("CEF:CLIENT:CreatePlayer", CreatePlayer);
            Events.Add("SERVER:CLIENT:SetUserId", SetUserId);
        }

        private void SetUserId(object[] args)
        {
            _createPlayerForm.Call("CLIENT:CEF:SetUserId", (string)args[0]);
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
            Events.CallRemote("CLIENT:SERVER:CreatePlayer", (string)args[0], (string)args[1], (string)args[2]);
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

        private void RegisterUser(object[] args)
        {
            Events.CallRemote("CLIENT:SERVER:RegisterUser", (string)args[0], (string)args[1], (string)args[2]);
        }

        private void SendLoginInfo(object[] args)
        {
            Events.CallRemote("CLIENT:SERVER:SendLoginInfo", (string)args[0], (string)args[1]);
        }
    }
}

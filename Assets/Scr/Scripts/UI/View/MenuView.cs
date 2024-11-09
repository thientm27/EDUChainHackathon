using Imba.UI;

namespace Scr.Scripts.UI.View
{
    public class MenuView : UIView
    {
        public void BTN_Setting()
        {
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.SettingPopup);
        }

        public void BtnPlay()
        {
            Hide();
            UIManager.Instance.ViewManager.ShowView(UIViewName.QuizView);
        }

        public void BtnEndGame()
        {
            Hide();
            UIManager.Instance.ViewManager.ShowView(UIViewName.MainView);
        }
    }
}
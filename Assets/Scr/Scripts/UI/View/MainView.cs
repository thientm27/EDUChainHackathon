using Imba.UI;

namespace Scr.Scripts.UI.View
{
    public class MainView : UIView
    {
        public void BTN_Setting()
        {
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.SettingPopup);
        }

        public void BtnStart()
        {
            UIManager.Instance.ViewManager.HideView(UIViewName.MainView);
            UIManager.Instance.ViewManager.ShowView(UIViewName.MenuView);
        }

        public void BtnRule()
        {
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.RulePopup);
        }
    }
}
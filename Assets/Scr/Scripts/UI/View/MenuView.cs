using Imba.UI;

namespace Scr.Scripts.UI.View
{
    public class MenuView : UIView
    {
        public void BtnXuatQuan()
        {
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.XuatQuanPopup);
        }

        public void BtnScan()
        {
            Hide();
            UIManager.Instance.ViewManager.ShowView(UIViewName.ScannerView);
        }

        public void BtnRollDice()
        {
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.RollDicePopup);
        }
        public void BtnEndGame()
        {
            Hide();
            UIManager.Instance.ViewManager.ShowView(UIViewName.MainView);
        }
    }
}
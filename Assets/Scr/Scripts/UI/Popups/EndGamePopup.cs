using Imba.UI;
using UnityEngine.SceneManagement;

namespace Scr.Scripts.UI.Popups
{
    public class EndGamePopup : UIPopup
    {
        public void ButtonClaim()
        {
            SceneManager.LoadScene("Scr/Scenes/GameScene");
        }
    }
}
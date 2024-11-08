using Imba.UI;
using UnityEngine;

namespace Scr.Scripts.GameScene
{
    public class GameController : MonoBehaviour
    {
        private void Start()
        {
            UIManager.Instance.ViewManager.ShowView(UIViewName.MainView);
        }
    }
}
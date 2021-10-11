using UnityEngine;
using UnityEngine.UI;

namespace Mew.UI
{
    [RequireComponent(typeof(Animator))]
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Image foreground;

        public void ShowController(int player)
        {
            foreground.color = Constants.Colors.PlayerColor[player];
            background.color = Constants.Colors.PlayerColorLight[player];
        }

        public void BumpController()
        {
            GetComponent<Animator>().Play("ControllerBump");
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Mew.UI
{
    [RequireComponent(typeof(Image))]
    public class ScoreCard : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Text playerNameText;
        [SerializeField] private Image winIcon;

        public void Initialize(int player)
        {
            playerNameText.text = $"Player {player + 1}";
            GetComponent<Image>().color = Constants.Colors.PlayerColor[player];
            scoreText.color = playerNameText.color = winIcon.color = Constants.Colors.PlayerColorLight[player];
            winIcon.gameObject.SetActive(false);
        }

        public void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }

        public void AddWin()
        {
            var win = Instantiate(winIcon, winIcon.transform.parent);
            win.gameObject.SetActive(true);
        }
    }
}

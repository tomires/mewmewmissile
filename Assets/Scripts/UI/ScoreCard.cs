using UnityEngine;
using UnityEngine.UI;

namespace Mew.UI
{
    [RequireComponent(typeof(Image))]
    public class ScoreCard : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Animator scoreTextAnimator;
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
            scoreTextAnimator.Play(Constants.Animations.ScoreTextIncrement);
        }

        public void AddWin()
        {
            var win = Instantiate(winIcon, winIcon.transform.parent);
            win.gameObject.SetActive(true);
            win.GetComponent<Animator>().Play(Constants.Animations.WinIconFadeIn);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Mew.UI
{
    [RequireComponent(typeof(Image))]
    public class ScoreCard : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Text playerNameText;

        public void Initialize(int player)
        {
            playerNameText.text = $"Player {player + 1}";
            GetComponent<Image>().color = Constants.Colors.PlayerColor[player];
            scoreText.color = playerNameText.color = Constants.Colors.PlayerColorLight[player];
        }

        public void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }

        public void SetWins(int wins)
        {
            //scoreText.text = score.ToString();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mew.Objects;

namespace Mew.Managers
{
    public class Results : MonoSingleton<Results>
    {
        [SerializeField] private List<Rocket> rockets;
        [SerializeField] private Text winnerText;
        [SerializeField] private Animator cameraAnimator;

        void Start()
        {
            var winner = PlayerRoster.Instance.GetWinner();
            winnerText.color = Constants.Colors.PlayerColorLight[winner];
            winnerText.text = $"Congratulations Player {winner + 1}!";

            foreach (var rocket in rockets)
            {
                rocket.GetComponent<Animator>().Play("RocketRotate");
                rocket.Initialize(winner);
            }

            cameraAnimator.Play("CameraResults");
            Audio.Instance.PlayMusic(Models.GameState.GameOver);
        }

        public void ReturnToMenu()
        {
            PlayerRoster.Instance.RemoveAllPlayers();
            PlayerRoster.Instance.JoiningEnabled = true;
            SceneManager.LoadScene(Constants.Scenes.Menu);
        }
    }
}

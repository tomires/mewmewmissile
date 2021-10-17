using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Mew.Models;
using Mew.UI;

namespace Mew.Managers
{
    public class Menu : MonoSingleton<Menu>
    {
        [SerializeField] GameObject controllerPrefab;
        [SerializeField] Transform controllersParent;
        private List<Controller> _controllers = new List<Controller>();

        IEnumerator Start()
        {
            yield return null;
            Audio.Instance.PlayMusic(GameState.Menu);

            for (int c = 0; c < Constants.Settings.MaxPlayerCount; c++)
            {
                var controller = Instantiate(controllerPrefab, controllersParent);
                _controllers.Add(controller.GetComponent<Controller>());
            }
        }

        public void StartGame()
        {
            PlayerRoster.Instance.JoiningEnabled = false;
            SceneManager.LoadScene(Constants.Scenes.Game);
        }

        public void ShowController(int player)
        {
            _controllers[player].ShowController(player);
        }

        public void BumpController(int player)
        {
            _controllers[player].BumpController();
        }
    }
}

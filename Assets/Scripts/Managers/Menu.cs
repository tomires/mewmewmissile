using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Mew.Models;

namespace Mew.Managers
{
    public class Menu : MonoSingleton<Menu>
    {
        [SerializeField] List<Image> controllers;

        IEnumerator Start()
        {
            yield return null;
            Audio.Instance.PlayMusic(GameState.Menu);
        }

        public void StartGame()
        {
            PlayerRoster.Instance.JoiningEnabled = false;
            SceneManager.LoadScene(Constants.Scenes.Game);
        }

        public void ShowController(int player)
        {
            Debug.Log(player);
            controllers[player].color = Constants.Colors.PlayerColor[player];
        }

        public void BumpController(int player)
        {
            controllers[player].GetComponent<Animator>().Play("ControllerBump");
        }
    }
}

using UnityEngine.SceneManagement;
using System.Collections;
using Mew.Models;

namespace Mew.Managers
{
    public class Menu : MonoSingleton<Menu>
    {
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
    }
}

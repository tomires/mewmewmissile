using UnityEngine;
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

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene(Constants.Scenes.Game);
        }
    }
}

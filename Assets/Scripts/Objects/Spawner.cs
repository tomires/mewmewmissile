using UnityEngine;
using System.Collections;
using Mew.Models;
using Mew.Managers;

namespace Mew.Objects
{
    public class Spawner : MonoBehaviour
    {
        private Direction _direction;
        private Vector2 _coordinates;

        public void Initialize(Vector2 coordinates, Direction direction)
        {
            _coordinates = coordinates;
            _direction = direction;
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(Constants.Settings.StartCooldown);

            while (true)
            {
                var random = Random.Range(0, Constants.Settings.DicePositions);
                if (Constants.Settings.CatDiceThrows.Contains(random)
                    && Game.Instance.CurrentMode != GameState.MouseMania)
                    SpawnCat();
                else if (Constants.Settings.MouseDiceThrows.Contains(random)
                    && Game.Instance.CurrentMode != GameState.CatMania)
                    SpawnMouse();

                yield return new WaitForSeconds(Game.Instance.SpawnRate);
            }
        }

        private void SpawnMouse()
        {
            if (!Game.Instance.MouseSpawnable) return;
            var mouse = Instantiate(Game.Instance.MousePrefab).GetComponent<Creature>();
            mouse.Initialize(_coordinates, _direction);
        }

        private void SpawnCat()
        {
            if (!Game.Instance.CatSpawnable) return;
            var cat = Instantiate(Game.Instance.CatPrefab).GetComponent<Creature>();
            cat.Initialize(_coordinates, _direction);
        }
    }
}

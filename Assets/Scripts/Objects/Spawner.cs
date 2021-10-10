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
            while (true)
            {
                var mouse = Instantiate(Game.Instance.MousePrefab).GetComponent<Creature>();
                mouse.Initialize(_coordinates, _direction);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}

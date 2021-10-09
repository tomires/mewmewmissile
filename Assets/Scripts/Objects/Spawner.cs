using UnityEngine;
using Mew.Models;

namespace Mew.Objects
{
    public class Spawner : MonoBehaviour
    {
        private Direction _direction;

        public void Initialize(Direction direction)
        {
            _direction = direction;
        }
    }
}

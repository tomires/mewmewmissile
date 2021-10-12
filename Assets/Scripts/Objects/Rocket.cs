using System.Collections.Generic;
using UnityEngine;
using Mew.Managers;

namespace Mew.Objects
{
    public class Rocket : MonoBehaviour
    {
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private List<MeshRenderer> renderers;

        int _player;

        public void Initialize(int player)
        {
            _player = player;
            var material = Instantiate(defaultMaterial);
            material.color = Constants.Colors.PlayerColor[player];

            foreach (var renderer in renderers)
                renderer.material = material;
        }

        public void PropagateMouseGain(bool bonus)
        {
            Audio.Instance.PlayOneShot(Audio.Sound.Mouse);
            PlayerRoster.Instance.PropagateMouseGain(_player, bonus);
            BumpRocket();
        }

        public void PropagateCatHit()
        {
            Audio.Instance.PlayOneShot(Audio.Sound.Explosion);
            PlayerRoster.Instance.PropagateCatHit(_player);
            BumpRocket();
        }

        public void BlastRocket()
        {
            GetComponent<Animator>().Play("RocketBlastOff");
        }

        private void BumpRocket()
        {
            GetComponent<Animator>().Play("RocketBump");
        }
    }
}

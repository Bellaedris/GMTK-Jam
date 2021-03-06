using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestGame.Player;
using UnityEngine.AI;

namespace TestGame
{
    public class EMPController : MonoBehaviour
    {

        public float divisionFactor = 2f; //how much the EMP decreases the speed of entities in it
        public float duration = 10f;

        private PlayerController player;
        private List<NavMeshAgent> bots = new List<NavMeshAgent>();

        private void Start() {
            StartCoroutine("AutoDestroy");
        }

        private void OnTriggerEnter(Collider other) {
            player = other.gameObject.GetComponent<PlayerController>();
            NavMeshAgent bot = other.gameObject.GetComponent<NavMeshAgent>();

            if (player != null) {
                player.DivideCurrentSpeed(divisionFactor);
            }

            if (bot != null) {
                bots.Add(bot);
                bot.speed /= divisionFactor;
            }
        }

        private void OnTriggerExit(Collider other) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            NavMeshAgent bot = other.gameObject.GetComponent<NavMeshAgent>();

            if (player != null) {
                player.RestoreBaseSpeed();
            }

            if (bot != null) {
                bots.Remove(bot);
                bot.speed *= divisionFactor;
            }
        }

        private IEnumerator AutoDestroy() {
            yield return new WaitForSeconds(duration);
            if (player != null)
                player.RestoreBaseSpeed();
                
            foreach(NavMeshAgent bot in bots) {
                bot.speed *= divisionFactor;
            }
    
            Destroy(this.gameObject);
        }
    }
}

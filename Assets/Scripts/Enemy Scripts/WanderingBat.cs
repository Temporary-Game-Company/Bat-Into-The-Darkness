using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McgillTeam3
{
    public class WanderingBat : MonoBehaviour
    {
        GameObject player;
        Vector3 distance;
        Vector2 velocity;
        bool diving = false;
        [SerializeField] private SpriteRenderer Renderer;

        // Start is called before the first frame update
        
        private void OnEnable()
        {
            player = GameObject.FindWithTag("Player");
            
            Echolocation.OnStartEcholocate += OnStartEcholocate;
        }

        private void OnDisable()
        {
            Echolocation.OnStartEcholocate -= OnStartEcholocate;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!diving){
                gameObject.transform.position = new Vector3 (gameObject.transform.position[0] - CaveGen.speed * SpeedController.speed, gameObject.transform.position[1], gameObject.transform.position[2]);
            }
            else gameObject.transform.position += (Vector3) velocity;

            if (!Renderer.isVisible && transform.position.x < 0) {
                GameObject.Destroy(gameObject);
                Debug.Log("Culled");
            }
        }

        void OnStartEcholocate(){
            if(!diving){
                Vector2 distance = (Vector2) player.transform.position - (Vector2) gameObject.transform.position;
                if (distance.magnitude < 8){
                    diving = true;
                    velocity = distance.normalized * (0.2f * SpeedController.speed);
                }
            }
        }
    }
}

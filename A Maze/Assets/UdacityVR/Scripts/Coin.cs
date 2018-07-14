using UnityEngine;
using _MyScripts;

namespace UdacityVR.Scripts {
    public class Coin : MonoBehaviour {
        [HideInInspector] public static int NumOfCoinsCreated = 0;

        // TODO: Create variables to reference the game objects we need access to
        // Declare a GameObject named 'coinPoofPrefab' and assign the 'CoinPoof' prefab to the field in Unity
        [SerializeField] private GameObject _coinPoofPrefab;
        [SerializeField] private float      _rotationSpeed = 50f;

        void Update() {
            // OPTIONAL-CHALLENGE: Animate the coin rotating
            // TIP: You could use a method from the Transform class
            transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
        }


        public void OnCoinClicked() {
            /// Called when the 'Coin' game object is clicked
            /// - Displays a poof effect (handled by the 'CoinPoof' prefab)
            /// - Plays an audio clip (handled by the 'CoinPoof' prefab)
            /// - Removes the coin from the scene

            // Prints to the console when the method is called
            Debug.Log("'Coin.OnCoinClicked()' was called");

            NumOfCoinsCreated--;
            if (NumOfCoinsCreated <= 1) {
                MyGameManager.Self.AllCoinsCollected();
            }

            // TODO: Display the poof effect and remove the coin from the scene
            // Use Instantiate() to create a clone of the 'CoinPoof' prefab at this coin's position and with the 'CoinPoof' prefab's rotation
            // Use Destroy() to delete the coin after for example 0.5 seconds

            if (_coinPoofPrefab != null) {
                Instantiate(_coinPoofPrefab, transform.position, _coinPoofPrefab.transform.rotation);
            } else {
                Debug.LogError("coinPoofPrefab is not assigned", gameObject);
            }

            Destroy(gameObject);
        }
    }
}
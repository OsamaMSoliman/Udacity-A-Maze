using MazeGenerator.Scripts;
using UdacityVR.Scripts;
using UnityEngine;

namespace _MyScripts {
    public class MyGameManager : MonoBehaviour {
        [SerializeField] private Transform   _player;
        [SerializeField] private MazeSpawner _mazeSpawner;
        [SerializeField] private Transform   _mazeExit;
        [SerializeField] private Door        _door;

        private bool _isKeyTaken        = false;
        private bool _allCoinsCollected = false;

        public static MyGameManager Self;
        private       void          Awake() { Self = this; }


        // Use this for initialization
        void Start() {
            if (_player      == null) Debug.LogError("Player is not set in the game manager",       gameObject);
            if (_mazeSpawner == null) Debug.LogError("_mazeSpawner is not set in the game manager", gameObject);
            if (_mazeExit    == null) Debug.LogError("_mazeExit is not set in the game manager",    gameObject);
            if (_door        == null) Debug.LogError("_door is not set in the game manager",        gameObject);
        }

        public void OnFountainClicked() {
            int randomIndex = Random.Range(0, _mazeSpawner.WayPoints.childCount);
            _player.position = _mazeSpawner.WayPoints.GetChild(randomIndex).position;
        }

        public void AllCoinsCollected() {
            _allCoinsCollected = true;
            if (_isKeyTaken) ExitTheMaze();
        }

        public void UnlockDoor() {
            _isKeyTaken = true;
            if (_door != null) {
                _door.Unlock();
            } else {
                Debug.LogError("_door is not set", gameObject);
            }

            if (_allCoinsCollected) ExitTheMaze();
        }

        private void ExitTheMaze() {
            _player.position = _mazeExit.position;
        }
    }
}
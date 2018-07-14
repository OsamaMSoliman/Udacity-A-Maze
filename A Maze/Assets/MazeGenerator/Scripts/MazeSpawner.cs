using UdacityVR.Scripts;
using UnityEngine;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
namespace MazeGenerator.Scripts {
    public class MazeSpawner : MonoBehaviour {
        public enum MazeGenerationAlgorithm {
            PureRecursive,
            RecursiveTree,
            RandomTree,
            OldestTree,
            RecursiveDivision,
        }

        public MazeGenerationAlgorithm Algorithm  = MazeGenerationAlgorithm.PureRecursive;
        public bool                    FullRandom = false;
        public int                     RandomSeed = 12345;
        public GameObject              Floor      = null;
        public GameObject              Wall       = null;
        public GameObject              Pillar     = null;
        public int                     Rows       = 5;
        public int                     Columns    = 5;
        public float                   CellWidth  = 5;
        public float                   CellHeight = 5;
        public bool                    AddGaps    = true;

        private BasicMazeGenerator mMazeGenerator = null;

        #region added for Udacity project

        [SerializeField] private Key      _keyPrefab;
        [SerializeField] private Coin     _coinPrefab;
        [SerializeField] private int      numOfCoinToPass = 5;
        [SerializeField] private Waypoint _waypointPrefab;
        [SerializeField] private float    _waypointHeight = 2;

        private bool       _keyExists = false;
        private GameObject _waypointsHolder;
        public  Transform  WayPoints { get { return _waypointsHolder.transform; } }

        #endregion

        void Start() {
            _waypointsHolder = new GameObject("_waypointsHolder");
            _waypointsHolder.transform.SetParent(transform);
            GameObject coinsHolder = new GameObject("_coinsHolder");
            coinsHolder.transform.SetParent(transform);
            GameObject wallsHolder = new GameObject("_wallsHolder");
            wallsHolder.transform.SetParent(transform);
            GameObject floorHolder = new GameObject("_floorHolder");
            floorHolder.transform.SetParent(transform);
            GameObject pillarsHolder = new GameObject("_pillarsHolder");
            pillarsHolder.transform.SetParent(transform);


            if (!FullRandom) {
                Random.seed = RandomSeed;
            }

            switch (Algorithm) {
                case MazeGenerationAlgorithm.PureRecursive:
                    mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
                    break;
                case MazeGenerationAlgorithm.RecursiveTree:
                    mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
                    break;
                case MazeGenerationAlgorithm.RandomTree:
                    mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
                    break;
                case MazeGenerationAlgorithm.OldestTree:
                    mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
                    break;
                case MazeGenerationAlgorithm.RecursiveDivision:
                    mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
                    break;
            }

            int coinsPlaced = 0;
            mMazeGenerator.GenerateMaze();
            for (int row = 0; row < Rows; row++) {
                for (int column = 0; column < Columns; column++) {
                    float      x    = transform.position.x + column * (CellWidth  + (AddGaps ? .2f : 0));
                    float      z    = transform.position.z + row    * (CellHeight + (AddGaps ? .2f : 0));
                    MazeCell   cell = mMazeGenerator.GetMazeCell(row, column);
                    GameObject tmp;
                    tmp                  = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = floorHolder.transform;
                    if (cell.WallRight) {
                        tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position,
                                          Quaternion.Euler(0, 90, 0)) as GameObject; // right
                        tmp.transform.parent = wallsHolder.transform;
                    }

                    if (cell.WallFront) {
                        tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position,
                                          Quaternion.Euler(0, 0, 0)) as GameObject; // front
                        tmp.transform.parent = wallsHolder.transform;
                    }

                    if (cell.WallLeft) {
                        tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position,
                                          Quaternion.Euler(0, 270, 0)) as GameObject; // left
                        tmp.transform.parent = wallsHolder.transform;
                    }

                    if (cell.WallBack) {
                        tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position,
                                          Quaternion.Euler(0, 180, 0)) as GameObject; // back
                        tmp.transform.parent = wallsHolder.transform;
                    }

                    if (cell.IsGoal && _coinPrefab != null && Coin.NumOfCoinsCreated < numOfCoinToPass) {
                        Coin coin = Instantiate(_coinPrefab, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0));
                        coin.transform.SetParent(coinsHolder.transform);
                        Coin.NumOfCoinsCreated++;
                    } else {
                        if (!_keyExists && _keyPrefab != null) {
                            Key key = Instantiate(_keyPrefab, new Vector3(x, _waypointHeight, z), _keyPrefab.transform.rotation);
                            key.transform.SetParent(transform);
                            _keyExists = true;
                        } else if (_waypointPrefab != null) {
                            Waypoint wp = Instantiate(_waypointPrefab, new Vector3(x, _waypointHeight, z), Quaternion.identity);
                            wp.transform.SetParent(_waypointsHolder.transform);
                        }
                    }
                }
            }

            if (Pillar != null) {
                for (int row = 0; row < Rows + 1; row++) {
                    for (int column = 0; column < Columns + 1; column++) {
                        float x = transform.position.x + column * (CellWidth  + (AddGaps ? .2f : 0));
                        float z = transform.position.z + row    * (CellHeight + (AddGaps ? .2f : 0));
                        GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2),
                                                     Quaternion.identity);
                        tmp.transform.parent = pillarsHolder.transform;
                        if (row != 0 && column != 0 && row != Rows && column != Columns)
                            tmp.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
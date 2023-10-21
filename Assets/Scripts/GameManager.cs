using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask boardLayer;

    [SerializeField]
    private Disc discBlackUp;

    [SerializeField]
    private Disc discWhiteUp;

    private Dictionary<Player, Disc> discPrefabs = new Dictionary<Player, Disc>();
    private GameState gameState = new GameState();
    private Disc[,] discs = new Disc[8, 8];

    // Start is called before the first frame update
    private void Start()
    {
        discPrefabs[Player.Black] = discBlackUp;
        discPrefabs[Player.White] = discWhiteUp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Position SceneToBoardPos(Vector3 scenePos)
    {
        // the 7 because position matrix from top left, but unity matrix from bottom left
        int row = 7 - (int)(scenePos.z - 0.25f);
        int col = (int)(scenePos.x - 0.25f);

        return new Position(row, col);
    }
}

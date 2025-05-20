using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set;  }
    public Vector3Int[] cells { get; private set;  }

    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float moveDelay = 0.1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float moveTime;
    private float lockTime;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.data = data;
        this.board = board;
        this.position = position;

        rotationIndex = 0;
        stepTime = Time.time + stepDelay;
        moveTime = Time.time + moveDelay;
        lockTime = 0f;

        if (cells == null)
        {
            cells = new Vector3Int[data.cells.Length];
        }

        for(int i = 0;  i < cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cells[i];
        }

    }

    // Update is called once per frame
    private void Update()
    {
        board.Clear(this);

        lockTime += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        } 
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Time.time > moveTime)
        {
            HandleMoveInputs();
        }
        if(Time.time > stepTime)
        {
            Step();
        }

        board.Set(this);
    }
}

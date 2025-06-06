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

    /// <summary>
    /// ゲームの初期化
    /// </summary>
    /// <param name="board"></param>
    /// <param name="position"></param>
    /// <param name="data"></param>
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
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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

    /// <summary>
    /// 入力処理
    /// </summary>
    private void HandleMoveInputs()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (Move(Vector2Int.down))
            {
                stepTime = Time.time + stepDelay;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Move(Vector2Int.right);
        }
    }

    private void Step()
    {
        stepTime = Time.time + stepDelay;

        Move(Vector2Int.down);

        if(lockTime >= lockDelay)
        {
            Lock();
        }

    }

    private void HardDrop()
    {
        while(Move(Vector2Int.down))
        {
            continue;
        }
        Lock();
    }

    private void Lock()
    {
        board.Set(this);
        board.ClearLines();
        board.SpawnPiece();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="translation"></param>
    /// <returns>移動可能かどうか</returns>
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = board.IsValidPosition(this, newPosition);

        if(valid)
        {
            position = newPosition;
            moveTime = Time.time + moveDelay;
            lockTime = 0f;
        }

        return valid;
    }

    /// <summary>
    /// ピースの回転処理
    /// </summary>
    /// <param name="direction"></param>
    private void Rotate(int direction)
    {
        int originalRotation = rotationIndex;

        rotationIndex = Wrap(rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);

        if(!TestWallKicks(rotationIndex, direction))
        {
            rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    /// <summary>
    /// 適用するべき回転行列
    /// </summary>
    /// <param name="direction"></param>
    private void ApplyRotationMatrix(int direction)
    {
        float[] matrix = Data.RotationMatrix;

        for(int i = 0; i < cells.Length; i++)
        {
            Vector3 cell = cells[i];
            int x, y;

            switch (data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;
            }

            cells[i] = new Vector3Int(x, y, 0);

        }
    }

    /// <summary>
    /// ウォールキックテスト関数
    /// </summary>
    /// <param name="rotationIndex"></param>
    /// <param name="rotationDirection"></param>
    /// <returns>できるかどうか</returns>
    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for(int i = 0; i < data.wallKicks.GetLength(i); i++)
        {
            Vector2Int translation = data.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }


        return false;
    }

    /// <summary>
    /// ウォールキックの処理を行う関数
    /// </summary>
    /// <param name="rotationIndex"></param>
    /// <param name="rotationDirection"></param>
    /// <returns>回転入れ</returns>
    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int walKickIndex = rotationIndex * 2;
        if(rotationDirection < 0 )
        {
            walKickIndex--;
        }

        return Wrap(walKickIndex, 0, data.wallKicks.GetLength(0));
    }

    /// <summary>
    /// 回転入れ
    /// </summary>
    /// <param name="input"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private int Wrap(int input, int min, int max)
    {
        if(input < min )
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}

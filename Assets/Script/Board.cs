using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

[DefaultExecutionOrder(-1)]
public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }

    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    /// <summary>
    /// �^�C���}�b�v�ƃe�g���~�m��������
    /// </summary>
    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponent<Piece>();

        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    /// <summary>
    /// �V�����s�[�X�𐶐�
    /// </summary>
    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            GameOver();
            
        }
    }

    /// <summary>
    /// �Q�[���I�[�o�[����
    /// </summary>
    public async void GameOver()
    {
        tilemap.ClearAllTiles();

        activePiece.enabled = false;

        // �ǉ�
        await SceneManager.LoadSceneAsync("Title");
        await Task.Delay(500);
        
    }

    /// <summary>
    /// ����������s�[�X���Z�b�g����
    /// </summary>
    /// <param name="piece"></param>
    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    /// <summary>
    /// �s�[�X�̍폜
    /// </summary>
    /// <param name="piece"></param>
    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    /// <summary>
    /// �L���ȏꏊ
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="position"></param>
    /// <returns>true: �L�� false: ����</returns>
    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false; 
            }

            if(tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// �����̍s�̃N���A
    /// </summary>
    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        while(row < bounds.yMax)
        {
            if(IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }
    /// <summary>
    /// �s�������ς����ǂ���
    /// </summary>
    /// <param name="row"></param>
    /// <returns>ture:�����ς� false: �����łȂ�</returns>
    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for(int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if(!tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 1���C���̃N���A
    /// </summary>
    /// <param name="row"></param>
    public void LineClear(int row)
    {
        RectInt bounds = Bounds;

        for(int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        while(row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }
}



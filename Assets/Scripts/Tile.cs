using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UIElements;

public enum TileType {
    Plains,
    Wall,
    Woods,
    River,
    Road
}


public class Tile : MonoBehaviour
{

    // NOTE : This are the game object that will be displayed on the tile depending on it's type
    [Header("Visuals Assets References")]
    public GameObject woods;
    public GameObject wall;
    public GameObject road;
    public GameObject river;

    // === Tile values ===
    private TileType tileType = TileType.Plains;
    private int xCoord;
    private int yCoord;
    [SerializeField] private Transform pathTransform;

    // === Components ===
    private TextMeshProUGUI tileText;
    private Renderer tileRenderer;

    // === SETTERS ===
    public void SetText(string text) { tileText.text = text; }
    public void SetColor(Color color) { tileRenderer.material.color = color; }

    public bool SetTileType(TileType type)
    {
        tileType = type;
        woods.SetActive(false);
        wall.SetActive(false);
        road.SetActive(false);
        river.SetActive(false);


        // Activate differents visuals assets based on the tile type
        switch (tileType) {

            case TileType.Wall:
                wall.SetActive(true);
                break;

            case TileType.Woods:
                woods.SetActive(true);
                break;

            case TileType.Road:
                road.SetActive(true);
                break;

            case TileType.River:
                river.SetActive(true);
                break;
        }
        
        // Returns a bool so it can be used efficiently in MapManager
        return true;
    }


    // === GETTERS ===
    public int GetXCoord() { return xCoord; }
    public int GetYCoord() { return yCoord; }
    public Vector3 GetPathPos() { return pathTransform.position; }
    public TileType GetTileType() { return tileType; }
    public Color GetColor() { return tileRenderer.material.color; }
    public string GetText() { return tileText.text; }
    // NOTE I wanted to avoid a switch case with magics numbers but for a project this small the other approach seemed overkill
    public float GetCost()
    {
        switch (tileType){

            case TileType.Road:
                return .5f;

            case TileType.Plains:
                return 1f;

            case TileType.Woods:
                return 3f;

            case TileType.River:
                return 5f;

            default:
                return 1f;
        }
    } 

    // NOTE : Need to be awake so it's initialised before MapGenerator Start
    private void Awake()
    {
        // Get the components
        tileText = GetComponentInChildren<TextMeshProUGUI>();
        tileRenderer = GetComponent<Renderer>();
    }

    public void Init(int x, int y)
    {
        xCoord = x;
        yCoord = y;
        name = "Tile_" + x + "_" + y;
    }
}

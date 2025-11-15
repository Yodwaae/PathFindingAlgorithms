using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UIElements;

public enum TileType {
    Plains,
    Wall,
    Woods
}

// TODO : See how I can improve the tile enum/system with scriptable so that the visual assets and cost of the tile are directly linked

public class Tile : MonoBehaviour
{

    // NOTE : This are the game object that will be displayed on the tile depending on it's type
    [Header("Visuals Assets References")]
    public GameObject woods;
    public GameObject wall;


    // === Tile values ===
    private TileType tileType = TileType.Plains;
    private int xCoord;
    private int yCoord;

    // === Components ===
    private TextMeshProUGUI tileText;
    private Renderer tileRenderer;

    // === SETTERS ===
    public void SetText(string text) { tileText.text = text; }
    public void SetColor(Color color) { tileRenderer.material.color = color; }

    public void SetTileType(TileType type)
    {
        tileType = type;

        // Activate differents visuals assets based on the tile type
        switch (tileType) {
            case TileType.Plains:
                woods.SetActive(false);
                wall.SetActive(false);
                break;

            case TileType.Wall:
                woods.SetActive(false);
                wall.SetActive(true);
                break;

            case TileType.Woods:
                woods.SetActive(true);
                wall.SetActive(false);
                break;
        }
    }


    // === GETTERS ===
    public int GetXCoord() { return xCoord; }
    public int GetYCoord() { return yCoord; }
    public TileType GetTileType() { return tileType; }
    public Color GetColor() { return tileRenderer.material.color; }
    public string GetText() { return tileText.text; }
    public int GetCost(){ return 1; } // TODO To implement but I want to avoid a simple switch/Case with magics numbers

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

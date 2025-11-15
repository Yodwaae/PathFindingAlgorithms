using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UIElements;

public enum TileType {
    Plains,
    Wall,
    Wood
}

// TODO : See how I can improve the tile enum/system with scriptable so that the visual assets and cost of the tile are directly linked

public class Tile : MonoBehaviour
{

    // NOTE : This are the game object that will be displayed on the tile depending on it's type
    [Header("Visuals Assets References")]
    public GameObject wood;
    public GameObject wall;


    // === Tile values ===
    private TileType tileType = TileType.Plains;
    private int xCoord;
    private int yCoord;

    // === Components ===
    private TextMeshPro tileText;
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
                wood.SetActive(false);
                wall.SetActive(false);
                break;

            case TileType.Wall:
                wood.SetActive(false);
                wall.SetActive(true);
                break;

            case TileType.Wood:
                wood.SetActive(true);
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
    public int GetCost(){ return 0; } // TODO To implement but I want to avoid a simple switch/Case with magics numbers

    private void Start()
    {
        // Get the components
        tileText = GetComponentInChildren<TextMeshPro>();
        tileRenderer = GetComponent<Renderer>();
    }

    public void Init(int x, int y)
    {
        xCoord = x;
        yCoord = y;
        name = "Tile_" + x + "_" + y;
    }
}

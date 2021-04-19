using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private GameObject Hexagon;
    public int width = 9;
    public int height = 8;
    public static GameObject[,] Hexagons;
    public Color[] Colors;
    private int ColorId;

    private float xOffset = 1.55f;
    private float zOffset = 1.8f;
    
    void Start()
    {
        Hexagons = new GameObject[width,height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float zPos = y * zOffset;

                if (x % 2 == 1)
                {
                    zPos += (zOffset / 2f) - zOffset;
                }
                ColorId = Random.Range(0, Colors.Length);
                //This is for not matching in the beginning
                CreateBoardWithoutMatch(x, y, Hexagons);
                //-----
                GameObject hex_go = (GameObject)Instantiate(Hexagon, new Vector3(x * xOffset , 0, zPos), Quaternion.identity);
                hex_go.name = x + "_" + y;
                hex_go.transform.SetParent(this.transform);
                hex_go.GetComponentInChildren<MeshRenderer>().material.color = Colors[ColorId];
                hex_go.GetComponent<HexagonID>().HexColor = ColorId;
                Hexagons[x, y] = hex_go;
            }
        }
    }

    void Update()
    {
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Hexagons[x, y] != null && (y + 1 < height && Hexagons[x, y + 1] != null) && (x - 1 > width && y + 1 < height && Hexagons[x - 1, y + 1] != null))
                {
                    if (Hexagons[x, y].GetComponent<HexagonID>().HexColor == Hexagons[x, y + 1].GetComponent<HexagonID>().HexColor && Hexagons[x, y].GetComponent<HexagonID>().HexColor == Hexagons[x - 1, y + 1].GetComponent<HexagonID>().HexColor)
                    {
                        Hexagons[x, y].SetActive(false);
                        Hexagons[x, y + 1].SetActive(false);
                        Hexagons[x, y + 1].SetActive(false);
                    }
                }
                else if (Hexagons[x, y] != null && Hexagons[x, y + 1] != null && Hexagons[x + 1, y] != null)
                {
                    if (Hexagons[x, y].GetComponent<HexagonID>().HexColor == Hexagons[x, y + 1].GetComponent<HexagonID>().HexColor && Hexagons[x, y].GetComponent<HexagonID>().HexColor == Hexagons[x + 1, y].GetComponent<HexagonID>().HexColor)
                    {
                        Hexagons[x, y].SetActive(false);
                        Hexagons[x, y + 1].SetActive(false);
                        Hexagons[x + 1, y].SetActive(false);
                    }
                }
                else if (Hexagons[x, y] != null && Hexagons[x - 1, y + 1] != null && Hexagons[x - 2, y] != null)
                {
                    if (Hexagons[x, y].GetComponent<HexagonID>().HexColor == Hexagons[x - 1, y + 1].GetComponent<HexagonID>().HexColor && Hexagons[x, y].GetComponent<HexagonID>().HexColor == Hexagons[x - 2, y].GetComponent<HexagonID>().HexColor)
                    {
                        Hexagons[x, y].SetActive(false);
                        Hexagons[x - 1, y + 1].SetActive(false);
                        Hexagons[x - 2, y].SetActive(false);
                    }
                }
                else if (Hexagons[x, y] != null && Hexagons[x + 1, y] != null && Hexagons[x, y - 1] != null)
                {
                    if (Hexagons[x, y].GetComponent<HexagonID>().HexColor == Hexagons[x + 1, y].GetComponent<HexagonID>().HexColor && Hexagons[x, y].GetComponent<HexagonID>().HexColor == Hexagons[x, y - 1].GetComponent<HexagonID>().HexColor)
                    {
                        Hexagons[x, y].SetActive(false);
                        Hexagons[x + 1, y].SetActive(false);
                        Hexagons[x, y - 1].SetActive(false);
                    }
                }
                else if (Hexagons[x, y] != null && Hexagons[x, y + 1] != null && Hexagons[x + 1, y + 1] != null)
                {
                    if (Hexagons[x, y].GetComponent<HexagonID>().HexColor == Hexagons[x, y + 1].GetComponent<HexagonID>().HexColor && Hexagons[x, y].GetComponent<HexagonID>().HexColor == Hexagons[x + 1, y + 1].GetComponent<HexagonID>().HexColor)
                    {
                        Hexagons[x, y].SetActive(false);
                        Hexagons[x, y + 1].SetActive(false);
                        Hexagons[x + 1, y + 1].SetActive(false);
                    }
                }
            }
        }
    }

    
    private void CreateBoardWithoutMatch(int x, int y, GameObject[,] Hexagons) 
    {
        if (x > 0 && y > 0 && ColorId == Hexagons[x, y - 1].GetComponent<HexagonID>().HexColor && ColorId == Hexagons[x - 1, y - 1].GetComponent<HexagonID>().HexColor)
        {
            ColorId = Random.Range(0, Colors.Length);
            if (x > 0 && y > 0 && ColorId == Hexagons[x, y - 1].GetComponent<HexagonID>().HexColor && ColorId == Hexagons[x - 1, y - 1].GetComponent<HexagonID>().HexColor)
            {
                CreateBoardWithoutMatch(x, y, Hexagons);
            }
        }
        else if (x > 0 && y > 0 && ColorId == Hexagons[x - 1, y].GetComponent<HexagonID>().HexColor && ColorId == Hexagons[x - 1, y - 1].GetComponent<HexagonID>().HexColor)
        {
            ColorId = Random.Range(0, Colors.Length);
            if (x > 0 && y > 0 && ColorId == Hexagons[x - 1, y].GetComponent<HexagonID>().HexColor && ColorId == Hexagons[x - 1, y - 1].GetComponent<HexagonID>().HexColor)
            {
                CreateBoardWithoutMatch(x, y, Hexagons);
            }
        }
        else if (x > 0 && y > 0 && ColorId == Hexagons[x - 1, y].GetComponent<HexagonID>().HexColor && ColorId == Hexagons[x, y - 1].GetComponent<HexagonID>().HexColor)
        {
            ColorId = Random.Range(0, Colors.Length);
            if (x > 0 && y > 0 && ColorId == Hexagons[x - 1, y].GetComponent<HexagonID>().HexColor && ColorId == Hexagons[x, y - 1].GetComponent<HexagonID>().HexColor)
            {
                CreateBoardWithoutMatch(x, y, Hexagons);
            }
        }
        else if (y < width && x > 0 && ColorId == Hexagons[x - 1, y].GetComponent<HexagonID>().HexColor && ColorId == Hexagons[x - 1, y + 1].GetComponent<HexagonID>().HexColor)
        {
            ColorId = Random.Range(0, Colors.Length);
            if (y < width && x > 0 && ColorId == Hexagons[x - 1, y].GetComponent<HexagonID>().HexColor && ColorId == Hexagons[x - 1, y + 1].GetComponent<HexagonID>().HexColor)
            {
                CreateBoardWithoutMatch(x, y, Hexagons);
            }
        }
    }
}

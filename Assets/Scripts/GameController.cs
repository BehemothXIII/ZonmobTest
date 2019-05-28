using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
  public GameObject nodePrefab;
  public GameObject knightPrefab;
  public GameObject targetPrefab;
  public GameObject tileHolder;
  public Sprite whiteSprite;
  public Sprite blackSprite;

  public InputField startX;
  public InputField startY;
  public InputField targetX;
  public InputField targetY;

  private GameObject[,] nodes = new GameObject[8, 8];
  private List<Vector2Int> positions = new List<Vector2Int>();
  private Vector2 start;
  private Vector2 end;
  void Start()
  {
    CreateBoard();
  }

  private void CreateBoard()
  {
    for (int i = 0; i < 8; i++)
    {
      for (int j = 0; j < 8; j++)
      {
        nodes[i, j] = Instantiate(nodePrefab, new Vector2(i, j), Quaternion.identity, tileHolder.transform);
        positions.Add(Vector2Int.RoundToInt(nodes[i, j].transform.position));
        if (i % 2 != 0 && j % 2 != 0 || i % 2 == 0 && j % 2 == 0)
        {
          nodes[i, j].GetComponent<Image>().sprite = blackSprite;
        }
        else
        {
          nodes[i, j].GetComponent<Image>().sprite = whiteSprite;
        }
        nodes[i, j].name = i + "," + j;
      }
    }
  }

  private GameObject SpawnKnight()
  {
    int x = string.IsNullOrEmpty(startX.text) ? 0 : (int)Convert.ToSingle(startX.text);
    int y = string.IsNullOrEmpty(startY.text) ? 0 : (int)Convert.ToSingle(startY.text);
    if (x < 8 && y < 8)
    {
      GameObject knight = GameObject.Find("Knight(Clone)") ?? Instantiate(knightPrefab);
      knight.transform.position = nodes[x, y].transform.position;
      start = positions.Find(vector => vector == new Vector2Int(x, y));
      return knight;
    }
    else
    {
      startX.text = "";
      startY.text = "";
    }
    return null;
  }

  private GameObject SpawnTarget()
  {
    int x = string.IsNullOrEmpty(targetX.text) ? 0 : (int)Convert.ToSingle(targetX.text);
    int y = string.IsNullOrEmpty(targetY.text) ? 0 : (int)Convert.ToSingle(targetY.text);
    if (x < 8 && y < 8)
    {
      GameObject target = GameObject.Find("Target(Clone)") ?? Instantiate(targetPrefab);
      target.transform.position = nodes[x, y].transform.position;
      end = positions.Find(vector => vector == new Vector2(x, y));
      return target;
    }
    else
    {
      targetX.text = "";
      targetY.text = "";
    }
    return null;
  }

  public void OnGoButtonClick()
  {
    GameObject knight = SpawnKnight();
    GameObject target = SpawnTarget();
    if (knight != null)
      knight.GetComponent<KnightController>().Move(new Vector2Int((int)start.x, (int)start.y), new Vector2Int((int)end.x, (int)end.y), positions, nodes);
  }
}

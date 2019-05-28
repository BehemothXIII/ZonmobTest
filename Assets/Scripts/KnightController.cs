using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class KnightController : MonoBehaviour
{
  public void Move(Vector2Int start, Vector2Int target, List<Vector2Int> map, GameObject[,] nodes)
  {
    PathNode result = PathFinding.GetRoute(target, start, map); // Searching from target to start so you can get the first step without having to backtrace the entire path history
    if (result != null)
    {
      StartCoroutine(MoveTileToTile(transform, target, result, nodes));
    }
  }

  private IEnumerator MoveTileToTile(Transform transform, Vector2Int target, PathNode result, GameObject[,] nodes)
  {
    float time = 0;
    while (result.cameFrom != null)
    {
      transform.position = Vector2.Lerp(
        nodes[result.position.x, result.position.y].transform.position,
        nodes[result.cameFrom.position.x, result.cameFrom.position.y].transform.position,
        time / 1f);
      result = result.cameFrom;
      time += Time.deltaTime * 5f;
      yield return null;
    }
    transform.position = nodes[target.x, target.y].transform.position;
  }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] playerMovement player;
    List<GameObject> levels;
    List<Vector3> startPositions =  new List<Vector3>{new Vector3(0, 10, 0), new Vector3(0, 10, 0)};
    int levelIndex = 0;
    GameObject currentLevel;
    void Start()
    {
        levels = Resources.LoadAll<GameObject>("Levels").ToList();

        Debug.Log("# of levels: " + levels.Count);

        currentLevel = GameObject.Instantiate(levels[levelIndex]);
        player.gameObject.transform.position = startPositions[levelIndex];
        player.compileLights();
    }

    public void RestartLevel(){
        //assuming player is dead
        GameObject.Destroy(currentLevel);
        currentLevel = GameObject.Instantiate(levels[levelIndex]);
        player.gameObject.transform.position = startPositions[levelIndex];
    }

    public void NextLevel(){
        levelIndex++;
        GameObject.Destroy(currentLevel);
        currentLevel = GameObject.Instantiate(levels[levelIndex]);
        player.gameObject.transform.position = startPositions[levelIndex];
        Debug.Log(player.transform.position);
    }
}

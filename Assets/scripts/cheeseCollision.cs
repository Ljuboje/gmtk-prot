using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class cheeseCollision : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textMesh;
    void OnTriggerEnter(Collider collider){
        textMesh.text = "pobedio si xd";
        Debug.Log("cheese collsion");
    }
}

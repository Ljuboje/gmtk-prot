using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;
using UnityEngine.Rendering.PostProcessing;
using Unity.VisualScripting;
public class playerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] CharacterController controller;
    [SerializeField] Transform lightTransform;
    [SerializeField] PostProcessVolume volume;
    [SerializeField] Camera cam;

    [SerializeField] GameManager gameManager;
    [SerializeField] int grabLayer = 6;
    Vignette vignette;
    [SerializeField] float maxTime = 3f;
    float currentTime = 0f;

    int layerMask = 1 << 3; // layer mask for shadow casters
    bool isAlive = true;
    List<GameObject> lights;
    GameObject grabbed;
    Vector3 newpos;

    bool IsInLight(){
        RaycastHit hit;

        bool hitByLight = false;
        foreach(GameObject light in lights){
            if(light == null) continue;
            Vector3 raydir = light.transform.position - transform.position;
            raydir = Vector3.Normalize(raydir);
            hitByLight |= !Physics.Raycast(transform.position - new Vector3(0, 1, 0), raydir, out hit, Mathf.Infinity);

            if(hitByLight) break;
        }
       
       return hitByLight;
            
    }

    public void compileLights(){
        lights = new List<GameObject>();
        GameObject[] objects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach(GameObject obj in objects){
            
            Debug.Log("object !");
            if(obj.GetComponent<Light>() != null) {
                Debug.Log("svetlo");
                lights.Add(obj);
            }
        }
    }
    void Start(){
        Vignette tmp;
        if(volume.profile.TryGetSettings<Vignette>(out tmp)){
            vignette = tmp;
        }

        compileLights();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H)) {
            gameManager.NextLevel();
            compileLights();
        }
        Vector3 movedir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if(isAlive){
            
            controller.SimpleMove(cam.transform.rotation * movedir * moveSpeed);
            
            if(Input.GetKey(KeyCode.E) && grabbed != null){
                grabbed.transform.position = transform.position;
            }

            if(IsInLight()){
                currentTime += Time.deltaTime;
                if(currentTime >= maxTime) Death();
            }
            else{
                currentTime -= Time.deltaTime;
                currentTime = Mathf.Clamp(currentTime, 0f, maxTime);
                //if(currentTime <= 0.1f) Debug.Log("reset time");
            }
        }
        vignette.intensity.value = Mathf.InverseLerp(0f, maxTime, currentTime);
    }
    void Death(){
        isAlive = false;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = Color.red;

        gameManager.RestartLevel();
        lights.Clear();
        compileLights();
        
        meshRenderer.material.color = Color.white;
        currentTime = 0f;
        isAlive = true;
    }
    void PreventMovement(){
         Vector3 movedir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));



        if(movedir != Vector3.zero){


            newpos = transform.position + (movedir * moveSpeed * Time.deltaTime)*6 - new Vector3(0, 1, 0);
            //newpos += new Vector3(0, 1.2f, 0);
            Vector3 raydir = lightTransform.position - newpos;
            raydir = Vector3.Normalize(raydir);
            RaycastHit ray;

            if(Physics.Raycast(newpos, raydir, out ray, Mathf.Infinity, layerMask)){
                //Debug.Log("senka");
                //Debug.DrawRay(newpos.position, newpos.TransformDirection(Vector3.forward) * ray.distance, Color.yellow);
                controller.SimpleMove(movedir*moveSpeed);
            }
            else{
                //Debug.DrawRay(newpos.position, newpos.TransformDirection(Vector3.forward) * 100, Color.white);
                // Debug.Log("not hit");
            }
        }
    }
}

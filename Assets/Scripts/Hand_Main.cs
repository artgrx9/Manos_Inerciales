using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class Hand_Main : MonoBehaviour
{
    public float distance = 1.0f;   //Offset de la mano respecto a la cámara

    public float DrawDistance = 0.3f;       //Radio desde la posición de la mano de la cuál puedes "escribir"
    public string DrawTag = "Draw";         //Etiqueta que deben de tener los objetos para poder activar la parte de dibujo
    public float Multiplier = 4.0f;    //(Para la física del objeto agarrado) "Fuerza" con la que es lanzado al ser soltado
    private Transform _currentObject;       //Variable para el objeto agarrado

    private Transform Hand;     //Variable para recuperar los atributos de los modelos de la mano

    private Rect drawArea;              //Variable para indicar el tamaño del área de dibujo
    public GUIStyle DrawPlaneOff;       //Definición del Box para cuando el usuario no puede escribir en él
    public GUIStyle DrawPlaneOn;        //Definición del Box para cuando el usuario puede escribir en él
    private bool flagActive = false;    //Bandera para indicar si el Box está o no activo

    void Start()
    {
        Hand = GameObject.Find("Hand").transform;       //Recupera el Empty Object "Hand "que contiene ambos modelos de mano
        MeshRenderer rend_open = Hand.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();     //GetChild(0) = Hand_Open
        MeshRenderer rend_closed = Hand.GetChild(1).GetChild(0).GetComponent<MeshRenderer>();   //GetChild(1) = Grabbing_Hand
        rend_open.enabled = true;       //Al inicio, empezar con la mano abierta
        rend_closed.enabled = false;

        drawArea = new Rect(0, 0, Screen.width - Screen.width / 3, Screen.height);  //Tamaño para Box que le aparece al usuario
    }


    void Update()
    {
        //Recuperar los inputs de posición del mouse
        Vector3 mousePosition = Input.mousePosition;
        //mousePosition.z = distance; //mousePosition.z += Input.mouseScrollDelta.y;
        float v = Input.GetAxis("Vertical");
        mousePosition.z += Multiplier*v;
        //gameObject.transform.Translate(new Vector3(h * .3f, v * .3f, 0));
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);

        //Mismos códigos que en Start()
        Hand = GameObject.Find("Hand").transform;
        MeshRenderer rend_open = Hand.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        MeshRenderer rend_closed = Hand.GetChild(1).GetChild(0).GetComponent<MeshRenderer>();


        //Checar si hay colliders en la proximidad
        Collider[] colliders = Physics.OverlapSphere(transform.position, DrawDistance); //(punto,radio)

        //Si detecta collider, es porque está cerca del plano
        if (colliders.Length > 0)   
        {
            //Revisar que el collider sea del tag específico
            if (colliders[0].transform.CompareTag(DrawTag))
            {
                //Cambiar a Draw Plane On
                flagActive = true;

                //Cambiar de modelo de mano -> abierta
                rend_open.enabled = false;
                rend_closed.enabled = true;

            }
        }
        else
        {
            //Cambiar Draw Plane
            flagActive = false;

            //Cambiar de modelo de mano -> cerrada
            rend_open.enabled = true;
            rend_closed.enabled = false;
        }
    }


    void OnGUI()
    {
        //Box para cuando no puedes dibujar
        if (flagActive == false)
        {
            GUI.Box(drawArea, "Acerca tu mano para escribir", DrawPlaneOff); 
        }
        //Box para cuando puedes dibujar
        else
        {
            GUI.Box(drawArea, "Escribe ahora", DrawPlaneOn);
        }
    }

}

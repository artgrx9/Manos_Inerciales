using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationController : MonoBehaviour
{
    public Text StrData;
    public GameObject device2;
    public GameObject hand;
    public List<float> datos = new List<float>();
    WifiController wifiController;

    public Vector3 currentRotation;
    public Vector3 anglesToRotate;


    public Vector3 currentRotation2;
    public Vector3 anglesToRotate2;
    // Use this for initialization
    void Start()
    {
        StrData.text = "Inicio";
        wifiController = new WifiController();
        wifiController.Begin("192.168.43.4", 80);
        currentRotation = new Vector3(currentRotation.x % 360f, currentRotation.y % 360f, currentRotation.z % 360f);
        //currentRotation2 = new Vector3(currentRotation2.x % 360f, currentRotation2.y % 360f, currentRotation2.z % 360f);
        // anglesToRotate = new Vector3(anglesToRotate.x % 360f, anglesToRotate.y % 360f, anglesToRotate.z % 360f);

        // currentRotation2 = new Vector3(currentRotation.x % 360f, currentRotation.y % 360f, currentRotation.z % 360f);
        // anglesToRotate2 = new Vector3(anglesToRotate.x % 360f, anglesToRotate.y % 360f, anglesToRotate.z % 360f);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateData();
        //Debug.DrawLine(transform.position, device2.transform.position, Color.green);
        //Debug.DrawLine(device2.transform.position, hand.transform.position, Color.red);

        //currentRotation = currentRotation + anglesToRotate * Time.deltaTime;
        currentRotation = new Vector3(datos[2] % 360f, datos[1] % 360f, datos[3] % 360f);
        this.transform.eulerAngles = currentRotation;
        // currentRotation2= new Vector3(datos[4] % 360f, -datos[5] % 360f, datos[6] % 360f);
        // device2.transform.eulerAngles = currentRotation2;


    }

    public void UpdateData()
    {
        datos.Clear();
        string[] vec6 = wifiController.CurrentValue.Split(','); //Separamos el String leido valiendonos de las comas y almacenamos los valores en un array.
        foreach (string element in vec6)
        {
            float.TryParse(element, out float number);
            datos.Add(number);

        }
        StrData.text = "IMU DATA: " + datos[0] + ",......" + datos[1] + ",......" + datos[2] + ",......" + datos[3];

    }
}

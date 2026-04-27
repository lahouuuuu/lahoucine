using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public GameObject lightObject;
    private bool isOn = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            lightObject.SetActive(isOn);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Spellcasting : MonoBehaviour
{
    public InputDevice targetDevice;
    public float threshHold = 0.2f;
    [Space(10)]
    public string[] spellMedal;
    [Space (10)]
    public GameObject[] fireBall;
    public TeleportationController teleportSpell;
    public GameObject teleportHand;
    public string currentSpell;
    public GameObject spellSocket;
    private XRSocketInteractor socketSettings;


    private float downTime = 0.0f;
    private int spawnBall = 0;

    // Start is called before the first frame update
    void Start()
    {
        socketSettings = spellSocket.GetComponent<XRSocketInteractor>();
        spellSocket.SetActive(false);
        teleportHand.SetActive(false);
        teleportSpell.enabled = false;
        currentSpell = spellMedal[0];
        searchDevice();
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
        {
            searchDevice();
        }
        else
        {
            //fireball size counter and button press
            if(currentSpell == spellMedal[1])
            {

                FireBallSpawn();
            }
            //spell 2 Teleport, doesnt need any code outside of the selector function to work
        }        
    }

   


    public void SelectFireBall()
    {
        currentSpell = spellMedal[1];
        spellSocket.SetActive(true);
        teleportHand.SetActive(false);
        teleportSpell.enabled = false;
    }
    public void SelectTeleport()
    {
        currentSpell = spellMedal[2];
        teleportSpell.enabled = true;
        spellSocket.SetActive(false);

    }


    private void FireBallSpawn()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > threshHold)
        {
            downTime += Time.deltaTime;
        }
        else if (triggerValue < threshHold)
        {
            if (downTime != 0.0f)
            {
                socketSettings.enabled = false;
                if (downTime < 1)
                {
                    spawnBall = 0;
                }
                else if (downTime >= 1 && downTime < 2)
                {
                    spawnBall = 1;
                    targetDevice.SendHapticImpulse(0, 0.3f, 0.3f);
                    Instantiate(fireBall[0], spellSocket.transform.position, Quaternion.identity);
                }
                else if (downTime >= 2 && downTime < 3)
                {
                    spawnBall = 2;
                    targetDevice.SendHapticImpulse(0, 0.6f, 0.3f);
                    Instantiate(fireBall[1], spellSocket.transform.position, Quaternion.identity);
                }
                else
                {
                    spawnBall = 3;
                    targetDevice.SendHapticImpulse(0, 0.9f, 0.3f);
                    Instantiate(fireBall[2], spellSocket.transform.position, Quaternion.identity);
                }
                Debug.Log(spawnBall);
                downTime = 0.0f;
            }
        }
    }    
    void searchDevice()
    {
        var rightHandController = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandController);

        foreach (var device in rightHandController)
        {
            Debug.Log(string.Format("Device name '{0}' has characteristics '{1}'", device.name, device.characteristics.ToString()));
            if (device.name == "Oculus Touch Controller - Right")
            {
                targetDevice = device;
            }
        }
    }
}

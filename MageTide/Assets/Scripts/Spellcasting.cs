using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

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
    [Space(20)]
    public Image manaImage;
    public float manaMax = 100, spellCost = 20, currentMana;
    public Text manaNumber;
    [Space(20)]
    public Image healthBar;
    public float maxHealth = 100, currentHealth, damage = 25;
    public Text healthNumber;


    private float downTime = 0.0f;
    private int spawnBall = 0;
    private int i;
    [Space(10)]
    public KeyGatePuzzle fade;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.fillAmount = currentHealth / 100;
        manaImage.fillAmount = (manaMax / 100);
        currentMana = manaMax;
        socketSettings = spellSocket.GetComponent<XRSocketInteractor>();
        spellSocket.SetActive(false);
        teleportHand.SetActive(false);
        teleportSpell.enabled = false;
        currentSpell = spellMedal[0];
        searchDevice();
        StartCoroutine(ManaAdd());
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
    
    
     IEnumerator ManaAdd()
     {
        i = (int)currentMana;
        manaNumber.text = i.ToString();
        yield return new WaitForSeconds(0.1f);
        if (currentMana >= 100)
        {
            currentMana = 100;
            manaImage.fillAmount = 1;
        }
        else if (currentMana < 100 && currentMana >= 0)
        {
            currentMana +=  0.5f;
            manaImage.fillAmount = currentMana / 100;
        }
        else if (currentMana < 0)
        {
            currentMana = 0;
        }
        StartCoroutine(ManaAdd());
     }

    public void HealthReduce()
    {
        currentHealth -= 20;
        healthBar.fillAmount = currentHealth / 100;
        healthNumber.text = currentHealth.ToString();
        if(currentHealth <= 0)
        {
            fade.EndDemo(0);
        }
    }

    public void ManaReduce()
    {
        currentMana -= 30;
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
        spellSocket.SetActive(false);
        StartCoroutine(canTeleport());
    }
    IEnumerator canTeleport()
    {        
        if(currentSpell == spellMedal[2])
        {
            if(currentMana >= 20)
            {
                teleportSpell.enabled = true;
                yield return new WaitForSeconds(1f);
                StartCoroutine(canTeleport());
            }
            else
            {
                teleportSpell.enabled = false;
                yield return new WaitForSeconds(1f);
                StartCoroutine(canTeleport());
            }
        }
    }

    private void FireBallSpawn()
    {
        if(currentMana > 20)
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
                        hapticShock(0.3f, 0.3f);
                        Instantiate(fireBall[0], spellSocket.transform.position, Quaternion.identity);
                        ManaReduce();

                    }
                    else if (downTime >= 2 && downTime < 3)
                    {
                        spawnBall = 2;
                        hapticShock(0.6f, 0.3f);
                        Instantiate(fireBall[1], spellSocket.transform.position, Quaternion.identity);
                        ManaReduce();
                    }
                    else
                    {
                        spawnBall = 3;
                        hapticShock(0.9f, 0.3f);
                        Instantiate(fireBall[2], spellSocket.transform.position, Quaternion.identity);
                        ManaReduce();
                    }
                    Debug.Log(spawnBall);
                    downTime = 0.0f;
                }
            }
        }        
    }  
    
    public void hapticShock(float amp, float duration)
    {
        targetDevice.SendHapticImpulse(0, amp, duration);
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

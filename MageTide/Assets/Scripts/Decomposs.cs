using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decomposs : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(DieBox());
    }

    IEnumerator DieBox()
    {
        yield return new WaitForSeconds(6f);
        Destroy(this.gameObject);
    }
}

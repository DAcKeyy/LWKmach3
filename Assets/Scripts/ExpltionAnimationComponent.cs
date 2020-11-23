using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpltionAnimationComponent : MonoBehaviour
{
    public void ExplotionEnded()
    {
        Destroy(this.gameObject);
    }
}

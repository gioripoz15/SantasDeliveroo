using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
	public GameObject TargetHouse;
	public Santa Owner;

	public void PickUpGameobject()
    {
		//play some audio/particles;
		gameObject.SetActive(false);
    }

	public void HighlightTargetHouse(bool highlight)
    {

    }
}

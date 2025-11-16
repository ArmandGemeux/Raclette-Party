using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Angle
{
    Right,
    Up,
    Forward
}


public class MouseClickCut : MonoBehaviour
{
    public Angle angle;

	public float cutForce;

	void Update(){

		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;

			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)){

				GameObject tOCutObject = hit.collider.gameObject;
				if(tOCutObject.tag == "Cuttable")
				{
                   
                    if(angle == Angle.Right)
					{
                        Cutter.Cut(tOCutObject, hit.point, Vector3.right, cutForce);

                    }
                    else if (angle == Angle.Up)
                    {
                        Cutter.Cut(tOCutObject, hit.point, Vector3.up, cutForce);

                    }
                    else if (angle == Angle.Forward)
					{
						Cutter.Cut(tOCutObject, hit.point, Vector3.forward, cutForce);
						
					}
				}
			}

		}
	}
}

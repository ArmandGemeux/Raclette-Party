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

    void Update(){

		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;

			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)){

				GameObject victim = hit.collider.gameObject;
				if(victim.tag == "Cuttable")
				{
                   
                    if(angle == Angle.Right)
					{
                        Cutter.Cut(victim, hit.point, new Vector3(0.01f,0f,0f));

                    }
                    else if (angle == Angle.Up)
                    {
                        Cutter.Cut(victim, hit.point, Vector3.up);

                    }
                    else if (angle == Angle.Forward)
					{
						Cutter.Cut(victim, hit.point, Vector3.forward);
						
					}
				}
			}

		}
	}
}

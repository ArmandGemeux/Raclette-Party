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
{    public static MouseClickCut Instance { get; private set; }

    public Angle angle;

	public float cutForce;

    public bool isCutting;
    void Awake()
    {
        // Vérifie qu’il n’y a qu’un seul GameManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // persiste entre les scènes
    }

    void Update(){

		if(Input.GetMouseButtonDown(0) && isCutting){
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

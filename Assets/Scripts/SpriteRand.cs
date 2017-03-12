using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SpriteRand : MonoBehaviour {

	// Use this for initialization
	public Sprite[] body_M, body_F, neck, head_normal_M, head_normal_F, lSleeve_M, lSleeve_F, lArm, rSleeve_M,rSleeve_F,rArm,crotch_M,crotch_F;

	public Sprite[] rLeg_F,rLeg_M, lLeg_M, lLeg_F, hats ;

	private Color[] clothingColors = new Color[4];
	void Start () {



					int gender = Random.Range (0,2);
					if(gender == 0)
					{
						setMatchingTorso (lSleeve_M, body_M, rSleeve_M, Random.Range(0, body_M.Length-1));
						setMatchingBottom (lLeg_M, crotch_M, rLeg_M, Random.Range(0,crotch_M.Length-1));

						setGrandChild ("Neck", "Head", head_normal_M, Random.Range(0, head_normal_M.Length-1 ));
					}else
						if(gender == 1)
						{
							setMatchingTorso (lSleeve_F, body_F, rSleeve_F, Random.Range(0, body_F.Length-1));
							setMatchingBottom (lLeg_F, crotch_F, rLeg_F, Random.Range(0,crotch_F.Length-1));

							setGrandChild ("Neck", "Head", head_normal_F, Random.Range(0, head_normal_F.Length-1));
						}


					//setPart ("Hat", hats, Random.Range (0, hats.Length-1));


					setGrandChild ("Left Sleeve", "Left Arm", lArm, Random.Range(0, lArm.Length-1));
					setGrandChild ("Right Sleeve", "Right Arm", rArm, Random.Range(0, rArm.Length-1));
					setImmediateChild("Neck", neck, Random.Range (0, neck.Length-1));
					randHat ();




	}
	void randHat()
	{
		GameObject hawt = gameObject.transform.FindChild ("Neck").gameObject.transform.FindChild ("Head").gameObject.transform.FindChild ("Hat").gameObject;
		hawt.GetComponent<SpriteRenderer> ().sprite = (hats[Random.Range(0,hats.Length-1)]);
	}
	// Update is called once per frame
	void Update () {


	}

	void setImmediateChild(string child, Sprite [] arr, int i)
	{

		GameObject g = gameObject.transform.FindChild (child).gameObject;

		g.GetComponent<SpriteRenderer> ().sprite = arr [i];
	}


	void setMatchingTorso(Sprite [] lSlv, Sprite[] bdy, Sprite[] rSlv, int index)
	{

		setImmediateChild ("Left Sleeve", lSlv, index);

		this.GetComponent<SpriteRenderer> ().sprite = bdy [index];

		setImmediateChild ("Right Sleeve", rSlv, index);
	}

	void setMatchingBottom(Sprite [] ll, Sprite[] crtch, Sprite[] rl, int index)
	{   setImmediateChild ("Crotch", crtch, index);
		setGrandChild ("Crotch","Left Leg", ll, index);

		setGrandChild ("Crotch","Right Leg", rl, index);
	}

	void setGrandChild(string immediate,string steptwo, Sprite[] l, int index)
	{


		GameObject target = gameObject.transform.FindChild (immediate).gameObject.transform.FindChild(steptwo).gameObject;
		target.GetComponent<SpriteRenderer> ().sprite = l [index];
	}

}

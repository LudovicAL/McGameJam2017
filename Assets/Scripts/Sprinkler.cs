using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : MonoBehaviour {

	// Use this for initialization
	private GameObject sprinkler;
	private GameObject [] flems;
	ParticleSystem partSys;

	void Start () {
		sprinkler = this.gameObject;
		partSys =  sprinkler.transform.Find("Particle System").gameObject.GetComponent<ParticleSystem>();

	}
	
	// Update is called once per frame
	void Update () {
		if(Random.Range(0,6) >  4)
		{
			StartCoroutine(ExtinguishFire());
		}
	}

	private IEnumerator ExtinguishFire()
	{
		partSys.Play();

		yield return new WaitForSeconds (3);
		flems = GameObject.FindGameObjectsWithTag ("Fire");
		foreach(GameObject feu in flems)
		{
			
			if(Random.Range (0, 101) > 75)
			{
				feu.GetComponent<FireSpreader> ().PutOutFire();
			}

		}partSys.Stop ();
	}

	 
}

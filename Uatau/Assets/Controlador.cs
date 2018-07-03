using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador : MonoBehaviour {

	private ArrayList objetos;
	private int pos=0;
	public bool automaticMode;
	public int changeModelTimeInSeconds;
    public bool randomMode;

	// Use this for initialization
	void Start () {
		objetos = new ArrayList ();
		foreach (Transform tr in transform)objetos.Add(tr.gameObject);
		((GameObject)objetos [pos]).SetActive (true);
		
	}
		

	// Update is called once per frame
	void Update () {
		
	}

	public void cambiarObjeto(){
		for (int i = 0; i < objetos.Count; i++) {
			((GameObject)objetos [i]).SetActive (false);
		}
		pos = (pos+1) % objetos.Count;
        if (randomMode)
        {
            ((GameObject)objetos[Random.RandomRange(0,objetos.Count)]).SetActive(true);
        }
        else
        {
            ((GameObject)objetos[pos]).SetActive(true);
        }
		
	}

    public void avanzar()
    {
        for (int i = 0; i < objetos.Count; i++)
        {
            ((GameObject)objetos[i]).SetActive(false);
        }
        pos = (pos + 1) % objetos.Count;
        ((GameObject)objetos[pos]).SetActive(true);
    }


    public void retroceder()
    {
        for (int i = 0; i < objetos.Count; i++)
        {
            ((GameObject)objetos[i]).SetActive(false);
        }
        if (pos == 0)
        {
            pos = objetos.Count - 1;
        }
        else pos = (pos - 1) % objetos.Count;
        ((GameObject)objetos[pos]).SetActive(true);
    }
}

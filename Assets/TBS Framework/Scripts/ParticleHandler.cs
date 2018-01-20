using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour {

    public GameObject particleSystem = null;
    public CellGrid grid = null;
    int cellCount;
    

	// Use this for initialization
	void Start () {
        //grid.Cells.Count;
	}
	
	// Update is called once per frame
	void Update () {
		if( CellGrid.GameOver)
        {
            Random r = new Random();
            int cell = Random.Range(0, grid.Cells.Count);
            bool spawn = Random.Range(0, 5) == 0;

            //Transform t = grid.Cells[cell].transform;
            Vector3 t = grid.Cells[cell].transform.position;
            t = new Vector3(t.x, t.y, -1.0f);
            if (spawn)
                SpawnExplosion(t);
                    //Instantiate(particleSystem, t, Quaternion.identity);
        }
	}

    public void SpawnExplosion(Vector3 position)
    {
        GameObject go = Instantiate(particleSystem, position, Quaternion.identity);
        go.GetComponent<DestroyParticleSystem>().Play();
        AudioSource audio = GameObject.Find("SoundPlayer").GetComponent<AudioSource>();
        audio.Play();
    }
}

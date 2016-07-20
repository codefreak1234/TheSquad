using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public QuadTree quadTree = new QuadTree(
		new Rect(0,0,1000,1000),
		4,
		4,
		4
	) ;
	public GameObject[] tar ;
	
	public BoxCollider main ;

	public BoxCollider[] quadReturns ;
	// Use this for initialization
	void Start () {
		quadTree.clear() ;
		quadTree.split() ;
		tar = GameObject.FindGameObjectsWithTag("tar") ;
		for(var i = 0 ; i < tar.Length ; i++) {
			// Debug.Log(quadTree.nodes.Count) ;
			quadTree.insert(tar[i].GetComponent<BoxCollider>()) ;
		}
	}
	
	// Update is called once per frame
	void Update () {
		quadReturns = null ;
		quadReturns = quadTree.retrieve(main);

	}
}

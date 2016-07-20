using UnityEngine;
using System.Collections.Generic ;
// using System.Collections ;
[System.SerializableAttribute]
public class QuadTree {
	private Rect size ;
	private int maxTargets ; //
	private int maxLevels ;
	private int levels ;
	private List<Rect> objects ;
	
	public List<QuadTree> nodes = new List<QuadTree>() ; //

	public object sortLock = new object() ;
	public Bounds mapSize ; //
	public List<BoxCollider> targets = new List<BoxCollider>() ; //
	// public List<Collider> targetCollider ; //
	
	public QuadTree(Rect _size , int _maxTargets , int _maxLevels , int _levels) {
		// size = _size ;
		maxTargets = _maxTargets ;
		maxLevels = _maxLevels ;
		levels = _levels ;

		mapSize.center = new Vector3(_size.x,_size.y,0); //
		mapSize.min = new Vector3(_size.x - _size.width/2 ,_size.y - _size.height/2) ; //
		mapSize.max = new Vector3(_size.y + _size.width/2 ,_size.y + _size.height/2) ; //
	}

	public void split() {
		lock (sortLock)
		{
			var new_level = maxLevels + 1;
			// var width = (mapSize.max.x - mapSize.center.x) ; 
			var side_width = mapSize.max.x - mapSize.center.x ;
			var side_height = mapSize.max.y - mapSize.center.y ;

			// var x = mapSize.center.x ;
			// var y = mapSize.center.y ;

			if(nodes.Count == 0 || nodes.Count < 4) {
				nodes = new List<QuadTree>(new QuadTree[4]) ;
			}

			nodes[0] = new QuadTree(
				new Rect(mapSize.max.x , mapSize.min.x , side_width , side_height) ,
				maxTargets ,
				maxLevels ,
				new_level
			) ;

			nodes[1] = new QuadTree(
				new Rect(mapSize.min.x , mapSize.min.y , side_width , side_height) ,
				maxTargets ,
				maxLevels ,
				new_level
			) ;
			nodes[2] = new QuadTree(
				new Rect(mapSize.min.x , mapSize.max.y , side_width , side_height) ,
				maxTargets ,
				maxLevels ,
				new_level
			) ;
			nodes[3] = new QuadTree(
				new Rect(mapSize.max.x , mapSize.max.y , side_width , side_height) ,
				maxTargets ,
				maxLevels ,
				new_level
			) ;
		}
		
	}
	public int getId(Bounds bound) {
		lock (sortLock)
		{
			var rectId = -1 ;
			var x_midpoint = mapSize.center.x ;
			var y_midpoint = mapSize.center.y ;

			var top_quad = (bound.min.y < y_midpoint && bound.max.y < y_midpoint) ;
			var bottom_quad = bound.min.y > y_midpoint ;

			if(bound.min.x < x_midpoint && bound.max.x < x_midpoint) {
				if(top_quad) rectId = 1;
				else if (bottom_quad) rectId = 2;
			}
			else if (bound.min.x > x_midpoint) {
				if(top_quad) rectId = 0 ;
				else if(bottom_quad) rectId = 3;
			}
			return rectId ;
		}
		
	}
	public void insert(BoxCollider collider) {
		lock(sortLock) {
			var i = 0 ;
			var rank = 0 ;
			// Debug.Log(nodes.Count);
			// Debug.Log(nodes[0]);
			// if(nodes.Count == 0 || nodes.Count < 4) {
			// 	nodes = new List<QuadTree>(new QuadTree[4]) ;
			// }


			// if(nodes.Count == 0) split() ;

			if(nodes[0] != null) {
				rank = getId(collider.bounds) ;
				if(rank != -1) {
					// Debug.Log(rank);

					nodes[rank].insert(collider) ;
					// Debug.Log(rank);
					return ;
				}
			}
			targets.Add(collider) ;

			if(targets.Count > maxTargets && levels > maxLevels) {
				if(nodes[0] == null) split() ;
				while (i < targets.Count) {
					rank = getId(targets[i].bounds) ;
					if(rank != -1) {
						targets.Remove(targets[i]) ;
						nodes[rank].insert(targets[0]) ;
					}
					else {
						i++ ;
					}
				}
			}
		}
	}
	public BoxCollider[] retrieve(BoxCollider collider) {
		lock (sortLock)
		{
			var rank = getId(collider.bounds) ;
			var getTargets = targets ;
			// Debug.Log
			// Debug.Log("Targets : "+getTargets.Count);
			// Debug.Log(rank);
			// if(nodes.Count == 0) split() ;


			// Debug.Log(nodes.Count);
			// if(nodes) Debug
			// Debug.Log(nodes.Count);
			// Debug.Log(nodes[0].GetType().ToString());// + nodes[0].mapSize.center);
			// Debug.Log(nodes[0].mapSize.center);
			if(nodes.Count != 0) {
				if(rank != -1) {
					// Debug.Log(rank);
					var re = nodes[rank].retrieve(collider);
					getTargets.AddRange(re);
					// getTargets.InsertRange(getTargets.Count -1, nodes[rank].retrieve(collider) ) ;
				}
				else {
					for(var i = 0 ; i < nodes.Count ; i++) {
						// Debug.Log(nodes[i]);
						// Debug.Log(nodes[i]+"i"+i);
						var re = nodes[i].retrieve(collider) ;
						getTargets.AddRange(re);
						// getTargets.InsertRange(getTargets.Count -1 , re) ;
					}
				}
			}
			// Debug.Log("GetTargets : "+getTargets.Count);
			return getTargets.ToArray() ;
		}
		
	}
	public void clear() {
		lock (sortLock)
		{
			targets.Clear() ;
			for(var i = 0 ; i < nodes.Count ; i++) {
				if(nodes[i] != null) {
					nodes[i].Clear() ;
				}
			}
			nodes.Clear() ;
		}
		
	}

// _______________________________________
	public void Split() {
		var n_l = maxLevels + 1;
		var s_w = Mathf.Round(size.width/2 ) ;
		var s_h = Mathf.Round(size.height/2);
		var x = Mathf.Round(size.x);
		var y = Mathf.Round(size.y);

		if(nodes.Count == 0 || nodes.Count < 4) nodes = new List<QuadTree>(4);

		nodes[0] = new QuadTree (	new Rect(x + s_w,y,s_w,s_h) ,
									maxTargets ,
									maxLevels , 
									n_l	);

		nodes[1] = new QuadTree (	new Rect(x,y,s_w,s_h) ,
									maxTargets ,
									maxLevels , 
									n_l	);

       	nodes[2] = new QuadTree (	new Rect(x,y+s_h,s_w,s_h) ,
									maxTargets ,
									maxLevels , 
									n_l	);

		nodes[3] = new QuadTree (	new Rect(x+s_w,y+s_h,s_w,s_h) ,
									maxTargets ,
									maxLevels , 
									n_l	);

		// if(nodes.Count == 0) nodes.Add(new QuadTree(new Rect(x,y,s_w,s_h) , maxTargets , maxLevels , n_l) );
	}

	public int GetId(Rect _rect) {
		var rectId = -1 ;
		var v_m = size.x + (size.width/2);
		var h_m = size.y + (size.height/2);

		var t_q = (_rect.y < h_m && _rect.y + _rect.height < h_m) ;
		var b_q = (_rect.y > h_m);

		if(_rect.x < v_m && _rect.x + _rect.width < v_m) {
			if(t_q) rectId = 1;
			else if(b_q) rectId = 2;
		}
		else if(_rect.x > v_m) {
			if(t_q) rectId = 0 ;
			else if(b_q) rectId = 3 ;
		}
		return rectId ;
	}

	
	public void Insert(Rect _rect) {
		var i = 0 ;
		var index = 0;
		if(nodes[0].GetType().ToString() != "undefined") {
			index = GetId(_rect) ;
			if(index != -1) {
				nodes[index].Insert(_rect);
				return ;
			}
		}
		objects.Add(_rect) ;

		if(objects.Count > maxTargets && levels > maxLevels) {
			if(nodes[0].GetType().ToString() == "undefined"){
				Split();
			}
			while(i < objects.Count) {
				index = GetId(objects[i]) ; 
				if(index != -1) {
					objects.RemoveRange(i,1);
					nodes[index].Insert(objects[0]);
				}
				else {
					i = i+1;
				}
			}

		}
	}

	
	// public List<T> CustomRect(Rect _rect) {
	// 	List<T> results = new List<T>() ;
		
	// }
	public Rect[] Retrieve(Rect _rect) {
		var index = GetId(_rect);
		var returnObjects = objects ;

		if(nodes[0].GetType().ToString() != "undefined") {
			if(index != -1) {
				returnObjects.InsertRange(returnObjects.Count , nodes[index].Retrieve(_rect) );
			}
			else {
				for(var i=0 ; i < nodes.Count ;i++) {
					returnObjects.InsertRange(returnObjects.Count , nodes[i].Retrieve(_rect));
				}
			}
		}
		return returnObjects.ToArray() ;
	}

	

	public void Clear() {
		objects.Clear();
		for(var i=0; i<nodes.Count ;i++){
			if(nodes[i].GetType().ToString() != "undefined"){
				nodes[i].Clear();
			}
		}
		nodes.Clear() ;
	}
	
}

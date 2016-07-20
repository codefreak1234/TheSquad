using UnityEngine;
using System.Collections.Generic ;

public class QuadTree {
	private Rect size ;
	private int maxObjects ;
	private int maxLevels ;
	private int subLevels ;
	private List<Rect> objects ;
	private List<QuadTree> nodes ;
	public QuadTree(Rect _size , int _maxObjects , int _maxLevels , int _subLevels) {
		size = _size ;
		maxObjects = _maxObjects ;
		maxLevels = _maxLevels ;
		subLevels = _subLevels ;
	}
	public void Split() {
		var n_l = maxLevels + 1;
		var s_w = Mathf.Round(size.width/2 ) ;
		var s_h = Mathf.Round(size.height/2);
		var x = Mathf.Round(size.x);
		var y = Mathf.Round(size.y);

		if(nodes.Count == 0 || nodes.Count < 4) nodes = new List<QuadTree>(4);

		nodes[0] = new QuadTree (	new Rect(x + s_w,y,s_w,s_h) ,
									maxObjects ,
									maxLevels , 
									n_l	);

		nodes[1] = new QuadTree (	new Rect(x,y,s_w,s_h) ,
									maxObjects ,
									maxLevels , 
									n_l	);

       	nodes[2] = new QuadTree (	new Rect(x,y+s_h,s_w,s_h) ,
									maxObjects ,
									maxLevels , 
									n_l	);

		nodes[3] = new QuadTree (	new Rect(x+s_w,y+s_h,s_w,s_h) ,
									maxObjects ,
									maxLevels , 
									n_l	);

		// if(nodes.Count == 0) nodes.Add(new QuadTree(new Rect(x,y,s_w,s_h) , maxObjects , maxLevels , n_l) );
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

		if(objects.Count > maxObjects && subLevels > maxLevels) {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CameraManager : MonoBehaviour {
	
	public int maxZoom;
	public int minZoom;
	private Camera cam;
	private float minX;
	private float maxX;
	private float minZ;
	private float maxZ;

	// Use this for initialization
	void Start () {
		cam = this.gameObject.GetComponent<Camera> ();
		GridGraph gridGraph = AstarPath.active.astarData.gridGraph;
		GridNode[] gn = gridGraph.nodes;
		minX = float.MaxValue;
		maxX = float.MinValue;
		minZ = float.MaxValue;
		maxZ = float.MaxValue;
		foreach (GridNode n in gn) {
			if (((Vector3)n.position).x < minX){
				minX = ((Vector3)n.position).x;
			} else if (((Vector3)n.position).x > maxX){
				maxX = ((Vector3)n.position).x;
			}
			if (((Vector3)n.position).z < minZ){
				minZ = ((Vector3)n.position).z;
			} else if (((Vector3)n.position).z > maxZ){
				minZ = ((Vector3)n.position).z;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Vertical") != 0) {
			MoveVertical ();
		}
		if (Input.GetAxis("Horizontal") != 0) {
			MoveHorizontal ();
		}
		if (Input.GetAxis("Mouse ScrollWheel") != 0){
			Zoom ();
		}
	}

	private void MoveVertical() {
		if (Input.GetAxis("Vertical") < 0 && this.transform.position.z > minZ) {
			this.transform.position = this.transform.position + Vector3.back;
		} else if (Input.GetAxis("Vertical") > 0 && this.transform.position.z < maxZ) {
			this.transform.position = this.transform.position + Vector3.forward;
		}
	}

	private void MoveHorizontal() {
		if (Input.GetAxis("Horizontal") < 0 && this.transform.position.x > minX) {
			this.transform.position = this.transform.position + Vector3.left;
		} else if (Input.GetAxis("Horizontal") > 0 && this.transform.position.x < maxX) {
			this.transform.position = this.transform.position + Vector3.right;
		}
	}

	private void Zoom() {
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && cam.orthographicSize > minZoom) {
			cam.orthographicSize--;
		} else if (Input.GetAxis("Mouse ScrollWheel") < 0 && cam.orthographicSize < maxZoom) {
			cam.orthographicSize++;
		}
	}
}

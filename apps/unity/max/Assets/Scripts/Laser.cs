using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
	public LayerMask collisionLayers;
	public Vector2 firstLaserDirection = new Vector2(Vector2.left.x, Vector2.down.y);
	public Laserfire laserFire;
	public bool isOn = false;
	public LineRenderer lineRenderer;

	private Vector2 laserDirection = new Vector2(Vector2.right.x, Vector2.up.y);
	private Laserfire laserfireClone;

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.sortingOrder = 10;
	}

	void AnimateTheWidth(){
		var n_width = Random.Range(.09f, .1f);
		lineRenderer.startWidth = n_width;
		lineRenderer.endWidth = n_width;
	}
		
	// Update is called once per frame
	void Update (){
		AnimateTheWidth ();

		if (!isOn) {
			lineRenderer.positionCount = 0;
			return;
		}
			
		List<Vector3> hitPoints = new List<Vector3> ();

		//Debug.Log (laserDirection);
		hitPoints.Add (transform.parent.position);

		laserDirection = firstLaserDirection;
		RaycastHit2D hit1 = Physics2D.Raycast (transform.parent.position, laserDirection, Mathf.Infinity, collisionLayers);
		//RaycastHit2D hit = Physics2D.Linecast (transform.parent.position, new Vector2(transform.parent.position.x+5f, transform.parent.position.y+5f));
		Debug.DrawLine (transform.parent.position, hit1.point, Color.green);
		//Debug.Log (hit.point);

		//lineRenderer.SetPosition (1, new Vector3 (hit.point.x, hit.point.y, 0));
		hitPoints.Add (hit1.point);

		if (hit1.collider) {//mirror1

			if (hit1.collider.gameObject.tag.ToLower () == "mirror") {
				
				laserDirection = Vector2.Reflect (hit1.point - laserDirection, hit1.normal);

				RaycastHit2D hit2 = Physics2D.Raycast (hit1.point + hit1.normal * .01f, laserDirection, Mathf.Infinity, collisionLayers);
				Debug.DrawLine (hit1.point, hit2.point, Color.green);
				//Debug.Log (hit2.collider.tag);
				hitPoints.Add (hit2.point);

				if (hit2.collider) {//mirror2
					
					if (hit2.collider.gameObject.tag.ToLower () == "mirror") {
						
						laserDirection = Vector2.Reflect (hit2.point - hit1.point, hit2.normal);

						RaycastHit2D hit3 = Physics2D.Raycast (hit2.point + hit2.normal * .01f, laserDirection, Mathf.Infinity, collisionLayers);
						Debug.DrawLine (hit2.point, hit3.point, Color.green);
						//Debug.Log (hit3.collider.tag);

						hitPoints.Add (hit3.point);

						if (hit3.collider) {
							
							if (hit3.collider.gameObject.tag.ToLower () == "mirror") {
								
								laserDirection = Vector2.Reflect (hit3.point - hit2.point, hit3.normal);

								RaycastHit2D hit4 = Physics2D.Raycast (hit3.point + hit3.normal * .01f, laserDirection, Mathf.Infinity, collisionLayers);
								Debug.DrawLine (hit3.point, hit4.point, Color.green);
								//Debug.Log (hit4.point);

								hitPoints.Add (hit4.point);
								if (hit4.collider) {
									
									if (hit4.collider.gameObject.tag.ToLower () == "mirror") {
										laserDirection = Vector2.Reflect (hit4.point - hit3.point, hit4.normal);

										RaycastHit2D hit5 = Physics2D.Raycast (hit4.point + hit4.normal * .01f, laserDirection, Mathf.Infinity, collisionLayers);
										Debug.DrawLine (hit4.point, hit5.point, Color.green);
										//Debug.Log (hit4.point);

										hitPoints.Add (hit5.point);
										if (hit5.collider) {											
											
											if (hit5.collider.gameObject.tag.ToLower () == "mirror") {
												laserDirection = Vector2.Reflect (hit5.point - hit4.point, hit5.normal);
												RaycastHit2D hit6 = Physics2D.Raycast (hit5.point + hit5.normal * .01f, laserDirection, Mathf.Infinity, collisionLayers);
												Debug.DrawLine (hit5.point, hit6.point, Color.green);
												hitPoints.Add (hit6.point);

                                                if (hit6.collider)
                                                {

                                                    if (hit6.collider.gameObject.tag.ToLower() == "mirror")
                                                    {
                                                        laserDirection = Vector2.Reflect(hit6.point - hit5.point, hit6.normal);
                                                        RaycastHit2D hit7 = Physics2D.Raycast(hit6.point + hit6.normal * .01f, laserDirection, Mathf.Infinity, collisionLayers);
                                                        Debug.DrawLine(hit6.point, hit7.point, Color.green);
                                                        hitPoints.Add(hit7.point);

                                                        if (hit7.collider)
                                                        {

                                                            if (hit7.collider.gameObject.tag.ToLower() == "mirror")
                                                            {
                                                                laserDirection = Vector2.Reflect(hit7.point - hit6.point, hit7.normal);
                                                                RaycastHit2D hit8 = Physics2D.Raycast(hit7.point + hit7.normal * .01f, laserDirection, Mathf.Infinity, collisionLayers);
                                                                Debug.DrawLine(hit7.point, hit8.point, Color.green);
                                                                hitPoints.Add(hit8.point);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
										}
									}
								}
							}
						}
					}
				}
			}
		}

		//Debug.Log (lineRenderer.positionCount);
		Vector3 lastLineRendererPos = Vector3.zero;
		if (lineRenderer.positionCount > 1) {
			lastLineRendererPos = lineRenderer.GetPosition (lineRenderer.positionCount - 1);
		}

		if (lastLineRendererPos != hitPoints [hitPoints.Count - 1]) {
			
			lineRenderer.positionCount = hitPoints.Count;
			lineRenderer.SetPositions (hitPoints.ToArray());

			if (laserFire && hitPoints.Count <= 8) {
				laserfireClone = Instantiate (laserFire, lineRenderer.GetPosition (lineRenderer.positionCount - 1), Quaternion.identity) as Laserfire;
				laserfireClone.transform.parent = this.transform;
			}
		}
	}
}

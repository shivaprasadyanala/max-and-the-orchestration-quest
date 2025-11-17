using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoint : MonoBehaviour {

	public enum MoveToPointMode
	{
		Nothing = 0,
		Rewind,
		Reverse,
        ReverseWithDifferentSpeed
            
	}
    public enum PointType
    {
        WorldSpaceMode = 0,
        LocalSpaceMode
    }
		
	public float speed = .5f;
    public float speedReverse = .5f;
	public Vector3 moveToPoint;
    public PointType pointType = PointType.WorldSpaceMode;
	public MoveToPointMode mode;
	public bool automatic = true;
	public bool doMove = true;

	public bool playMovingSound = false;
	public int movingSoundIndexBegin = 0;
	public int movingSoundIndexEnd = 0;

	//Private
	private Vector3 originalPositon, tempPoint, _moveToPoint;
	private Color debugCollisionColor = Color.yellow;
	private bool playedOnce = false;
    private float tempSpeed = 0f;
	private AudioPlayer audioPlayer;

	void Start(){		
		audioPlayer = GetComponent<AudioPlayer> ();


        if (pointType == PointType.WorldSpaceMode)
        {
            originalPositon = transform.position;
            _moveToPoint = transform.TransformPoint(moveToPoint);
        }
        else if (pointType == PointType.LocalSpaceMode)
        {
            originalPositon = transform.localPosition;
            _moveToPoint = moveToPoint;
        }
        //if (gameObject.name == "fence2")
        //    Debug.LogError(transform.position + "------------------" + WorldSpaceMoveToPoint);
    }

	void Update(){
        Vector3 pos = Vector3.zero;        

        if (doMove)
        {
            if (pointType == PointType.WorldSpaceMode)
            {
                pos = transform.position;
                transform.position = Vector3.MoveTowards(pos, _moveToPoint, speed * Time.deltaTime);
            }
            else if (pointType == PointType.LocalSpaceMode)
            {
                pos = transform.localPosition;
                transform.localPosition = Vector3.MoveTowards(pos, _moveToPoint, speed * Time.deltaTime);
            }            
            
            //if (gameObject.name == "fence2")
            //    Debug.LogError(transform.position + "------------------" + WorldSpaceMoveToPoint);

            if (playMovingSound && !audioPlayer.isPlaying && !playedOnce)
            {
                audioPlayer.Audio_Play_Independently_At_Indices(movingSoundIndexBegin, movingSoundIndexEnd);

                playedOnce = true;
            }
        }
        else
            playedOnce = false;
		


		//When reaching end point
		if (Vector3.Distance (pos, _moveToPoint) == 0 && doMove) {
			doMove = false;

			if (mode == MoveToPointMode.Rewind) {
                if(pointType == PointType.WorldSpaceMode)
				    transform.position = originalPositon;
                else
                    transform.localPosition = originalPositon;

            } else if (mode == MoveToPointMode.Reverse || mode == MoveToPointMode.ReverseWithDifferentSpeed) {
				SwapPoints ();   
                
			}

			if(automatic) doMove = true;
		}
	}

	void SwapPoints(){
		if (Vector3.Distance (_moveToPoint, originalPositon) != 0) {
			tempPoint = _moveToPoint;
            _moveToPoint = originalPositon;

            if(mode == MoveToPointMode.ReverseWithDifferentSpeed)
            {
                tempSpeed = speed;
                speed = speedReverse;
            }
            
		} else {
            _moveToPoint = tempPoint;   

            if(mode == MoveToPointMode.ReverseWithDifferentSpeed)
            {
                speed = tempSpeed;
            }
		}
	}

	public void DoMove(){
		doMove = true;
	}

	void OnDrawGizmos(){
        Vector3 pos = Vector3.zero;

        if (Application.isPlaying)
        {
            if(pointType == PointType.WorldSpaceMode)
                pos = _moveToPoint;
            else
                pos = transform.TransformPoint(moveToPoint);
        }
        else
        {
            pos = transform.TransformPoint(moveToPoint);
        }
        //pos.x += transform.position.x;
        //pos.y += transform.position.y;

        //if(gameObject.name == "fence2")
        //    Debug.LogWarning(transform.position + "------------------" + pos);

        Gizmos.color = debugCollisionColor;
		Gizmos.DrawSphere (pos, .05f);

		Gizmos.color = debugCollisionColor;

		Gizmos.DrawLine (transform.position, pos);
    }

}

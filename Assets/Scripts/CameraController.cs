using System;
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

	public float verticalScrollArea = 10f;
	public float horizontalScrollArea = 10f;
	public float verticalScrollSpeed = 10f;
	public float horizontalScrollSpeed = 10f;

	public float scrollSpeed = 1f;
	public float axisSpeed = 1f;

	public void EnableControls(bool _enable)
	{

		if (_enable)
		{
			ZoomEnabled = true;
			MoveEnabled = true;
			CombinedMovement = true;
		}
		else
		{
			ZoomEnabled = false;
			MoveEnabled = false;
			CombinedMovement = false;
		}
	}

	public bool ZoomEnabled = true;
	public bool MoveEnabled = true;
	public bool CombinedMovement = true;
	public bool AreaMouse = false;

	private Vector2 _mousePos;
	private Vector3 _moveVector;
	private float _xMove;
	private float _yMove;
	private float _yMoveO;
	private float _zMove;


	private void Start()
	{
	}


	void Update()
	{
		_mousePos = Input.mousePosition;

		//Move camera if mouse is at the edge of the screen
		if (MoveEnabled)
		{

			//Move camera if mouse is at the edge of the screen
			if (AreaMouse)
			{
				if (_mousePos.x < horizontalScrollArea)
				{
					_xMove = -1;
				}
				else if (_mousePos.x >= Screen.width - horizontalScrollArea)
				{
					_xMove = 1;
				}
				else {
					_xMove = 0;
				}

				if (_mousePos.y < verticalScrollArea)
				{
					_zMove = -1;
				}
				else if (_mousePos.y >= Screen.height - verticalScrollArea)
				{
					_zMove = 1;
				}
				else {
					_zMove = 0;
				}
			} else
			{
				_zMove = 0;
				_xMove = 0;
			}
			//Move camera if wasd or arrow keys are pressed
			float xAxisValue = Input.GetAxis("Horizontal");
			float zAxisValue = Input.GetAxis("Vertical");

			if (xAxisValue != 0)
			{
				_xMove = (CombinedMovement ? _xMove : 0) + xAxisValue * axisSpeed;
			}

			if (zAxisValue != 0)
			{
				_zMove = (CombinedMovement ? _zMove : 0) + zAxisValue * axisSpeed;
			}

		}
		else {
			_xMove = 0;
			_yMove = 0;
		}

		// Zoom Camera in or out
		if (ZoomEnabled)
		{
			_yMove = -scrollSpeed*Input.GetAxis("Mouse ScrollWheel");
		}
		else {
			_yMove = 0;
		}
		_yMoveO = Mathf.Lerp(_yMoveO, _yMove, 0.1f);
		//move the object
		MoveMe(_xMove, _yMoveO, _zMove);
	}

	private void MoveMe(float x, float y, float z)
	{
		_moveVector = (new Vector3(x * horizontalScrollSpeed,y * verticalScrollSpeed, z * horizontalScrollSpeed) * Time.deltaTime);
		transform.Translate(_moveVector, Space.World);
		//Debug.Log(transform.position.y);
		transform.position = new Vector3(transform.position.x,Mathf.Clamp(transform.position.y, 60, 160) , transform.position.z);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiChuyen_Hau : MonoBehaviour
{
	public float speed = 2f;
	public float patrolRange = 3f; //phạm vi tuần tra
	public float chaseRange = 3f;
	public Transform player;

	private Vector3 startPos;
	private bool goingForward = true;
	private SpriteRenderer spriteRenderer;

	void Start()
	{
		startPos = transform.position;
		spriteRenderer = GetComponent<SpriteRenderer>();

		if (player == null)
		{
			GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
			if (playerObj != null)
			{
				player = playerObj.transform;
			}
		}
	}

	void Update()
	{
		float distanceToPlayer = Vector3.Distance(transform.position, player.position);

		if (distanceToPlayer <= chaseRange)
		{
			Vector3 targetPos = player.position;
			targetPos.x = Mathf.Clamp(targetPos.x, startPos.x, startPos.x + patrolRange);
			targetPos.y = transform.position.y;

			Vector3 direction = (targetPos - transform.position).normalized;
			transform.position += direction * speed * Time.deltaTime;

			// Flip sprite nếu đủ xa để tránh bị lật liên tục
			if (Mathf.Abs(targetPos.x - transform.position.x) > 0.1f)
			{
				spriteRenderer.flipX = (targetPos.x < transform.position.x);
			}
		}
		else
		{
			if (goingForward)
			{
				transform.position += Vector3.right * speed * Time.deltaTime;
				spriteRenderer.flipX = false;

				if (transform.position.x >= startPos.x + patrolRange)
				{
					goingForward = false;
				}
			}
			else
			{
				transform.position += Vector3.left * speed * Time.deltaTime;
				spriteRenderer.flipX = true;

				if (transform.position.x <= startPos.x)
				{
					goingForward = true;
				}
			}
		}
	}
}


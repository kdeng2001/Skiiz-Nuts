using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{

	public Sprite[] sprites;
	//public int spritePerFrame = 6;
	public bool loop = true;
	public bool destroyOnEnd = false;

	private int index = 0;
	private Image image;
	private int frame = 0;

	public int framesPerSecond = 12;
	private float secondsPerFrame;
	private float timeElapsed = 0;
	void Awake()
	{
		image = GetComponent<Image>();
	}

    void Update()
	{
		if (!loop && index == sprites.Length) return;
		frame++;
		timeElapsed += Time.deltaTime;
		//if (frame < spritePerFrame) return;
		if(timeElapsed < (float) 1f/framesPerSecond) { return; }
		image.sprite = sprites[index];
		frame = 0;
		timeElapsed = 0;
		index++;
		if (index >= sprites.Length)
		{
			if (loop) index = 0;
			if (destroyOnEnd) Destroy(gameObject);
		}
	}
}
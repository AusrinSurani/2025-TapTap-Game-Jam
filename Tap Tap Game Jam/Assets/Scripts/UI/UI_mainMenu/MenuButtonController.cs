using UnityEngine;

public class MenuButtonController : BaseUI 
{
	
	public int index;
	[SerializeField] bool keyDown;
	[SerializeField] int maxIndex;
	[SerializeField] private UI_option optionsUI;
	public AudioSource audioSource;

	void Start () 
	{
		audioSource = GetComponent<AudioSource>();
	}

	void Update () 
	{
		if(Input.GetAxis ("Vertical") != 0)
		{
			if(!keyDown)
			{
				if (Input.GetAxis ("Vertical") < 0)
				{
					if(index < maxIndex)
					{
						index++;
					}else
					{
						index = 0;
					}
				} 
				else if(Input.GetAxis ("Vertical") > 0)
				{
					if(index > 0)
					{
						index --; 
					}
					else
					{
						index = maxIndex;
					}
				}
				keyDown = true;
			}
		}
		else
		{
			keyDown = false;
		}
	}
}

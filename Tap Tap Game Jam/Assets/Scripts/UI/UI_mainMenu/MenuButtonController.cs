using UnityEngine;

public class MenuButtonController : BaseUI 
{
	public int index;
	[SerializeField] bool keyDown;
	[SerializeField] int maxIndex;
	[SerializeField] private UI_option optionsUI;

    [Header("制作人员名单")] public GameObject creatorList;

    public MenuButton[] buttons;

	void Start () 
	{
        AudioManager.Instance.AudioLoopPlay(AudioManager.Instance.mainMenuBGM);
        buttons = GetComponentsInChildren<MenuButton>();
        
        //显示制作人员名单
        if (GameFlowManager.Instance.currentDay == 3 &&
            GameFlowManager.Instance.currentIsOver && 
            GameFlowManager.Instance.currentChapter == ChapterOfGame.ChapterProgrammer)
        {
            creatorList.SetActive(true);

            foreach (var t in buttons)
            {
                t.canControl = false;
            }
        }
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

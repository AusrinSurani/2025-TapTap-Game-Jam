using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
	[Header("事件广播")]
	public VoidEventSO menuToOption;
	
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;

    void Update()
    {
		if(menuButtonController.index == thisIndex)
		{
			animator.SetBool ("selected", true);
			
			if(Input.GetAxis ("Submit") == 1)
			{
				animator.SetBool ("pressed", true);
			}
			else if (animator.GetBool ("pressed"))
			{
				animator.SetBool ("pressed", false);
				animatorFunctions.disableOnce = true;

				switch (thisIndex)
				{
					case 0:
						//TODO:切换场景开始游戏
						break;
					case 1:
						//TODO:切换场景继续游戏
						break;
					case 2:
						//唤出optionsUI
						menuToOption.OnEventRaise();
						break;
					case 3:
						//退出游戏
						Debug.Log("退出游戏");
						Application.Quit();
						break;
				}
			}
		}
		else
		{
			animator.SetBool ("selected", false);
		}
    }
}

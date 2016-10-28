using UnityEngine;
using System.Collections;

public class Script_Menu : MonoBehaviour 
{
	
	//游戏界面状态机
	
	//主菜单界面
	public const int STATE_MAINMENU = 0;
	//开始游戏界面
	public const int STATE_STARTGAME = 1;
	//游戏设置界面
	public const int STATE_OPTION = 2;
	//游戏帮助界面
	public const int STATE_HELP = 3;
	//游戏退出界面
	public const int STATE_EXIT = 4;
	
	//GUI皮肤
	public GUISkin mySkin;
	
	//游戏背景贴图
	public Texture textureBG; 
	//开始菜单截图
	public Texture tex_startInfo;
	//帮助菜单贴图
	public Texture tex_helpInfo;
	
	//游戏音乐资源
	public AudioSource music;  
	//当前游戏状态
	private int gameState;
	
	void Start ()
	{
		//初始化游戏状态为：主菜单界面
		gameState = STATE_MAINMENU;
		//设置鼠标显示
		Cursor.visible = true;
	}
	
	void OnGUI()
	{
	
		switch(gameState)
		{
			case STATE_MAINMENU:
				//绘制主菜单界面
				RenderMainMenu();
			break;
			case STATE_STARTGAME:
				//绘制游戏开始界面
				RenderStart();
			break;
			case STATE_OPTION:
				//绘制游戏设置界面
				RenderOption();
			break;
			case STATE_HELP:
				//绘制游戏帮助界面
				RenderHelp();
			break;
			case STATE_EXIT:
				//绘制游戏退出界面
				//目前直接关闭退出游戏
			break;
		}
		

	}
	//绘制主菜单界面
	void RenderMainMenu()
	{
		//设置界面皮肤
		GUI.skin = mySkin;
		//绘制游戏背景图
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),textureBG);
		//开始游戏按钮
		if(GUI.Button(new Rect (0,30,623,153),"","start"))
		{
			//进入开始游戏状态
			//目前由于是测试阶段
			//后期会在这里重新载入新的游戏场景
			//gameState = STATE_STARTGAME;
			
			Application.LoadLevel ("Scene_Game");
		}
		//游戏设置按钮
		if(GUI.Button(new Rect (0,180,623,153),"","option"))
		{
			//进入开始游戏状态
			gameState = STATE_OPTION;
		}
		//游戏帮助按钮
		if(GUI.Button(new Rect (0,320,623,153),"","help"))
		{
			//进入游戏帮助状态
			gameState = STATE_HELP;
		}
		//游戏退出按钮
		if(GUI.Button(new Rect (0,470,623,153),"","exit"))
		{
			//退出游戏
			Application.Quit();
		}
	}
	//绘制游戏开始界面
	void RenderStart()
	{
		GUI.skin = mySkin;
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),tex_startInfo);
		//绘制返回按钮
		if(GUI.Button(new Rect (0,500,403,78),"","back"))
		{
			//返回游戏主菜单
			gameState = STATE_MAINMENU;
		}
	}
	//绘制游戏帮助界面
	void RenderHelp()
	{
		GUI.skin = mySkin;
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),tex_helpInfo);
		if(GUI.Button(new Rect (0,500,403,78),"","back"))
		{
			gameState = STATE_MAINMENU;
		}
	}
	//绘制游戏设置界面
	void RenderOption()
	{
		GUI.skin = mySkin;
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),textureBG);
		
		//开启音乐按钮
		if(GUI.Button(new Rect (0,0,403,75),"","music_on"))
		{
			if (!music.isPlaying)
			{  
          		//播放音乐
                music.Play();  
            }  

		}
		//关闭音乐按钮
		if(GUI.Button(new Rect (0,200,403,75),"","music_off"))
		{
			//关闭音乐
			music.Stop();
		}
		//返回按钮
		if(GUI.Button(new Rect (0,500,403,78),"","back"))
		{
			//返回游戏主菜单
			gameState = STATE_MAINMENU;
		}
	}
}

using UnityEngine;
using System.Collections;

public class Script_Game : MonoBehaviour 
{
	//游戏状态机
	
	//游戏中状态
	public const int STATE_GAME =0;
	//游戏胜利状态
	public const int STATE_WIN =1;
	//游戏失败状态
	public const int STATE_LOSE =2;
	
	//枚举角色鼠标位置
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	float rotationY = 0F;
	//鼠标准心
	public Texture2D tex_fire;
	
	//血条资源贴图
	public Texture2D tex_red;
	public Texture2D tex_black;
	
	//生命值贴图
	public Texture2D tex_hp;
	
	//战斗胜利资源贴图
	public Texture2D tex_win;
	//战斗失败资源贴图
	public Texture2D tex_lose;
	//游戏音乐资源
	public AudioSource music;  
	
	//主角血值
	public int HP = 100;
	
	//图片数字资源
	Object[] texmube;
	
	
	//当前游戏状态
	int gameState;
	
	
	void Start ()
	{
		//取消默认鼠标图标
		Cursor.visible = false;
		//读取图片资源
		texmube = Resources.LoadAll("number");
		//设置默认状态为游戏中
		gameState = STATE_GAME;
		
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
	
	
	void Update ()
	{
		
		switch(gameState)
		{
		case STATE_GAME:
			UpdateGame();
			break;
		case STATE_WIN:
		case STATE_LOSE:
			if (Input.GetKey (KeyCode.Escape))
			{
			Application.LoadLevel ("MainMenu");
			}
			break;	
		}

			
	}
	
	void OnGUI()
	{

		switch(gameState)
		{
		case STATE_GAME:
			RenderGame();
			break;
		case STATE_WIN:
			GUI.DrawTexture(new Rect(0,0,tex_win.width,tex_win.height),tex_win);
			break;
		case STATE_LOSE:
			GUI.DrawTexture(new Rect(0,0,tex_lose.width,tex_lose.height),tex_lose);
			break;	
		}
	}	
	

	void UpdateGame()
	{
		if (Input.GetKey (KeyCode.Escape))
		{
//			Application.LoadLevel ("MainMenu");
		}
		
		//计算摄像头的旋转
		if (axes == RotationAxes.MouseX)
		{
			//旋转角色
			transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
		}
		else
		{	
			//设置角色欧拉角
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}
	
	void RenderGame()
	{
//		if(tex_fire)
//		{
//			//绘制鼠标准心
//			float x = Input.mousePosition.x - (tex_fire.width>>1);
//			float y = Input.mousePosition.y + (tex_fire.height>>1);
//			GUI.DrawTexture(new Rect(x,Screen.height - y,tex_fire.width,tex_fire.height),tex_fire);
//		}
			
			//绘制主角血条
			int blood_width = tex_red.width * HP/100;
			GUI.DrawTexture(new Rect(5,Screen.height - 50,tex_black.width,tex_black.height),tex_black);
			GUI.DrawTexture(new Rect(5,Screen.height - 50,blood_width,tex_red.height),tex_red);
			
			//绘制生命值贴图
			GUI.DrawTexture(new Rect(5,Screen.height - 80,tex_hp.width,tex_hp.height),tex_hp);
			//绘制主角血值
			Tools.DrawImageNumber(200,Screen.height - 80,HP,texmube);
	}

	//主角被攻击
	void HeroHurt()
	{
		HP--;
		if(HP <=0)
		{
			HP = 0;
			gameState = STATE_LOSE;
		}
	}
	//吃道具加血
	void HeroAddBlood()
	{
		HP+=50;
		if(HP>=100)
		{
			HP = 100;
		}
	}
	
	//敌人被攻击
	void EnemyHurt()
	{
		GameObject []enemy = GameObject.FindGameObjectsWithTag("enemy");
		//敌人对象数字长度为1表示敌人全部死亡
		if(enemy.Length == 1)
		{
			gameState = STATE_WIN;
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit) 
	{
		//获取角色控制器碰撞到的游戏对象
		GameObject collObj = hit.gameObject;
		//是否是加血箱子对象
		if(collObj.tag == "Yaowan")
		{
			//主角加血
			HeroAddBlood();
			//销毁箱子
			Destroy(collObj);
		}
		
	}
}
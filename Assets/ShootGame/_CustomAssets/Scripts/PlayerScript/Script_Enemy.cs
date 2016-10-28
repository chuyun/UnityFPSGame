using UnityEngine;
using System.Collections;

public class Script_Enemy : MonoBehaviour 
{
	//敌人状态
	
	//敌人站立状态
	public const int STATE_STAND = 0;
	//敌人行走状态
	public const int STATE_WALK = 1;
	//敌人奔跑状态
	public const int STATE_RUN = 2;
	//敌人暂停状态
	public const int STATE_PAUSE = 3;
	
	//记录当前敌人状态
	private int enemyState;
	//主角对象
	private GameObject hero;
	
	//备份上一次敌人思考时间
	private float backUptime;
	//敌人思考后一次行为的时间
	public const int AI_THINK_TIME = 2;
	//敌人的巡逻范围
	public const int AI_ATTACK_DISTANCE = 10;
	
	//是否绘制敌人血条
	bool showBlood = false;
	
	//血条资源贴图
	public Texture2D tex_red;
	public Texture2D tex_black;
	
	//生命值贴图
	public Texture2D tex_hp;
	
	//血值
	private int HP = 100;
	
	//敌人仇恨
	private bool ishatred = false;
	
	//图片数字资源
	Object[] texmube;
	
	void Start()
	{
		//得到主角对象
		hero = GameObject.Find("/SinglePlayerCharacter");
		//读取图片资源
		texmube = Resources.LoadAll("number");
		//设置默认敌人状态为站立状态
		enemyState = STATE_STAND;

	}

	void OnGUI()
	{
		if(showBlood)
		{
			//绘制生命值贴图
			GUI.DrawTexture(new Rect(5,0,tex_hp.width,tex_hp.height),tex_hp);
			//绘制主角血值
			Tools.DrawImageNumber(200,0,HP,texmube);
			
			//绘制敌人血条
			int blood_width = tex_red.width * HP/100;
			GUI.DrawTexture(new Rect(5,50,tex_black.width,tex_black.height),tex_black);
			GUI.DrawTexture(new Rect(5,50,blood_width,tex_red.height),tex_red);
			
			
		}	
	}
	void Update() 
	{
		if (Vector3.Distance (transform.position, hero.transform.position) < AI_ATTACK_DISTANCE) {
			ishatred = true;

		}
		if (Vector3.Distance (transform.position, hero.transform.position) > 3*AI_ATTACK_DISTANCE) {
			ishatred = false;

		}


		//判断敌人与主角的距离
//		if(Vector3.Distance(transform.position,hero.transform.position) < AI_ATTACK_DISTANCE || ishatred)
		if(ishatred)
		{
		     //敌人进入奔跑状态
			 gameObject.GetComponent<Animation>().Play("run");
			 enemyState = STATE_RUN;
			 //设置敌人面朝主角方向
			 transform.LookAt(hero.transform);  

			if (Vector3.Distance (transform.position, hero.transform.position) <= 5 && ishatred) {			 //随机攻击主角

				int rand = Random.Range (0, 20);
				if (rand == 0) {
					hero.SendMessage ("HeroHurt");
				}

			} else {

			}



		}
		//敌人进入巡逻状态
		else
		{
			ishatred = false;
			//计算敌人思考时间
			if(Time.time - backUptime >=AI_THINK_TIME)
			{
				//敌人开始思考
				backUptime = Time.time;
				
				//取得0 - 2之间随机数
				int rand = Random.Range(0,2);
				
				if(rand == 0)
				{
					//敌人进入站立状态
					gameObject.GetComponent<Animation>().Play("idle");
					enemyState = STATE_STAND;
				}
				else if(rand == 1)
				{  
					//敌人进入行走状态
					//敌人随机旋转角度
					Quaternion rotate = Quaternion.Euler(0,Random.Range(1,5) *90,0);
					//1秒内完成敌人旋转
					transform.rotation = Quaternion.Slerp(transform.rotation,rotate, Time.deltaTime*1000);  
					//播放行走动画
					gameObject.GetComponent<Animation>().Play("walk");
					enemyState = STATE_WALK;
				}
			}
		}
		
			switch(enemyState)
			{
			case STATE_STAND:
				
				break;
			case STATE_WALK:
				//敌人行走
				transform.Translate(Vector3.forward *Time.deltaTime);
				break;
			case STATE_RUN:
				//敌人朝向主角奔跑
				if(Vector3.Distance(transform.position,hero.transform.position) >3)
				{
					transform.Translate(Vector3.forward *Time.deltaTime * 5);
				}
				break;
			
			}
	}	
	
	void OnMouseDown()
	{
		//击打中敌人目标
		if(HP > 0)
		{
			//减血
			HP-=5;
			//击打回馈
			transform.Translate(Vector3.back * 0.5F);
			//击中敌人后立刻进入战斗状态
			ishatred = true;
		}else
		{
			//敌人死亡
			Destroy (gameObject);
			//发送死亡消息
			hero.SendMessage("EnemyHurt");
		}
	}
	
	void OnMouseUp()
	{
		//击打回馈还原
		transform.Translate(Vector3.forward * 0.5F);
	}
	
	void OnMouseOver()
	{
		//开始绘制血条
		showBlood = true;
	}
	
	void OnMouseExit()
	{
		//结束绘制血条
		showBlood = false;
	}

}

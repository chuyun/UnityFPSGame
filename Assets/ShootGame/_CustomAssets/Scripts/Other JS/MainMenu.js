

#pragma strict
import System.Collections.Generic;

var guiStyle : GUISkin;
var objective : String;
var showTime : boolean = true;

@HideInInspector
var finishedGame : boolean;
private var weaponManager : WeaponManager;
private var startGame : boolean = true;
private var timer : float;
private var mainMenu : boolean;
private var resolutions : Resolution[];
private var QualityNames : String[];
private var resolutionIndex : int = 3;
private var scroll : Vector2;
private var scroll2 : Vector2;
private var scroll3 : Vector2;


function Start () {
//初始化weaponManager
	weaponManager = GameObject.FindWithTag("WeaponManager").GetComponent.<WeaponManager>();
//	初始化主菜单为true 第一次进入游戏
	mainMenu = true;
	Invoke("Pause", 0.01);
	resolutions = Screen.resolutions;
	resolutionIndex = (resolutions.Length-1)/2;
	QualityNames = QualitySettings.names;
}

function Update () {
	if(startGame && weaponManager.SelectedWeapon){
		weaponManager.SelectedWeapon.gameObject.SetActive(false);
	}
	if(!startGame){
		if(!finishedGame){
			timer += Time.deltaTime;
		}
//		按下Tab键，弹出主菜单
		if(Input.GetKeyDown(KeyCode.Tab)){
			mainMenu = !mainMenu;
			Pause();
		}
//		当前界面不是MainMenu时，锁定鼠标箭头，防止误触
		if(!mainMenu){
			Screen.lockCursor = true;
		}
	}
	
	if(Input.GetKeyDown(KeyCode.P)){
		Screen.fullScreen = !Screen.fullScreen;
		if(!Screen.fullScreen){
			Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, true);
		}
	}
}

function OnGUI(){
	GUI.skin = guiStyle;
	
	GUI.color.a = 0.7;

    if(mainMenu){
    	GUI.Window (0, Rect (Screen.width/2 - 250, Screen.height/2 - 150, 500, 300), MainMenu, "Main Menu");

    }

}
//主菜单界面设置
function MainMenu (windowID : int) {  
	GUILayout.Space (10); 
	GUILayout.BeginHorizontal();

 	GUILayout.Space (175); 
// 	Start Game 使用 GUI 的BUtton控件
    if(startGame){
    	if(GUILayout.Button("Start Game", GUILayout.Width(150), GUILayout.Height(30))){
    		startGame = false;
    		mainMenu = false;
    		Pause();
    		weaponManager.SelectedWeapon.gameObject.SetActive(true);
    		weaponManager.TakeFirstWeapon(weaponManager.SelectedWeapon.gameObject);
    	}
    }else{
//    回到游戏
    	GUILayout.BeginVertical();
	  	if(GUILayout.Button("Resume Game", GUILayout.Width(150), GUILayout.Height(30))){
	  		Time.timeScale = 1;
	  		startGame = false;
    		mainMenu = false;
    		Pause();

    	}
    	GUILayout.EndVertical();
    }
    GUILayout.EndHorizontal();
    
	GUILayout.Space (5);
	GUI.color = Color(0, 20, 0, 0.6);
//	ScrollView 显示帮助信息
    GUILayout.Space (5);
    GUI.color = Color.white;
    scroll3 = GUILayout.BeginScrollView(scroll3, GUILayout.Width(480), GUILayout.Height(150));
    	GUI.color = Color(20, 20,0, 0.6);
	    GUILayout.Label("Tab - Main Menu"); 
	    GUILayout.Label("Q - slow motion");
	    GUILayout.Label("F - weapon pick up");
	    GUILayout.Label("R - reload");
	    GUILayout.Label("Space - jump");
	    GUILayout.Label("1/2 - weapon change");
    GUILayout.EndScrollView();
}

//游戏暂停
function Pause(){
	if(mainMenu){
		Time.timeScale = 0.0001;
		Screen.lockCursor = false;
	}else{
		Time.timeScale = 1;
		Screen.lockCursor = true;//锁定鼠标
	}
}
var isQuit=false;
var isIsland1=false;
var isIsland2=false;
var isDesert=false;
var isBack=false;
var myTexture;
var myPlane;

function Start(){
	Screen.showCursor = true;
}

function OnMouseEnter(){
	myPlane = GameObject.Find("Plane");
	//change text color
	renderer.material.color=Color.red;
	if (isIsland1==true){
		myTexture = Resources.Load("MainMenuTexture") as Texture;
		myPlane.renderer.material.mainTexture = myTexture;
	}
	if (isIsland2==true){
		myTexture = Resources.Load("Island2Texture") as Texture;
		myPlane.renderer.material.mainTexture = myTexture;
	}
	if (isDesert == true){
		myTexture = Resources.Load("DesertImage") as Texture;
		myPlane.renderer.material.mainTexture = myTexture;
	}
}

function OnMouseExit(){
	//change text color
	renderer.material.color=Color.white;
}

function OnMouseUp(){
	if (isBack==true){
		Application.LoadLevel(0);
	}
	if (isIsland1==true){
		//Load Island 1
		Application.LoadLevel(3);
	}
	if (isIsland2==true){
		//Load Island 2
		Application.LoadLevel(5);
	}
	if (isDesert==true){
		//Load Desert Level
		Application.LoadLevel(4);
	}
	if (isQuit==true) {
		//quit the game
		Application.Quit();
	}
}

function Update(){
	//quit game if escape key is pressed
	if (Input.GetKey(KeyCode.Escape)) { 
		Application.Quit();
	}
}
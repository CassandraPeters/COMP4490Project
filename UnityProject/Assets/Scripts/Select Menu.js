var isQuit=false;
var isIsland1=false;
var isIsland2=false;
var isDesert=false;
var myTexture;
var myPlane;

function OnMouseEnter(){
	myPlane = GameObject.Find("Plane");
	//change text color
	renderer.material.color=Color.red;
	if (isIsland1==true){
		myTexture = Resources.LoadAssetAtPath("Assets/Graphics/Textures/MainMenuTexture.png",typeof(Texture));
		myPlane.renderer.material.mainTexture = myTexture;
	}
	if (isDesert == true){
		myTexture = Resources.LoadAssetAtPath("Assets/Graphics/Textures/DesertImage.png",typeof(Texture));
		myPlane.renderer.material.mainTexture = myTexture;
	}
}

function OnMouseExit(){
	//change text color
	renderer.material.color=Color.white;
}

function OnMouseUp(){
	if (isIsland1==true){
		//Load Island 1
		Application.LoadLevel(3);
	}
	if (isIsland2==true){
		//Load Island 2
		
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
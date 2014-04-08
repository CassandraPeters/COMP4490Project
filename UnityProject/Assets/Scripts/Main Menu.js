var isQuit=false;
var isSelect=false;
var isRandom=false;
var isOption=false;

function Start(){
	Screen.showCursor = true;
}

function OnMouseEnter(){
	//change text color
	renderer.material.color=Color.red;
}

function OnMouseExit(){
	//change text color
	renderer.material.color=Color.white;
}

function OnMouseUp(){
	//is this quit
	if (isSelect==true){
		Application.LoadLevel(2);
	}
	if (isOption==true){
		Application.LoadLevel(1);
	}
	if (isRandom == true){
		Application.LoadLevel(6);
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
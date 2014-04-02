var isQuit=false;
var isSelect=false;
var isRandom=false;
var isOption=false;
var isBack=false;

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
	}
	if (isOption==true){
	}
	if(isBack==true){
		Application.LoadLevel(0);
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
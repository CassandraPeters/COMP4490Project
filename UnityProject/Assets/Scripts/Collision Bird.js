var script : RandomFly;

function OnTriggerEnter(other : Collider){
	if(other.tag == "Bird"){
		var bird = other;
		script = bird.GetComponent(RandomFly);
		bird.animation.enabled = true;
		script.enabled = true;
	}
}
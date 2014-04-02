#pragma strict

private var textureObject:GameObject;

public var firstTexture:Texture;
public var secondTexture:Texture;
public var thirdTexture:Texture;

private var newTexture:Texture;

private var triggerChange = false;
private var changeCount = 1.0;


function Start () {
    textureObject = gameObject.Find("myTextureObject");
    textureObject.renderer.material.SetFloat( "_Blend", 1 );
}

function Update () {
     if(triggerChange == true) {
          changeCount = changeCount - 0.05;

          textureObject.renderer.material.SetFloat( "_Blend", changeCount );
          if(changeCount <= 0) {
               triggerChange = false;
               changeCount = 1.0;
               textureObject.renderer.material.SetTexture ("_Texture2", newTexture);
               textureObject.renderer.material.SetFloat( "_Blend", 1);
          }

     }
}

public function changeTexture(myArg : float) {
  //yieldWaitForSeconds(0.3);

  if(myArg == 1) {
      newTexture = firstTexture;
  } else if (myArg == 2){
      newTexture = secondTexture;
  } else if (myArg == 3){
      newTexture = thirdTexture;
  }

  textureObject.renderer.material.mainTexture = newTexture;
  triggerChange = true;

}
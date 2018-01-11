#pragma strict

var visible:boolean=true; //Show or hide gameObject object in game

function Start () {
    if (!visible) GetComponent.<MeshRenderer>().enabled=false;
    if (visible) GetComponent.<MeshRenderer>().enabled=true;
}

#pragma strict

function Start () {
    if (Screen.fullScreen)
    {
        //MacBook Pro Retina 15: width = 2880 , MacBook Pro Retina 13: width = 2496 ?
        //could check device model name, but not sure now about retina 13 device model name
        //if last resolution is almost retina resolution...
        var resolutions : Resolution[] = Screen.resolutions;
        if (resolutions.length && resolutions[resolutions.length - 1].width > 2048)
        {
            Screen.fullScreen = false;
            yield;
            
            Screen.fullScreen = true;
            yield;
        }
    }
}

function Update () {

}
function UnityProgress (gameInstance, progress) { 
	this.message = "Loading...";
	console.log("Progress: "+progress);

  if (!gameInstance.Module)
    return;
  if (!gameInstance.logo) {
    if (this.progress < progress)
      this.progress = progress;
    if (progress == 1) {
      this.message = "Initializing...";
    } 
    /*var length = 200 * Math.min(this.progress, 1);
    bar = document.getElementById("progressBar")
    bar.style.width = length + "px";
    document.getElementById("loadingInfo").innerHTML = this.message;*/
  }
  
}

function UnityProgressDone(){
	console.log("Progress Done");
    document.getElementById("spinner").style.display = "none";
	document.getElementById("loadingBox").style.display = "none";
}
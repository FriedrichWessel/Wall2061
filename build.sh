#!/bin/bash
cp TemplateData/ Build/TemplateData
cp index.html Build/index.html
 var gameInstance = UnityLoader.instantiate("gameContainer", "Build/NormalTemplateCustomLoadingBar.json", { 
--> BUILDNAME.json

Remove: 
        UnityLoader.SystemInfo.hasWebGL ? UnityLoader.SystemInfo.mobile ? e.popup("Please note that Unity WebGL is not currently supported on mobiles. Press OK if you wish to continue anyway.", [{
            text: "OK",
            callback: t
        }]) : ["Firefox", "Chrome", "Safari"].indexOf(UnityLoader.SystemInfo.browser) == -1 ? e.popup("Please note that your browser is not currently supported for this Unity WebGL content. Press OK if you wish to continue anyway.", [{
            text: "OK",
            callback: t
        }]) : t() : e.popup("Your browser does not support WebGL", [{
            text: "OK",
            callback: r
        }])

NOTE: This script will only create passive 3D effect on Passive 3D displays! Your display must have polarizers for left and right eyes in order for this script to work. It will not function properly on Active Stereo displays or displays without horizontal polarizers. This is usually the #1 issue when trying to use this script, so ensure you are using the proper hardware please.

1) Drop the InterlaceCamRigGenerator into your scene.
2) Assign the appropriate variables via the editor as you like.
3) Ensure that stereo is enabled on the script in the inspector
4) Make sure you either have Attach to Vive HMD set to true, OR that you have designated a gameobject as the parent for the camera rig
5) Run your game and the script will populate all the things needed to interlace for passive 3d.

Extra stuff:
You can adjust the interlace at runtime using F1 & F2. If you notice that they eyes are flipped you can quickly swap with F3 & F4. If the adjustment seems to happen to quickly or too slowly you can change the "Disp Step" in the inspector.

If the screen resizes at runtime, the interlace mask will need to be rebuilt. You can rebuild the texture by hitting F5. Note that this is slow and not something you should do at runtime if possible, or without writing a more efficient method for doing it than my simple hack. That said, if you need to do it occasionally it should be ok.

In order to save a settings file hit f6 after you have properly calibrated for your scene. When you want to use these saved settings make sure the box "use saved settings" is ticked. This will cause whatever settings you have in the inspector to be over written with the saved settings, assuming the script can find the save file.
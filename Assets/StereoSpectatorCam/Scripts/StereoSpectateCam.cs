//(C) 2017 Andrew Guagliardo
//Laboratory for Advanced Visualization & Applications, Academy for Creative Media
//University of Hawaii at Manoa
//Original version: 1/23/17
//Current version: 4/13/17

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


//this will generate three cameras and attach them to a parent gameobject
//and create an interleaved image that is suitable for viewing on passive stereo displays or televisions
//you should not need to enable any 3D settings on your TV or monitor, this script should automatically line
//the left and right eye images to the appropriate polarizers on the screen
public class StereoSpectateCam : MonoBehaviour {
    //public stuff
    [Tooltip("Read from saved settings. If none are found, you will get an error message.")]
    public bool useSavedSettings;

    [Tooltip("The GameObject you want this interlace camera rig to be attached to.")]
    public GameObject InterlaceCamRigParent;

    [Tooltip("The amount of distance between the left and right cameras. You can adjust at runtime with F1 or F2")]
    public float displacement = 0.2f;

    [Tooltip("The amount the displacement changes when using F1(-) or F2(+) at runtime")]
    public float dispStep = 0.001f;

    [Tooltip("This will flip the interlace mask so that the images displayed to each eye are swapped. If your stereo image looks strange try enabling this. At runtime you can toggle this on with F3 and off with F4")]
    public bool flip = false;

    [Tooltip("Toggle this on if you want to have a smoothing effect on the camera motion")]
    public bool slerping = false;

    [Tooltip("Slerping strength, lower and the camera will lag behind the player's movement, higher and it will eventually just appear to match the players motion exactly")]
    public float slerpValue = .05f;

    [Tooltip("Disabling this will turn off the cameras that create the interlace effect")]
	public bool stereo = true;

    [Tooltip("The material we are using for our graphics magic. It is recommended that you do not adjust or change this, unless you know what you are doing")]
    public Material mat;

    //private stuff that shouldn't need to be touched
    Camera rightCamera, leftCamera, interlacedCam;
    float dUpdate;
    Texture2D interlaceMask;
    
    // Use this for initialization
    void Start ()
    {
        if(useSavedSettings)
        {
            Debug.Log("Attempting to load saved settings.");
            LoadSettings();
        }

        dUpdate = displacement;

        if(InterlaceCamRigParent == null)
        {
            EditorUtility.DisplayDialog("Missing Object", "You have not set a GameObject parent for the StereoSpectateCam", "Oops!");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        MakeCameras();

        SetCameraProperties();
            
        CreateTextures();

        //when testing pause and make sure everything is right
        //Debug.Break();
    }

    void MakeCameras()
    {
        Color black = new Vector4(0, 0, 0, .02f);
        rightCamera = new GameObject().AddComponent<Camera>();
        rightCamera.name = "rightCamera";
        rightCamera.clearFlags = CameraClearFlags.SolidColor;
        rightCamera.backgroundColor = black;
        leftCamera = new GameObject().AddComponent<Camera>();
        leftCamera.name = "leftCamera";
        leftCamera.clearFlags = CameraClearFlags.SolidColor;
        leftCamera.backgroundColor = black;
        interlacedCam = gameObject.AddComponent<Camera>();
        interlacedCam.name = "interlacedCam";
    }

    void SetCameraProperties()
    {
        //set their transforms to be children of the InterlaceCameraRigParent, this is only done if slerping is false
        if (!slerping)
        {
            rightCamera.transform.parent = leftCamera.transform.parent = transform.parent = InterlaceCamRigParent.transform;
        }
        else
        {
            rightCamera.transform.parent = leftCamera.transform.parent = transform;
            transform.localScale = InterlaceCamRigParent.transform.lossyScale;
        }

        rightCamera.transform.localPosition = new Vector3(displacement, 0, 0);
        leftCamera.transform.localPosition = new Vector3(-displacement, 0, 0);
        transform.localPosition = new Vector3(0, 0, 0);
        rightCamera.transform.localRotation = leftCamera.transform.localRotation = transform.localRotation = Quaternion.Euler(0, 0, 0);

        //misc adjustments, making sure clip plane is small, setting the depth so cameras write to the display screen
        //if using a HMD
        rightCamera.nearClipPlane = leftCamera.nearClipPlane = interlacedCam.nearClipPlane = .01f;
        rightCamera.depth = leftCamera.depth = interlacedCam.depth = 1;
        rightCamera.stereoTargetEye = leftCamera.stereoTargetEye = interlacedCam.stereoTargetEye = StereoTargetEyeMask.None;
    }

    void CreateTextures()
    {
        MakeInterlaceTexture();
        //assign interlaceMask
        mat.mainTexture = interlaceMask;

        //make our other textures to grab from the cameras
        RenderTexture leftCamTex = new RenderTexture(Screen.width, Screen.height, 24);
        RenderTexture rightCamTex = new RenderTexture(Screen.width, Screen.height, 24);

        //plug in right/left camera rendertexture to the right slot 
        leftCamera.targetTexture = leftCamTex;
        rightCamera.targetTexture = rightCamTex;

        mat.SetTexture("_Texture1", leftCamTex);
        mat.SetTexture("_Texture2", rightCamTex);
    }

    void MakeInterlaceTexture()
    {
        Debug.Log(Screen.width + " X " + Screen.height);

        interlaceMask = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        //set the main texture to a r/g texture that matches the camera screen size and alternates r-g every row.
        //this is used to create our mask for interlacing
        for (int y = 0; y < Screen.height; y++)
        {
            if (y % 2 > 0)
            {
                for (int x = 0; x < Screen.width; x++)
                {
                    interlaceMask.SetPixel(x, y, Color.green);
                }
            }
            else
            {
                for (int x = 0; x < Screen.width; x++)
                {
                    interlaceMask.SetPixel(x, y, Color.red);
                }
            }
        }
        interlaceMask.Apply();

        //byte[] bytes = interlaceMask.EncodeToPNG();

        //// For testing purposes, also write to a file in the project folder
        //File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);

        if (flip)
        {
            mat.SetInt("_Flip", 1);
        }
    }

    //do the magic
    void OnPostRender()
    {
        Graphics.Blit(mat.mainTexture, mat);
    }


    void Update()
    {
        if (transform.localScale != InterlaceCamRigParent.transform.lossyScale)
        {
            transform.localScale = InterlaceCamRigParent.transform.lossyScale;
        }
    }

    void LateUpdate() {
        //TODO allow for a key command to store appropriate displacement for each project
        //        & checked for save file on startup, use file to populate displacement if found


        if(slerping)
        {
            transform.position = Vector3.Slerp(transform.position, InterlaceCamRigParent.transform.position, slerpValue);
            transform.rotation = Quaternion.Slerp(transform.rotation, InterlaceCamRigParent.transform.rotation, slerpValue);
        }
        //allows for runtime adjustment if so desired
        //once you find a good setting that works for you just override the displacement value
        //in the inspector with value that works for you
        if (Input.GetKey("f1") && displacement > -200)
        {
            dUpdate = displacement - dispStep;
        }
        if (Input.GetKey("f2") && displacement < 200)
        {
            dUpdate = displacement + dispStep;
        }
        //key command to swap left and right eyes
        if(Input.GetKeyUp("f3"))
        {
            flip = true;
            mat.SetInt("_Flip", 1);
        }
        if(Input.GetKeyUp("f4"))
        {
            flip = false;
            mat.SetInt("_Flip", 0);
        }
        //rebuild interlace mask, for use if the screen changes size for some reason after Start() is called
        if(Input.GetKeyUp("f5"))
        {
            CreateTextures();
        }

        //save settings to be read later
        if(Input.GetKeyUp("f6"))
        {
            SaveSettings();
        }

        CheckProjectionStatus();
          
    }

    void CheckProjectionStatus()
    {
        if (stereo)
        {
            if(!rightCamera.enabled)
            {
                rightCamera.enabled = leftCamera.enabled = true;
            }

            if (dUpdate != displacement)
            {
                UpdateCameraPositions();
            }
        }
        else if(rightCamera.enabled)
        {
            //rightCamera.enabled = leftCamera.enabled = false;
        }
    }

	void UpdateCameraPositions()
    {
        
        rightCamera.transform.localPosition = new Vector3(dUpdate, 0, 0);
        leftCamera.transform.localPosition = new Vector3(-dUpdate, 0, 0);
        displacement = dUpdate;
            
    }

    void SaveSettings()
    {
        string path = "Assets/Resources/StereoSpectateSettings";
        StreamWriter writer = new StreamWriter(path, false);
        List<string> info = new List<string>();
        info.Add(displacement.ToString());
        info.Add(dispStep.ToString());
        info.Add(flip.ToString());
        info.Add(slerping.ToString());
        info.Add(slerpValue.ToString());
        info.Add(stereo.ToString());

        for(int i = 0; i < info.Count; i++)
        {
            writer.WriteLine(info[i]);
        }
        writer.Close();

        Debug.Log("Saved Settings");
    }

    void LoadSettings()
    {
        string path = "Assets/Resources/StereoSpectateSettings";
        string line;
        StreamReader reader = null;
        try
        {
           reader = new StreamReader(path);
        }
        catch(FileNotFoundException ex)
        {
            EditorUtility.DisplayDialog("No Settings File Found", "There was no settings file found!", "Oops!", ex.ToString());
            UnityEditor.EditorApplication.isPlaying = false;
        }

        if(reader != null)
        {
            try
            {

                line = reader.ReadLine();
                displacement = float.Parse(line);

                line = reader.ReadLine();
                dispStep = float.Parse(line);

                line = reader.ReadLine();
                if (line.Equals("True"))
                {
                    flip = true;
                }
                else { flip = false; }

                line = reader.ReadLine();
                if (line.Equals("True"))
                {
                    slerping = true;
                }
                else { slerping = false; }

                line = reader.ReadLine();
                slerpValue = float.Parse(line);

                line = reader.ReadLine();
                if (line.Equals("True"))
                {
                    stereo = true;
                }
                else { stereo = false; }
            }
            catch (NullReferenceException ex)
            {
                EditorUtility.DisplayDialog("Something is wrong with the settings file.", "Could not parse the settings file.", "Ok", ex.ToString());
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }

        Debug.Log("Settings loaded.");
    }
}


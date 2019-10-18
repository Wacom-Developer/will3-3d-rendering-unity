# Getting Started - WILL for HTC Vive

## Download the SDK

Download the SDK from https://developer.wacom.com/developer-dashboard

* Login using your Wacom ID
* Select **Downloads for ink**
* Download **WILL Unity Plugin**
* Accept the End User License Agreement to use the SDK

The downloaded Zip file contains the package: *Wacom.Ink.Unity.0.3.5.unitypackage*.

## WILL for HTC Vive

1. Create a new 3D project with Unity or open an existing project.

2. In the PlayerSettings menu check "Virtual Reality Supported" and leave only OpenVR under "Virtual Reality SDKs".

3. Download SteamVR and Import it in your project. (SteamVR Unity Plugin 2.0)

4. Delete the Main Camera from your scene

5. In the Project tree navigate to Assets/SteamVR/Prefabs/ and add SteamVR and CameraRig prefabs to your scene.

6. Import the Wacom.Ink.Unity package (Go to Assets -> Import Package -> Custom Package and browse for Wacom.Ink.Unity.0.3.5.unitypackage)




> Pick the InkWriter prefab from the Will.Ink folder and drag it to the scene.

> Create an empty Game Object in the scene and assign it to the "Strokes Collection" field of the Ink Writer.

If you run the app at this point you should be able to draw strokes with the HTC Vive Controller.


----

        





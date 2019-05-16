# WILL XR Ink for Unity


**NOTICE:**

All the content provided in this repository is **classified as Wacom Confidential Material**, therefore the signed NON-DISCLOSURE AGREEMENT applies.
Be aware that the **technology components are still under active development** with minimal QA. Moreover, **API interfaces and functionality could be changed or removed**.


## Description

WILL XR Ink is a component for producing and rendering of 3D digital ink in Unity applications.

### Key Features:

  - Volumetric 3D ink with variable width.

  - Particle 3D ink with variable width.

  - Smoothing of input values.


### Included components:

  Wacom.Ink.Unity.0.3.3.unitypackage - a unity package containing the WILL XR Ink component.

### Included demo projects:

  TestWillInk_HTCVive - a sample project that demonstrates the usage of WILL XR Ink with Steam VR 2.0 / HTC Vive.

---

# Getting Started

## Development Environment

Unity 2019.1.2f1

## Running the Demo Application

  Open TestWillInk_HTCVive\Assets\Scenes\SampleScene.unity

  Attach an HTC Vive or Vive Pro setup.

  Press the Play button to start and use the HTC Vive controller's triggers to draw in VR.

  By default volumetric ink drawing is enabled. To enable particle ink - select the ParticleInk object in the Scene Hierarchy and enable it in the Inspector.

## Technology Guide

  The scripts that provide the inking functionality are ParticleInkWriter and VolumetricInkWriter (located in Will.Ink.Unity.dll). Assign one of them to an empty GameObject.
  Assign another script to the same GameObject to handle controller input. From the second script obtain a reference to the the IInkWriter interface:

```csharp
    private IInkWriter m_inkWriter;

	void Start()
	{
		m_inkWriter = GetComponent<IInkWriter>();
	}
```
  
  In the Update method you can call the OnPointerPressed, OnPointerMoved and OnPointerReleased methods in response to controller input:


```csharp
 	void Update()
	{
        ...

        if (controller.ButtonPressed)
        {
            PointerPoint pp = new PointerPoint(controller.position, controller.pressure, timestamp);

            m_inkWriter.OnPointerPressed(ref pp);
        }

        ...
    }
```

  Please see the TestWillInk_HTCVive sample for details.


## Feedback / Support
Participants of the Wacom Beta Program will be provided with optional access to our Slack channel:

- [Slack channel](https://wacom-will.slack.com)

Product Managers and a support engineer will be available in the channel to answer questions and receive valuable feedback.

If you experience issues with the technology components, please file a ticket in our Developer Support Portal:

- [Developer Support Portal](https://developer.wacom.com/developer-dashboard/support)

## Technology Usage
**No Commercial Use**. NOTWITHSTANDING ANYTHING TO THE CONTRARY, THIS AGREEMENT DOES NOT CONVEY ANY LICENSE TO USE THE EVALUATION MATERIALS IN PRODUCTION, OR TO DISTRIBUTE THE EVALUATION MATERIALS TO ANY THIRD PARTY. THE PARTNER ARE REQUIRED TO EXECUTE A SEPARATE LICENSE AGREEMENT WITH WACOM BEFORE MANUFACTURING OR DISTRIBUTING THE EVALUATION MATERIALS OR ANY PRODUCTS THAT CONTAIN THE EVALUATION MATERIALS. The Partner hereby acknowledge and agree that: (i) any use by The Partner of the Evaluation Materials in production, or any other distribution of the Evaluation Materials is a material breach of this Agreement; and (ii) any such unauthorized use or distribution will be at The Partner sole risk. No such unauthorized use or distribution shall impose any liability on Wacom, or any of its licensors, whether by implication, by estoppel, through course of dealing, or otherwise. The Partner hereby agree to indemnify Wacom, its affiliates and licensors against any and all claims, losses, and damages based on The Partner use or distribution of the Evaluation Materials in breach of this Agreement.

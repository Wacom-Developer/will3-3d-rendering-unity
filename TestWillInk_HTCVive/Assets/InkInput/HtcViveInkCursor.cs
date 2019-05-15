using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Wacom.Ink
{
	public class HtcViveInkCursor : MonoBehaviour
	{
		#region Fields

		public SteamVR_Input_Sources InputSource = SteamVR_Input_Sources.RightHand;

		#endregion

		// Use this for initialization
		void Start()
		{
			Renderer renderer = this.GetComponent<Renderer>();
			renderer.receiveShadows = false;
			renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		}

		// Update is called once per frame
		void Update()
		{
			this.transform.position = SteamVR_Actions._default.Pose[InputSource].localPosition;
		}
	}
}

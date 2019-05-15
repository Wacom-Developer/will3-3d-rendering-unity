using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Wacom.Ink
{
	public class HtcViveInk : MonoBehaviour
	{
		#region Fields

		private IInkWriter m_inkWriter;
		private SteamVR_Input_Sources? m_strokeDevice;

		#endregion

		// Use this for initialization
		void Start()
		{
			m_inkWriter = GetComponent<IInkWriter>();
		}

		void Update()
		{
			if (m_strokeDevice == null)
			{
				SteamVR_Action_Boolean_Source drawStrokeAction = SteamVR_Actions._default.DrawStroke[SteamVR_Input_Sources.Any];

				if (drawStrokeAction.stateDown)
				{
					if (drawStrokeAction.activeDevice == SteamVR_Input_Sources.Any)
					{
						m_strokeDevice = SteamVR_Input_Sources.RightHand;
					}
					else
					{
						m_strokeDevice = drawStrokeAction.activeDevice;
					}

					//Debug.Log($"DOWN: {m_strokeDevice.Value}");

					PointerPoint pp = CreatePointerPoint();

					m_inkWriter.OnPointerPressed(ref pp);
				}
			}
			else
			{
				SteamVR_Action_Boolean_Source action = SteamVR_Actions._default.DrawStroke[m_strokeDevice.Value];

				if (action.stateUp)
				{
					//Debug.Log($"UP: {m_strokeDevice.Value}");

					PointerPoint pp = CreatePointerPoint();

					m_inkWriter.OnPointerReleased(ref pp);

					m_strokeDevice = null;
				}
				else if (action.state)
				{
					//Debug.Log($"MOVE: {m_strokeDevice.Value}");

					PointerPoint pp = CreatePointerPoint();

					m_inkWriter.OnPointerMoved(ref pp);
				}
			}
		}

		PointerPoint CreatePointerPoint()
		{
			var ps = SteamVR_Actions._default.Pose[m_strokeDevice.Value];

			return new PointerPoint(ps.localPosition, 0.5f, ps.updateTime);
		}
	}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Wacom.Ink
{
	interface IPathCollection
	{
		int GetPathsCount();

		List<float> GetPathAt(int index);

		void ClearAllPaths();
	}

	public class HtcViveInk : MonoBehaviour, IPathCollection
	{
		#region Fields

		private IInkWriter m_inkWriter;
		private IPathProvider m_pathProvider;
		private SteamVR_Input_Sources? m_strokeDevice;

		private List<List<float>> m_paths;

		#endregion

		// Use this for initialization
		void Start()
		{
			m_inkWriter = GetComponent<IInkWriter>();
			m_pathProvider = m_inkWriter as IPathProvider;

			m_paths = new List<List<float>>();
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

					if (m_pathProvider != null)
					{
						// copy the path
						List<float> path = new List<float>(m_pathProvider.GetPath());
						m_paths.Add(path);
					}
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

		#region IPathCollection

		public int GetPathsCount()
		{
			return m_paths.Count;
		}

		public List<float> GetPathAt(int index)
		{
			return m_paths[index];
		}

		public void ClearAllPaths()
		{
			m_paths.Clear();
		}

		#endregion
	}
}


using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wacom.Ink.Geometry;
using Wacom.Ink.Serialization.Model;

namespace Wacom.Ink
{
	public class InkSaveLoad : MonoBehaviour
	{
		string m_saveFilePath;

		// Start is called before the first frame update
		void Start()
		{
			m_saveFilePath = Application.persistentDataPath + "/strokes.will3";
		}

		// Update is called once per frame
		void Update()
		{

			// Press F2 to save, F3 to load
			if (Input.GetKeyDown(KeyCode.F2))
			{
				InkDataModel model = CreateWill3Document();
				byte[] encodedModel = model.ToByteArray();

				FileStream file;

				if (File.Exists(m_saveFilePath))
				{
					File.Delete(m_saveFilePath);
				}

				file = File.Create(m_saveFilePath);

				file.Write(encodedModel, 0, encodedModel.Length);

				file.Close();
			}
			else if (Input.GetKeyDown(KeyCode.F3))
			{
				FileStream file;

				if (!File.Exists(m_saveFilePath))
				{
					Debug.LogError("Save file not found");
					return;
				}

				//file = File.OpenRead();

				byte[] modelBytes = File.ReadAllBytes(m_saveFilePath);

				InkDataModel model = InkDataModel.FromByteArray(modelBytes);

				LoadStrokes(model);
			}
		}

		void LoadStrokes(InkDataModel model)
		{
			IPathCollection paths = GetComponentInChildren<IPathCollection>();

			if (paths == null)
				return;

			paths.ClearAllPaths();

			IStrokeRenderer renderer = GetComponentInChildren<IStrokeRenderer>();

			if (renderer == null)
				return;

			var enumerator = model.InkTree.Root.GetRecursiveEnumerator();

			while (enumerator.MoveNext())
			{
				if (enumerator.Current is PathNode pathNode)
				{					
					renderer.DrawStroke(pathNode.Path.Spline.Data);
				}
			}
		}

		public static List<System.Numerics.Vector3> CreateDummyBrush()
		{
			List<System.Numerics.Vector3> brushPoints = new List<System.Numerics.Vector3>();

			brushPoints.Add(new System.Numerics.Vector3(-0.5f, -0.5f, -0.5f));
			brushPoints.Add(new System.Numerics.Vector3(0.5f, -0.5f, -0.5f));
			brushPoints.Add(new System.Numerics.Vector3(-0.5f, 0.5f, -0.5f));
			brushPoints.Add(new System.Numerics.Vector3(0.5f, 0.5f, -0.5f));
			brushPoints.Add(new System.Numerics.Vector3(-0.5f, -0.5f, 0.5f));
			brushPoints.Add(new System.Numerics.Vector3(0.5f, -0.5f, 0.5f));
			brushPoints.Add(new System.Numerics.Vector3(-0.5f, 0.5f, 0.5f));
			brushPoints.Add(new System.Numerics.Vector3(0.5f, 0.5f, 0.5f));

			return brushPoints;
		}

		public InkDataModel CreateWill3Document()
		{
			IPathCollection paths = GetComponentInChildren<IPathCollection>();

			if (paths == null)
				return null;

			InkDataModel will3Doc = new InkDataModel();

			PathGroupNode root = new PathGroupNode(Identifier.FromNewGuid());
			will3Doc.InkTree.Root = root;

			PathPointLayout layoutXYZS = new PathPointLayout(PathPoint.Property.X, PathPoint.Property.Y, PathPoint.Property.Z, PathPoint.Property.Size);

			List<System.Numerics.Vector3> brushPolyhedron = CreateDummyBrush();
			VectorBrush brush = new VectorBrush(Identifier.FromNewGuid(), brushPolyhedron);
			brush.RenderModeUri = "will3://rendering//pen";
			brush.RenderingProperties = new RenderingProperties()
			{
				Red = 0.0f,
				Green = 0.0f,
				Blue = 0.0f,
				Alpha = 1.0f
			};

			will3Doc.Brushes.AddVectorBrush(brush);

			Style style = new Style(brush);

			int pathsCount = paths.GetPathsCount();

			for (int i = 0; i < pathsCount; i++)
			{
				Spline spline = new Spline();
				spline.Ts = 0.0f;
				spline.Tf = 1.0f;
				spline.Data = paths.GetPathAt(i);

				Wacom.Ink.Serialization.Model.Path will3Path = new Wacom.Ink.Serialization.Model.Path(Identifier.FromNewGuid(),
					spline, style, layoutXYZS, Identifier.Empty);

				PathNode pathNode = new PathNode(Identifier.FromNewGuid(), will3Path);

				root.Add(pathNode);
			}

			return will3Doc;
		}


		/*	IEnumerator LoadYourAsyncScene(string path)
			{
				// The Application loads the Scene in the background as the current Scene runs.
				// This is particularly good for creating loading screens.
				// You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
				// a sceneBuildIndex of 1 as shown in Build Settings.

				AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(path);

				// Wait until the asynchronous scene fully loads
				while (!asyncLoad.isDone)
				{
					yield return null;
				}
			}*/
	}
}
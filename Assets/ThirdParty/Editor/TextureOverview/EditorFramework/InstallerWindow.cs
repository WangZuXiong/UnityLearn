using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000065 RID: 101
	public class InstallerWindow : EditorWindow
	{
		// Token: 0x0600025E RID: 606 RVA: 0x0002C560 File Offset: 0x0002A760
		private void OnGUI()
		{
			if (!InstallerWindow.ValidateVersion(this.MinimumMajorVersion, this.MinimumMinorVersion, this.ProductTitle, this.ProductName, this.FeedbackUrl))
			{
				base.Close();
				return;
			}
			if (!string.IsNullOrEmpty(this._ErrorMessage) && this._ErrorTime + 5f < Time.realtimeSinceStartup)
			{
				this.DrawInstallFailed();
				return;
			}
			if (this._LayoutCalls == 0 && (int)Event.current.type == 8)
			{
				this.Init();
			}
			if (this._LayoutCalls == 2 && (int)Event.current.type == 8)
			{
				this.Install();
				AssetDatabase.Refresh();
			}
			if ((int)Event.current.type == 8)
			{
				this._LayoutCalls++;
			}
			GUILayout.Label(this._Message, this._MessageStyle, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(true),
				GUILayout.ExpandHeight(true)
			});
			base.Repaint();
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0002C648 File Offset: 0x0002A848
		private void DrawInstallFailed()
		{
			GUILayout.Label(new GUIContent(this._Title + " - Setup failed", Images.Error16x16), EditorStyles.boldLabel, new GUILayoutOption[0]);
			if (GUILayout.Button(new GUIContent(" > Get help in Unity forum", this.FeedbackUrl), GUIStyles.Hyperlink, new GUILayoutOption[0]))
			{
				Application.OpenURL(this.FeedbackUrl);
			}
			EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
			GUILayout.Space(20f);
			GUILayout.Label(this._ErrorMessage, EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
			GUILayout.Space(20f);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0002C6E4 File Offset: 0x0002A8E4
		private void Init()
		{
			this._Title = string.Format("{0} for Unity {1}.{2}", this.ProductTitle, EditorApplication2.MajorVersion, EditorApplication2.MinorVersion);
			this._Message = string.Format("Please wait while {0} is being installed.\n\nIf you upgrade Unity, you need to reinstall {1}.", this._Title, this.ProductName);
			this._MessageStyle = new GUIStyle(EditorStyles.boldLabel);
			this._MessageStyle.wordWrap = true;
			this._MessageStyle.alignment = TextAnchor.MiddleCenter;
			if (EditorApplication2.MajorVersion < 4)
			{
				this._ErrorMessage = string.Format("{0} requires Unity 4.x and newer, but you are using Unity {1}.\n", this.ProductTitle, Application.unityVersion);
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0002C784 File Offset: 0x0002A984
		private void Install()
		{
			InternalEditorUtility.RepaintAllViews();
			int majorVersion = EditorApplication2.MajorVersion;
			int minorVersion = EditorApplication2.MinorVersion;
			if (majorVersion < this.MinimumMajorVersion || (majorVersion == this.MinimumMajorVersion && minorVersion < this.MinimumMinorVersion))
			{
				this._ErrorTime = Time.realtimeSinceStartup;
				this._ErrorMessage += string.Format("\n{0} can't be installed. Unity {1}.{2} and newer is required to run {0}.\n", this.ProductTitle, this.MinimumMajorVersion, this.MinimumMinorVersion);
				Debug.LogError(this._ErrorMessage);
				return;
			}
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string arg = string.Format("{0}", majorVersion);
			for (int i = minorVersion; i >= 0; i--)
			{
				if (File.Exists(string.Format("{0}.Unity{1}_{2}", executingAssembly.Location, majorVersion, i)))
				{
					arg = string.Format("{0}_{1}", majorVersion, i);
					break;
				}
				if (File.Exists(string.Format("{0}.Unity{1}{2}", executingAssembly.Location, majorVersion, i)))
				{
					arg = string.Format("{0}{1}", majorVersion, i);
					break;
				}
			}
			this.CopyFile(string.Format("{0}.Unity{1}", executingAssembly.Location, arg), executingAssembly.Location, ref this._ErrorMessage);
			this.CopyFile(string.Format("{0}.mdb.Unity{1}", executingAssembly.Location, arg), executingAssembly.Location + ".mdb", ref this._ErrorMessage);
			if (!string.IsNullOrEmpty(this._ErrorMessage))
			{
				Debug.LogError(this._ErrorMessage);
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0002C91C File Offset: 0x0002AB1C
		private void CopyFile(string sourcePath, string targetPath, ref string errorMsg)
		{
			try
			{
				File.Copy(sourcePath, targetPath, true);
			}
			catch (Exception)
			{
				this._ErrorTime = Time.realtimeSinceStartup;
				errorMsg += string.Format("\nError while copying '{0}' to '{1}'. Please check if the file is read-only.\n", sourcePath, targetPath);
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0002C968 File Offset: 0x0002AB68
		public static bool ValidateVersion(int major, int minor, string productTitle, string productName, string feedbackUrl)
		{
			return true;
		}

		// Token: 0x04000191 RID: 401
		private int _LayoutCalls;

		// Token: 0x04000192 RID: 402
		private string _Title = "";

		// Token: 0x04000193 RID: 403
		private string _Message = "";

		// Token: 0x04000194 RID: 404
		private string _ErrorMessage = "";

		// Token: 0x04000195 RID: 405
		private float _ErrorTime;

		// Token: 0x04000196 RID: 406
		private GUIStyle _MessageStyle;

		// Token: 0x04000197 RID: 407
		public string ProductName = "";

		// Token: 0x04000198 RID: 408
		public string ProductTitle = "";

		// Token: 0x04000199 RID: 409
		public string FeedbackUrl = "";

		// Token: 0x0400019A RID: 410
		public string AssetStoreUrl = "";

		// Token: 0x0400019B RID: 411
		public int MinimumMajorVersion = 4;

		// Token: 0x0400019C RID: 412
		public int MinimumMinorVersion = 3;
	}
}

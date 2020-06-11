using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x02000002 RID: 2
	public class AboutWindow : EditorWindow
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private void Awake()
		{
			base.minSize = new Vector2(480f, 240f);
			base.maxSize = new Vector2(480f, 240f);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000207C File Offset: 0x0000027C
		private void OnGUI()
		{
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			this.DrawLinkLabel(string.Format("{0} by Peter Schraut", this.ProductName), "http://www.console-dev.de");
			GUILayout.Box("", new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(true),
				GUILayout.MaxHeight(1f),
				GUILayout.MinHeight(1f)
			});
			GUILayout.Space(16f);
			this.DrawLinkLabel("Feedback", this.FeedbackUrl);
			this.DrawLinkLabel("Rate this plugin", this.AssetStoreUrl);
			this.DrawLinkLabel("Find more of my products", "https://www.assetstore.unity3d.com/#/publisher/3683");
			GUILayout.EndVertical();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002128 File Offset: 0x00000328
		private void DrawLinkLabel(string title, string url)
		{
			GUILayout.Label(title, EditorStyles.boldLabel, new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(12f);
			if (GUILayout.Button(url, GUIStyles.Hyperlink, new GUILayoutOption[0]))
			{
				Application.OpenURL(url);
			}
			EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
			GUILayout.EndHorizontal();
			GUILayout.Space(4f);
		}

		// Token: 0x04000001 RID: 1
		public string ProductName = "";

		// Token: 0x04000002 RID: 2
		public string FeedbackUrl = "";

		// Token: 0x04000003 RID: 3
		public string AssetStoreUrl = "";
	}
}

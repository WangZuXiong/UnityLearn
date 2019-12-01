using System;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
	// Token: 0x0200002E RID: 46
	public abstract class EditorWindow2 : EditorWindow
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000B211 File Offset: 0x00009411
		public bool IsInited
		{
			get
			{
				return this._initphase >= 4;
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000B21F File Offset: 0x0000941F
		protected void SetTitle(string name)
		{
			base.titleContent = new GUIContent(name);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000B22D File Offset: 0x0000942D
		protected virtual bool __OnCheckCompatibility()
		{
			return true;
		}

		// Token: 0x0600016E RID: 366
		protected abstract void __OnCreate();

		// Token: 0x0600016F RID: 367
		protected abstract void __OnInit();

		// Token: 0x06000170 RID: 368
		protected abstract void __OnGUI();

		// Token: 0x06000171 RID: 369 RVA: 0x0000B230 File Offset: 0x00009430
		protected virtual void __OnResize()
		{
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000B232 File Offset: 0x00009432
		public void TriggerResize()
		{
			if (!this._TriggerResize)
			{
				this._TriggerResize = true;
				base.Repaint();
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000B249 File Offset: 0x00009449
		protected virtual void __OnFindCommand()
		{
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000B24B File Offset: 0x0000944B
		protected virtual void __OnEnterPlayMode()
		{
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000B24D File Offset: 0x0000944D
		protected virtual void __OnLeavePlayMode()
		{
		}

		// Token: 0x06000176 RID: 374
		protected abstract void __OnDestroy();

		// Token: 0x06000177 RID: 375 RVA: 0x0000B250 File Offset: 0x00009450
		private void OnGUI()
		{
			try
			{
				GUI.matrix = Matrix4x4.identity;
				if (this._waitforlayout && (int)Event.current.type != 8)
				{
					base.Repaint();
				}
				else
				{
					this._waitforlayout = false;
					if (this._initphase == 1)
					{
						if (!this.__OnCheckCompatibility())
						{
							base.Close();
						}
						else
						{
							this.__OnCreate();
							this.__OnResize();
							this._initphase = 2;
							this._waitforlayout = true;
							base.Repaint();
						}
					}
					else
					{
						if (this._initphase == 3)
						{
							this._initphase = 4;
							this.__OnInit();
							EditorApplication.playModeStateChanged += this.OnPlayModeStateChanged;
						}
						if (this._enabled)
						{
							if ((int)Event.current.type == 8 && (this._TriggerResize || Mathf.Abs(base.position.width - this._lastposition.width) > 1f || Mathf.Abs(base.position.height - this._lastposition.height) > 1f))
							{
								this.__OnResize();
								base.Repaint();
								this._lastposition = base.position;
								this._TriggerResize = false;
							}
							GUIColors.Update();
							this.__OnGUI();
							if (this._initphase > 3)
							{
								this.DoHandleCommands();
							}
						}
						if (this._initphase == 2 && (int)Event.current.type == 7)
						{
							this._initphase = 3;
							this._waitforlayout = true;
							base.Repaint();
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (!this.IsCriticalException(ex))
				{
					throw ex;
				}
				Debug.LogException(ex);
				string text = "Unity 2018.2";
				string text2 = base.titleContent.text;
				if (!EditorUtility.DisplayDialog("ka-booom!!!", string.Format("An unhandled error occurred in {0}.\n\nPlease include a screenshot of this message and a description how to reproduce the error if you send a bug-report to the {0} developer (see Unity AssetStore for contact details).\n\n{1}\n\nUnity={2}, Platform={3}, BuildTarget={4}, PluginCreatedWith={5}", new object[]
				{
					text2,
					ex,
					Application.unityVersion,
					Application.platform,
					EditorUserBuildSettings.activeBuildTarget,
					text
				}), "Ignore", string.Format("Close {0}", text2)))
				{
					base.Close();
				}
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000B490 File Offset: 0x00009690
		private void DoHandleCommands()
		{
			if ((int)Event.current.type != 13 && (int)Event.current.type != 14)
			{
				return;
			}
			if (string.Equals(Event.current.commandName, "Find", StringComparison.OrdinalIgnoreCase))
			{
				if ((int)Event.current.type == 14)
				{
					this.__OnFindCommand();
				}
				Event.current.Use();
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000B4EF File Offset: 0x000096EF
		private void OnEnable()
		{
			this._waitforlayout = true;
			this._initphase = 1;
			this._enabled = true;
			base.Repaint();
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000B50C File Offset: 0x0000970C
		private void OnDisable()
		{
			this._initphase = 0;
			this._enabled = false;
			this._waitforlayout = false;
			EditorApplication.playModeStateChanged -= this.OnPlayModeStateChanged;
			this.__OnDestroy();
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000B53A File Offset: 0x0000973A
		private void OnDestroy()
		{
			this._initphase = 0;
			this._enabled = false;
			EditorApplication.playModeStateChanged -= this.OnPlayModeStateChanged;
			EditorApplication2.OnPluginDestroy();
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000B560 File Offset: 0x00009760
		private void OnPlayModeStateChanged(PlayModeStateChange value)
		{
			if ((int)value == 1)
			{
				this.__OnEnterPlayMode();
			}
			if ((int)value == 3)
			{
				this.__OnLeavePlayMode();
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000B578 File Offset: 0x00009778
		private bool IsCriticalException(Exception e)
		{
			if (e is ExitGUIException)
			{
				return false;
			}
			if (!string.IsNullOrEmpty(e.StackTrace))
			{
				bool flag = false;
				string[] array = e.StackTrace.Split(new char[]
				{
					'\n'
				}, StringSplitOptions.RemoveEmptyEntries);
				for (int i = array.Length - 1; i >= 0; i--)
				{
					string text = array[i].Replace("\\", "/");
					if (text.IndexOf("in c:/buildslave/unity/", StringComparison.OrdinalIgnoreCase) != -1)
					{
						flag = true;
					}
					if (text.IndexOf("in c:/users/crash/", StringComparison.OrdinalIgnoreCase) != -1)
					{
						flag = false;
					}
				}
				if (flag)
				{
					return false;
				}
			}
			return !(e is ArgumentException) || string.IsNullOrEmpty(e.Message) || e.Message.IndexOf("Getting control", StringComparison.OrdinalIgnoreCase) == -1 || e.Message.IndexOf("position in a group with only", StringComparison.OrdinalIgnoreCase) == -1;
		}

		// Token: 0x040000D1 RID: 209
		private int _initphase;

		// Token: 0x040000D2 RID: 210
		private bool _enabled;

		// Token: 0x040000D3 RID: 211
		private bool _waitforlayout = true;

		// Token: 0x040000D4 RID: 212
		private Rect _lastposition;

		// Token: 0x040000D5 RID: 213
		private bool _TriggerResize;
	}
}

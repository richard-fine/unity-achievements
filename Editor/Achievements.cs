using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

namespace UnityEditor.Achievements
{

public abstract class Achievement
{
	public readonly string PrefsKey;
	public readonly string Title;
	public readonly string Description;

		public void RefreshLockState()
		{
			_isUnlocked = EditorPrefs.GetBool (PrefsKey, false);
		}

		private bool? _isUnlocked;
		public bool IsUnlocked
		{
			get
			{
				if(!_isUnlocked.HasValue)
				{
					RefreshLockState();
				}
				return _isUnlocked.Value;
			}
			set
			{
				if(_isUnlocked.HasValue && _isUnlocked.Value == true) throw new System.InvalidOperationException("Can't re-lock an unlocked achievemnt.");

				_isUnlocked = value;
				EditorPrefs.SetBool(PrefsKey, value);
			}
		}
		

	public Achievement(string key, string title, string description)
	{
		PrefsKey = key;
		Title = title;
		Description = description;
	}

		public abstract void Update();
}

[InitializeOnLoad]
public class Achievements {

	#region Registration

	private static List<Achievement> _registeredAchievements;

	public static void Register(Achievement a)
	{
			if(_registeredAchievements == null)
				_registeredAchievements = new List<Achievement>();
		_registeredAchievements.Add (a);
	}

		public static void Register(params Achievement[] a)
		{
			if(_registeredAchievements == null)
				_registeredAchievements = new List<Achievement>();
			_registeredAchievements.AddRange (a);
		}

	#endregion

	#region Icon Handling

	private const string _icon64 = 
		"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" +
			"ks3TvwAAAABJRU5ErkJggg==";

	private static Texture2D _icon;

	public static Texture2D GetIcon()
	{
		if(!_icon)
		{
			var b = System.Convert.FromBase64String(_icon64);
			_icon = new Texture2D(1, 1);
			_icon.LoadImage(b);
		}
		return _icon;
	}

	#endregion

	#region Small icon

	private const string _smallIcon64 =
			"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";

	private static Texture2D _smallIcon;

	public static Texture2D GetSmallIcon()
	{
		if(!_smallIcon)
		{
			var b = System.Convert.FromBase64String(_smallIcon64);
			_smallIcon = new Texture2D(1, 1);
			_smallIcon.LoadImage(b);
		}
		return _smallIcon;
	}

	#endregion

	#region Preferences pane

	private static Vector2 _prefsScroll;

	[PreferenceItem("Achievements")]
	public static void PreferencesPanel()
	{
			if(GUILayout.Button("Reset"))
			{
				ResetAllAchievements();
			}

		_prefsScroll = GUILayout.BeginScrollView(_prefsScroll, false, true);
		foreach(var achievement in _registeredAchievements)
		{
			var gc = new GUIContent(achievement.Title + "\n" + achievement.Description, GetSmallIcon ());
				GUI.enabled = achievement.IsUnlocked;
			GUILayout.Label (gc);
		}
		GUILayout.EndScrollView();
	}

		private static void ResetAllAchievements()
		{
			if(_registeredAchievements == null) return;
			foreach(var a in _registeredAchievements)
			{
				EditorPrefs.DeleteKey(a.PrefsKey);
				a.RefreshLockState();
			}
		}

	#endregion

	#region Notification

	private static bool IsEditorWindowHiddenTab(EditorWindow w)
	{
		var fi = typeof(EditorWindow).GetField ("m_Parent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		var parent = fi.GetValue (w);
		if(parent == null) return false;

		if(parent.GetType().Name != "DockArea") return false;

		var fiPanes = parent.GetType ().GetField ("m_Panes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		var parentPanes = (List<EditorWindow>)fiPanes.GetValue (parent);
		var index = parentPanes.IndexOf (w);

		var piSelected = parent.GetType ().GetProperty ("selected");
		var selected = (int)piSelected.GetValue (parent, null);

		return (selected != index);
	}

	private const float WidthBias = 1.1f;

	private static void ShowAchievementUnlocked(Achievement a)
	{
		var content = new GUIContent("ACHIEVEMENT UNLOCKED\n" + a.Title, GetIcon ());

		// Find the best window to display this on
		var allWindows = new HashSet<EditorWindow>((EditorWindow[])Resources.FindObjectsOfTypeAll(typeof(EditorWindow)));
		allWindows.RemoveWhere (IsEditorWindowHiddenTab);

		var targetWindow = allWindows.OrderByDescending(w => Mathf.Pow(w.position.width, WidthBias) * w.position.height).FirstOrDefault();
		if(targetWindow)
		{
			targetWindow.ShowNotification(content);
		}

			Debug.Log (string.Format("ACHIEVEMENT UNLOCKED: {0}\n{1}\n\n\n\n\n", a.Title, a.Description));
		}

		public static void Unlock(Achievement a)
		{
			// Don't allow double-granting of achievements
			if(a.IsUnlocked) return;

			a.IsUnlocked = true;

			ShowAchievementUnlocked(a);
		}

	#endregion

		#region Updates

		public const double UpdateDeltaTime = 10f;
		private static double lastUpdate;

		static Achievements()
		{
			lastUpdate = EditorApplication.timeSinceStartup;
			EditorApplication.update += Achievements.Update;
		}

		private static System.Reflection.FieldInfo _sCurFi;

		private static bool EditorStylesReady()
		{
			if(_sCurFi == null)
				_sCurFi = typeof(EditorStyles).GetField ("s_Current", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			return (_sCurFi.GetValue (null) != null);
		}

		private static void Update()
		{
			if(_registeredAchievements == null) return;

			if(!EditorStylesReady()) return;

			if(EditorApplication.timeSinceStartup - lastUpdate > UpdateDeltaTime)
			{
				lastUpdate = EditorApplication.timeSinceStartup;

				foreach(var a in _registeredAchievements)
				{
					if(a.IsUnlocked) continue;
					a.Update();
					return;
				}
			}
		}

		#endregion
}

}

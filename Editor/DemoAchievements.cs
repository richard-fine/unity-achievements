using UnityEngine;
using System.Collections;
using System.Linq;

namespace UnityEditor.Achievements
{

	[InitializeOnLoad]
	internal class DemoAchievements {

		static DemoAchievements()
		{
			Achievements.Register(
				new BasicUser("UnityAchievements.Demo.BasicUser", "I AM MAEK GAEM!", "You launched Unity."),
				new FileTypeDetected("UnityAchievements.Demo.Textures", "Adios, VRAM!", "You added a texture to your project.", ".png", ".tga", ".bmp", ".dds", ".jpg", ".gif", ".tiff"),
				new FileTypeDetected("UnityAchievements.Demo.CSharp", "Looking sharp", "You added a C# script to your project.", ".cs"),
				new FileTypeDetected("UnityAchievements.Demo.Model", "A model citizen", "You added a model to your project.", ".fbx", ".max", ".mb", ".ma", ".obj", ".collada", ".blend"),
				new MadeBuild("UnityAchievements.Demo.MadeBuild", "I AM MAED GAEM!!!", "You successfully built your project.")
				);
		}

		public class BasicUser : Achievement
		{
			public BasicUser(string key, string title, string description) : base(key, title, description) { }

			public override void Update ()
			{
				Achievements.Unlock(this);
			}
		}

		public class FileTypeDetected : Achievement
		{
			private readonly string[] _extensions;

			public FileTypeDetected(string key, string title, string description, params string[] extensions) 
				: base(key, title, description)
			{
				_extensions = extensions.Select(e => e.ToLower()).ToArray();
			}

			bool HasAnySatisfyingFiles ()
			{
				var paths = AssetDatabase.GetAllAssetPaths ();
				for (int i = 0; i < paths.Length; ++i) {
					for (int j = 0; j < _extensions.Length; ++j) {
						if(paths[i].EndsWith(_extensions[j])) return true;
					}
				}
				return false;
			}

			public override void Update()
			{
				if(HasAnySatisfyingFiles ())
					Achievements.Unlock(this);
			}
		}

		public class MadeBuild : Achievement
		{
			private static MadeBuild _inst;
			public MadeBuild(string key, string title, string description)
				: base(key, title, description)
			{
				_inst = this;
			}

			public override void Update ()
			{

			}

			[UnityEditor.Callbacks.PostProcessBuild]
			public static void OnPostProcessBuild(BuildTarget target, string path)
			{
				if(_inst != null) Achievements.Unlock(_inst);
			}
		}

	}

}
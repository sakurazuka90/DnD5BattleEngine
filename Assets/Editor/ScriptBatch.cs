using UnityEditor;
using System.Diagnostics;

public class ScriptBatch 
{
	[MenuItem("MyTools/Windows Build With Postprocess")]
	public static void BuildGame ()
	{
		// Get filename.
		string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
		string[] levels = new string[] {"Assets/Scenes/MenuScene.unity", "Assets/Scenes/Creator.unity", "Assets/Scenes/TestScene.unity"};

		// Build player.
		BuildPipeline.BuildPlayer(levels, path + "/DnDBattleEngine.exe", BuildTarget.StandaloneWindows64, BuildOptions.Development);


		// Copy a file from the project folder to the build folder, alongside the built game.
		FileUtil.CopyFileOrDirectory("Assets/Database", path + "/DnDBattleEngine_Data/Database");

		// Run the game (Process class from System.Diagnostics).
		Process proc = new Process();
		proc.StartInfo.FileName = path + "/DnDBattleEngine.exe";
		proc.Start();
	}
}

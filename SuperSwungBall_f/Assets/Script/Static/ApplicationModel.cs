using System.Collections.Generic;
using Boomlagoon.JSON;

public static class ApplicationModel {

	public static JSONArray ChallengeCompleted = null;

	private static int subviewCount = 0;

	public static void AddSubview(){ subviewCount++; }
	public static void RemoveSubview(){ subviewCount--; }

	public static bool BackgroundSceneActionAllowed {
		get { return subviewCount == 0; }
	}
}
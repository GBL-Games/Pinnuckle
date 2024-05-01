#if TOOLS

using Godot;
using Godot.Collections;

namespace Pinnuckle.addons.ScenicRoute;

[Tool]
public partial class ScenicRouteSignals : Node
{
    [Signal]
    public delegate void SceneListUpdatedEventHandler(Dictionary<string, SceneData> scenes);

    [Signal]
    public delegate void RemoveSceneEventHandler(string sceneKey);
}
#endif
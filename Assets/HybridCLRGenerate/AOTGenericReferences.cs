using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"Assembly-CSharp-firstpass.dll",
		"System.Core.dll",
		"UnityEngine.AssetBundleModule.dll",
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// System.Action<UnityEngine.RaycastHit>
	// System.Action<object,System.IntPtr>
	// System.Action<object,byte>
	// System.Action<object,double>
	// System.Action<object,float>
	// System.Action<object,int>
	// System.Action<object,long>
	// System.Action<object,object>
	// System.Action<object,sbyte>
	// System.Action<object,short>
	// System.Action<object,uint>
	// System.Action<object,ulong>
	// System.Action<object,ushort>
	// System.Action<object>
	// System.ArraySegment<byte>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.Dictionary<ulong,object>
	// System.Collections.Generic.Dictionary<ushort,object>
	// System.Collections.Generic.Dictionary<ushort,ushort>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.List.Enumerator<XFramework.General.GenerateAttributesTypeGroup>
	// System.Collections.Generic.List.Enumerator<XFramework.TimeTaskInfo>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.List<XFramework.AudioComponentData.AudioInfo>
	// System.Collections.Generic.List<XFramework.General.GenerateAttributesTypeGroup>
	// System.Collections.Generic.List<XFramework.TimeTaskInfo>
	// System.Collections.Generic.List<byte>
	// System.Collections.Generic.List<double>
	// System.Collections.Generic.List<float>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<long>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.List<sbyte>
	// System.Collections.Generic.List<short>
	// System.Collections.Generic.List<uint>
	// System.Collections.Generic.List<ulong>
	// System.Collections.Generic.List<ushort>
	// System.Collections.Generic.Queue<FlyingWormConsole3.LiteNetLib.NatPunchModule.RequestEventData>
	// System.Collections.Generic.Queue<FlyingWormConsole3.LiteNetLib.NatPunchModule.SuccessEventData>
	// System.Collections.Generic.Queue<int>
	// System.Collections.Generic.Queue<object>
	// System.Func<object,System.IntPtr>
	// System.Func<object,byte>
	// System.Func<object,double>
	// System.Func<object,float>
	// System.Func<object,int>
	// System.Func<object,long>
	// System.Func<object,object>
	// System.Func<object,sbyte>
	// System.Func<object,short>
	// System.Func<object,uint>
	// System.Func<object,ulong>
	// System.Func<object,ushort>
	// System.Nullable<System.DateTime>
	// UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene,UnityEngine.SceneManagement.LoadSceneMode>
	// UnityEngine.Events.UnityAction<object>
	// UnityEngine.Events.UnityEvent<object>
	// }}

	public void RefMethods()
	{
		// object LitJson.JsonMapper.ToObject<object>(string)
		// object System.Activator.CreateInstance<object>()
		// object[] System.Array.Empty<object>()
		// System.Void System.Array.Resize<byte>(byte[]&,int)
		// System.Void System.Array.Resize<object>(object[]&,int)
		// ushort System.Linq.Enumerable.First<ushort>(System.Collections.Generic.IEnumerable<ushort>)
		// object System.Threading.Interlocked.CompareExchange<object>(object&,object,object)
		// object UnityEngine.AssetBundle.LoadAsset<object>(string)
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.Component.GetComponentInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object[] UnityEngine.GameObject.GetComponents<object>()
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
	}
}
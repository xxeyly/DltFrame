using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"System.Core.dll",
		"UniTask.dll",
		"UnityEngine.AssetBundleModule.dll",
		"UnityEngine.CoreModule.dll",
		"UnityEngine.JSONSerializeModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<UniTaskFrameComponent.<ExecuteTask>d__9,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<XFramework.AnimatorControllerManager.<PlayAnim>d__11,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<XFramework.HotFixFrameComponent.<InstantiateHotFixAssetBundle>d__8,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<XFramework.HotFixFrameComponent.<LoadAssetBundleSceneToSystem>d__11,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<XFramework.HotFixFrameComponent.<LoadHotFixSceneConfig>d__10,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<XFramework.HttpFrameComponent.<HttpUnityWebRequest>d__12,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<XFramework.HttpFrameComponent.<UnityHttpWebRequest>d__14,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<UniTaskFrameComponent.<ExecuteTask>d__9,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<XFramework.AnimatorControllerManager.<PlayAnim>d__11,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<XFramework.HotFixFrameComponent.<InstantiateHotFixAssetBundle>d__8,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<XFramework.HotFixFrameComponent.<LoadAssetBundleSceneToSystem>d__11,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<XFramework.HotFixFrameComponent.<LoadHotFixSceneConfig>d__10,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<XFramework.HttpFrameComponent.<HttpUnityWebRequest>d__12,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<XFramework.HttpFrameComponent.<UnityHttpWebRequest>d__14,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>
	// Cysharp.Threading.Tasks.CompilerServices.IStateMachineRunnerPromise<object>
	// Cysharp.Threading.Tasks.ITaskPoolNode<object>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.IUniTaskSource<object>
	// Cysharp.Threading.Tasks.TaskPool<object>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<object>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<object>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<object>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask<object>
	// Cysharp.Threading.Tasks.UniTaskCompletionSourceCore<object>
	// System.Action<LitJson.PropertyMetadata>
	// System.Action<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Action<UnityEngine.EventSystems.RaycastResult>
	// System.Action<UnityEngine.RaycastHit>
	// System.Action<XFramework.AudioComponentData.AudioInfo>
	// System.Action<XFramework.GenerateAttributesTypeGroup>
	// System.Action<XFramework.TimeTaskInfo>
	// System.Action<int>
	// System.Action<object>
	// System.Buffers.ArrayPool<int>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool.LockedStack<int>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool.PerCoreLockedStacks<int>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool<int>
	// System.ByReference<int>
	// System.Collections.Generic.ArraySortHelper<LitJson.PropertyMetadata>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ArraySortHelper<XFramework.AudioComponentData.AudioInfo>
	// System.Collections.Generic.ArraySortHelper<XFramework.GenerateAttributesTypeGroup>
	// System.Collections.Generic.ArraySortHelper<XFramework.TimeTaskInfo>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<LitJson.PropertyMetadata>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.Comparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.Comparer<XFramework.AudioComponentData.AudioInfo>
	// System.Collections.Generic.Comparer<XFramework.GenerateAttributesTypeGroup>
	// System.Collections.Generic.Comparer<XFramework.TimeTaskInfo>
	// System.Collections.Generic.Comparer<byte>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,LitJson.ArrayMetadata>
	// System.Collections.Generic.Dictionary.Enumerator<object,LitJson.ObjectMetadata>
	// System.Collections.Generic.Dictionary.Enumerator<object,LitJson.PropertyMetadata>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,LitJson.ArrayMetadata>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,LitJson.ObjectMetadata>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,LitJson.PropertyMetadata>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,LitJson.ArrayMetadata>
	// System.Collections.Generic.Dictionary.KeyCollection<object,LitJson.ObjectMetadata>
	// System.Collections.Generic.Dictionary.KeyCollection<object,LitJson.PropertyMetadata>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,LitJson.ArrayMetadata>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,LitJson.ObjectMetadata>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,LitJson.PropertyMetadata>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,LitJson.ArrayMetadata>
	// System.Collections.Generic.Dictionary.ValueCollection<object,LitJson.ObjectMetadata>
	// System.Collections.Generic.Dictionary.ValueCollection<object,LitJson.PropertyMetadata>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<object,LitJson.ArrayMetadata>
	// System.Collections.Generic.Dictionary<object,LitJson.ObjectMetadata>
	// System.Collections.Generic.Dictionary<object,LitJson.PropertyMetadata>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.EqualityComparer<LitJson.ArrayMetadata>
	// System.Collections.Generic.EqualityComparer<LitJson.ObjectMetadata>
	// System.Collections.Generic.EqualityComparer<LitJson.PropertyMetadata>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.EqualityComparer<byte>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.ICollection<LitJson.PropertyMetadata>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,LitJson.ArrayMetadata>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,LitJson.ObjectMetadata>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,LitJson.PropertyMetadata>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ICollection<XFramework.AudioComponentData.AudioInfo>
	// System.Collections.Generic.ICollection<XFramework.GenerateAttributesTypeGroup>
	// System.Collections.Generic.ICollection<XFramework.TimeTaskInfo>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.ICollection<ushort>
	// System.Collections.Generic.IComparer<LitJson.PropertyMetadata>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IComparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IComparer<XFramework.AudioComponentData.AudioInfo>
	// System.Collections.Generic.IComparer<XFramework.GenerateAttributesTypeGroup>
	// System.Collections.Generic.IComparer<XFramework.TimeTaskInfo>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IDictionary<int,object>
	// System.Collections.Generic.IDictionary<object,LitJson.ArrayMetadata>
	// System.Collections.Generic.IDictionary<object,LitJson.ObjectMetadata>
	// System.Collections.Generic.IDictionary<object,LitJson.PropertyMetadata>
	// System.Collections.Generic.IDictionary<object,object>
	// System.Collections.Generic.IEnumerable<LitJson.PropertyMetadata>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.UIntPtr,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,LitJson.ArrayMetadata>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,LitJson.ObjectMetadata>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,LitJson.PropertyMetadata>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IEnumerable<XFramework.AudioComponentData.AudioInfo>
	// System.Collections.Generic.IEnumerable<XFramework.GenerateAttributesTypeGroup>
	// System.Collections.Generic.IEnumerable<XFramework.TimeTaskInfo>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerable<ushort>
	// System.Collections.Generic.IEnumerator<LitJson.PropertyMetadata>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.UIntPtr,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,LitJson.ArrayMetadata>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,LitJson.ObjectMetadata>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,LitJson.PropertyMetadata>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IEnumerator<XFramework.AudioComponentData.AudioInfo>
	// System.Collections.Generic.IEnumerator<XFramework.GenerateAttributesTypeGroup>
	// System.Collections.Generic.IEnumerator<XFramework.TimeTaskInfo>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEnumerator<ushort>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<LitJson.PropertyMetadata>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IList<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IList<XFramework.AudioComponentData.AudioInfo>
	// System.Collections.Generic.IList<XFramework.GenerateAttributesTypeGroup>
	// System.Collections.Generic.IList<XFramework.TimeTaskInfo>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.IList<ushort>
	// System.Collections.Generic.KeyValuePair<System.UIntPtr,object>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<object,LitJson.ArrayMetadata>
	// System.Collections.Generic.KeyValuePair<object,LitJson.ObjectMetadata>
	// System.Collections.Generic.KeyValuePair<object,LitJson.PropertyMetadata>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.List.Enumerator<LitJson.PropertyMetadata>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.List.Enumerator<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.List.Enumerator<XFramework.AudioComponentData.AudioInfo>
	// System.Collections.Generic.List.Enumerator<XFramework.GenerateAttributesTypeGroup>
	// System.Collections.Generic.List.Enumerator<XFramework.TimeTaskInfo>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<LitJson.PropertyMetadata>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.List<XFramework.AudioComponentData.AudioInfo>
	// System.Collections.Generic.List<XFramework.GenerateAttributesTypeGroup>
	// System.Collections.Generic.List<XFramework.TimeTaskInfo>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<LitJson.PropertyMetadata>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.ObjectComparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ObjectComparer<XFramework.AudioComponentData.AudioInfo>
	// System.Collections.Generic.ObjectComparer<XFramework.GenerateAttributesTypeGroup>
	// System.Collections.Generic.ObjectComparer<XFramework.TimeTaskInfo>
	// System.Collections.Generic.ObjectComparer<byte>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<LitJson.ArrayMetadata>
	// System.Collections.Generic.ObjectEqualityComparer<LitJson.ObjectMetadata>
	// System.Collections.Generic.ObjectEqualityComparer<LitJson.PropertyMetadata>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.Stack.Enumerator<int>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<int>
	// System.Collections.Generic.Stack<object>
	// System.Collections.Generic.ValueListBuilder<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<LitJson.PropertyMetadata>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.ObjectModel.ReadOnlyCollection<XFramework.AudioComponentData.AudioInfo>
	// System.Collections.ObjectModel.ReadOnlyCollection<XFramework.GenerateAttributesTypeGroup>
	// System.Collections.ObjectModel.ReadOnlyCollection<XFramework.TimeTaskInfo>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<LitJson.PropertyMetadata>
	// System.Comparison<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Comparison<UnityEngine.EventSystems.RaycastResult>
	// System.Comparison<XFramework.AudioComponentData.AudioInfo>
	// System.Comparison<XFramework.GenerateAttributesTypeGroup>
	// System.Comparison<XFramework.TimeTaskInfo>
	// System.Comparison<int>
	// System.Comparison<object>
	// System.Func<byte>
	// System.Func<int>
	// System.Func<object,byte>
	// System.IEquatable<object>
	// System.Predicate<LitJson.PropertyMetadata>
	// System.Predicate<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Predicate<UnityEngine.EventSystems.RaycastResult>
	// System.Predicate<XFramework.AudioComponentData.AudioInfo>
	// System.Predicate<XFramework.GenerateAttributesTypeGroup>
	// System.Predicate<XFramework.TimeTaskInfo>
	// System.Predicate<int>
	// System.Predicate<object>
	// System.ReadOnlySpan<int>
	// System.Runtime.CompilerServices.ConditionalWeakTable.CreateValueCallback<System.UIntPtr,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.CreateValueCallback<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.Enumerator<System.UIntPtr,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.Enumerator<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable<System.UIntPtr,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable<object,object>
	// System.Span<int>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,object>>
	// System.ValueTuple<byte,object>
	// UnityEngine.Events.InvokableCall<byte>
	// UnityEngine.Events.InvokableCall<object>
	// UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene,int>
	// UnityEngine.Events.UnityAction<byte>
	// UnityEngine.Events.UnityAction<object>
	// UnityEngine.Events.UnityEvent<byte>
	// UnityEngine.Events.UnityEvent<object>
	// }}

	public void RefMethods()
	{
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskFrameComponent.<ExecuteTask>d__9>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskFrameComponent.<ExecuteTask>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,XFramework.AnimatorControllerManager.<PlayAnim>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter&,XFramework.AnimatorControllerManager.<PlayAnim>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UnityAsyncExtensions.AssetBundleCreateRequestAwaiter,XFramework.HotFixFrameComponent.<InstantiateHotFixAssetBundle>d__8>(Cysharp.Threading.Tasks.UnityAsyncExtensions.AssetBundleCreateRequestAwaiter&,XFramework.HotFixFrameComponent.<InstantiateHotFixAssetBundle>d__8&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UnityAsyncExtensions.AssetBundleCreateRequestAwaiter,XFramework.HotFixFrameComponent.<LoadAssetBundleSceneToSystem>d__11>(Cysharp.Threading.Tasks.UnityAsyncExtensions.AssetBundleCreateRequestAwaiter&,XFramework.HotFixFrameComponent.<LoadAssetBundleSceneToSystem>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UnityAsyncExtensions.AssetBundleRequestAwaiter,XFramework.HotFixFrameComponent.<InstantiateHotFixAssetBundle>d__8>(Cysharp.Threading.Tasks.UnityAsyncExtensions.AssetBundleRequestAwaiter&,XFramework.HotFixFrameComponent.<InstantiateHotFixAssetBundle>d__8&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter,XFramework.HotFixFrameComponent.<LoadHotFixSceneConfig>d__10>(Cysharp.Threading.Tasks.UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter&,XFramework.HotFixFrameComponent.<LoadHotFixSceneConfig>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter,XFramework.HttpFrameComponent.<HttpUnityWebRequest>d__12>(Cysharp.Threading.Tasks.UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter&,XFramework.HttpFrameComponent.<HttpUnityWebRequest>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter,XFramework.HttpFrameComponent.<UnityHttpWebRequest>d__14>(Cysharp.Threading.Tasks.UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter&,XFramework.HttpFrameComponent.<UnityHttpWebRequest>d__14&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<UniTaskFrameComponent.<ExecuteTask>d__9>(UniTaskFrameComponent.<ExecuteTask>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<XFramework.AnimatorControllerManager.<PlayAnim>d__11>(XFramework.AnimatorControllerManager.<PlayAnim>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<XFramework.HotFixFrameComponent.<InstantiateHotFixAssetBundle>d__8>(XFramework.HotFixFrameComponent.<InstantiateHotFixAssetBundle>d__8&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<XFramework.HotFixFrameComponent.<LoadAssetBundleSceneToSystem>d__11>(XFramework.HotFixFrameComponent.<LoadAssetBundleSceneToSystem>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<XFramework.HotFixFrameComponent.<LoadHotFixSceneConfig>d__10>(XFramework.HotFixFrameComponent.<LoadHotFixSceneConfig>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<XFramework.HttpFrameComponent.<HttpUnityWebRequest>d__12>(XFramework.HttpFrameComponent.<HttpUnityWebRequest>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<XFramework.HttpFrameComponent.<UnityHttpWebRequest>d__14>(XFramework.HttpFrameComponent.<UnityHttpWebRequest>d__14&)
		// ushort System.Linq.Enumerable.First<ushort>(System.Collections.Generic.IEnumerable<ushort>)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,XFramework.GameRootStart.<InitSceneStartSingletons>d__15>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,XFramework.GameRootStart.<InitSceneStartSingletons>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,XFramework.HttpFrameComponent.<SendHttpUnityWebRequest>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,XFramework.HttpFrameComponent.<SendHttpUnityWebRequest>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,XFramework.HttpFrameComponent.<SendHttpUnityWebRequest>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,XFramework.HttpFrameComponent.<SendHttpUnityWebRequest>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,XFramework.SceneLoadFrameComponent.<LoadAsyncScene>d__14>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,XFramework.SceneLoadFrameComponent.<LoadAsyncScene>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,XFramework.SceneLoadFrameComponent.<SceneLoad>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,XFramework.SceneLoadFrameComponent.<SceneLoad>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UnityAsyncExtensions.AsyncOperationAwaiter,XFramework.SceneLoadFrameComponent.<LoadAsyncScene>d__14>(Cysharp.Threading.Tasks.UnityAsyncExtensions.AsyncOperationAwaiter&,XFramework.SceneLoadFrameComponent.<LoadAsyncScene>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<XFramework.GameRootStart.<InitSceneStartSingletons>d__15>(XFramework.GameRootStart.<InitSceneStartSingletons>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<XFramework.HttpFrameComponent.<SendHttpUnityWebRequest>d__10>(XFramework.HttpFrameComponent.<SendHttpUnityWebRequest>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<XFramework.HttpFrameComponent.<SendHttpUnityWebRequest>d__11>(XFramework.HttpFrameComponent.<SendHttpUnityWebRequest>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<XFramework.SceneLoadFrameComponent.<LoadAsyncScene>d__14>(XFramework.SceneLoadFrameComponent.<LoadAsyncScene>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<XFramework.SceneLoadFrameComponent.<SceneLoad>d__11>(XFramework.SceneLoadFrameComponent.<SceneLoad>d__11&)
		// object UnityEngine.AssetBundle.LoadAsset<object>(string)
		// UnityEngine.AssetBundleRequest UnityEngine.AssetBundle.LoadAssetAsync<object>(string)
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.Component.GetComponentInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object[] UnityEngine.GameObject.GetComponents<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>(bool)
		// object UnityEngine.JsonUtility.FromJson<object>(string)
		// object UnityEngine.Object.FindObjectOfType<object>()
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
	}
}
#if Obfuz4HybridCLR
using Obfuz;
using Obfuz.EncryptionVM;
using UnityEngine;

namespace DltFramework
{
    public class Encryption
    {
        [ObfuzIgnore]
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Initialize()
        {
            Debug.Log("SetUpStaticSecret begin");
            EncryptionService<DefaultStaticEncryptionScope>.Encryptor = new GeneratedEncryptionVirtualMachine(Resources.Load<TextAsset>("Obfuz/defaultStaticSecretKey").bytes);
            Debug.Log("SetUpStaticSecret end");
        }
    }
}
#endif

using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework
{
    public class ResourceUnification : BaseEditor
    {
#pragma warning disable 414
        [BoxGroup("替换字体")] [LabelText("替换字体")] [LabelWidth(60)]
        public Font changeFont;
#pragma warning restore 414
        [BoxGroup("替换字体")]
        [Button(ButtonSizes.Medium)]
        [LabelText("字体替换")]
        public void OnChangeFont()
        {
            if (changeFont == null)
            {
                return;
            }

            List<Text> sceneAllText = DataSvc.GetAllObjectsInScene<Text>();
            foreach (Text text in sceneAllText)
            {
                text.font = changeFont;
            }

            Debug.Log("场景字体替换完毕:" + sceneAllText.Count);
        }

        [BoxGroup("替换场景物体文字")] [LabelText("替换前文字")] [LabelWidth(60)]
        public string sceneReplaceBeforeName;

        [BoxGroup("替换场景物体文字")] [LabelText("替换后文字")] [LabelWidth(60)]
        public string sceneReplaceAfterName;

        [BoxGroup("替换场景物体文字")]
        [Button("替换文字", ButtonSizes.Medium)]
        public void OnReplaceSceneGameObjectName()
        {
            foreach (GameObject sceneObj in DataSvc.GetAllObjectsOnlyInScene())
            {
                string replace = sceneObj.name.Replace(sceneReplaceBeforeName, sceneReplaceAfterName);
                sceneObj.name = replace;
            }
        }

        [BoxGroup("替换Text文字")] [LabelText("替换前文字")] [LabelWidth(60)]
        public string textReplaceBeforeName;

        [BoxGroup("替换Text文字")] [LabelText("替换后文字")] [LabelWidth(60)]
        public string textReplaceAfterName;


        [BoxGroup("替换Text文字")]
        [Button("替换文字", ButtonSizes.Medium)]
        public void OnReplaceTextContent()
        {
            foreach (Text text in DataSvc.GetAllObjectsInScene<Text>())
            {
                string replace = text.text.Replace(textReplaceBeforeName, textReplaceAfterName);
                text.text = replace;
            }

            foreach (TextMeshProUGUI text in DataSvc.GetAllObjectsInScene<TextMeshProUGUI>())
            {
                string replace = text.text.Replace(textReplaceBeforeName, textReplaceAfterName);
                text.text = replace;
            }
        }

        [BoxGroup("字体压缩")] [LabelText("要压缩的字体")]
        public TMP_FontAsset TargetFontAsset;

        [BoxGroup("字体压缩")]
        [Button("TextMeshPro字体压缩", ButtonSizes.Medium)]
        public void TextMeshProCompress()
        {
            ExtractTexture(AssetDatabase.GetAssetPath(TargetFontAsset));
            /*Texture2D texture2D = new Texture2D(TargetFontAsset.atlasTexture.width, TargetFontAsset.atlasTexture.height, TextureFormat.Alpha8, false);
            Graphics.CopyTexture(TargetFontAsset.atlasTexture, texture2D);
            byte[] dataBytes = texture2D.EncodeToPNG();
            FileStream fs = File.Open(AssetDatabase.GetAssetPath(TargetFontAsset).Replace(".asset", ".png"), FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();
            AssetDatabase.Refresh();
            Texture2D atlas = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GetAssetPath(TargetFontAsset));
            AssetDatabase.RemoveObjectFromAsset(TargetFontAsset.atlasTexture);
            TargetFontAsset.atlasTextures[0] = atlas;
            TargetFontAsset.material.mainTexture = atlas;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();*/
        }
        
        //这里的fontPath是绝对路径
        public void ExtractTexture(string fontPath){
            string texturePath = fontPath.Replace(".asset", ".png");
            TMP_FontAsset targeFontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(fontPath.Replace(Application.dataPath, "Assets"));
            Texture2D texture2D = new Texture2D(targeFontAsset.atlasTexture.width, targeFontAsset.atlasTexture.height, TextureFormat.Alpha8, false);
            Graphics.CopyTexture(targeFontAsset.atlasTexture, texture2D);
            byte[] dataBytes = texture2D.EncodeToPNG();
            FileStream fs = File.Open(texturePath, FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();
            AssetDatabase.Refresh();
            Texture2D atlas = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath.Replace(Application.dataPath, "Assets"));
            AssetDatabase.RemoveObjectFromAsset(targeFontAsset.atlasTexture);
            targeFontAsset.atlasTextures[0] = atlas;
            targeFontAsset.material.mainTexture = atlas;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [BoxGroup("图片压缩")]
        [LabelText("平台类型")]
        public enum PlatformType
        {
            WebGl,
            Android
        }

        [BoxGroup("图片压缩/信息")] [LabelText("平台")]
        public PlatformType platformType;

        [BoxGroup("图片压缩/信息")] [LabelText("检测图片长")]
        public int textureWidth = 300;

        [BoxGroup("图片压缩/信息")] [LabelText("检测图片高")]
        public int textureHigh = 300;

        [BoxGroup("图片压缩")]
        [Button("图片压缩(Png.Jpg.Tif.Tiff.Tga)", ButtonSizes.Medium)]
        public void OnTextureCompress()
        {
            List<string> pngPaths = DataSvc.GetSpecifyTypeOnlyInAssetsPath("png");
            List<string> jpgPaths = DataSvc.GetSpecifyTypeOnlyInAssetsPath("jpg");
            List<string> tifPaths = DataSvc.GetSpecifyTypeOnlyInAssetsPath("tif");
            List<string> tiffPaths = DataSvc.GetSpecifyTypeOnlyInAssetsPath("tiff");
            List<string> tgaPaths = DataSvc.GetSpecifyTypeOnlyInAssetsPath("tga");
            OnTextureCompressByPath(pngPaths);
            OnTextureCompressByPath(jpgPaths);
            OnTextureCompressByPath(tifPaths);
            OnTextureCompressByPath(tiffPaths);
            OnTextureCompressByPath(tgaPaths);
        }

        /// <summary>
        /// 根据地址压缩
        /// </summary>
        /// <param name="texturePath"></param>
        private void OnTextureCompressByPath(List<string> texturePath)
        {
            foreach (string path in texturePath)
            {
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                if (textureImporter != null && (textureImporter.textureType == TextureImporterType.NormalMap || textureImporter.textureType == TextureImporterType.Sprite ||
                                                textureImporter.textureType == TextureImporterType.Default))
                {
                    switch (platformType)
                    {
                        case PlatformType.WebGl:
                            textureImporter.SetPlatformTextureSettings(new TextureImporterPlatformSettings()
                            {
                                maxTextureSize = 1024,
                                compressionQuality = 50,
                                name = "WebGL",
                                overridden = true,
                                format = TextureImporterFormat.DXT5Crunched
                            });


                            break;
                        case PlatformType.Android:
                            textureImporter.SetPlatformTextureSettings(new TextureImporterPlatformSettings()
                            {
                                maxTextureSize = 2048,
                                compressionQuality = 50,
                                name = "Android",
                                overridden = true,
                                format = TextureImporterFormat.DXT5Crunched
                            });
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                    //图片大小超出了这个尺寸
                    if (texture2D.width >= textureWidth || texture2D.height >= textureHigh)
                    {
                        if (texture2D.width % 4 == 0 && texture2D.height % 4 == 0)
                        {
                        }
                        else
                        {
                            Debug.Log("该图片不能被压缩:" + texture2D.name + "[" + texture2D.width + ":" + texture2D.height + "]");
                        }
                    }


                    if (textureImporter)
                    {
                    }

                    AssetDatabase.ImportAsset(path);
                }
            }
        }


        public override void OnDisable()
        {
        }

        public override void OnCreateConfig()
        {
        }

        public override void OnSaveConfig()
        {
        }

        public override void OnLoadConfig()
        {
        }

        public override void OnInit()
        {
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System; 

namespace DilmerGames
{
    public class ScreenShot : MonoBehaviour
    {
        private static ScreenShot instance; 

        private Camera myCamera;
        private bool takeScreenshotOnNextFrame;

        private string m_UserPath;
        private string m_OldUserPath;

        public const string kAppFolderName = "VRDraw";

        private string m_SaveDir;

        private void Awake()
        {
            instance = this; 
            myCamera = gameObject.GetComponent<Camera>();

            if (Application.platform == RuntimePlatform.Android)
            {
                m_UserPath = "/sdcard/";
                m_OldUserPath = Application.persistentDataPath;
            }
            
            #if UNITY_EDITOR
            m_UserPath = Application.dataPath + "/"; 
            #elif UNITY_ANDROID
            m_UserPath = Application.persistentDataPath; 
            #elif UNITY_IPHONE
            m_UserPath = Application.persistentDataPath+"/"; 
            #else
            return Application.dataPath +"/";
            #endif

            //m_UserPath = Path.Combine(m_UserPath, kAppFolderName);
            m_SaveDir = m_UserPath; 
            //m_SaveDir = Path.Combine(m_UserPath, "Sketches");

            /*if (!Directory.Exists(m_SaveDir))
            {
                Directory.CreateDirectory(m_SaveDir);
            }*/ 
        }

        private void OnPostRender()
        {

            if (takeScreenshotOnNextFrame)
            {
                if (!Directory.Exists(m_SaveDir))
                {
                    Directory.CreateDirectory(m_SaveDir);
                }

                takeScreenshotOnNextFrame = false;
                RenderTexture renderTexture = myCamera.targetTexture;

                Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
                Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
                renderResult.ReadPixels(rect, 0, 0);

                byte[] byteArray = renderResult.EncodeToPNG();
                System.DateTime theTime = System.DateTime.Now;

                System.IO.File.WriteAllBytes(m_SaveDir + "CameraScreenshot.png", byteArray);

                /*System.IO.File.WriteAllBytes(Path.Combine(
                m_SaveDir, theTime.ToString("yyyy-MM-dd")) + ".png", byteArray);
                */ 

                RenderTexture.ReleaseTemporary(renderTexture);
                myCamera.targetTexture = null; 
            }
        }

        private void TakeScreenshot(int width, int height)
        {
            myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);  
            takeScreenshotOnNextFrame = true;
        }

        public static void TakeScreenshot_Static(int width, int height) 
        {
            instance.TakeScreenshot(width, height);
            //instance.TakeScreenshot(500, 500);
        }

        public void Button_onClick()
        {
            TakeScreenshot_Static(500, 500);
        }

        /*private byte[] m_HiResBytes;
        class CameraInfo
        {
            // Material is mutated to display renderTexture
            public MeshRenderer renderer;
            // Camera is mutated to write to renderTexture
            public Camera camera;
            public RenderTexture renderTexture;
        }

        private CameraInfo m_LeftInfo;
        private CameraInfo m_RightInfo;

        /// Where the camera's output should be fed.
        public MeshRenderer m_Display;

        // Various Save Icon render textures.
        [SerializeField] private int m_SaveIconHiResWidth = 1920;
        [SerializeField] private int m_SaveIconHiResHeight = 1080;
        private RenderTexture m_SaveIconHiResRenderTexture;

        private RenderTexture m_SaveIconRenderTexture;

        public string screenshotName = "sample.png";

        private string m_UserPath;
        private string m_OldUserPath;

        public const string kAppFolderName = "VRDraw";

        private string m_SaveDir;
        public const string TILT_SUFFIX = ".tilt";

        private DiskSceneFileInfo m_AutosaveFileInfo;
        private string m_AutosaveTargetFilename;
        [SerializeField] private string m_AutosaveFilenamePattern;

        public const string FN_THUMBNAIL = "screenshot.png";

        private CameraInfo LeftInfo
        {
            get
            {
                // Need to lazy-init this; others might try to call our public API before
                // our Awake() and Start() have been called (because object starts inactive)
                if (m_LeftInfo == null)
                {
                    m_LeftInfo = new CameraInfo();
                    m_LeftInfo.camera = GetComponent<Camera>();
                    m_LeftInfo.renderer = m_Display;
                }
                return m_LeftInfo;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                m_UserPath = "/sdcard/";
                m_OldUserPath = Application.persistentDataPath;
            }

            m_UserPath = Path.Combine(m_UserPath, kAppFolderName);
            m_SaveDir = Path.Combine(m_UserPath, "Sketches");

            if (!Directory.Exists(m_SaveDir))
            {
                Directory.CreateDirectory(m_SaveDir);
            }

            m_SaveIconHiResRenderTexture = new RenderTexture(m_SaveIconHiResWidth, m_SaveIconHiResHeight,
                                                         0, RenderTextureFormat.ARGB32);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TakePhoto()
        {
            //DiskSceneFileInfo fileInfo = GetWritableFile();
            //Directory.CreateDirectory(Path.Combine(m_UserPath, "hello"));
            string autosaveStart = m_AutosaveFilenamePattern.Substring(
                0, m_AutosaveFilenamePattern.IndexOf("{"));

            m_AutosaveTargetFilename = Path.Combine(
                m_SaveDir, string.Format(m_AutosaveFilenamePattern, DateTime.Now));

            m_AutosaveFileInfo = new DiskSceneFileInfo(m_AutosaveTargetFilename);

            if (m_SaveIconHiResRenderTexture != null)
            {
                //tool.CurrentCameraRigState = iconXform;
                //saveIconScreenshotManager.RenderToTexture(m_SaveIconHiResRenderTexture);

                RenderTextureFormat format = GetComponent<Camera>().allowHDR
                    ? RenderTextureFormat.ARGBFloat
                    : RenderTextureFormat.ARGB32;
                int depth = 24;

                // Use a temporary rather than rendering to rTexture because we don't know
                // what format rTexture is... it may not be the correct format.
                RenderTexture targetA = RenderTexture.GetTemporary(
                    m_SaveIconHiResRenderTexture.width, m_SaveIconHiResRenderTexture.height, depthBuffer: depth, format: format);

                {
                    // Instead of doing a new Render(), it might seem tempting to copy from
                    // the camera target.  That would be wrong, because the camera target's
                    // resolution might be much lower than rTexture.
                    var camera = LeftInfo.camera;
                    var prev = camera.targetTexture;
                    camera.targetTexture = targetA;
                    camera.Render();
                    camera.targetTexture = prev;
                }

                if (targetA != m_SaveIconHiResRenderTexture)
                {
                    Graphics.Blit(targetA, m_SaveIconHiResRenderTexture);
                    RenderTexture.ReleaseTemporary(targetA);
                }

                //yield return null;
            }

            if (m_SaveIconHiResRenderTexture != null)
            {
                //yield return null;
                m_HiResBytes = SaveToMemory(m_SaveIconHiResRenderTexture, false);
            }

            // We need to save off the thumbnail position so that future quicksaves will know
            // where to take a thumbnail from.
            //m_LastThumbnail_SS = App.Scene.Pose.inverse * iconXform.GetLossyTrTransform();

            //string folderPath = Application.persistentDataPath + "sample.png";
            //string folderPath = System.IO.Directory.GetCurrentDirectory() + "/Screenshots/";
            //ScreenCapture.CaptureScreenshot(folderPath);
            //ScreenCapture.CaptureScreenshot(screenshotName);  
            //I360Render.Capture(1024, true, null, true);

            WriteSnapshotToFile(m_AutosaveFileInfo.FullPath);
        }

        public string WriteSnapshotToFile(string path)
        {
            try
            {
                using (var tiltWriter = new AtomicWriter(path))
                {
                    
                    if (m_HiResBytes != null)
                    {
                        using (var stream = tiltWriter.GetWriteStream(FN_THUMBNAIL))
                        {
                            stream.Write(m_HiResBytes, 0, m_HiResBytes.Length);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                return ex.Message;
            }
            catch (UnauthorizedAccessException ex)
            {
                return ex.Message;
            }
            return null;
        }

        sealed public class AtomicWriter : IDisposable
        {
            private string m_destination;
            private string m_temporaryPath;
            private bool m_finished = false;

            public AtomicWriter(string path)
            {
                m_destination = path;
                m_temporaryPath = path + "_part";
                Destroy(m_temporaryPath);

                Directory.CreateDirectory(m_temporaryPath);
                
            }

            /// Returns a writable stream to an empty subfile.
            public Stream GetWriteStream(string subfileName)
            {
                Debug.Assert(!m_finished);
                
                Directory.CreateDirectory(m_temporaryPath);
                string fullPath = Path.Combine(m_temporaryPath, subfileName);
                return new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                
            }

            /// Raises exception on failure.
            /// On failure, existing file is untouched.
            public void Commit()
            {
                if (m_finished) { return; }
                m_finished = true;

                string previous = m_destination + "_previous";
                Destroy(previous);
                // Don't destroy previous version until we know the new version is in place.
                try { Rename(m_destination, previous); }
                // The *NotFound exceptions are benign; they happen when writing a new file.
                // Let the other IOExceptions bubble up; they probably indicate some problem
                catch (FileNotFoundException) { }
                catch (DirectoryNotFoundException) { }
                Rename(m_temporaryPath, m_destination);
                Destroy(previous);
            }

            public void Rollback()
            {
                if (m_finished) { return; }
                m_finished = true;

                Destroy(m_temporaryPath);
            }

            // IDisposable support

            ~AtomicWriter() { Dispose(); }
            public void Dispose()
            {
                if (!m_finished) { Rollback(); }
                GC.SuppressFinalize(this);
            }

            // Static API

            // newpath must not already exist
            private static void Rename(string oldpath, string newpath)
            {
                Directory.Move(oldpath, newpath);
            }

            // Handles directories, files, and read-only flags.
            private static void Destroy(string path)
            {
                if (File.Exists(path))
                {
                    File.SetAttributes(path, FileAttributes.Normal);
                    File.Delete(path);
                }
                else if (Directory.Exists(path))
                {
                    RecursiveUnsetReadOnly(path);
                    Directory.Delete(path, true);
                }
            }

            private static void RecursiveUnsetReadOnly(string directory)
            {
                foreach (string sub in Directory.GetFiles(directory))
                {
                    File.SetAttributes(Path.Combine(directory, sub), FileAttributes.Normal);
                }
                foreach (string sub in Directory.GetDirectories(directory))
                {
                    RecursiveUnsetReadOnly(Path.Combine(directory, sub));
                }
            }
        }

        static public byte[] SaveToMemory(RenderTexture rTextureToSave, bool bSaveAsPng)
        {
            Debug.Assert(rTextureToSave.format == RenderTextureFormat.ARGB32);

            // Copy out of the RenderTexture
            Texture2D rNoAlphaTexture;
            {
                RenderTexture prev = RenderTexture.active;
                RenderTexture.active = rTextureToSave;
                rNoAlphaTexture = new Texture2D(rTextureToSave.width, rTextureToSave.height, TextureFormat.RGB24, false);
                rNoAlphaTexture.ReadPixels(new Rect(0, 0, rTextureToSave.width, rTextureToSave.height), 0, 0);
                RenderTexture.active = prev;
            }

            byte[] bytes = null;
            if (bSaveAsPng)
            {
                bytes = rNoAlphaTexture.EncodeToPNG();
            }
            else
            {
                bytes = rNoAlphaTexture.EncodeToJPG();
            }
            Destroy(rNoAlphaTexture);

            return bytes;
        }

        /*private DiskSceneFileInfo GetWritableFile()
        {
            // hermetic gltf files currently don't work with AccessLevel.PRIVATE
            SceneFileInfo currentFileInfo = SaveLoadScript.m_Instance.SceneFile;

            DiskSceneFileInfo fileInfo;
            
            // Save as a new file
            fileInfo = new DiskSceneFileInfo();
            
            return fileInfo;
        }

        public DiskSceneFileInfo GetNewNameSceneFileInfo()
        {
            DiskSceneFileInfo fileInfo = new DiskSceneFileInfo(GenerateNewUntitledFilename(m_SaveDir, TILT_SUFFIX));
            return fileInfo;
        }

        public string GenerateNewUntitledFilename(string directory, string extension)
        {
            int iIndex = m_LastNonexistentFileIndex;
            int iSanity = 9999;
            while (iSanity > 0)
            {
                string attempt = UNTITLED_PREFIX + iIndex.ToString();
                --iSanity;
                ++iIndex;

                attempt = Path.Combine(directory, attempt) + extension;
                if (!File.Exists(attempt) && !Directory.Exists(attempt))
                {
                    m_LastNonexistentFileIndex = iIndex;
                    return attempt;
                }
            }

            Debug.Assert(false, "Could not generate a name");
            return null;
        }
        */
    }

}
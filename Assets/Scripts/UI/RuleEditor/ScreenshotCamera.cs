using System;
using UnityEngine;

namespace UI.RuleEditor
{
    public class ScreenshotCamera:MonoBehaviour
    {
        private Camera camera, mainCamera;

        public GameObject interactablesContainer;
        public int resWidth = 2550; 
        public int resHeight = 2550;
        
        //TODO ottimizzare: 1) usare unity recorder package 2) creare solo uno screenshot per tipo di cubo
        
        private void Start()
        {
            camera = this.GetComponent<Camera>();
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        public void TakeScreenshot(GameObject gameObject, InteractionCreationController.Modalities modality)
        {
            HideOtherGameobjects(gameObject);
            
            Collider collider = gameObject.GetComponent<Collider>();
            
            //Position the camera
            float cameraDistance = 2.0f; // Constant factor
            Vector3 objectSizes = collider.bounds.max - collider.bounds.min;
            float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
            float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * camera.fieldOfView); // Visible height 1 meter in front
            float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
            distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object
            camera.transform.position = collider.bounds.center - distance * camera.transform.forward;
            camera.transform.rotation = mainCamera.transform.rotation;
            
            CaptureImageFromCamera();
            
            ShowGameobjects(gameObject);
        }
        
        public static string ScreenShotName(int width, int height) {
            return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png", 
                Application.dataPath, 
                width, height, 
                System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        }


        private void HideOtherGameobjects(GameObject gameObject)
        {
            //Hide all the other objects except the one we are capturing to take the screenshot
            for (int i = 0; i < interactablesContainer.transform.childCount; i++)
            {
                GameObject child = interactablesContainer.transform.GetChild(i).gameObject;
                if (child != gameObject)
                {
                    child.SetActive(false);
                }
            }
        }

        private void ShowGameobjects(GameObject gameObject)
        {
            for (int i = 0; i < interactablesContainer.transform.childCount; i++)
            {
                GameObject child = interactablesContainer.transform.GetChild(i).gameObject;
                if(child != gameObject)
                    child.SetActive(true);
            }
        }

        private void CaptureImageFromCamera()
        {
            //Take screenshot
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class SinglePhotoShareController:MonoBehaviour  {

	private bool isProcessing;
	private Texture2D texturaActual;
	public GameObject imagenTextura;
	public RawImage imagen;

	public void sacarFoto(){
		RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default);        
		Texture2D screenShot = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);

		//no es necesario todas las camaras, puede ser camaraPosta y listo
		foreach(Camera cam in Camera.allCameras)
		{
			cam.targetTexture = rt;
			cam.Render();
			cam.targetTexture = null;
		}

		RenderTexture.active = rt;
		screenShot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0, false);
		Camera.main.targetTexture = null;
		RenderTexture.active = null;

		screenShot.Apply();
		texturaActual = screenShot;
		imagen.texture = texturaActual;
		imagenTextura.SetActive (true);
	}

	public void volver(){
		imagenTextura.SetActive (false);
	}

	public void share()
	{
		if(!isProcessing){
			
			StartCoroutine( ShareOnePicture(texturaActual) );
		}
	}


	public IEnumerator ShareOnePicture(Texture2D img)
	{
		isProcessing = true;
		yield return new WaitForEndOfFrame();

		byte[] dataToSave = img.EncodeToJPG();
		string destination = Path.Combine(Application.persistentDataPath,System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".jpg");
		File.WriteAllBytes(destination, dataToSave);
		if(!Application.isEditor)
		{
			// block to open the file and share it ------------START
			AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
			AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
			intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND_MULTIPLE"));
			AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse","file://" + destination);

			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

			intentObject.Call<AndroidJavaObject> ("setType", "text/plain");
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Festival UATAU '18 Club Universitario, Bahía Blanca");
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "UATAU 2018");

			intentObject.Call<AndroidJavaObject>("setType", "*/*");
			AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

			currentActivity.Call("startActivity", intentObject);
		}
		isProcessing = false;
	}

}

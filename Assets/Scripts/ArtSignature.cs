/*
UniGif
Copyright (c) 2015 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArtSignature : MonoBehaviour
{
	public InputField input;
    const int BASE_SCREEN_WIDTH = 640;
    
    bool mutex;

    UniGifTexture uniGifTexture;
    FixPlaneAspectRatio fixAspect;

    void Awake ()
    {
        uniGifTexture = GetComponent<UniGifTexture> ();
        fixAspect = GetComponent<FixPlaneAspectRatio> ();
    }

	public void OnBtnOK()
	{
		if (mutex == false && uniGifTexture != null && !string.IsNullOrEmpty(input.text)) 
		{
			mutex = true;
			uniGifTexture.Stop ();
			StartCoroutine (ViewGifCoroutine ());
		}
	}

    IEnumerator ViewGifCoroutine ()
    {
		string url = "http://www.yishuzi.com/a/re.php";
		WWWForm form = new WWWForm ();
		form.AddField ("id", input.text);
		form.AddField ("id_", "pihun");
		form.AddField ("i_d", "jiqie");
		form.AddField ("id1", "901");
		form.AddField ("id2", "#ffffff");
		form.AddField ("id3", "");
		form.AddField ("id4", "#000000");
		form.AddField ("id5", "");
		form.AddField ("id6", "#000000");
		
		WWW www = new WWW (url, form);
		yield return www;
		
		int startIndex = www.text.IndexOf ('"');
		int lastIndex = www.text.LastIndexOf ('"');
		string picPath = "http://www.yishuzi.com/" + www.text.Substring (startIndex + 1, lastIndex - startIndex-1);
		
		Debug.Log ("picPath:" + picPath);
		yield return StartCoroutine (uniGifTexture.SetGifFromUrlCoroutine (picPath));

        fixAspect.FixAspectRatio (uniGifTexture.width, uniGifTexture.height);

        mutex = false;
    }
}
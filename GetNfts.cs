using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Threading;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;

public class GetNfts : MonoBehaviour
{
    FirebaseFirestore db;
    string url = "bos";

    int count = 0;

    public List<string> nfts = new List<string>();
    public RawImage textureDisplayer;
    public int textureWidth = 400;
    public int textureHeight = 400;

    // Start is called before the first frame update
    async void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        fetchNfts();
        string imageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2f/Google_2015_logo.svg/408px-Google_2015_logo.svg.png";

    }

    void OnGUI()
    {
        // ıf rıght arrow ıs pressed
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            Debug.Log("presss"+count);
            count++;
            if (count > nfts.Count - 1)
            {
                count = 0;
            }
            StartCoroutine(LoadImg(nfts[count]));
        }
    }

    void displayImage(Texture2D imgToDisp)
    {
        //Resize Image
        textureDisplayer.GetComponent<RectTransform>().sizeDelta = new Vector2(textureWidth, textureHeight);
        textureDisplayer.texture = imgToDisp;
    }


    IEnumerator LoadImg(string url)
    {
        yield return null;
        WWW www = new WWW(url);
        yield return www;

        displayImage(www.texture);
    }


    public void fetchNfts()
    {

        db.Collection("sellings").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot eventsQuerySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in eventsQuerySnapshot.Documents)
            {
                Debug.Log(String.Format("Document data for {0} document:", documentSnapshot.Id));
                //fıll the list with nfts ımage
                foreach
                (KeyValuePair<string, object> pair in documentSnapshot.ToDictionary())
                {
                    if (pair.Key == "image")
                    {
                        url = pair.Value.ToString();
                        nfts.Add(url);
                    }
                }


            }
            Debug.Log("LOG" + nfts);
            StartCoroutine(LoadImg(url));

        });





    }

}
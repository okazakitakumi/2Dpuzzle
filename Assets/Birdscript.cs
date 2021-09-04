using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Birdscript : MonoBehaviour
{
    //鳥のプレハブを格納する配列
    public GameObject[] birdPrehabs;

    //連鎖を消す最低限
    [SerializeField]
    private float removeBirdMinCount = 3;

    // 連鎖判定用の距離
    [SerializeField]
    private float birdDistance = 1.4f;
    //クリックされた鳥を格納
    private GameObject firstbird;
    private GameObject lastBird;
    private string currentName;
    List<GameObject> removableBirdList = new List<GameObject>();
    int score;
    [SerializeField]
    Text scoreText = default;
    [SerializeField]
    GameObject pointEffectPrehab = default;

    [SerializeField]
    Text timerText = default;
    int timeCount;

    [SerializeField]
    GameObject resultPanel = default;

    
    void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.BGM.Game);
        score = 0;
        AddScore(0);
        timeCount = 30;
        scoreText.text = score.ToString();
        StartCoroutine(CountDown());
        touchmanager.Began += (info) =>
        {
            //クリック時点でヒットしているOBJを取得
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(info.screenPoint),
                Vector2.zero);
            if (hit.collider)
            {
                GameObject hitObj = hit.collider.gameObject;
                //ヒットしたオブジェのTAGを判別
                if(hitObj.tag == "Bird")
                {
                    firstbird = hitObj;
                    lastBird = hitObj;
                    currentName = hitObj.name;
                    removableBirdList = new List<GameObject>();
                    PushToBirdList(hitObj);
                }

            }

        };
        touchmanager.Moved += (info) =>
        {
            
            if (!firstbird)
            {
                return;
            }
            //クリック時点でヒットしているOBJを取得
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(info.screenPoint),
                Vector2.zero);
            if (hit.collider)
            {
                GameObject hitObj = hit.collider.gameObject;
                //ヒットしたオブジェのTAGが鳥＆名前一致＆最後にヒットしたオブジェと違う＆
                //リストに格納されていない
                if(hitObj.tag=="Bird"&&hitObj.name==currentName&&hitObj!=lastBird && 0> removableBirdList.
                IndexOf(hitObj))
                {
                    float distance = Vector2.Distance(hitObj.transform.position,
                        lastBird.transform.position);
                    if (distance > birdDistance)
                    {
                        return;
                    }
                    lastBird = hitObj;
                    PushToBirdList(hitObj);
                }
                
                

            }

        };
        touchmanager.Ended += (info) =>
        {
            // リストの格納数を取り出す
            int removeCount = removableBirdList.Count;
            if (removeCount >= removeBirdMinCount)
            {
                foreach (GameObject obj in removableBirdList)
                {
                    Destroy(obj);
                }
                //補充
                StartCoroutine(DropBirds(removeCount));

                AddScore(removeCount * 100) ;
                Debug.Log($"スコア:{removeCount*100}");
                int score = removeCount * 100;
                SpawnPointEffect(removableBirdList[removableBirdList.Count-1].transform.position,score);
                SoundManager.instance.PlaySE(SoundManager.SE.Destroy);
            }
            foreach (GameObject obj in removableBirdList)
            {
                ChangeColor(obj, 1.0f);
            }
            removableBirdList = new List<GameObject>();
            firstbird = null;
            lastBird = null;

        };
        StartCoroutine(DropBirds(70));
    }

    IEnumerator CountDown()
    {
        while (timeCount > 0)
        {


            yield return new WaitForSeconds(1);
            timeCount--;
            timerText.text = timeCount.ToString();
        }
        Debug.Log("終了！");
        resultPanel.SetActive(true);
    }

    public void OnRetryButton()
    {
        SceneManager.LoadScene("Title");
    }
   


    void AddScore(int point)
    {
        score += point;
        scoreText.text = score.ToString();
    }

    void SpawnPointEffect(Vector2 position,int score)
    {
        Instantiate(pointEffectPrehab, position, Quaternion.identity)
;    }
        private void PushToBirdList(GameObject obj)
        {
            removableBirdList.Add(obj);
            ChangeColor(obj, 0.5f);
             SoundManager.instance.PlaySE(SoundManager.SE.Touch);

        }

        private void ChangeColor(GameObject obj, float transparency)
        {
            SpriteRenderer birdSpriteRenderer = obj.GetComponent<SpriteRenderer>();
            birdSpriteRenderer.color = new Color(birdSpriteRenderer.color.r, birdSpriteRenderer.color.g,
               birdSpriteRenderer.color.b, transparency);

        }

        IEnumerator DropBirds(int count)
        {
            for (int i = 0; i < count; i++)
            {
                //ランダムで出現させる
                Vector2 pos = new Vector2(Random.Range(-8.0f, 10.3f), 25.4f);
                //ランダムで鳥を出現させてIDを格納
                int id = Random.Range(0, birdPrehabs.Length);
                //鳥を発生させる
                GameObject bird = (GameObject)Instantiate(birdPrehabs[id],pos,
                    Quaternion.AngleAxis(Random.Range(-40, 40), Vector3.forward));
                //作成した鳥の名前変更
                bird.name = "Bird" + id;
                //0.05秒待って次の処理へ
                yield return new WaitForSeconds(0.1f);


            }
            
            
        }
    




    void Update()
        {

        }
    
}

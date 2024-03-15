using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Stage : MonoBehaviour
{
    public int stageNumber;
    public string sceneNameToLoad;

    private void Start()
    {

        if (stageNumber > 0)
        {
            // Xây dựng tên scene cần chuyển đến
            sceneNameToLoad = "stage_" + stageNumber;
        }
        else
        {
            sceneNameToLoad = "stage_selection";
        }
        // Đảm bảo rằng TMP có thể nhận sự kiện click
        GetComponent<TMP_Text>().enabled = true;
    }

    private void Update()
    {
        // Kiểm tra nếu người dùng nhấp chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            // Lấy vị trí click chuột
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Kiểm tra xem click có trúng TMP không
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(clickPosition))
            {
                // Chuyển đến scene mới
                SceneManager.LoadScene(sceneNameToLoad);
            }
        }
    }
}

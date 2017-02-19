using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FavoriteCourseUIScript : MonoBehaviour {
    void Start()
    {
        var courseSeed = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>().GetCourseSeed();
        if (courseSeed != 0)
        {
            var IsFavorited = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CourseHistoryManager>().IsCourseFavorited(courseSeed, ApplicationManager.GetDifficultyLevel());
            GetComponent<Toggle>().isOn = IsFavorited;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void AddCourseToFavorite(bool SetFavorite)
    {
        var courseSeed = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>().GetCourseSeed();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<CourseHistoryManager>().FavoriteCourse(courseSeed, ApplicationManager.GetDifficultyLevel(), SetFavorite);
    }
}

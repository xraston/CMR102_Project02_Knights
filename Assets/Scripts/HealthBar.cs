using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public KnightManager knightManagerB;
    RectTransform m_RectTransform;
    float m_XAxis;

    // Start is called before the first frame update
    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveHealthBar();
    }

    public void MoveHealthBar()
    {
        m_RectTransform.anchoredPosition = new Vector2(m_XAxis, -71);
        m_XAxis = knightManagerB.knightHealthCurrent - 72;
    }
}

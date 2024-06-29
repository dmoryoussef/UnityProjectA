using TMPro;
using UnityEngine;

public class DisplayCurrentStateController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] StateMachine sm;

    void Update()
    {
        string name = sm.getCurrentState(sm.RootState);
        text.SetText(name);

    }
}

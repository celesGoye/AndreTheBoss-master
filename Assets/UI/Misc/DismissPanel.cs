using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DismissPanel : MonoBehaviour
{
    public GeneralPanel generalPanel;
    public void OnSure()
    {
        if (generalPanel != null)
            generalPanel.OnDismissSure();
        this.gameObject.SetActive(false);
    }

    public void OnCancel()
    {
        this.gameObject.SetActive(false);
    }
}

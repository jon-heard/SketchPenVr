
using Ruccho.GraphicsCapture;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PluginExtension_CaptureTexture_Screen : CaptureTexture
{
  private void Start()
  {
    var targets = Utils.GetTargets();
    GetComponent<CaptureTexture>().SetTarget(targets.Last());
    StartCoroutine(PostStart());
  }

  private IEnumerator PostStart()
  {
    yield return new WaitForSecondsRealtime(0.5f);
    GetComponent<Renderer>().material.mainTexture.filterMode = FilterMode.Point;
  }
}

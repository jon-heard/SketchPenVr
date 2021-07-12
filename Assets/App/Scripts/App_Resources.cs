using UnityEngine;

public class App_Resources : Common.SingletonComponent<App_Resources>
{
  public Common.CommonResources MyCommonResources;

  public Material ControllerVisHidden;
  public Material ControllerVisShadowed;
  public Material ControllerVisFull;
  public Material ScreenWithShadow;
  public Material ScreenWithoutShadow;
}

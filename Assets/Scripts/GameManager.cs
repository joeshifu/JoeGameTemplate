using XGame.Event;

public class GameManager
{
    public GameManager()
    {
        
    }

    public void Start()
    {
        VersionMgr.Instance.InitStart(() =>
        {
            VersionMgr.Instance.Destroy();

            LuaManager.Instance.InitStart();

        });
    }

    public void Update()
    {
        EventDispatcher.TriggerEvent(EventCosnt.GameTick);
    }

    public void OnDestroy()
    {
       
    }
}

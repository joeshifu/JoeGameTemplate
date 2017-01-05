
public class AppConst
{
    public const bool DebugMode = true;                       //调试模式,是否开发期
    public const bool UpdateMode = false;                      //更新模式-默认关闭 
    public const bool LuaByteMode = false;                     //Lua字节码模式-默认关闭 
    public const bool LuaBundleMode = false;                    //Lua代码AssetBundle模式

    public const string AppName = "zsqp";                       //应用程序名称
    public const string LuaTempDir = "Lua/";                    //临时目录
    public const string AppPrefix = AppName + "_";              //应用程序前缀
    public const string ExtName = ".unity3d";                   //素材扩展名
    public const string AssetDir = "StreamingAssets";           //素材目录 
    public const string WebUrl = "http://localhost:6688/";      //更新地址

    public static int SocketPort = 16050;                     //Socket服务器端口
    public static string SocketAddress = "127.0.0.1";          //Socket服务器地址
}

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
#endif
using System.IO;

public static class XCodePostProcess
{

#if UNITY_EDITOR
    [PostProcessBuild(999)]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target != BuildTarget.iOS)
        {
            Debug.LogWarning("Target is not iPhone. XCodePostProcess will not run");
            return;
        }

        //得到xcode工程的路径
        string path = Path.GetFullPath(pathToBuiltProject);

        // Create a new project object from build target
        XCProject project = new XCProject(pathToBuiltProject);

        // Find and run through all projmods files to patch the project.
        // Please pay attention that ALL projmods files in your project folder will be excuted!
        string[] files = Directory.GetFiles(Application.dataPath, "*.projmods", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            UnityEngine.Debug.Log("ProjMod File: " + file);
            project.ApplyMod(file);
        }

        //TODO implement generic settings as a module option
        //project.overwriteBuildSetting("CODE_SIGN_IDENTITY[sdk=iphoneos*]", "iPhone Distribution", "Release");



        //增加一个编译标记。。没有的话sharesdk会报错。。
        //project.AddOtherLinkerFlags("-licucore");
        project.AddOtherLinkerFlags("-ObjC");



        //设置签名的证书， 第二个参数 你可以设置成你的证书
        //project.overwriteBuildSetting ("CODE_SIGN_IDENTITY", "xxxxxx", "Release");
        //project.overwriteBuildSetting ("CODE_SIGN_IDENTITY", "xxxxxx", "Debug");

        project.overwriteBuildSetting("ENABLE_BITCODE", "NO", "Debug");
        project.overwriteBuildSetting("ENABLE_BITCODE", "NO", "Release");

        // 编辑plist 文件
        EditorPlist(path);

        //编辑代码文件
        EditorCode(path);

        //Finally save the xcode project
        project.Save();

    }


    //https
    static string plistAdd1 = @"
	<key>NSAppTransportSecurity</key>
    <dict>
        <key>NSAllowsArbitraryLoads</key>
        <true/>
    </dict>	
	";

    //Microphone
    private static string plistAdd2 = @"
    <key>NSMicrophoneUsageDescription</key>    
    <string>microphoneDesciption</string>
 <key>NSLocationAlwaysUsageDescription</key>    
    <string></string>";


    /*URL Type
        string PlistAdd = @"  
            <key>CFBundleURLTypes</key>
            <array>
            <dict>
            <key>CFBundleTypeRole</key>
            <string>Editor</string>
            <key>CFBundleURLIconFile</key>
            <string>Icon@2x</string>
            <key>CFBundleURLName</key>
            <string>"+bundle+@"</string>
            <key>CFBundleURLSchemes</key>
            <array>
            <string>ww123456</string>
            </array>
            </dict>
            </array>";
            */

    /// <summary>
    /// 编辑plist 文件
    /// </summary>
    /// <param name="filePath"></param>
    private static void EditorPlist(string filePath)
    {

        XCPlist list = new XCPlist(filePath);
        list.Init();

        //在plist里面增加一行
        list.AddKey(plistAdd2);

        //在plist里面替换一行  //string bundle = "com.joeshifu.baidumapdemo";
        //list.ReplaceKey("<string>com.yusong.${PRODUCT_NAME}</string>","<string>"+bundle+"</string>");

        //保存
        list.Save();

    }

    static string codeAdd1 = @"";
    static string codeAdd2 = @"";





    /// <summary>
    /// 编辑代码文件
    /// </summary>
    /// <param name="filePath"></param>
    private static void EditorCode(string filePath)
    {
        //读取UnityAppController.mm文件
        XClass UnityAppController = new XClass(filePath + "/Classes/UnityAppController.mm");

        //在指定代码后面增加一行代码
        //此方法在某一时刻才生效（2.0 < iOS Version < 9.0）
        UnityAppController.WriteBelow("@synthesize interfaceOrientation	= _curOrientation;",
            "- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url{[[IapppayKit sharedInstance] handleOpenUrl:url];return YES;}");



        //此方法在某一时刻才生效（iOS Version > 9.0）
        UnityAppController.WriteBelow("@synthesize renderDelegate			= _renderDelegate;",
            "- (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<NSString*, id> *)options{[[IapppayKit sharedInstance] handleOpenUrl:url];return YES;}");


        UnityAppController.WriteBelow("AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);",
            "[[IapppayKit sharedInstance] handleOpenUrl:url];");

        //声明头文件
        UnityAppController.WriteBelow("#import <OpenGLES/ES2/glext.h>",
            "#import <IapppayKit/IapppayKit.h>");

        //在指定代码中替换一行
        //UnityAppController.Replace(@"return YES;",codeAdd2);

    }
#endif

}

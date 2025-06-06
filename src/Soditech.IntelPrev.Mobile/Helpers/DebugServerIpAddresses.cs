namespace Soditech.IntelPrev.Mobile.Helpers;

public class DebugServerIpAddresses
{
	/*
     * This field is being used for setting IP address for debugging the clients (eg: Maui application)
     * It's being set in
     *  - SplashActivity.cs (StartApplication method) in *.Mobile.Droid project,
     *  - AppDelegate.cs (FinishedLaunching method) in *.Mobile.iOS project.
     */
	public static string Current = "192.168.1.100";//192.168.1.41
}
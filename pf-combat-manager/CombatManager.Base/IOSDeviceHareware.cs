using System.Runtime.InteropServices;

namespace CombatManager
{
#if MONO
    public class IosDeviceHardware
    {
        public const string HardwareProperty = "hw.machine";
        
        public enum IosHardware {
            IPhone,
            IPhone3G,
            IPhone3Gs,
            IPhone4,
            IPhone4RevA,
            IPhone4Cdma,
            IPhone4S,
            IPhone5Gsm,
            IPhone5Cdmagsm,
            IPodTouch1G,
            IPodTouch2G,
            IPodTouch3G,
            IPodTouch4G,
            IPodTouch5G,
            IPad,
            IPad3G,
            IPad2,
            IPad2Gsm,
            IPad2Cdma,
            IPad2RevA,
            IPadMini,
            IPadMiniGsm,
            IPadMiniCdmagsm,
            IPad3,
            IPad3Cdma,
            IPad3Gsm,
            IPad4,
            IPad4Gsm,
            IPad4Cdmagsm,
            IPhoneSimulator,
            IPhoneRetinaSimulator,
            IPadSimulator,
            IPadRetinaSimulator,
            Unknown
        }
         
        static internal extern int Sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property, IntPtr output, IntPtr oldLen, IntPtr newp, uint newlen);
        
        public static IosHardware Version {
            get {
                var pLen = Marshal.AllocHGlobal(sizeof(int));
                Sysctlbyname(IosDeviceHardware.HardwareProperty, IntPtr.Zero, pLen, IntPtr.Zero, 0);
                
                var length = Marshal.ReadInt32(pLen);
                
                if (length == 0) {
                    Marshal.FreeHGlobal(pLen);
                    
                    return IosHardware.Unknown;
                }
                
                var pStr = Marshal.AllocHGlobal(length);
                Sysctlbyname(IosDeviceHardware.HardwareProperty, pStr, pLen, IntPtr.Zero, 0);
                
                var hardwareStr = Marshal.PtrToStringAnsi(pStr);

                Marshal.FreeHGlobal(pLen);
                Marshal.FreeHGlobal(pStr);

                System.Diagnostics.Debug.WriteLine("Hardware: " + hardwareStr);
                
                if (hardwareStr == "iPhone1,1") return IosHardware.IPhone;
                if (hardwareStr == "iPhone1,2") return IosHardware.IPhone3G;
                if (hardwareStr == "iPhone2,1") return IosHardware.IPhone3Gs;
                if (hardwareStr == "iPhone3,1") return IosHardware.IPhone4;
                if (hardwareStr == "iPhone3,2") return IosHardware.IPhone4RevA;
                if (hardwareStr == "iPhone3,3") return IosHardware.IPhone4Cdma;
                if (hardwareStr == "iPhone4,1") return IosHardware.IPhone4S;
                if (hardwareStr == "iPhone5,1") return IosHardware.IPhone5Gsm;
                if (hardwareStr == "iPhone5,2") return IosHardware.IPhone5Cdmagsm;
                
                if (hardwareStr == "iPad1,1") return IosHardware.IPad;
                if (hardwareStr == "iPad1,2") return IosHardware.IPad3G;
                if (hardwareStr == "iPad2,1") return IosHardware.IPad2;
                if (hardwareStr == "iPad2,2") return IosHardware.IPad2Gsm;
                if (hardwareStr == "iPad2,3") return IosHardware.IPad2Cdma;
                if (hardwareStr == "iPad2,4") return IosHardware.IPad2RevA;
                if (hardwareStr == "iPad2,5") return IosHardware.IPadMini;
                if (hardwareStr == "iPad2,6") return IosHardware.IPadMiniGsm;
                if (hardwareStr == "iPad2,7") return IosHardware.IPadMiniCdmagsm;
                if (hardwareStr == "iPad3,1") return IosHardware.IPad3;
                if (hardwareStr == "iPad3,2") return IosHardware.IPad3Cdma;
                if (hardwareStr == "iPad3,3") return IosHardware.IPad3Gsm;
                if (hardwareStr == "iPad3,4") return IosHardware.IPad4;
                if (hardwareStr == "iPad3,5") return IosHardware.IPad4Gsm;
                if (hardwareStr == "iPad3,6") return IosHardware.IPad4Cdmagsm;
                
                if (hardwareStr == "iPod1,1") return IosHardware.IPodTouch1G;
                if (hardwareStr == "iPod2,1") return IosHardware.IPodTouch2G;
                if (hardwareStr == "iPod3,1") return IosHardware.IPodTouch3G;
                if (hardwareStr == "iPod4,1") return IosHardware.IPodTouch4G;
                if (hardwareStr == "iPod5,1") return IosHardware.IPodTouch5G;
                
                if (hardwareStr == "i386" || hardwareStr=="x86_64")
                {
                    if (Device.CurrentDevice.Model.Contains("iPhone"))
                    {
                        //if(UIScreen.MainScreen.Scale > 1.5f)
                        //    return IOSHardware.iPhoneRetinaSimulator;
                        //else
                            return IosHardware.IPhoneSimulator;
                    }
                    else
                    {
                        //if(UIScreen.MainScreen.Scale > 1.5f)
                        //    return IOSHardware.iPadRetinaSimulator;
                        //else
                            return IosHardware.IPadSimulator;
                    }
                }
                
                return IosHardware.Unknown;
            }
        }
    }
#endif
}


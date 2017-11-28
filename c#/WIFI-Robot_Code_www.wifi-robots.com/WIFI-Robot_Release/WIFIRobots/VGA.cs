using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ChangeVGA
{ 
    public class VGA
    {
        int i = Screen.PrimaryScreen.Bounds.Width;
        int j = Screen.PrimaryScreen.Bounds.Height;
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public short dmOrientation;
            public short dmPaperSize;
            public short dmPaperLength;
            public short dmPaperWidth;
            public short dmScale;
            public short dmCopies;
            public short dmDefaultSource;
            public short dmPrintQuality;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;


            public const int DM_DISPLAYFREQUENCY = 0x400000;
             public const int DM_PELSWIDTH = 0x80000;
                public const int DM_PELSHEIGHT = 0x100000;
                 private const int CCHDEVICENAME = 32;
                    private const int CCHFORMNAME = 32;

        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int ChangeDisplaySettings([In] ref DEVMODE lpDevMode, int dwFlags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool EnumDisplaySettings(string lpszDeviceName, Int32 iModeNum, ref DEVMODE lpDevMode);
       public void ChangeRes(int width,int height,int displayFrequency,int bitPerPel)
        {

            DEVMODE DevM = new DEVMODE();
            DevM.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            bool mybool;
            mybool = EnumDisplaySettings(null, 0, ref DevM);
            DevM.dmPelsWidth = width;//宽
            DevM.dmPelsHeight = height;//高
            DevM.dmDisplayFrequency = displayFrequency;//刷新频率
            //DevM.dmBitsPerPel = DEVMODE.DM_PELSWIDTH | DEVMODE.DM_PELSHEIGHT | DEVMODE.DM_DISPLAYFREQUENCY;//颜色象素
            DevM.dmBitsPerPel = bitPerPel;
            long result = ChangeDisplaySettings(ref DevM, 0);
        }
       public void FuYuan()
        {
            DEVMODE DevM = new DEVMODE();
            DevM.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            bool mybool;
            mybool = EnumDisplaySettings(null, 0, ref DevM);
            DevM.dmPelsWidth = i;//恢复宽
            DevM.dmPelsHeight = j;//恢复高
            DevM.dmDisplayFrequency = 60;//刷新频率
            DevM.dmBitsPerPel = 32;//颜色象素
            long result = ChangeDisplaySettings(ref DevM, 0);
        }

    }
}

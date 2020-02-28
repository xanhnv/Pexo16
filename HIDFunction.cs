using System;
using System.Runtime.InteropServices;

namespace Pexo16
{
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
    public class hidDeviceInfo 
    { 
        public String path;
        public UInt16 vendorId;
        public UInt16 productId;
        public String serialNumber; 
        public String manufacturerString;
        public String productString;
        [MarshalAs(UnmanagedType.LPStruct)]
        public hidDeviceInfo next; 
    }
    
    class HIDFunction
    {
        [DllImport("hidapi.dll", EntryPoint = "hid_init", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)] //convert to cdecl calling convention. I am not sure why this works but it fixs the problem
        public static extern int hid_Init();

        [DllImport("hidapi.dll", EntryPoint = "hid_device_next_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)] //convert to cdecl calling convention. I am not sure why this works but it fixs the problem
        public static extern Int64 hid_DeviceNext(Int64 device);

        [DllImport("hidapi.dll", EntryPoint = "hid_device_product_string_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void hid_DeviceProductString(Int64 device, ref byte s);

        [DllImport("hidapi.dll", EntryPoint = "hid_device_vendor_id_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern ushort hid_DeviceVendorID(Int64 device);

        [DllImport("hidapi.dll", EntryPoint = "hid_device_product_id_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern ushort hid_DeviceProductID(Int64 device);

        [DllImport("hidapi.dll", EntryPoint = "hid_free_enumeration_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void hid_FreeEnumeration(Int64 device);

        [DllImport("hidapi.dll", EntryPoint = "hid_set_nonblocking_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern short hid_SetNonBlocking(Int64 device, int nonblock);

        [DllImport("hidapi.dll", EntryPoint = "hid_send_feature_report_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern short hid_SendFeatureReport(Int64 device, ref byte s, ushort lenght);

        [DllImport("hidapi.dll", EntryPoint = "hid_get_feature_report_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern short hid_GetFeatureReport(Int64 device, ref byte data, ushort lenght);

        [DllImport("hidapi.dll", EntryPoint = "hid_enumerate_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern Int64 hid_Enumerate(ushort vendorId, ushort productId);

        [DllImport("hidapi.dll", EntryPoint = "hid_exit_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern short hid_Exit();

        [DllImport("hidapi.dll", EntryPoint = "hid_device_manufacturer_string_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void hid_DeviceManufacturerString(Int64 device, byte s);

        [DllImport("hidapi.dll", EntryPoint = "hid_device_interface_number_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern ushort hid_DeviceInterfaceNumber(Int64 device);

        [DllImport("hidapi.dll", EntryPoint = "hid_open_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern Int64 hid_Open(ushort vendor_id, ushort product_id, ref byte s);

        [DllImport("hidapi.dll", EntryPoint = "hid_open", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 hidOpen(UInt16 vendorId, UInt16 productId, String serialNumber); //xong


        [DllImport("hidapi.dll", EntryPoint = "hid_read_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern short hid_Read(Int64 device, ref byte data, ushort lenght);

        [DllImport("hidapi.dll", EntryPoint = "hid_write_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern short hid_Write(Int64 device, ref byte data, ushort lenght);

        [DllImport("hidapi.dll", EntryPoint = "hid_close_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void hid_Close(Int64 device);

        [DllImport("hidapi.dll", EntryPoint = "hid_error_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void hid_Error(Int64 device, ref byte s);

        [DllImport("hidapi.dll", EntryPoint = "hid_get_caps_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void hid_GetCap(Int64 device, ref ushort InputReportLength, ref ushort OutputReportLength, ref ushort FeatureReportLength);

        [DllImport("hidapi.dll", EntryPoint = "hid_get_serial_number_string_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern short hid_GetSerialNumberString(Int64 device, ref byte s, ushort maxlen);

        [DllImport("hidapi.dll", EntryPoint = "hid_device_serial_number_vb", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void hid_DeviceSerialNum(Int64 device, ref byte s);

        [DllImport("hidapi.dll", EntryPoint = "hid_read_timeout", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern short hid_ReadTimeOut(Int64 device, ref byte data, ushort lenght, int milisecond);

        //[DllImport("hidapi_2.dll", EntryPoint = "hid_get_feature_report", CallingConvention = CallingConvention.Cdecl)]
        //public static extern short hid_Get_Feature_Report(Int64 device, ref byte s, ushort lenght);

        //[DllImport("hidapi_2.dll", EntryPoint = "hid_read", CallingConvention = CallingConvention.Cdecl)]
        //public static extern short hidRead(Int64 device, ref byte s, ushort lenght);



        [DllImport("hidapi_g.dll", EntryPoint = "hid_send_feature_report", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern short hid_Send_Feature_Report(Int64 device, ref byte s, ushort lenght);

        [DllImport("hidapi_g.dll", EntryPoint = "hid_get_feature_report", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern short hid_Get_Feature_Report(Int64 device, ref byte data, ushort lenght);

        [DllImport("hidapi_g.dll", EntryPoint = "hid_read", CallingConvention = CallingConvention.Cdecl)]
        public static extern int hid_ReadG(IntPtr device, [Out] byte[] data, int lenght);

        [DllImport("hidapi_g.dll", EntryPoint = "hid_write", CallingConvention = CallingConvention.Cdecl)]
        public static extern int hid_WriteG(IntPtr device, [In] byte[] data, int lenght);

        [DllImport("hidapi_g.dll", EntryPoint = "hid_open", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr hid_OpenG(UInt16 vendor_id, UInt16 product_id,  String s);

        [DllImport("hidapi_g.dll", EntryPoint = "hid_set_nonblocking", CallingConvention = CallingConvention.Cdecl)]
        public static extern short hid_SetNonBlockingG(IntPtr device, int nonblock);
    }
}

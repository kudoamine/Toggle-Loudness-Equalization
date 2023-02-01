
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.LinkLabel

Public Class Form1

    Public Enum HRESULT As Integer
        S_OK = 0
        S_FALSE = 1
        E_NOINTERFACE = &H80004002
        E_NOTIMPL = &H80004001
        E_FAIL = &H80004005
        E_UNEXPECTED = &H8000FFFF
    End Enum

    <ComImport, Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IPropertyStore
        Function GetCount(<Out> ByRef propertyCount As UInteger) As HRESULT
        Function GetAt(<[In]> ByVal propertyIndex As UInteger, <Out, MarshalAs(UnmanagedType.Struct)> ByRef key As PROPERTYKEY) As HRESULT
        Function GetValue(<[In], MarshalAs(UnmanagedType.Struct)> ByRef key As PROPERTYKEY, <Out, MarshalAs(UnmanagedType.Struct)> ByRef pv As PROPVARIANT) As HRESULT
        Function SetValue(<[In], MarshalAs(UnmanagedType.Struct)> ByRef key As PROPERTYKEY, <[In], MarshalAs(UnmanagedType.Struct)> ByRef pv As PROPVARIANT) As HRESULT
        Function Commit() As HRESULT
    End Interface

    Public Const STGM_READ As Integer = &H0
    Public Const STGM_WRITE As Integer = &H1
    Public Const STGM_READWRITE As Integer = &H2

    Public Structure PROPERTYKEY
        Public Sub New(ByVal InputId As Guid, ByVal InputPid As UInt32)
            fmtid = InputId
            pid = InputPid
        End Sub

        Private fmtid As Guid
        Private pid As UInteger
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure PROPARRAY
        Public cElems As UInt32
        Public pElems As IntPtr
    End Structure

    <StructLayout(LayoutKind.Explicit, Pack:=1)>
    Public Structure PROPVARIANT
        <FieldOffset(0)>
        Public varType As UShort
        <FieldOffset(2)>
        Public wReserved1 As UShort
        <FieldOffset(4)>
        Public wReserved2 As UShort
        <FieldOffset(6)>
        Public wReserved3 As UShort
        <FieldOffset(8)>
        Public bVal As Byte
        <FieldOffset(8)>
        Public cVal As SByte
        <FieldOffset(8)>
        Public uiVal As UShort
        <FieldOffset(8)>
        Public iVal As Short
        <FieldOffset(8)>
        Public uintVal As UInt32
        <FieldOffset(8)>
        Public intVal As Int32
        <FieldOffset(8)>
        Public ulVal As UInt64
        <FieldOffset(8)>
        Public lVal As Int64
        <FieldOffset(8)>
        Public fltVal As Single
        <FieldOffset(8)>
        Public dblVal As Double
        <FieldOffset(8)>
        Public boolVal As Short
        <FieldOffset(8)>
        Public pclsidVal As IntPtr
        <FieldOffset(8)>
        Public pszVal As IntPtr
        <FieldOffset(8)>
        Public pwszVal As IntPtr
        <FieldOffset(8)>
        Public punkVal As IntPtr
        <FieldOffset(8)>
        Public ca As PROPARRAY
        <FieldOffset(8)>
        Public filetime As System.Runtime.InteropServices.ComTypes.FILETIME
    End Structure

    Public Enum EDataFlow
        eRender = 0
        eCapture = (eRender + 1)
        eAll = (eCapture + 1)
        EDataFlow_enum_count = (eAll + 1)
    End Enum

    Public Enum ERole
        eConsole = 0
        eMultimedia = (eConsole + 1)
        eCommunications = (eMultimedia + 1)
        ERole_enum_count = (eCommunications + 1)
    End Enum

    <ComImport>
    <Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IMMDeviceEnumerator
        Function EnumAudioEndpoints(ByVal dataFlow As EDataFlow, ByVal dwStateMask As Integer, <Out> ByRef ppDevices As IMMDeviceCollection) As HRESULT
        ' for 0x80070490 : Element not found
        <PreserveSig>
        Function GetDefaultAudioEndpoint(ByVal dataFlow As EDataFlow, ByVal role As ERole, <Out> ByRef ppEndpoint As IMMDevice) As HRESULT
        Function GetDevice(ByVal pwstrId As String, <Out> ByRef ppDevice As IMMDevice) As HRESULT
        Function RegisterEndpointNotificationCallback(ByVal pClient As IMMNotificationClient) As HRESULT
        Function UnregisterEndpointNotificationCallback(ByVal pClient As IMMNotificationClient) As HRESULT
    End Interface

    Public Const DEVICE_STATE_ACTIVE As Integer = &H1
    Public Const DEVICE_STATE_DISABLED As Integer = &H2
    Public Const DEVICE_STATE_NOTPRESENT As Integer = &H4
    Public Const DEVICE_STATE_UNPLUGGED As Integer = &H8
    Public Const DEVICE_STATEMASK_ALL As Integer = &HF

    <ComImport>
    <Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IMMDeviceCollection
        Function GetCount(<Out> ByRef pcDevices As UInteger) As HRESULT
        Function Item(ByVal nDevice As UInteger, <Out> ByRef ppDevice As IMMDevice) As HRESULT
    End Interface

    <ComImport>
    <Guid("D666063F-1587-4E43-81F1-B948E807363F")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IMMDevice
        Function Activate(ByRef iid As Guid, ByVal dwClsCtx As Integer, ByRef pActivationParams As PROPVARIANT, <Out> ByRef ppInterface As IntPtr) As HRESULT
        Function OpenPropertyStore(ByVal stgmAccess As Integer, <Out> ByRef ppProperties As IPropertyStore) As HRESULT
        Function GetId(<Out> ByRef ppstrId As IntPtr) As HRESULT
        Function GetState(<Out> ByRef pdwState As Integer) As HRESULT
    End Interface

    <ComImport>
    <Guid("7991EEC9-7E89-4D85-8390-6C703CEC60C0")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IMMNotificationClient
        Function OnDeviceStateChanged(ByVal pwstrDeviceId As String, ByVal dwNewState As Integer) As HRESULT
        Function OnDeviceAdded(ByVal pwstrDeviceId As String) As HRESULT
        Function OnDeviceRemoved(ByVal pwstrDeviceId As String) As HRESULT
        Function OnDefaultDeviceChanged(ByVal flow As EDataFlow, ByVal role As ERole, ByVal pwstrDefaultDeviceId As String) As HRESULT
        Function OnPropertyValueChanged(ByVal pwstrDeviceId As String, ByRef key As PROPERTYKEY) As HRESULT
    End Interface

    <ComImport>
    <Guid("1BE09788-6894-4089-8586-9A2A6C265AC5")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IMMEndpoint
        Function GetDataFlow(<Out> ByRef pDataFlow As EDataFlow) As HRESULT
    End Interface

    <ComImport>
    <Guid("f8679f50-850a-41cf-9c72-430f290290c8")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IPolicyConfig
        Function GetMixFormat(<[In]> <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDeviceName As String, <Out> ByRef ppFormat As WAVEFORMATEXTENSIBLE) As HRESULT
        Function GetDeviceFormat(<[In]> <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDeviceName As String, <[In]> <MarshalAs(UnmanagedType.Bool)> ByVal bDefault As Boolean, <Out> ByRef ppFormat As WAVEFORMATEXTENSIBLE) As HRESULT
        Function ResetDeviceFormat(<[In]> <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDeviceName As String) As HRESULT
        Function SetDeviceFormat(<[In]> <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDeviceName As String, <[In]> <MarshalAs(UnmanagedType.LPStruct)> ByVal pEndpointFormat As WAVEFORMATEXTENSIBLE,
                                 <[In]> <MarshalAs(UnmanagedType.LPStruct)> ByVal pMixFormat As WAVEFORMATEXTENSIBLE) As HRESULT
        Function GetProcessingPeriod(<[In]> <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDeviceName As String, <[In]> <MarshalAs(UnmanagedType.Bool)> ByVal bDefault As Boolean, <Out> ByRef pmftDefaultPeriod As Int64, <Out> ByRef pmftMinimumPeriod As Int64) As HRESULT
        Function SetProcessingPeriod(<[In]> <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDeviceName As String, ByVal pmftPeriod As Int64) As HRESULT
        Function GetShareMode(<[In]> <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDeviceName As String, <Out> ByRef pMode As DeviceShareMode) As HRESULT
        Function SetShareMode(<[In]> <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDeviceName As String, <[In]> ByVal mode As DeviceShareMode) As HRESULT
        Function GetPropertyValue(<[In]> <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDeviceName As String, <[In]> <MarshalAs(UnmanagedType.Bool)> ByVal bFxStore As Boolean, ByRef pKey As PROPERTYKEY, <Out> ByRef pv As PROPVARIANT) As HRESULT
        Function SetPropertyValue(<[In]> <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDeviceName As String, <[In]> <MarshalAs(UnmanagedType.Bool)> ByVal bFxStore As Boolean, <[In]> ByRef pKey As PROPERTYKEY, ByRef pv As PROPVARIANT) As HRESULT
        Function SetDefaultEndpoint(<[In]> <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDeviceName As String, <[In]> <MarshalAs(UnmanagedType.U4)> ByVal role As ERole) As HRESULT
        Function SetEndpointVisibility(<[In]> <MarshalAs(UnmanagedType.LPWStr)> ByVal pszDeviceName As String, <[In]> <MarshalAs(UnmanagedType.Bool)> ByVal bVisible As Boolean) As HRESULT
    End Interface

    <StructLayout(LayoutKind.Explicit, Pack:=1)>
    Public Class WAVEFORMATEXTENSIBLE
        Inherits WAVEFORMATEX

        <FieldOffset(0)>
        Public wValidBitsPerSample As Short
        <FieldOffset(0)>
        Public wSamplesPerBlock As Short
        <FieldOffset(0)>
        Public wReserved As Short
        <FieldOffset(2)>
        Public dwChannelMask As WaveMask
        <FieldOffset(6)>
        Public SubFormat As Guid
    End Class

    <Flags>
    Public Enum WaveMask
        None = &H0
        FrontLeft = &H1
        FrontRight = &H2
        FrontCenter = &H4
        LowFrequency = &H8
        BackLeft = &H10
        BackRight = &H20
        FrontLeftOfCenter = &H40
        FrontRightOfCenter = &H80
        BackCenter = &H100
        SideLeft = &H200
        SideRight = &H400
        TopCenter = &H800
        TopFrontLeft = &H1000
        TopFrontCenter = &H2000
        TopFrontRight = &H4000
        TopBackLeft = &H8000
        TopBackCenter = &H10000
        TopBackRight = &H20000
    End Enum

    <StructLayout(LayoutKind.Sequential, Pack:=2)>
    Public Class WAVEFORMATEX
        Public wFormatTag As Short
        Public nChannels As Short
        Public nSamplesPerSec As Integer
        Public nAvgBytesPerSec As Integer
        Public nBlockAlign As Short
        Public wBitsPerSample As Short
        Public cbSize As Short
    End Class

    Public Enum DeviceShareMode
        [Shared]
        Exclusive
    End Enum

    <DllImport("Shlwapi.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
    Public Shared Function PathParseIconLocationW(pszIconFile As String) As Integer
    End Function

    Public Shared PKEY_Device_FriendlyName As PROPERTYKEY = New PROPERTYKEY(New Guid("a45c254e-df1c-4efd-8020-67d146a850e0"), 14)
    Public Shared PKEY_Device_DeviceDesc As PROPERTYKEY = New PROPERTYKEY(New Guid("a45c254e-df1c-4efd-8020-67d146a850e0"), 2)
    Public Shared PKEY_DeviceClass_IconPath As PROPERTYKEY = New PROPERTYKEY(New Guid("259abffc-50a7-47ce-af08-68c9a7d73366"), 12)
    Public Shared PKEY_AudioEndpoint_Disable_SysFx As PROPERTYKEY = New PROPERTYKEY(New Guid("1da5d803-d492-4edd-8c23-e0c0ffee7f0e"), 5)
    Public Shared MFPKEY_CORR_LOUDNESS_EQUALIZATION_ON As PROPERTYKEY = New PROPERTYKEY(New Guid("fc52a749-4be9-4510-896e-966ba6525980"), 3)

    Dim colorDefault As Color = Color.LightGreen


    ' Main coding start here

    Dim selectedDeviceID As String = ""
    Dim selectedMode As Integer = 0
    Dim selectedNotify As Integer = 1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If File.Exists("Settings.ini") Then
            Dim fileReader As String = ""
            Try
                fileReader = My.Computer.FileSystem.ReadAllText("Settings.ini")
            Catch ex As Exception
                MsgBox("Failed to open Settings.ini file, try running the software as administrator!", MsgBoxStyle.Critical, "Error")
                End
            End Try

            Try
                selectedDeviceID = fileReader.Substring(fileReader.IndexOf("DeviceID = ") + 11, fileReader.LastIndexOf("}") - 10)
                selectedMode = fileReader.Substring(fileReader.IndexOf("Mode = ") + 7, 1)
                selectedNotify = fileReader.Substring(fileReader.IndexOf("Notify = ") + 9, 1)
            Catch ex As Exception
                MsgBox("Failed to read Settings.ini values, try deleting Settings.ini and reopen the software again!", MsgBoxStyle.Critical, "Error")
                End
            End Try

            If selectedMode = 1 Then
                toggleLoudnessEqualization()
            ElseIf selectedMode = 2 Then
                toggleAllFX()
            End If

            Timer1.Start()
        Else
            NotifyIcon1.Visible = False
            Me.Opacity = 1
            Me.ShowInTaskbar = True
            ToolTip1.SetToolTip(PictureBox1, "Visit the github repository if you have any issues!")
            ToolTip1.SetToolTip(RadioButton1, "Choose this to toggle the Loudness Equalization only!")
            ToolTip1.SetToolTip(RadioButton2, "Choose this to toggle all Windows Sound FX (use this as an alternative if the other one doesn't work)!")
            ToolTip1.SetToolTip(CheckBox1, "Show windows notifications when you toggle the Loudness Equalization or keep it silent.")
            PopulateListView()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        selectedDeviceID = ListView1.SelectedItems.Item(0).SubItems(2).Text
        If RadioButton1.Checked Then
            selectedMode = 1
        Else
            selectedMode = 2
        End If
        If CheckBox1.Checked Then
            selectedNotify = 1
        Else
            selectedNotify = 0
        End If
        Try
            My.Computer.FileSystem.WriteAllText("Settings.ini", "DeviceID = " & selectedDeviceID & vbCrLf & "Mode = " & selectedMode & vbCrLf & "Notify = " & selectedNotify, False)
            MsgBox("The audio device was saved, the software will close now." & vbCrLf & "Reopen it to toggle the loudness equalization every time!", MsgBoxStyle.Information, "Success")
            End
        Catch ex As Exception
            MsgBox("Failed to save Settings.ini file, try running the software as administrator!", MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub PopulateListView()
        Try
            Dim hr As HRESULT = HRESULT.E_FAIL
            Dim CLSID_MMDeviceEnumerator As Guid = New Guid("{BCDE0395-E52F-467C-8E3D-C4579291692E}")
            Dim MMDeviceEnumeratorType As Type = Type.GetTypeFromCLSID(CLSID_MMDeviceEnumerator, True)
            Dim MMDeviceEnumerator As Object = Activator.CreateInstance(MMDeviceEnumeratorType)
            Dim pMMDeviceEnumerator As IMMDeviceEnumerator = CType(MMDeviceEnumerator, IMMDeviceEnumerator)
            If (pMMDeviceEnumerator IsNot Nothing) Then
                Dim sIdDefaultRender As String = Nothing
                Dim sIdDefaultCapture As String = Nothing
                Dim pDefaultDevice As IMMDevice = Nothing
                hr = pMMDeviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eConsole, pDefaultDevice)
                If hr = HRESULT.S_OK Then
                    Dim hGlobal As IntPtr = Marshal.AllocHGlobal(260)
                    hr = pDefaultDevice.GetId(hGlobal)
                    sIdDefaultRender = Marshal.PtrToStringUni(hGlobal)
                    Marshal.FreeHGlobal(hGlobal)
                    Marshal.ReleaseComObject(pDefaultDevice)
                End If
                hr = pMMDeviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eConsole, pDefaultDevice)
                If hr = HRESULT.S_OK Then
                    Dim hGlobal As IntPtr = Marshal.AllocHGlobal(260)
                    hr = pDefaultDevice.GetId(hGlobal)
                    sIdDefaultCapture = Marshal.PtrToStringUni(hGlobal)
                    Marshal.FreeHGlobal(hGlobal)
                    Marshal.ReleaseComObject(pDefaultDevice)
                End If

                Dim pDeviceCollection As IMMDeviceCollection = Nothing
                hr = pMMDeviceEnumerator.EnumAudioEndpoints(EDataFlow.eAll, DEVICE_STATE_ACTIVE Or DEVICE_STATE_DISABLED, pDeviceCollection)
                If (hr = HRESULT.S_OK) Then
                    Dim nDevices As UInteger = 0
                    hr = pDeviceCollection.GetCount(nDevices)
                    Dim currentId As Integer = 1
                    For i As UInteger = 0 To CUInt(nDevices - 1)
                        Dim pDevice As IMMDevice = Nothing
                        hr = pDeviceCollection.Item(i, pDevice)
                        If (hr = HRESULT.S_OK) Then
                            Dim pPropertyStore As IPropertyStore = Nothing
                            hr = pDevice.OpenPropertyStore(STGM_READ, pPropertyStore)
                            If hr = HRESULT.S_OK Then

                                Dim sFriendlyName As String = Nothing
                                Dim pv As PROPVARIANT = New PROPVARIANT()
                                hr = pPropertyStore.GetValue(PKEY_Device_FriendlyName, pv)
                                If hr = HRESULT.S_OK Then
                                    sFriendlyName = Marshal.PtrToStringUni(pv.pwszVal)
                                End If

                                Dim sIconPath As String = Nothing
                                Dim nIconId As Integer = 0
                                Dim pvIconPath As PROPVARIANT = New PROPVARIANT()
                                hr = pPropertyStore.GetValue(PKEY_DeviceClass_IconPath, pvIconPath)
                                If hr = HRESULT.S_OK Then
                                    ' %windir%\system32\mmres.dll,-3011
                                    sIconPath = Marshal.PtrToStringUni(pvIconPath.pwszVal)
                                    nIconId = PathParseIconLocationW(sIconPath)
                                End If

                                Dim hGlobal As IntPtr = Marshal.AllocHGlobal(260)
                                hr = pDevice.GetId(hGlobal)
                                Dim sId As String = Marshal.PtrToStringUni(hGlobal)
                                Marshal.FreeHGlobal(hGlobal)

                                Dim pEndpoint As IMMEndpoint = Nothing
                                pEndpoint = CType(pDevice, IMMEndpoint)
                                Dim eDirection As EDataFlow = EDataFlow.eAll
                                hr = pEndpoint.GetDataFlow(eDirection)

                                Dim nState As Integer = 0
                                Dim sState As String = ""
                                hr = pDevice.GetState(nState)
                                If (nState = DEVICE_STATE_ACTIVE) Then
                                    sState = "Active"
                                ElseIf (nState = DEVICE_STATE_DISABLED) Then
                                    sState = "Disabled"
                                ElseIf (nState = DEVICE_STATE_NOTPRESENT) Then
                                    sState = "Not present"
                                ElseIf (nState = DEVICE_STATE_UNPLUGGED) Then
                                    sState = "Unplugged"
                                End If

                                Dim lvi As ListViewItem = Nothing
                                If (eDirection = EDataFlow.eRender) Then
                                    lvi = ListView1.Items.Add(New ListViewItem(New String() {currentId.ToString(), sFriendlyName, sId, sState}))
                                    currentId += 1
                                End If
                                If (sId = sIdDefaultRender) Then
                                    lvi.BackColor = colorDefault
                                End If
                                Marshal.ReleaseComObject(pPropertyStore)
                            End If
                            Marshal.ReleaseComObject(pDevice)
                        End If
                    Next
                    Marshal.ReleaseComObject(pDeviceCollection)
                End If
                Marshal.ReleaseComObject(pMMDeviceEnumerator)
            End If
        Catch ex As Exception
            MsgBox("Failed to get audio devices, head to the github repository for help!", MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If
    End Sub


    Private Sub toggleAllFX()
        Try
            Dim hr As HRESULT = HRESULT.E_FAIL
            Dim CLSID_PolicyConfig As Guid = New Guid("{870af99c-171d-4f9e-af0d-e63df40c2bc9}")
            Dim PolicyConfigType As Type = Type.GetTypeFromCLSID(CLSID_PolicyConfig, True)
            Dim PolicyConfig As Object = Activator.CreateInstance(PolicyConfigType)
            Dim pPolicyConfig As IPolicyConfig = CType(PolicyConfig, IPolicyConfig)

            Dim sCurrentId As String = selectedDeviceID

            If pPolicyConfig IsNot Nothing Then
                Dim prop As New PROPVARIANT

                hr = pPolicyConfig.GetPropertyValue(sCurrentId, True, PKEY_AudioEndpoint_Disable_SysFx, prop)

                Dim status As String = ""

                If prop.uiVal = 0 Then
                    prop.uiVal = 1
                    status = "OFF"
                ElseIf prop.uiVal = 1 Then
                    prop.uiVal = 0
                    status = "ON"
                End If
                hr = pPolicyConfig.SetPropertyValue(sCurrentId, True, PKEY_AudioEndpoint_Disable_SysFx, prop)

                hr = pPolicyConfig.SetEndpointVisibility(sCurrentId, False)
                hr = pPolicyConfig.SetEndpointVisibility(sCurrentId, True)

                Marshal.ReleaseComObject(PolicyConfig)

                If selectedNotify = 1 Then
                    NotifyIcon1.ShowBalloonTip(2000, "Loudness Equalization", status, ToolTipIcon.Info)
                End If
            End If
        Catch ex As Exception
            MsgBox("Couldn't toggle All FX! head to the github repository for help!", MsgBoxStyle.Critical, "Error")
            End
        End Try
    End Sub

    Private Sub toggleLoudnessEqualization()
        Try
            Dim hr As HRESULT = HRESULT.E_FAIL
            Dim CLSID_PolicyConfig As Guid = New Guid("{870af99c-171d-4f9e-af0d-e63df40c2bc9}")
            Dim PolicyConfigType As Type = Type.GetTypeFromCLSID(CLSID_PolicyConfig, True)
            Dim PolicyConfig As Object = Activator.CreateInstance(PolicyConfigType)
            Dim pPolicyConfig As IPolicyConfig = CType(PolicyConfig, IPolicyConfig)

            Dim sCurrentId As String = selectedDeviceID

            If pPolicyConfig IsNot Nothing Then
                Dim prop As New PROPVARIANT

                hr = pPolicyConfig.GetPropertyValue(sCurrentId, True, MFPKEY_CORR_LOUDNESS_EQUALIZATION_ON, prop)

                Dim status As String = ""

                If prop.uiVal = 0 Then
                    prop.uiVal = 65535
                    status = "ON"
                ElseIf prop.uiVal = 65535 Then
                    prop.uiVal = 0
                    status = "OFF"
                End If

                hr = pPolicyConfig.SetPropertyValue(sCurrentId, True, MFPKEY_CORR_LOUDNESS_EQUALIZATION_ON, prop)

                hr = pPolicyConfig.SetEndpointVisibility(sCurrentId, False)
                hr = pPolicyConfig.SetEndpointVisibility(sCurrentId, True)

                Marshal.ReleaseComObject(PolicyConfig)

                If selectedNotify = 1 Then
                    NotifyIcon1.ShowBalloonTip(2000, "Loudness Equalization", status, ToolTipIcon.Info)
                End If
            End If
        Catch ex As Exception
            MsgBox("Couldn't toggle Loudness Equalization! delete Settings.ini and try the other Mode instead!", MsgBoxStyle.Critical, "Error")
            End
        End Try
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim ps = New ProcessStartInfo("https://github.com/KudoAmine/toggle-loudness-equalization") With {.UseShellExecute = True}
        Process.Start(ps)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        NotifyIcon1.Visible = False
        End
    End Sub
End Class

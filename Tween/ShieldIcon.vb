Imports System.Runtime.InteropServices
Imports System


Public Class ShieldIcon


    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
    Private Structure SHSTOCKICONINFO
        Public cbSize As Integer
        Public hIcon As IntPtr
        Public iSysImageIndex As Integer
        Public iIcon As Integer
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)> _
        Public szPath As String
    End Structure

    Private Declare Function SHGetStockIconInfo Lib "shell32.dll" (ByVal siid As Integer, ByVal uFlags As UInteger, ByRef psii As SHSTOCKICONINFO) As Integer

    Private Declare Function DestroyIcon Lib "shell32.dll" (ByVal hIcon As IntPtr) As Boolean


    Const SIID_SHIELD As Integer = 77
    Const SHGFI_ICON As UInteger = &H100
    Const SHGFI_SMALLICON As UInteger = &H1


    Private icondata As Image = Nothing
    Private sii As SHSTOCKICONINFO


    Public Sub New()
        'NT6 kernelかどうか検査
        If Environment.OSVersion.Platform <> PlatformID.Win32NT OrElse Environment.OSVersion.Version.Major <> 6 Then
            icondata = Nothing
            Return
        End If

        Try
            sii = New SHSTOCKICONINFO
            sii.cbSize = Marshal.SizeOf(sii)
            sii.hIcon = IntPtr.Zero
            SHGetStockIconInfo(SIID_SHIELD, SHGFI_ICON Or SHGFI_SMALLICON, sii)
            icondata = Bitmap.FromHicon(sii.hIcon)
        Catch ex As Exception
            icondata = Nothing
        End Try
        Return
    End Sub

    Public Sub Dispose()
        If icondata IsNot Nothing Then
            icondata.Dispose()
        End If
    End Sub

    Public ReadOnly Property Icon() As Image
        Get
            Return icondata
        End Get
    End Property

End Class

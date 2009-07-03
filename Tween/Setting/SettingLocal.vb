﻿<Serializable()> _
Public Class SettingLocal
    Inherits SettingBase(Of SettingLocal)

#Region "Settingクラス基本"
    Public Shared Function Load() As SettingLocal
        Return LoadSettings()
    End Function

    Public Sub Save()
        SaveSettings(Me)
    End Sub
#End Region

    Private _fc As New FontConverter
    Private _cc As New ColorConverter

    Public FormLocation As New Point(0, 0)
    Public SplitterDistance As Integer = 320
    Public FormSize As New Size(436, 476)
    Public StatusText As String = ""
    Public UseRecommendStatus As Boolean = False
    Public Width1 As Integer = 48
    Public Width2 As Integer = 80
    Public Width3 As Integer = 290
    Public Width4 As Integer = 120
    Public Width5 As Integer = 50
    Public Width6 As Integer = 16
    Public Width7 As Integer = 32
    Public Width8 As Integer = 50
    Public DisplayIndex1 As Integer = 0
    Public DisplayIndex2 As Integer = 1
    Public DisplayIndex3 As Integer = 2
    Public DisplayIndex4 As Integer = 3
    Public DisplayIndex5 As Integer = 4
    Public DisplayIndex6 As Integer = 5
    Public DisplayIndex7 As Integer = 6
    Public DisplayIndex8 As Integer = 7
    Public BrowserPath As String = ""
    Public ProxyType As ProxyTypeEnum = ProxyTypeEnum.IE
    Public ProxyAddress As String = "127.0.0.1"
    Public ProxyPort As Integer = 80
    Public ProxyUser As String = ""
    Public StatusMultiline As Boolean = False
    Public StatusTextHeight As Integer = 38

    <Xml.Serialization.XmlIgnore()> _
    Public FontUnread As New Font(SystemFonts.DefaultFont, FontStyle.Bold Or FontStyle.Underline)
    Public Property FontUnreadStr() As String
        Get
            Return _fc.ConvertToString(FontUnread)
        End Get
        Set(ByVal value As String)
            FontUnread = DirectCast(_fc.ConvertFromString(value), Font)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public ColorUnread As Color = System.Drawing.SystemColors.ControlText
    Public Property ColorUnreadStr() As String
        Get
            Return _cc.ConvertToString(ColorUnread)
        End Get
        Set(ByVal value As String)
            ColorUnread = DirectCast(_cc.ConvertFromString(value), Color)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public FontRead As Font = System.Drawing.SystemFonts.DefaultFont
    Public Property FontReadStr() As String
        Get
            Return _fc.ConvertToString(FontRead)
        End Get
        Set(ByVal value As String)
            FontRead = DirectCast(_fc.ConvertFromString(value), Font)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public ColorRead As Color = Color.FromKnownColor(System.Drawing.KnownColor.Gray)
    Public Property ColorReadStr() As String
        Get
            Return _cc.ConvertToString(ColorRead)
        End Get
        Set(ByVal value As String)
            ColorRead = DirectCast(_cc.ConvertFromString(value), Color)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public ColorFav As Color = Color.FromKnownColor(System.Drawing.KnownColor.Red)
    Public Property ColorFavStr() As String
        Get
            Return _cc.ConvertToString(ColorFav)
        End Get
        Set(ByVal value As String)
            ColorFav = DirectCast(_cc.ConvertFromString(value), Color)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public ColorOWL As Color = Color.FromKnownColor(System.Drawing.KnownColor.Blue)
    Public Property ColorOWLStr() As String
        Get
            Return _cc.ConvertToString(ColorOWL)
        End Get
        Set(ByVal value As String)
            ColorOWL = DirectCast(_cc.ConvertFromString(value), Color)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public FontDetail As Font = System.Drawing.SystemFonts.DefaultFont
    Public Property FontDetailStr() As String
        Get
            Return _fc.ConvertToString(FontDetail)
        End Get
        Set(ByVal value As String)
            FontDetail = DirectCast(_fc.ConvertFromString(value), Font)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public ColorSelf As Color = Color.FromKnownColor(System.Drawing.KnownColor.AliceBlue)
    Public Property ColorSelfStr() As String
        Get
            Return _cc.ConvertToString(ColorSelf)
        End Get
        Set(ByVal value As String)
            ColorSelf = DirectCast(_cc.ConvertFromString(value), Color)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public ColorAtSelf As Color = Color.FromKnownColor(System.Drawing.KnownColor.AntiqueWhite)
    Public Property ColorAtSelfStr() As String
        Get
            Return _cc.ConvertToString(ColorAtSelf)
        End Get
        Set(ByVal value As String)
            ColorAtSelf = DirectCast(_cc.ConvertFromString(value), Color)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public ColorTarget As Color = Color.FromKnownColor(System.Drawing.KnownColor.LemonChiffon)
    Public Property ColorTargetStr() As String
        Get
            Return _cc.ConvertToString(ColorTarget)
        End Get
        Set(ByVal value As String)
            ColorTarget = DirectCast(_cc.ConvertFromString(value), Color)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public ColorAtTarget As Color = Color.FromKnownColor(System.Drawing.KnownColor.LavenderBlush)
    Public Property ColorAtTargetStr() As String
        Get
            Return _cc.ConvertToString(ColorAtTarget)
        End Get
        Set(ByVal value As String)
            ColorAtTarget = DirectCast(_cc.ConvertFromString(value), Color)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public ColorAtFromTarget As Color = Color.FromKnownColor(System.Drawing.KnownColor.Honeydew)
    Public Property ColorAtFromTargetStr() As String
        Get
            Return _cc.ConvertToString(ColorAtFromTarget)
        End Get
        Set(ByVal value As String)
            ColorAtFromTarget = DirectCast(_cc.ConvertFromString(value), Color)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public ColorAtTo As Color = Color.FromKnownColor(System.Drawing.KnownColor.Pink)
    Public Property ColorAtToStr() As String
        Get
            Return _cc.ConvertToString(ColorAtTo)
        End Get
        Set(ByVal value As String)
            ColorAtTo = DirectCast(_cc.ConvertFromString(value), Color)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public ColorInputBackcolor As Color = Color.FromKnownColor(System.Drawing.KnownColor.LemonChiffon)
    Public Property ColorInputBackcolorStr() As String
        Get
            Return _cc.ConvertToString(ColorInputBackcolor)
        End Get
        Set(ByVal value As String)
            ColorInputBackcolor = DirectCast(_cc.ConvertFromString(value), Color)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public ColorInputFont As Color = Color.FromKnownColor(System.Drawing.KnownColor.ControlText)
    Public Property ColorInputFontStr() As String
        Get
            Return _cc.ConvertToString(ColorInputFont)
        End Get
        Set(ByVal value As String)
            ColorInputFont = DirectCast(_cc.ConvertFromString(value), Color)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public FontInputFont As Font = System.Drawing.SystemFonts.DefaultFont
    Public Property FontInputFontStr() As String
        Get
            Return _fc.ConvertToString(FontInputFont)
        End Get
        Set(ByVal value As String)
            FontInputFont = DirectCast(_fc.ConvertFromString(value), Font)
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public ProxyPassword As String = ""
    Public Property EncryptProxyPassword() As String
        Get
            Dim pwd As String = ProxyPassword
            If String.IsNullOrEmpty(pwd) Then pwd = ""
            If pwd.Length > 0 Then
                Try
                    Return EncryptString(pwd)
                Catch ex As Exception
                    Return ""
                End Try
            Else
                Return ""
            End If
        End Get
        Set(ByVal value As String)
            Dim pwd As String = value
            If String.IsNullOrEmpty(pwd) Then pwd = ""
            If pwd.Length > 0 Then
                Try
                    pwd = DecryptString(pwd)
                Catch ex As Exception
                    pwd = ""
                End Try
            End If
            ProxyPassword = pwd
        End Set
    End Property
End Class

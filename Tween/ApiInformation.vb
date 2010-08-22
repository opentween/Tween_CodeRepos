﻿Imports System.ComponentModel

Public Class ApiInformationChangedEventArgs
    Inherits EventArgs
    Public ApiInfo As New ApiInfo
End Class

Public MustInherit Class ApiInfoBase
    Protected Shared _MaxCount As Integer = -1
    Protected Shared _RemainCount As Integer = -1
    Protected Shared _ResetTime As New DateTime
    Protected Shared _ResetTimeInSeconds As Integer = -1
    Protected Shared _UsingCount As Integer = -1
End Class

Public Class ApiInfo
    Inherits ApiInfoBase
    Public MaxCount As Integer
    Public RemainCount As Integer
    Public ResetTime As DateTime
    Public ResetTimeInSeconds As Integer
    Public UsingCount As Integer

    Public Sub New()
        Me.MaxCount = _MaxCount
        Me.RemainCount = _RemainCount
        Me.ResetTime = _ResetTime
        Me.ResetTimeInSeconds = _ResetTimeInSeconds
        Me.UsingCount = _ResetTimeInSeconds
    End Sub
End Class

Public Class ApiInformation
    Inherits ApiInfoBase

    'Private ReadOnly _lockobj As New Object 更新時にロックが必要かどうかは様子見

    Public Sub Initialize()
        _MaxCount = -1
        _RemainCount = -1
        _ResetTime = New DateTime
        _ResetTimeInSeconds = -1
        'UsingCountは初期化対象外
    End Sub

    Public Function ConvertResetTimeInSecondsToResetTime(ByVal seconds As Integer) As DateTime
        If seconds >= 0 Then
            Return System.TimeZone.CurrentTimeZone.ToLocalTime((New DateTime(1970, 1, 1, 0, 0, 0)).AddSeconds(seconds))
        Else
            Return New DateTime
        End If
    End Function

    Public Event Changed(ByVal sender As Object, ByVal e As ApiInformationChangedEventArgs)

    Private Sub Raise_Changed()
        Dim arg As New ApiInformationChangedEventArgs
        RaiseEvent Changed(Me, arg)
        _MaxCount = arg.ApiInfo.MaxCount
        _RemainCount = arg.ApiInfo.RemainCount
        _ResetTime = arg.ApiInfo.ResetTime
        _ResetTimeInSeconds = arg.ApiInfo.ResetTimeInSeconds
        _UsingCount = arg.ApiInfo.UsingCount
    End Sub

    Public Property MaxCount As Integer
        Get
            Return _MaxCount
        End Get
        Set(ByVal value As Integer)
            If _MaxCount <> value Then
                _MaxCount = value
                Raise_Changed()
            End If
        End Set
    End Property

    Public Property RemainCount As Integer
        Get
            Return _RemainCount
        End Get
        Set(ByVal value As Integer)
            If _RemainCount <> value Then
                _RemainCount = value
                Raise_Changed()
            End If
        End Set
    End Property

    Public Property ResetTime As DateTime
        Get
            Return _ResetTime
        End Get
        Set(ByVal value As DateTime)
            If _ResetTime <> value Then
                _ResetTime = value
                Raise_Changed()
            End If
        End Set
    End Property

    Public Property ResetTimeInSeconds As Integer
        Get
            Return _ResetTimeInSeconds
        End Get
        Set(ByVal value As Integer)
            If _ResetTimeInSeconds <> value Then
                _ResetTimeInSeconds = value
                Raise_Changed()
            End If
        End Set
    End Property

    Public Property UsingCount As Integer
        Get
            Return _UsingCount
        End Get
        Set(ByVal value As Integer)
            If _UsingCount <> value Then
                _UsingCount = value
                Raise_Changed()
            End If
        End Set
    End Property
End Class



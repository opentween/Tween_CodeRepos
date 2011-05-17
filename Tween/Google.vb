﻿' Tween - Client of Twitter
' Copyright (c) 2007-2011 kiri_feather (@kiri_feather) <kiri.feather@gmail.com>
'           (c) 2008-2011 Moz (@syo68k)
'           (c) 2008-2011 takeshik (@takeshik) <http://www.takeshik.org/>
'           (c) 2010-2011 anis774 (@anis774) <http://d.hatena.ne.jp/anis774/>
'           (c) 2010-2011 fantasticswallow (@f_swallow) <http://twitter.com/f_swallow>
' All rights reserved.
' 
' This file is part of Tween.
' 
' This program is free software; you can redistribute it and/or modify it
' under the terms of the GNU General Public License as published by the Free
' Software Foundation; either version 3 of the License, or (at your option)
' any later version.
' 
' This program is distributed in the hope that it will be useful, but
' WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
' or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License
' for more details. 
' 
' You should have received a copy of the GNU General Public License along
' with this program. If not, see <http://www.gnu.org/licenses/>, or write to
' the Free Software Foundation, Inc., 51 Franklin Street - Fifth Floor,
' Boston, MA 02110-1301, USA.

Imports System.Web
Imports System.Text.RegularExpressions
Imports System.Runtime.Serialization.Json
Imports System.Net
Imports System.Runtime.Serialization
Imports System.Linq

Public Class Google

#Region "Translation"
    ' http://code.google.com/intl/ja/apis/ajaxlanguage/documentation/#fonje
    ' デベロッパー ガイド - Google AJAX Language API - Google Code

    Private Const TranslateEndPoint As String = "http://ajax.googleapis.com/ajax/services/language/translate"
    Private Const LanguageDetectEndPoint As String = "https://ajax.googleapis.com/ajax/services/language/detect"

#Region "言語テーブル定義"
    Private Shared LanguageTable As New List(Of String) From {
        "af",
        "sq",
        "am",
        "ar",
        "hy",
        "az",
        "eu",
        "be",
        "bn",
        "bh",
        "br",
        "bg",
        "my",
        "ca",
        "chr",
        "zh",
        "zh-CN",
        "zh-TW",
        "co",
        "hr",
        "cs",
        "da",
        "dv",
        "nl",
        "en",
        "eo",
        "et",
        "fo",
        "tl",
        "fi",
        "fr",
        "fy",
        "gl",
        "ka",
        "de",
        "el",
        "gu",
        "ht",
        "iw",
        "hi",
        "hu",
        "is",
        "id",
        "iu",
        "ga",
        "it",
        "ja",
        "jw",
        "kn",
        "kk",
        "km",
        "ko",
        "ku",
        "ky",
        "lo",
        "la",
        "lv",
        "lt",
        "lb",
        "mk",
        "ms",
        "ml",
        "mt",
        "mi",
        "mr",
        "mn",
        "ne",
        "no",
        "oc",
        "or",
        "ps",
        "fa",
        "pl",
        "pt",
        "pt-PT",
        "pa",
        "qu",
        "ro",
        "ru",
        "sa",
        "gd",
        "sr",
        "sd",
        "si",
        "sk",
        "sl",
        "es",
        "su",
        "sw",
        "sv",
        "syr",
        "tg",
        "ta",
        "tt",
        "te",
        "th",
        "bo",
        "to",
        "tr",
        "uk",
        "ur",
        "uz",
        "ug",
        "vi",
        "cy",
        "yi",
        "yo"
    }
#End Region

    <DataContract()> _
    Public Class TranslateResponseData
        <DataMember(Name:="translatedText")> Public TranslatedText As String
    End Class


    <DataContract()> _
    Private Class TranslateResponse
        <DataMember(Name:="responseData")> Public ResponseData As TranslateResponseData
        <DataMember(Name:="responseDetails")> Public ResponseDetails As String
        <DataMember(Name:="responseStatus")> Public ResponseStatus As HttpStatusCode
    End Class


    <DataContract()> _
    Public Class LanguageDetectResponseData
        <DataMember(Name:="language")> Public Language As String
        <DataMember(Name:="isReliable")> Public IsReliable As Boolean
        <DataMember(Name:="confidence")> Public Confidence As Double
    End Class

    <DataContract()> _
    Private Class LanguageDetectResponse
        <DataMember(Name:="responseData")> Public ResponseData As LanguageDetectResponseData
        <DataMember(Name:="responseDetails")> Public ResponseDetails As String
        <DataMember(Name:="responseStatus")> Public ResponseStatus As HttpStatusCode
    End Class

    Public Function Translate(ByVal srclng As String, ByVal dstlng As String, ByVal source As String, ByRef destination As String, ByRef ErrMsg As String) As Boolean
        Dim http As New HttpVarious()
        Dim apiurl As String = TranslateEndPoint
        Dim headers As New Dictionary(Of String, String)
        headers.Add("v", "1.0")

        ErrMsg = ""
        If String.IsNullOrEmpty(srclng) OrElse String.IsNullOrEmpty(dstlng) Then
            Return False
        End If
        headers.Add("User-Agent", GetUserAgentString())
        headers.Add("langpair", srclng + "|" + dstlng)

        headers.Add("q", source)

        Dim content As String = ""
        If http.GetData(apiurl, headers, content) Then
            Dim serializer As New DataContractJsonSerializer(GetType(TranslateResponse))
            Dim res As TranslateResponse

            Try
                res = CreateDataFromJson(Of TranslateResponse)(content)
            Catch ex As Exception
                ErrMsg = "Err:Invalid JSON"
                Return False
            End Try

            If res.ResponseData Is Nothing Then
                ErrMsg = "Err:" + res.ResponseDetails
                Return False
            End If
            Dim _body As String = res.ResponseData.TranslatedText
            Dim buf As String = HttpUtility.UrlDecode(_body)

            destination = String.Copy(buf)
            Return True
        End If
        Return False
    End Function

    Public Function LanguageDetect(ByVal source As String) As String
        Dim http As New HttpVarious()
        Dim apiurl As String = LanguageDetectEndPoint
        Dim headers As New Dictionary(Of String, String)
        headers.Add("User-Agent", GetUserAgentString())
        headers.Add("v", "1.0")
        headers.Add("q", source)
        Dim content As String = ""
        If http.GetData(apiurl, headers, content) Then
            Dim serializer As New DataContractJsonSerializer(GetType(LanguageDetectResponse))
            Try
                Dim res As LanguageDetectResponse = CreateDataFromJson(Of LanguageDetectResponse)(content)
                Return res.ResponseData.Language
            Catch ex As Exception
                Return ""
            End Try
        End If
        Return ""
    End Function

    Public Function GetLanguageEnumFromIndex(ByVal index As Integer) As String
        Return LanguageTable(index)
    End Function

    Public Function GetIndexFromLanguageEnum(ByVal lang As String) As Integer
        Return LanguageTable.IndexOf(lang)
    End Function
#End Region

#Region "UrlShortener"
    ' http://code.google.com/intl/ja/apis/urlshortener/v1/getting_started.html
    ' Google URL Shortener API

    <DataContract()>
    Private Class UrlShortenerParameter
        <DataMember(Name:="longUrl")> Dim LongUrl As String
    End Class

    <DataContract()>
    Private Class UrlShortenerResponse

    End Class

    Public Function Shorten(ByVal source As String) As String
        Dim http As New HttpVarious
        Dim apiurl As String = "https://www.googleapis.com/urlshortener/v1/url"
        Dim headers As New Dictionary(Of String, String)
        headers.Add("User-Agent", GetUserAgentString())
        headers.Add("Content-Type", "application/json")

        http.PostData(apiurl, headers)
        Return ""
    End Function
#End Region

#Region "GoogleMaps"
    Public Overloads Function CreateGoogleMapsUri(ByVal locate As GlobalLocation) As String
        Return CreateGoogleMapsUri(locate.Latitude, locate.Longitude)
    End Function

    Public Overloads Function CreateGoogleMapsUri(ByVal lat As Double, ByVal lng As Double) As String
        Return "http://maps.google.com/maps/api/staticmap?center=" + lat.ToString + "," + lng.ToString + "&size=" + AppendSettingDialog.Instance.FoursquarePreviewWidth.ToString + "x" + AppendSettingDialog.Instance.FoursquarePreviewHeight.ToString + "&zoom=" + AppendSettingDialog.Instance.FoursquarePreviewZoom.ToString + "&markers=" + lat.ToString + "," + lng.ToString + "&sensor=false"
    End Function

    Public Class GlobalLocation
        Public Property Latitude As Double
        Public Property Longitude As Double
        Public Property LocateInfo As String
    End Class

#End Region

End Class

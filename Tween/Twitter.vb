﻿' Tween - Client of Twitter
' Copyright (c) 2007-2009 kiri_feather (@kiri_feather) <kiri_feather@gmail.com>
'           (c) 2008-2009 Moz (@syo68k) <http://iddy.jp/profile/moz/>
'           (c) 2008-2009 takeshik (@takeshik) <http://www.takeshik.org/>
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
Imports System.Xml
Imports System.Text
Imports System.Threading
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

Public Module Twitter
    Delegate Sub GetIconImageDelegate(ByVal post As PostClass)
    Delegate Function GetTimelineDelegate(ByVal page As Integer, _
                                ByVal read As Boolean, _
                                ByRef endPage As Integer, _
                                ByVal gType As WORKERTYPE, _
                                ByRef getDM As Boolean) As String
    Delegate Function GetDirectMessageDelegate(ByVal page As Integer, _
                                    ByVal read As Boolean, _
                                    ByVal endPage As Integer, _
                                    ByVal gType As WORKERTYPE) As String
    Private ReadOnly LockObj As New Object
    Private GetTmSemaphore As New Threading.Semaphore(8, 8)

    Private follower As New Collections.Specialized.StringCollection
    Private followerId As New List(Of Long)
    Private tmpFollower As New Collections.Specialized.StringCollection

    'プロパティからアクセスされる共通情報
    Private _uid As String
    Private _pwd As String
    Private _proxyType As ProxyTypeEnum
    Private _proxyAddress As String
    Private _proxyPort As Integer
    Private _proxyUser As String
    Private _proxyPassword As String

    Private _nextThreshold As Integer
    Private _nextPages As Integer

    Private _iconSz As Integer
    Private _getIcon As Boolean
    Private _lIcon As ImageList
    Private _dIcon As Dictionary(Of String, Image)

    Private _tinyUrlResolve As Boolean
    Private _restrictFavCheck As Boolean
    Private _useAPI As Boolean

    Private _hubServer As String
    Private _defaultTimeOut As Integer      ' MySocketクラスへ渡すタイムアウト待ち時間（秒単位　ミリ秒への換算はMySocketクラス側で行う）
    Private _countApi As Integer
    Private _usePostMethod As Boolean
    Private _ApiMethod As MySocket.REQ_TYPE

    '共通で使用する状態
    Private _authKey As String              'StatusUpdate、発言削除で使用
    Private _authKeyDM As String              'DM送信、DM削除で使用
    Private _signed As Boolean
    Private _infoTwitter As String = ""
    Private _dmCount As Integer
    Private _getDm As Boolean
    Private _remainCountApi As Integer = -1

    Private _ShortUrlService() As String = { _
            "http://tinyurl.com/", _
            "http://is.gd/", _
            "http://snipurl.com/", _
            "http://snurl.com/", _
            "http://nsfw.in/", _
            "http://qurlyq.com/", _
            "http://dwarfurl.com/", _
            "http://icanhaz.com/", _
            "http://tiny.cc/", _
            "http://urlenco.de/", _
            "http://bit.ly/", _
            "http://piurl.com/", _
            "http://linkbee.com/", _
            "http://traceurl.com/", _
            "http://twurl.nl/", _
            "http://cli.gs/", _
            "http://rubyurl.com/", _
            "http://budurl.com/", _
            "http://ff.im/", _
            "http://twitthis.com/", _
            "http://blip.fm/", _
            "http://tumblr.com/", _
            "http://www.qurl.com/", _
            "http://digg.com/", _
            "http://u.nu/", _
            "http://ustre.am/" _
        }

    Private Const _baseUrlStr As String = "twitter.com"
    Private Const _loginPath As String = "/sessions"
    Private Const _homePath As String = "/home"
    Private Const _replyPath As String = "/replies"
    Private Const _DMPathRcv As String = "/inbox"
    Private Const _DMPathSnt As String = "/sent"
    Private Const _DMDestroyPath As String = "/direct_messages/destroy/"
    Private Const _StDestroyPath As String = "/status/destroy/"
    Private Const _uidHeader As String = "session[username_or_email]="
    Private Const _pwdHeader As String = "session[password]="
    Private Const _pageQry As String = "?page="
    Private Const _statusHeader As String = "status="
    Private Const _statusUpdatePath As String = "/status/update?page=1&tab=home"
    Private Const _statusUpdatePathAPI As String = "/statuses/update.xml"
    Private Const _linkToOld As String = "class=""section_links"" rel=""prev"""
    Private Const _postFavAddPath As String = "/favorites/create/"
    Private Const _postFavRemovePath As String = "/favorites/destroy/"
    Private Const _authKeyHeader As String = "authenticity_token="
    Private Const _parseLink1 As String = "<a href="""
    Private Const _parseLink2 As String = """>"
    Private Const _parseLink3 As String = "</a>"
    Private Const _GetFollowers As String = "/statuses/followers.xml"
    Private Const _ShowStatus As String = "/statuses/show/"
    Private Const _rateLimitStatus As String = "/account/rate_limit_status.xml"

    '''Wedata対応
    Private Const wedataUrl As String = "http://wedata.net/databases/Tween/items.json"

    Private Function SignIn() As String
        If _endingFlag Then Return ""

        'ユーザー情報からデータ部分の生成
        Dim account As String = ""

        SyncLock LockObj
            If _signed Then Return ""

            '未認証
            _signed = False

            MySocket.ResetCookie()

            Dim resStatus As String = ""
            Dim resMsg As String = ""

            resMsg = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + "/", resStatus, MySocket.REQ_TYPE.ReqGET), String)
            If resMsg.Length = 0 Then
                Return "SignIn -> " + resStatus
            End If
            Dim authToken As String = ""
            Dim rg As New Regex("authenticity_token"" type=""hidden"" value=""(?<auth>[a-z0-9]+)""")
            Dim m As Match = rg.Match(resMsg)
            If m.Success Then
                authToken = m.Result("${auth}")
            Else
                Return "SignIn -> Can't get token."
            End If

            account = _authKeyHeader + authToken + "&" + _uidHeader + _uid + "&" + _pwdHeader + _pwd + "&" + "remember_me=1"

            resMsg = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + _loginPath, resStatus, MySocket.REQ_TYPE.ReqPOST, account), String)
            If resStatus.StartsWith("OK") Then
                'OK (username/passwordが合致しない)
                Dim msg As String = resStatus
                If resMsg.Contains("Wrong Username/Email and password combination.") Then
                    msg = "Wrong Username or password."
                Else
                    '未知の応答(May be required Chapta)
                    msg = "Wrong Username or password. Try from web."
                End If
                Return "SignIn Failed -> " + msg
            ElseIf resMsg.Contains("https://twitter.com/account/locked") Then   '302 FOUND
                Dim msg As String = "You account is Locked Out."
                Return "SignIn Failed -> " + msg
            ElseIf resMsg.Contains("https://twitter.com:443/") Then '302 FOUND
                'OK
            ElseIf resMsg.Contains("https://twitter.com/") Then '302 FOUND
                'OK
            ElseIf resStatus.StartsWith("Err:") Then
                ' その他プロトコルエラー
                Return "SignIn Failed -> " + resMsg
            Else
                '応答がOK でありサインインできていない場合の未知の応答
                TraceOut(True, "SignIn Failed." + vbCrLf + "resStatus:" + resStatus + vbCrLf + "resMsg:" + vbCrLf + resMsg)
                Return "SignIn Failed -> " + "Unknown problems."
            End If

            _signed = True
            Return ""
        End SyncLock
    End Function

    Public Function GetTimeline(ByVal page As Integer, _
                                ByVal read As Boolean, _
                                ByRef endPage As Integer, _
                                ByVal gType As WORKERTYPE, _
                                ByRef getDM As Boolean) As String

        If endPage = 0 Then
            '通常モード
            Dim epage As Integer = page
            GetTmSemaphore.WaitOne()
            Dim trslt As String = ""
            trslt = GetTimelineThread(page, read, epage, gType, getDM)
            If trslt.Length > 0 Then Return trslt
            page += 1
            If epage < page OrElse gType = WORKERTYPE.Reply Then Return ""
            endPage = epage
        End If
        '起動時モード or 通常モードの読み込み継続 -> 複数ページ同時取得
        Dim num As Integer = endPage - page
        Dim ar(num) As IAsyncResult
        Dim dlgt(num) As GetTimelineDelegate

        For idx As Integer = 0 To num
            dlgt(idx) = New GetTimelineDelegate(AddressOf GetTimelineThread)
            GetTmSemaphore.WaitOne()
            ar(idx) = dlgt(idx).BeginInvoke(page + idx, read, endPage + idx, gType, getDM, Nothing, Nothing)
        Next
        Dim rslt As String = ""
        For idx As Integer = 0 To num
            Dim epage As Integer = 0
            Dim dm As Boolean = False
            Dim trslt As String = ""
            Try
                trslt = dlgt(idx).EndInvoke(epage, dm, ar(idx))
            Catch ex As Exception
                '最後までendinvoke回す（ゾンビ化回避）
                ex.Data("IsTerminatePermission") = False
                Throw
                rslt = "GetTimelineErr"
            End Try
            If trslt.Length > 0 AndAlso rslt.Length = 0 Then rslt = trslt
            If dm Then getDM = True
        Next
        Return rslt
    End Function

    Private Function GetTimelineThread(ByVal page As Integer, _
                                ByVal read As Boolean, _
                                ByRef endPage As Integer, _
                                ByVal gType As WORKERTYPE, _
                                ByRef getDM As Boolean) As String
        Try
            If _endingFlag Then Return ""

            Dim retMsg As String = ""
            Dim resStatus As String = ""

            Static redirectToTimeline As String = ""
            Static redirectToReply As String = ""

            If _signed = False Then
                retMsg = SignIn()
                If retMsg.Length > 0 Then
                    Return retMsg
                End If
            End If

            If _endingFlag Then Return ""

            'リクエストメッセージを作成する
            Dim pageQuery As String

            If page = 1 Then
                pageQuery = ""
            Else
                pageQuery = _pageQry + page.ToString
            End If

RETRY:
            If gType = WORKERTYPE.Timeline Then
                If redirectToTimeline = "" Then
                    retMsg = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + _homePath + pageQuery, resStatus, MySocket.REQ_TYPE.ReqGetApp), String)
                    If resStatus.StartsWith("Found") Then
                        redirectToTimeline = resStatus.Substring(6)
                        If redirectToTimeline.Contains("?") Then
                            redirectToTimeline = redirectToTimeline.Remove(redirectToTimeline.IndexOf("?"))
                        End If
                        GoTo RETRY
                    End If
                Else
                    retMsg = DirectCast(CreateSocket.GetWebResponse(redirectToTimeline + pageQuery, resStatus, MySocket.REQ_TYPE.ReqGetApp), String)
                End If
            Else
                If redirectToReply = "" Then
                    retMsg = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + _replyPath + pageQuery, resStatus, MySocket.REQ_TYPE.ReqGetApp), String)
                    If resStatus.StartsWith("Found") Then
                        redirectToReply = resStatus.Substring(6)
                        If redirectToReply.Contains("?") Then
                            redirectToReply = redirectToReply.Remove(redirectToReply.IndexOf("?"))
                        End If
                        GoTo RETRY
                    End If
                Else
                    retMsg = DirectCast(CreateSocket.GetWebResponse(redirectToReply + pageQuery, resStatus, MySocket.REQ_TYPE.ReqGetApp), String)
                End If
            End If

            If retMsg.Length = 0 Then
                _signed = False
                Return resStatus
            End If

            ' tr 要素の class 属性を消去
            Do
                Dim idx As Integer = retMsg.IndexOf(_removeClass, StringComparison.Ordinal)
                If idx = -1 Then Exit Do
                Dim idx2 As Integer = retMsg.IndexOf("""", idx + _removeClass.Length, StringComparison.Ordinal) - idx + 1 - 3
                If idx2 > 0 Then retMsg = retMsg.Remove(idx + 3, idx2)
            Loop

            If _endingFlag Then Return ""

            '各メッセージに分割可能か？
            Dim strSepTmp As String
            If gType = WORKERTYPE.Timeline Then
                strSepTmp = _splitPostRecent
            Else
                strSepTmp = _splitPost
            End If

            Dim pos1 As Integer
            Dim pos2 As Integer

            pos1 = retMsg.IndexOf(strSepTmp, StringComparison.Ordinal)
            If pos1 = -1 Then
                '0件 or 取得失敗
                _signed = False
                Return "GetTimeline -> Err: tweets count is 0."
            End If

            Dim strSep() As String = {strSepTmp}
            Dim posts() As String = retMsg.Split(strSep, StringSplitOptions.RemoveEmptyEntries)
            Dim intCnt As Integer = 0
            Dim listCnt As Integer = 0
            SyncLock LockObj
                listCnt = TabInformations.GetInstance.ItemCount
            End SyncLock
            Dim dlgt(20) As GetIconImageDelegate
            Dim ar(20) As IAsyncResult
            Dim arIdx As Integer = -1

            For Each strPost As String In posts
                intCnt += 1

                If intCnt = 1 Then
                    If page = 1 And gType = WORKERTYPE.Timeline Then
                        ''siv取得
                        'pos1 = strPost.IndexOf(_getSiv, 0)
                        'If pos1 > 0 Then
                        '    pos2 = strPost.IndexOf(_getSivTo, pos1 + _getSiv.Length)
                        '    If pos2 > -1 Then
                        '        _authSiv = strPost.Substring(pos1 + _getSiv.Length, pos2 - pos1 - _getSiv.Length)
                        '    Else
                        '        '取得失敗
                        '        _signed = False
                        '        Return "GetTimeline -> Err: Can't get Siv."
                        '    End If
                        'Else
                        '    '取得失敗
                        '    _signed = False
                        '    Return "GetTimeline -> Err: Can't get Siv."
                        'End If

                        'AuthKeyの取得
                        If GetAuthKey(retMsg) < 0 Then
                            _signed = False
                            Return "GetTimeline -> Err: Can't get auth token."
                        End If

                        'TwitterInfoの取得
                        pos1 = retMsg.IndexOf(_getInfoTwitter, StringComparison.Ordinal)
                        If pos1 > -1 Then
                            pos2 = retMsg.IndexOf(_getInfoTwitterTo, pos1, StringComparison.Ordinal)
                            If pos2 > -1 Then
                                _infoTwitter = retMsg.Substring(pos1 + _getInfoTwitter.Length, pos2 - pos1 - _getInfoTwitter.Length)
                            Else
                                _infoTwitter = ""
                            End If
                        Else
                            _infoTwitter = ""
                        End If
                    End If
                Else

                    Dim post As New PostClass

                    Try
                        'Get ID
                        pos1 = 0
                        pos2 = strPost.IndexOf(_statusIdTo, 0, StringComparison.Ordinal)
                        post.Id = Long.Parse(HttpUtility.HtmlDecode(strPost.Substring(0, pos2)))
                    Catch ex As Exception
                        _signed = False
                        TraceOut("TM-ID:" + strPost)
                        Return "GetTimeline -> Err: Can't get ID."
                    End Try
                    'Get Name
                    Try
                        pos1 = strPost.IndexOf(_parseName, pos2, StringComparison.Ordinal)
                        pos2 = strPost.IndexOf(_parseNameTo, pos1, StringComparison.Ordinal)
                        post.Name = HttpUtility.HtmlDecode(strPost.Substring(pos1 + _parseName.Length, pos2 - pos1 - _parseName.Length))
                    Catch ex As Exception
                        _signed = False
                        TraceOut("TM-Name:" + strPost)
                        Return "GetTimeline -> Err: Can't get Name."
                    End Try
                    'Get Nick
                    '''バレンタイン対応
                    If strPost.IndexOf("twitter.com/images/heart.png", pos2, StringComparison.Ordinal) > -1 Then
                        post.Nickname = post.Name
                    Else
                        Try
                            pos1 = strPost.IndexOf(_parseNick, pos2, StringComparison.Ordinal)
                            pos2 = strPost.IndexOf(_parseNickTo, pos1 + _parseNick.Length, StringComparison.Ordinal)
                            post.Nickname = HttpUtility.HtmlDecode(strPost.Substring(pos1 + _parseNick.Length, pos2 - pos1 - _parseNick.Length))
                        Catch ex As Exception
                            _signed = False
                            TraceOut("TM-Nick:" + strPost)
                            Return "GetTimeline -> Err: Can't get Nick."
                        End Try
                    End If

                    '二重取得回避
                    SyncLock LockObj
                        If TabInformations.GetInstance.ContainsKey(post.Id) Then Continue For
                    End SyncLock

                    Dim orgData As String = ""
                    'バレンタイン
                    If strPost.IndexOf("<form action=""/status/update"" id=""heartForm", 0, StringComparison.Ordinal) > -1 Then
                        Try
                            pos1 = strPost.IndexOf("<strong>", 0, StringComparison.Ordinal)
                            pos2 = strPost.IndexOf("</strong>", pos1, StringComparison.Ordinal)
                            orgData = strPost.Substring(pos1 + 8, pos2 - pos1 - 8)
                        Catch ex As Exception
                            _signed = False
                            TraceOut("TM-VBody:" + strPost)
                            Return "GetTimeline -> Err: Can't get Valentine body."
                        End Try
                    End If


                    'Get ImagePath
                    Try
                        pos1 = strPost.IndexOf(_parseImg, pos2, StringComparison.Ordinal)
                        pos2 = strPost.IndexOf(_parseImgTo, pos1 + _parseImg.Length, StringComparison.Ordinal)
                        post.ImageUrl = HttpUtility.HtmlDecode(strPost.Substring(pos1 + _parseImg.Length, pos2 - pos1 - _parseImg.Length))
                    Catch ex As Exception
                        _signed = False
                        TraceOut("TM-Img:" + strPost)
                        Return "GetTimeline -> Err: Can't get ImagePath."
                    End Try

                    'Protect
                    If strPost.IndexOf(_isProtect, pos2, StringComparison.Ordinal) > -1 Then
                        post.IsProtect = True
                    End If

                    'Get Message
                    pos1 = strPost.IndexOf(_parseMsg1, pos2, StringComparison.Ordinal)
                    If pos1 < 0 Then
                        'Valentine対応その２
                        Try
                            If strPost.IndexOf("<div id=""doyouheart", pos2, StringComparison.Ordinal) > -1 Then
                                'バレンタイン
                                orgData += " <3 you! Do you <3 "
                                pos1 = strPost.IndexOf("<a href", pos2, StringComparison.Ordinal)
                                pos2 = strPost.IndexOf("?", pos1, StringComparison.Ordinal)
                                orgData += strPost.Substring(pos1, pos2 - pos1 + 1)
                            Else
                                pos1 = strPost.IndexOf(_parseProtectMsg1, pos2, StringComparison.Ordinal)
                                If pos1 = -1 Then
                                    'バレンタイン
                                    orgData += " <3 's "
                                    pos1 = strPost.IndexOf("<a href", pos2, StringComparison.Ordinal)
                                    If pos1 > -1 Then
                                        pos2 = strPost.IndexOf("!", pos1, StringComparison.Ordinal)
                                        orgData += strPost.Substring(pos1, pos2 - pos1 + 1)
                                    End If
                                Else
                                    'プロテクトメッセージ
                                    pos2 = strPost.IndexOf(_parseProtectMsg2, pos1, StringComparison.Ordinal)
                                    orgData = strPost.Substring(pos1 + _parseProtectMsg1.Length, pos2 - pos1 - _parseProtectMsg1.Length).Trim()
                                End If
                            End If
                        Catch ex As Exception
                            _signed = False
                            TraceOut("TM-VBody2:" + strPost)
                            Return "GetTimeline -> Err: Can't get Valentine body2."
                        End Try
                    Else
                        '通常メッセージ
                        Try
                            pos2 = strPost.IndexOf(_parseMsg2, pos1, StringComparison.Ordinal)
                            orgData = strPost.Substring(pos1 + _parseMsg1.Length, pos2 - pos1 - _parseMsg1.Length).Trim()
                        Catch ex As Exception
                            _signed = False
                            TraceOut("TM-Body:" + strPost)
                            Return "GetTimeline -> Err: Can't get body."
                        End Try
#If 0 Then
                        '原文リンク削除
                        orgData = Regex.Replace(orgData, "<a href=""https://twitter\.com/" + post.Name + "/status/[0-9]+"">\.\.\.</a>$", "")
#End If
                        'ハート変換
                        orgData = orgData.Replace("&lt;3", "♡")
                    End If

                    'URL前処理（IDNデコードなど）
                    orgData = PreProcessUrl(orgData)

                    '短縮URL解決処理（orgData書き換え）
                    orgData = ShortUrlResolve(orgData)

                    '表示用にhtml整形
                    post.OriginalData = AdjustHtml(orgData)

                    '単純テキストの取り出し（リンクタグ除去）
                    Try
                        post.Data = GetPlainText(orgData)
                    Catch ex As Exception
                        _signed = False
                        TraceOut("TM-Link:" + strPost)
                        Return "GetTimeline -> Err: Can't parse links."
                    End Try

                    ' Imageタグ除去（ハロウィン）
                    Dim ImgTag As New Regex("<img src=.*?/>", RegexOptions.IgnoreCase)
                    If ImgTag.IsMatch(post.Data) Then post.Data = ImgTag.Replace(post.Data, "<img>")

                    'Get Date
#If 0 Then
                    Try
                        pos1 = strPost.IndexOf(_parseDate, pos2, StringComparison.Ordinal)
                        pos2 = strPost.IndexOf(_parseDateTo, pos1 + _parseDate.Length, StringComparison.Ordinal)
                        post.PDate = DateTime.ParseExact(strPost.Substring(pos1 + _parseDate.Length, pos2 - pos1 - _parseDate.Length), "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz", System.Globalization.DateTimeFormatInfo.InvariantInfo, Globalization.DateTimeStyles.None)
                    Catch ex As Exception
                        _signed = False
                        TraceOut("TM-Date:" + strPost)
                        Return "GetTimeline -> Err: Can't get date."
                    End Try
#Else
                    '取得できなくなったため暫定対応(2/26)
                    post.PDate = Now()
#End If


                    'from Sourceの取得
                    'ToDo: _parseSourceFromを正規表現へ。wedataからの取得へ変更（次版より）
                    Dim rg As New Regex("<span>.+>(?<name>.+)</a>.*</span> ")
                    Dim m As Match = rg.Match(strPost)
                    If m.Success Then
                        post.Source = m.Result("${name}")
                    Else
                        post.Source = "Web"
                    End If
                    'Try
                    '    pos1 = strPost.IndexOf(_parseSourceFrom, pos2, StringComparison.Ordinal)
                    '    If pos1 = -1 Then pos1 = strPost.IndexOf(_parseSourceFrom2, pos2, StringComparison.Ordinal)
                    '    If pos1 > -1 Then
                    '        pos1 = strPost.IndexOf(_parseSource2, pos1 + 19, StringComparison.Ordinal)
                    '        pos2 = strPost.IndexOf(_parseSourceTo, pos1 + 2, StringComparison.Ordinal)
                    '        post.Source = HttpUtility.HtmlDecode(strPost.Substring(pos1 + 2, pos2 - pos1 - 2))
                    '    Else
                    '        post.Source = "Web"
                    '    End If
                    'Catch ex As Exception
                    '    _signed = False
                    '    TraceOut("TM-Src:" + strPost)
                    '    Return "GetTimeline -> Err: Can't get src."
                    'End Try

                    'Get Reply(in_reply_to_user/id)
                    'ToDo: _isReplyEngを正規表現へ。wedataからの取得へ変更（次版より）
                    rg = New Regex("<a href=""https?:\/\/twitter\.com\/(?<name>[a-zA-Z0-9_]+)\/status\/(?<id>[0-9]+)"">(in reply to )*\k<name>")
                    m = rg.Match(strPost)
                    If m.Success Then
                        post.InReplyToUser = m.Result("${name}")
                        post.InReplyToId = Long.Parse(m.Result("${id}"))
                        post.IsReply = post.InReplyToUser.Equals(_uid, StringComparison.OrdinalIgnoreCase)
                    End If

                    '@先リスト作成
                    rg = New Regex("@<a href=""\/(?<1>[a-zA-Z0-9_]+)[^a-zA-Z0-9_]")
                    m = rg.Match(orgData)
                    While m.Success
                        post.ReplyToList.Add(m.Groups(1).Value.ToLower())
                        m = m.NextMatch
                    End While
                    If Not post.IsReply Then post.IsReply = post.ReplyToList.Contains(_uid)

                    If gType = WORKERTYPE.Reply Then post.IsReply = True

                    'Get Fav
                    pos1 = strPost.IndexOf(_parseStar, pos2, StringComparison.Ordinal)
                    If pos1 > -1 Then
                        Try
                            pos2 = strPost.IndexOf(_parseStarTo, pos1 + _parseStar.Length, StringComparison.Ordinal)
                            If strPost.Substring(pos1 + _parseStar.Length, pos2 - pos1 - _parseStar.Length) = _parseStarEmpty Then
                                post.IsFav = False
                            Else
                                post.IsFav = True
                            End If
                        Catch ex As Exception
                            _signed = False
                            TraceOut("TM-Fav:" + strPost)
                            Return "GetTimeline -> Err: Can't get fav status."
                        End Try
                    Else
                        post.IsFav = False
                    End If


                    If _endingFlag Then Return ""

                    post.IsMe = post.Name.Equals(_uid, StringComparison.OrdinalIgnoreCase)
                    SyncLock LockObj
                        If follower.Count > 1 Then
                            post.IsOwl = Not follower.Contains(post.Name.ToLower())
                        Else
                            post.IsOwl = False
                        End If
                    End SyncLock
                    post.IsRead = read

                    arIdx += 1
                    dlgt(arIdx) = New GetIconImageDelegate(AddressOf GetIconImage)
                    ar(arIdx) = dlgt(arIdx).BeginInvoke(post, Nothing, Nothing)

                End If

                'テスト実装：DMカウント取得
                If intCnt = posts.Length AndAlso gType = WORKERTYPE.Timeline AndAlso page = 1 Then
                    pos1 = strPost.IndexOf(_parseDMcountFrom, pos2, StringComparison.Ordinal)
                    If pos1 > -1 Then
                        Try
                            pos2 = strPost.IndexOf(_parseDMcountTo, pos1 + _parseDMcountFrom.Length, StringComparison.Ordinal)
                            Dim dmCnt As Integer = Integer.Parse(strPost.Substring(pos1 + _parseDMcountFrom.Length, pos2 - pos1 - _parseDMcountFrom.Length))
                            If dmCnt > _dmCount Then
                                _dmCount = dmCnt
                                _getDm = True
                            End If
                        Catch ex As Exception
                            Return "GetTimeline -> Err: Can't get DM count."
                        End Try
                    End If
                End If
                getDM = _getDm
            Next

            For i As Integer = 0 To arIdx
                Try
                    dlgt(i).EndInvoke(ar(i))
                Catch ex As Exception
                    '最後までendinvoke回す（ゾンビ化回避）
                    ex.Data("IsTerminatePermission") = False
                    Throw
                End Try
            Next

            SyncLock LockObj
                If page = 1 AndAlso (TabInformations.GetInstance.ItemCount - listCnt) >= _nextThreshold Then
                    '新着が閾値の件数以上なら、次のページも念のため読み込み
                    endPage = _nextPages + 1
                End If
            End SyncLock

            Return ""
        Finally
            GetTmSemaphore.Release()
        End Try
    End Function

    Public Function GetDirectMessage(ByVal page As Integer, _
                                    ByVal read As Boolean, _
                                    ByVal endPage As Integer, _
                                    ByVal gType As WORKERTYPE) As String
        If endPage = 0 Then
            '通常モード(DMはモード関係なし)
            endPage = 1
        End If
        '起動時モード 
        Dim num As Integer = endPage - page
        Dim ar(num) As IAsyncResult
        Dim dlgt(num) As GetDirectMessageDelegate

        For idx As Integer = 0 To num
            gType = WORKERTYPE.DirectMessegeRcv
            dlgt(idx) = New GetDirectMessageDelegate(AddressOf GetDirectMessageThread)
            GetTmSemaphore.WaitOne()
            ar(idx) = dlgt(idx).BeginInvoke(page + idx, read, endPage + idx, gType, Nothing, Nothing)
        Next
        Dim rslt As String = ""
        For idx As Integer = 0 To num
            Dim trslt As String = ""
            Try
                trslt = dlgt(idx).EndInvoke(ar(idx))
            Catch ex As Exception
                '最後までendinvoke回す（ゾンビ化回避）
                ex.Data("IsTerminatePermission") = False
                Throw
                rslt = "GetDirectMessageErr"
            End Try
            If trslt.Length > 0 AndAlso rslt.Length = 0 Then rslt = trslt
        Next
        For idx As Integer = 0 To num
            gType = WORKERTYPE.DirectMessegeSnt
            dlgt(idx) = New GetDirectMessageDelegate(AddressOf GetDirectMessageThread)
            GetTmSemaphore.WaitOne()
            ar(idx) = dlgt(idx).BeginInvoke(page + idx, read, endPage + idx, gType, Nothing, Nothing)
        Next
        For idx As Integer = 0 To num
            Dim trslt As String = ""
            Try
                trslt = dlgt(idx).EndInvoke(ar(idx))
            Catch ex As Exception
                '最後までendinvoke回す（ゾンビ化回避）
                ex.Data("IsTerminatePermission") = False
                Throw
                rslt = "GetDirectMessageErr"
            End Try
            If trslt.Length > 0 AndAlso rslt.Length = 0 Then rslt = trslt
        Next
        Return rslt
    End Function

    Private Function GetDirectMessageThread(ByVal page As Integer, _
                                    ByVal read As Boolean, _
                                    ByVal endPage As Integer, _
                                    ByVal gType As WORKERTYPE) As String
        Try
            If _endingFlag Then Return ""

            Dim retMsg As String = ""
            Dim resStatus As String = ""

            Static redirectToDmRcv As String = ""
            Static redirectToDmSnd As String = ""

            _getDm = False

            If _signed = False Then
                retMsg = SignIn()
                If retMsg.Length > 0 Then
                    Return retMsg
                End If
            End If

            If _endingFlag Then Return ""

            'リクエストメッセージを作成する
            Dim pageQuery As String = _pageQry + page.ToString
RETRY:
            If gType = WORKERTYPE.DirectMessegeRcv Then
                If redirectToDmRcv = "" Then
                    retMsg = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + _DMPathRcv + pageQuery, resStatus, MySocket.REQ_TYPE.ReqGetApp), String)
                    If resStatus.StartsWith("Found") Then
                        redirectToDmRcv = resStatus.Substring(6)
                        If redirectToDmRcv.Contains("?") Then
                            redirectToDmRcv = redirectToDmRcv.Remove(redirectToDmRcv.IndexOf("?"))
                        End If
                        GoTo RETRY
                    End If
                Else
                    retMsg = DirectCast(CreateSocket.GetWebResponse(redirectToDmRcv + pageQuery, resStatus, MySocket.REQ_TYPE.ReqGetApp), String)
                End If
            Else
                If redirectToDmSnd = "" Then
                    retMsg = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + _DMPathSnt + pageQuery, resStatus, MySocket.REQ_TYPE.ReqGetApp), String)
                    If resStatus.StartsWith("Found") Then
                        redirectToDmSnd = resStatus.Substring(6)
                        If redirectToDmSnd.Contains("?") Then
                            redirectToDmSnd = redirectToDmSnd.Remove(redirectToDmSnd.IndexOf("?"))
                        End If
                        GoTo RETRY
                    End If
                Else
                    retMsg = DirectCast(CreateSocket.GetWebResponse(redirectToDmSnd + pageQuery, resStatus, MySocket.REQ_TYPE.ReqGetApp), String)
                End If
            End If

            If retMsg.Length = 0 Then
                _signed = False
                Return resStatus
            End If

            ' tr 要素の class 属性を消去
            Do
                Dim idx As Integer = retMsg.IndexOf(_removeClass, StringComparison.Ordinal)
                If idx = -1 Then Exit Do
                Dim idx2 As Integer = retMsg.IndexOf("""", idx + _removeClass.Length, StringComparison.Ordinal) - idx + 1 - 3
                If idx2 > 0 Then retMsg = retMsg.Remove(idx + 3, idx2)
            Loop

            If _endingFlag Then Return ""

            ''AuthKeyの取得
            'If GetAuthKeyDM(retMsg) < 0 Then
            '    _signed = False
            '    Return "GetDirectMessage -> Err: Busy(1)"
            'End If

            Dim pos1 As Integer
            Dim pos2 As Integer

            '各メッセージに分割可能か？
            pos1 = retMsg.IndexOf(_splitDM, StringComparison.Ordinal)
            If pos1 = -1 Then
                '0件（メッセージなし。エラーの場合もありうるが判別できないので正常として戻す）
                Return ""
            End If

            Dim strSep() As String = {_splitDM}
            Dim posts() As String = retMsg.Split(strSep, StringSplitOptions.RemoveEmptyEntries)
            Dim intCnt As Integer = 0   'カウンタ
            Dim listCnt As Integer = 0
            SyncLock LockObj
                listCnt = TabInformations.GetInstance.ItemCount
            End SyncLock
            Dim dlgt(20) As GetIconImageDelegate
            Dim ar(20) As IAsyncResult
            Dim arIdx As Integer = -1

            For Each strPost As String In posts
                intCnt += 1

                If intCnt > 1 Then  '1件目はヘッダなので無視
                    'Dim lItem As New MyListItem
                    Dim post As New PostClass()

                    'Get ID
                    Try
                        pos1 = 0
                        pos2 = strPost.IndexOf("""", 0, StringComparison.Ordinal)
                        post.Id = Long.Parse(HttpUtility.HtmlDecode(strPost.Substring(0, pos2)))
                    Catch ex As Exception
                        _signed = False
                        TraceOut("DM-ID:" + strPost)
                        Return "GetDirectMessage -> Err: Can't get ID"
                    End Try

                    'Get Name
                    Try
                        pos1 = strPost.IndexOf(_parseName, pos2, StringComparison.Ordinal)
                        pos2 = strPost.IndexOf(_parseNameTo, pos1, StringComparison.Ordinal)
                        post.Name = HttpUtility.HtmlDecode(strPost.Substring(pos1 + _parseName.Length, pos2 - pos1 - _parseName.Length))
                    Catch ex As Exception
                        _signed = False
                        TraceOut("DM-Name:" + strPost)
                        Return "GetDirectMessage -> Err: Can't get Name"
                    End Try

                    'Get Nick
                    Try
                        pos1 = strPost.IndexOf(_parseNick, pos2, StringComparison.Ordinal)
                        pos2 = strPost.IndexOf(_parseNickTo, pos1 + _parseNick.Length, StringComparison.Ordinal)
                        post.Nickname = HttpUtility.HtmlDecode(strPost.Substring(pos1 + _parseNick.Length, pos2 - pos1 - _parseNick.Length))
                    Catch ex As Exception
                        _signed = False
                        TraceOut("DM-Nick:" + strPost)
                        Return "GetDirectMessage -> Err: Can't get Nick."
                    End Try

                    SyncLock LockObj
                        If TabInformations.GetInstance.ContainsKey(post.Id) Then Continue For
                    End SyncLock

                    'Get ImagePath
                    Try
                        pos1 = strPost.IndexOf(_parseImg, pos2, StringComparison.Ordinal)
                        pos2 = strPost.IndexOf(_parseImgTo, pos1 + _parseImg.Length, StringComparison.Ordinal)
                        post.ImageUrl = HttpUtility.HtmlDecode(strPost.Substring(pos1 + _parseImg.Length, pos2 - pos1 - _parseImg.Length))
                    Catch ex As Exception
                        _signed = False
                        TraceOut("DM-Img:" + strPost)
                        Return "GetDirectMessage -> Err: Can't get ImagePath"
                    End Try

                    'Get Protect 
                    Try
                        pos1 = strPost.IndexOf(_isProtect, pos2, StringComparison.Ordinal)
                        If pos1 > -1 Then post.IsProtect = True
                    Catch ex As Exception
                        _signed = False
                        TraceOut("DM-Protect:" + strPost)
                        Return "GetDirectMessage -> Err: Can't get Protect"
                    End Try

                    Dim orgData As String = ""

                    'Get Message
                    Try
                        pos1 = strPost.IndexOf(_parseDM1, pos2, StringComparison.Ordinal)
                        If pos1 > -1 Then
                            pos2 = strPost.IndexOf(_parseDM2, pos1, StringComparison.Ordinal)
                            orgData = strPost.Substring(pos1 + _parseDM1.Length, pos2 - pos1 - _parseDM1.Length).Trim()
                        Else
                            pos1 = strPost.IndexOf(_parseDM11, pos2, StringComparison.Ordinal)
                            pos2 = strPost.IndexOf(_parseDM2, pos1, StringComparison.Ordinal)
                            orgData = strPost.Substring(pos1 + _parseDM11.Length, pos2 - pos1 - _parseDM11.Length).Trim()
                        End If
#If 0 Then
                        orgData = Regex.Replace(orgData, "<a href=""https://twitter\.com/" + post.Name + "/status/[0-9]+"">\.\.\.</a>$", "")
#End If
                        orgData = orgData.Replace("&lt;3", "♡")
                    Catch ex As Exception
                        _signed = False
                        TraceOut("DM-Body:" + strPost)
                        Return "GetDirectMessage -> Err: Can't get body"
                    End Try

                    'URL前処理（IDNデコードなど）
                    orgData = PreProcessUrl(orgData)

                    '短縮URL解決処理（orgData書き換え）
                    orgData = ShortUrlResolve(orgData)

                    '表示用にhtml整形
                    post.OriginalData = AdjustHtml(orgData)

                    '単純テキストの取り出し（リンクタグ除去）
                    Try
                        post.Data = GetPlainText(orgData)
                    Catch ex As Exception
                        _signed = False
                        TraceOut("DM-Link:" + strPost)
                        Return "GetDirectMessage -> Err: Can't parse links"
                    End Try

#If 0 Then
                'Get Date
                Try
                    pos1 = strPost.IndexOf(_parseDate, pos2, StringComparison.Ordinal)
                    pos2 = strPost.IndexOf(_parseDateTo, pos1 + _parseDate.Length, StringComparison.Ordinal)
                    post.PDate = DateTime.ParseExact(strPost.Substring(pos1 + _parseDate.Length, pos2 - pos1 - _parseDate.Length), "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz", System.Globalization.DateTimeFormatInfo.InvariantInfo, Globalization.DateTimeStyles.None)
                Catch ex As Exception
                    _signed = False
                    TraceOut("DM-Date:" + strPost)
                    Return "GetDirectMessage -> Err: Can't get date."
                End Try
#Else
                    '取得できなくなったため暫定対応(2/26)
                    post.PDate = Now()
#End If

                    'Get Fav
                    'pos1 = strPost.IndexOf(_parseStar, pos2)
                    'pos2 = strPost.IndexOf("""", pos1 + _parseStar.Length)
                    'If strPost.Substring(pos1 + _parseStar.Length, pos2 - pos1 - _parseStar.Length) = "empty" Then
                    '    lItem.Fav = False
                    'Else
                    '    lItem.Fav = True
                    'End If
                    post.IsFav = False


                    If _endingFlag Then Return ""

                    '受信ＤＭかの判定で使用
                    If gType = WORKERTYPE.DirectMessegeRcv Then
                        post.IsOwl = False
                    Else
                        post.IsOwl = True
                    End If

                    post.IsRead = read
                    post.IsDm = True

                    'Imageの取得
                    arIdx += 1
                    dlgt(arIdx) = New GetIconImageDelegate(AddressOf GetIconImage)
                    ar(arIdx) = dlgt(arIdx).BeginInvoke(post, Nothing, Nothing)
                End If
            Next

            For i As Integer = 0 To arIdx
                Try
                    dlgt(i).EndInvoke(ar(i))
                Catch ex As Exception
                    ex.Data("IsTerminatePermission") = False
                    Throw
                End Try
            Next

            Return ""

        Finally
            GetTmSemaphore.Release()
        End Try
    End Function

    Private Function PreProcessUrl(ByVal orgData As String) As String
        Dim posl1 As Integer
        Dim posl2 As Integer = 0
        Dim IDNConveter As IdnMapping = New IdnMapping()
        Dim href As String = "<a href="""

        Do While True
            If orgData.IndexOf(href, posl2, StringComparison.Ordinal) > -1 Then
                Dim urlStr As String = ""
                ' IDN展開
                posl1 = orgData.IndexOf(href, posl2, StringComparison.Ordinal)
                posl1 += href.Length
                posl2 = orgData.IndexOf("""", posl1, StringComparison.Ordinal)
                urlStr = orgData.Substring(posl1, posl2 - posl1)

                If Not urlStr.StartsWith("http://") AndAlso Not urlStr.StartsWith("https://") AndAlso Not urlStr.StartsWith("ftp://") Then
                    Continue Do
                End If

                Dim replacedUrl As String = IDNDecode(urlStr)
                If replacedUrl Is Nothing Then Continue Do
                If replacedUrl = urlStr Then Continue Do

                orgData = orgData.Replace("<a href=""" + urlStr, "<a href=""" + replacedUrl)
                posl2 = 0
            Else
                Exit Do
            End If
        Loop
        Return orgData
    End Function

    Private Function ShortUrlResolve(ByVal orgData As String) As String
        If _tinyUrlResolve Then
            For Each svc As String In _ShortUrlService
                Dim posl1 As Integer
                Dim posl2 As Integer = 0

                Do While True
                    If orgData.IndexOf("<a href=""" + svc, posl2, StringComparison.Ordinal) > -1 Then
                        Dim urlStr As String = ""
                        Try
                            posl1 = orgData.IndexOf("<a href=""" + svc, posl2, StringComparison.Ordinal)
                            posl1 = orgData.IndexOf(svc, posl1, StringComparison.Ordinal)
                            posl2 = orgData.IndexOf("""", posl1, StringComparison.Ordinal)
                            urlStr = orgData.Substring(posl1, posl2 - posl1)
                            Dim Response As String = ""
                            Dim retUrlStr As String = ""
                            Dim tmpurlStr As String = urlStr
                            Dim SchemeAndDomain As Regex = New Regex("http://.+?/+?")
                            Dim tmpSchemeAndDomain As String = ""
                            For i As Integer = 0 To 4   'とりあえず5回試す
                                retUrlStr = DirectCast(CreateSocket.GetWebResponse(tmpurlStr, Response, MySocket.REQ_TYPE.ReqGETForwardTo), String)
                                If retUrlStr.Length > 0 Then
                                    ' 転送先URLが返された (まだ転送されるかもしれないので返値を引数にしてもう一度)
                                    ' 取得試行回数オーバーの場合は取得結果を転送先とする
                                    Dim scd As Match = SchemeAndDomain.Match(retUrlStr)
                                    If scd.Success AndAlso scd.Value <> svc Then
                                        svc = scd.Value()
                                    End If
                                    tmpurlStr = retUrlStr
                                    Continue For
                                Else
                                    ' 転送先URLが返されなかった
                                    If tmpurlStr <> urlStr Then
                                        '少なくとも一度以上転送されている (前回の結果を転送先とする)
                                        retUrlStr = tmpurlStr
                                    Else
                                        ' 一度も転送されていない
                                        retUrlStr = ""
                                    End If
                                    Exit For
                                End If
                            Next
                            If retUrlStr.Length > 0 Then
                                If Not retUrlStr.StartsWith("http") Then
                                    If retUrlStr.StartsWith("/") Then
                                        retUrlStr = svc + retUrlStr.Substring(1)
                                    Else
                                        retUrlStr = retUrlStr.Insert(0, svc)
                                    End If
                                End If
                                orgData = orgData.Replace("<a href=""" + urlStr, "<a href=""" + urlEncodeMultibyteChar(retUrlStr))
                                posl2 = 0   '置換した場合は頭から再探索（複数同時置換での例外対応）
                            End If
                        Catch ex As Exception
                            '_signed = False
                            'Return "GetTimeline -> Err: Can't get tinyurl."
                        End Try
                    Else
                        Exit Do
                    End If
                Loop
            Next
        End If
        Return orgData
    End Function

    Private Function GetPlainText(ByVal orgData As String) As String
        Dim retStr As String

        '単純テキストの取り出し（リンクタグ除去）
        If orgData.IndexOf(_parseLink1, StringComparison.Ordinal) = -1 Then
            retStr = HttpUtility.HtmlDecode(orgData)
        Else
            Dim posl1 As Integer
            Dim posl2 As Integer
            Dim posl3 As Integer = 0

            retStr = ""

            posl3 = 0
            Do While True
                posl1 = orgData.IndexOf(_parseLink1, posl3, StringComparison.Ordinal)
                If posl1 = -1 Then Exit Do

                If (posl3 + _parseLink3.Length <> posl1) Or posl3 = 0 Then
                    If posl3 <> 0 Then
                        retStr += HttpUtility.HtmlDecode(orgData.Substring(posl3 + _parseLink3.Length, posl1 - posl3 - _parseLink3.Length))
                    Else
                        retStr += HttpUtility.HtmlDecode(orgData.Substring(0, posl1))
                    End If
                End If
                posl2 = orgData.IndexOf(_parseLink2, posl1, StringComparison.Ordinal)
                posl3 = orgData.IndexOf(_parseLink3, posl2, StringComparison.Ordinal)
                retStr += HttpUtility.HtmlDecode(orgData.Substring(posl2 + _parseLink2.Length, posl3 - posl2 - _parseLink2.Length))
            Loop
            retStr += HttpUtility.HtmlDecode(orgData.Substring(posl3 + _parseLink3.Length))
        End If

        Return retStr
    End Function

    ' htmlの簡易サニタイズ(詳細表示に不要なタグの除去)

    Private Function SanitizeHtml(ByVal orgdata As String) As String
        Dim retdata As String = orgdata

        '  <script ～ </script>
        Dim rx As Regex = New Regex( _
            "<(script|object|applet|image|frameset|fieldset|legend|style).*" & _
            "</(script|object|applet|image|frameset|fieldset|legend|style)>", RegexOptions.IgnoreCase)
        retdata = rx.Replace(retdata, "")

        ' <frame src="...">
        rx = New Regex("<(frame|link|iframe|img)>", RegexOptions.IgnoreCase)
        retdata = rx.Replace(retdata, "")

        Return retdata
    End Function

    Private Function AdjustHtml(ByVal orgData As String) As String
        Dim retStr As String = orgData
        retStr = retStr.Replace("<a href=""/", "<a href=""https://twitter.com/")
        retStr = retStr.Replace("<a href=", "<a target=""_self"" href=")
        retStr = retStr.Replace(vbLf, "<br>")

        Return SanitizeHtml(retStr)
    End Function

    Private Sub GetIconImage(ByVal post As PostClass)
        If Not _getIcon Then
            post.ImageIndex = -1
            TabInformations.GetInstance.AddPost(post)
            Exit Sub
        End If

        SyncLock LockObj
            post.ImageIndex = _lIcon.Images.IndexOfKey(post.ImageUrl)
        End SyncLock

        If post.ImageIndex > -1 Then
            TabInformations.GetInstance.AddPost(post)
            Exit Sub
        End If

        Dim resStatus As String = ""
        Dim img As Image = DirectCast(CreateSocket.GetWebResponse(post.ImageUrl, resStatus, MySocket.REQ_TYPE.ReqGETBinary), System.Drawing.Image)
        If img Is Nothing Then
            post.ImageIndex = -1
            TabInformations.GetInstance.AddPost(post)
            Exit Sub
        End If

        If _endingFlag Then Exit Sub

        Dim bmp2 As New Bitmap(_iconSz, _iconSz)
        Using g As Graphics = Graphics.FromImage(bmp2)
            g.InterpolationMode = Drawing2D.InterpolationMode.High
            g.DrawImage(img, 0, 0, _iconSz, _iconSz)
        End Using

        SyncLock LockObj
            post.ImageIndex = _lIcon.Images.IndexOfKey(post.ImageUrl)
            If post.ImageIndex = -1 Then
                _dIcon.Add(post.ImageUrl, img)  '詳細表示用ディクショナリに追加
                _lIcon.Images.Add(post.ImageUrl, bmp2)
                post.ImageIndex = _lIcon.Images.IndexOfKey(post.ImageUrl)
            End If
        End SyncLock
        TabInformations.GetInstance.AddPost(post)
    End Sub

    Private Function GetAuthKey(ByVal resMsg As String) As Integer
        Dim pos1 As Integer
        Dim pos2 As Integer

        pos1 = resMsg.IndexOf(_getAuthKey, StringComparison.Ordinal)
        If pos1 < 0 Then
            'データ不正？
            Return -7
        End If
        pos2 = resMsg.IndexOf(_getAuthKeyTo, pos1 + _getAuthKey.Length, StringComparison.Ordinal)
        If pos2 > -1 Then
            _authKey = resMsg.Substring(pos1 + _getAuthKey.Length, pos2 - pos1 - _getAuthKey.Length)
        Else
            Return -7
        End If

        Return 0
    End Function

    Private Function GetAuthKeyDM(ByVal resMsg As String) As Integer
        Dim pos1 As Integer
        Dim pos2 As Integer

        pos1 = resMsg.IndexOf(_getAuthKey, StringComparison.Ordinal)
        If pos1 < 0 Then
            'データ不正？
            Return -7
        End If
        pos2 = resMsg.IndexOf("""", pos1 + _getAuthKey.Length, StringComparison.Ordinal)
        _authKeyDM = resMsg.Substring(pos1 + _getAuthKey.Length, pos2 - pos1 - _getAuthKey.Length)

        Return 0
    End Function

    Private Structure PostInfo
        Public CreatedAt As String
        Public Id As String
        Public Text As String
        Public UserId As String
        Public Sub New(ByVal Created As String, ByVal IdStr As String, ByVal txt As String, ByVal uid As String)
            CreatedAt = Created
            Id = IdStr
            Text = txt
            UserId = uid
        End Sub
        Public Shadows Function Equals(ByVal dst As PostInfo) As Boolean
            If Me.CreatedAt = dst.CreatedAt AndAlso Me.Id = dst.Id AndAlso Me.Text = dst.Text AndAlso Me.UserId = dst.UserId Then
                Return True
            Else
                Return False
            End If
        End Function
    End Structure

    Private Function IsPostRestricted(ByRef resMsg As String) As Boolean
        Static _prev As New PostInfo("", "", "", "")
        Dim _current As New PostInfo("", "", "", "")


        Dim xd As XmlDocument = New XmlDocument()
        Try
            xd.LoadXml(resMsg)
            _current.CreatedAt = xd.SelectSingleNode("/status/created_at/text()").Value
            _current.Id = xd.SelectSingleNode("/status/id/text()").Value
            _current.Text = xd.SelectSingleNode("/status/text/text()").Value
            _current.UserId = xd.SelectSingleNode("/status/user/id/text()").Value

            If _current.Equals(_prev) Then
                Return True
            End If
            _prev.CreatedAt = _current.CreatedAt
            _prev.Id = _current.Id
            _prev.Text = _current.Text
            _prev.UserId = _current.UserId
        Catch ex As XmlException
            Return False
        End Try

        Return False
    End Function

    Public Function PostStatus(ByVal postStr As String, ByVal reply_to As Long) As String

        If _endingFlag Then Return ""

        postStr = postStr.Trim()

        'データ部分の生成
        Dim dataStr As String
        If reply_to = 0 Then
            dataStr = _statusHeader + HttpUtility.UrlEncode(postStr) + "&source=Tween"
        Else
            dataStr = _statusHeader + HttpUtility.UrlEncode(postStr) + "&source=Tween" + "&in_reply_to_status_id=" + HttpUtility.UrlEncode(reply_to.ToString)
        End If

        Dim resStatus As String = ""
        Dim resMsg As String = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + _statusUpdatePathAPI, resStatus, MySocket.REQ_TYPE.ReqPOSTAPI, dataStr), String)

        If resStatus.StartsWith("OK") Then
            If Not postStr.StartsWith("D ", StringComparison.OrdinalIgnoreCase) AndAlso _
                    Not postStr.StartsWith("DM ", StringComparison.OrdinalIgnoreCase) AndAlso _
                    IsPostRestricted(resMsg) Then
                Return "Err:POST規制？"
            End If
            resStatus = Outputz.Post(CreateSocket, postStr.Length)
            If resStatus.Length > 0 Then
                Return "Outputz:" + resStatus
            Else
                Return ""
            End If
        Else
            Return resStatus
        End If
    End Function

    Public Function RemoveStatus(ByVal id As Long) As String
        If _endingFlag Then Return ""

        'データ部分の生成
        Dim resStatus As String = ""
        Dim resMsg As String = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + _StDestroyPath + id.ToString + ".xml", resStatus, MySocket.REQ_TYPE.ReqPOSTAPI), String)

        If resMsg.StartsWith("<?xml") = False OrElse resStatus.StartsWith("OK") = False Then
            Return resStatus
        End If

        Return ""
    End Function

    Public Function RemoveDirectMessage(ByVal id As Long) As String
        If _endingFlag Then Return ""

        'データ部分の生成
        Dim dataStr As String = _authKeyHeader + HttpUtility.UrlEncode(_authKey)
        Dim resStatus As String = ""
        Dim resMsg As String = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + _DMDestroyPath + id.ToString + ".xml", resStatus, MySocket.REQ_TYPE.ReqPOSTAPI), String)

        If resMsg.StartsWith("<?xml") = False OrElse resStatus.StartsWith("OK") = False Then
            Return resStatus
        End If

        Return ""
    End Function

    ' Contributed by shuyoko <http://twitter.com/shuyoko> BEGIN:
    Public Function GetBlackFavId(ByVal id As Long, ByRef blackid As Long) As String
        Dim dataStr As String = _authKeyHeader + HttpUtility.UrlEncode(_authKey)
        Dim resStatus As String = ""
        Dim resMsg As String = DirectCast(CreateSocket.GetWebResponse("http://blavotter.hocha.org/blackfav/getblack.php?format=simple&id=" + id.ToString(), resStatus, MySocket.REQ_TYPE.ReqGET), String)

        If resStatus.StartsWith("OK") = False Then
            Return resStatus
        End If

        blackid = Long.Parse(resMsg)

        Return ""

    End Function
    ' Contributed by shuyoko <http://twitter.com/shuyoko> END.

    Public Function PostFavAdd(ByVal id As Long) As String
        If _endingFlag Then Return ""

        'データ部分の生成
        'Dim dataStr As String = _authKeyHeader + HttpUtility.UrlEncode(_authKey)
        Dim resStatus As String = ""
        Dim resMsg As String = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + _postFavAddPath + id.ToString() + ".xml", resStatus, MySocket.REQ_TYPE.ReqPOSTAPI), String)

        If resStatus.StartsWith("OK") = False Then
            Return resStatus
        End If

        If _restrictFavCheck = False Then Return ""

        'http://twitter.com/statuses/show/id.xml APIを発行して本文を取得

        resMsg = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + _ShowStatus + id.ToString() + ".xml", resStatus, MySocket.REQ_TYPE.ReqGetAPI), String)

        Try
            Using rd As Xml.XmlTextReader = New Xml.XmlTextReader(New System.IO.StringReader(resMsg))
                rd.Read()
                While rd.EOF = False
                    If rd.IsStartElement("favorited") Then
                        If rd.ReadElementContentAsBoolean() = True Then
                            Return ""  '正常にふぁぼれている
                        Else
                            Return "NG(Restricted?)"  '正常応答なのにふぁぼれてないので制限っぽい
                        End If
                    Else
                        rd.Read()
                    End If
                End While
                rd.Close()
            End Using
        Catch ex As XmlException
            '
        End Try

        Return ""
    End Function

    Public Function PostFavRemove(ByVal id As Long) As String
        If _endingFlag Then Return ""

        'データ部分の生成
        'Dim dataStr As String = _authKeyHeader + HttpUtility.UrlEncode(_authKey)
        Dim resStatus As String = ""
        Dim resMsg As String = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + _postFavRemovePath + id.ToString() + ".xml", resStatus, MySocket.REQ_TYPE.ReqPOSTAPI), String)

        If resStatus.StartsWith("OK") = False Then
            Return resStatus
        End If

        Return ""
    End Function

#Region "follower取得"
    Delegate Function GetFollowersDelegate(ByVal Query As Integer) As String
    Private semaphore As Threading.Semaphore = Nothing
    Private threadNum As Integer = 0
    Private _threadErr As Boolean = False

    Private Function GetFollowersMethod(ByVal Query As Integer) As String
        Dim resStatus As String = ""
        Dim resMsg As String = ""

        Try
            resMsg = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + _GetFollowers + _pageQry + Query.ToString, resStatus, MySocket.REQ_TYPE.ReqGetAPI), String)
            If resStatus.StartsWith("OK") = False Then
                _threadErr = True
                Return resStatus
            End If
            Using rd As Xml.XmlTextReader = New Xml.XmlTextReader(New System.IO.StringReader(resMsg))
                Dim lc As Integer = 0
                rd.Read()
                While rd.EOF = False
                    If rd.IsStartElement("screen_name") Then
                        Dim tmp As String = rd.ReadElementString("screen_name").ToLower()
                        SyncLock LockObj
                            If Not tmpFollower.Contains(tmp) Then
                                tmpFollower.Add(tmp)
                            End If
                        End SyncLock
                        lc += 1
                    Else
                        rd.Read()
                    End If
                End While
            End Using
        Catch ex As XmlException
            _threadErr = True
            TraceOut("NG(XmlException)")
            Return "NG(XmlException)"
        End Try

        Return ""
    End Function

    Private Sub GetFollowersCallback(ByVal ar As IAsyncResult)
        Dim dlgt As GetFollowersDelegate = DirectCast(ar.AsyncState, GetFollowersDelegate)

        Try
            Dim ret As String = dlgt.EndInvoke(ar)
            If Not ret.Equals("") AndAlso Not _threadErr Then
                TraceOut(ret)
                _threadErr = True
            End If
        Catch ex As Exception
            _threadErr = True
            ex.Data("IsTerminatePermission") = False
            Throw
        Finally
            semaphore.Release()                     ' セマフォから出る
            Interlocked.Decrement(threadNum)        ' スレッド数カウンタを-1
        End Try

    End Sub

    ' キャッシュの検証と読み込み　-1を渡した場合は読み込みのみ行う（APIエラーでFollowersCountが取得できなかったとき）
    Private Function ValidateCache(ByVal _FollowersCount As Integer) As Integer
        Dim CacheFileName As String = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "FollowersCache")

        If Not File.Exists(CacheFileName) Then
            ' 存在しない場合はそのまま帰る
            Return _FollowersCount
        End If

        Dim serializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(tmpFollower.GetType())

        Try
            Using fs As New IO.FileStream(CacheFileName, FileMode.Open)
                tmpFollower = CType(serializer.Deserialize(fs), Specialized.StringCollection)
                If Not tmpFollower(0).Equals(_uid.ToLower()) Then
                    ' 別IDの場合はキャッシュ破棄して読み直し
                    tmpFollower.Clear()
                    tmpFollower.Add(_uid.ToLower())
                    Return _FollowersCount
                End If
            End Using
        Catch ex As XmlException
            ' 不正なxmlの場合は読み直し
            tmpFollower.Clear()
            tmpFollower.Add(_uid.ToLower())
            Return _FollowersCount
        End Try

        If _FollowersCount = -1 Then Return tmpFollower.Count

        If (_FollowersCount + 1) = tmpFollower.Count Then
            '変動がないので読み込みの必要なし
            Return 0
        ElseIf (_FollowersCount + 1) < tmpFollower.Count Then
            '減っている場合はどこが抜けているのかわからないので全部破棄して読み直し
            tmpFollower.Clear()
            tmpFollower.Add(_uid.ToLower())
            Return _FollowersCount
        End If

        ' 増えた場合は差分だけ読む

        Return _FollowersCount - tmpFollower.Count

    End Function

    Private Sub UpdateCache()
        Dim CacheFileName As String = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "FollowersCache")

        Dim serializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(follower.GetType())

        Using fs As New IO.FileStream(CacheFileName, FileMode.Create)
            serializer.Serialize(fs, follower)
        End Using

    End Sub

    Public Function GetFollowers(ByVal CacheInvalidate As Boolean) As String
#If DEBUG Then
        Dim sw As New System.Diagnostics.Stopwatch
        sw.Start()
#End If
        Dim resStatus As String = ""
        Dim resMsg As String = ""
        Dim i As Integer = 0
        Dim DelegateInstance As GetFollowersDelegate = New GetFollowersDelegate(AddressOf GetFollowersMethod)
        Dim threadMax As Integer = 4            ' 最大スレッド数
        Dim followersCount As Integer = 0

        Interlocked.Exchange(threadNum, 0)      ' スレッド数カウンタ初期化
        _threadErr = False
        follower.Clear()
        tmpFollower.Clear()
        follower.Add(_uid.ToLower())
        tmpFollower.Add(_uid.ToLower())

        resMsg = DirectCast(CreateSocket.GetWebResponse("https://twitter.com/users/show/" + _uid + ".xml", resStatus, MySocket.REQ_TYPE.ReqGetAPI), String)
        Dim xd As XmlDocument = New XmlDocument()
        Try
            xd.LoadXml(resMsg)
            followersCount = Integer.Parse(xd.SelectSingleNode("/user/followers_count/text()").Value)
        Catch ex As XmlException
            If CacheInvalidate OrElse ValidateCache(-1) < 0 Then
                ' FollowersカウントがAPIで取得できず、なおかつキャッシュから読めなかった
                SyncLock LockObj
                    follower.Clear()
                    follower.Add(_uid.ToLower())
                End SyncLock
                Return "Can't get followers_count and invalid cache."
            Else
                'キャッシュを読み出せたのでキャッシュを使う
                SyncLock LockObj
                    follower = tmpFollower
                End SyncLock
                Return ""
            End If
        End Try

        Dim tmp As Integer

        If CacheInvalidate Then
            tmp = followersCount
        Else
            tmp = ValidateCache(followersCount)
        End If


        If tmp <> 0 Then
            i = (tmp + 100) \ 100 - 1 ' Followersカウント取得しページ単位に切り上げる
        Else
            ' キャッシュの件数に変化がなかった
#If DEBUG Then
            sw.Stop()
            Console.WriteLine(sw.ElapsedMilliseconds)
#End If
            SyncLock LockObj
                follower = tmpFollower
            End SyncLock
            Return ""
        End If


        semaphore = New System.Threading.Semaphore(threadMax, threadMax) 'スレッド最大数

        For cnt As Integer = 0 To i
            If _endingFlag Then Exit For
            semaphore.WaitOne()                     'セマフォ取得 threadMax以上ならここでブロックされる
            Interlocked.Increment(threadNum)        'スレッド数カウンタを+1
            DelegateInstance.BeginInvoke(cnt + 1, New System.AsyncCallback(AddressOf GetFollowersCallback), DelegateInstance)
        Next

        '全てのスレッドの終了を待つ(スレッド数カウンタが0になるまで待機)
        Do
            Thread.Sleep(50)
        Loop Until Interlocked.Add(threadNum, 0) = 0

        semaphore.Close()

        If _endingFlag Then Return ""

        ' エラーが発生しているならFollowersリストクリア

        If _threadErr Then
            ' エラーが発生しているならFollowersリストクリア
            SyncLock LockObj
                follower.Clear()
                follower.Add(_uid.ToLower())
            End SyncLock
            Return "Can't get followers."
        End If

        follower = tmpFollower
        If Not _endingFlag Then UpdateCache()

#If DEBUG Then
        sw.Stop()
        Console.WriteLine(sw.ElapsedMilliseconds)
#End If

        Return ""
    End Function

#End Region

    Public Sub RefreshOwl()
        If follower.Count > 1 Then TabInformations.GetInstance.RefreshOwl(follower)
    End Sub

    Public Property Username() As String
        Get
            Return _uid
        End Get
        Set(ByVal value As String)
            _uid = value.ToLower
            _signed = False
        End Set
    End Property

    Public Property Password() As String
        Get
            Return _pwd
        End Get
        Set(ByVal value As String)
            _pwd = value
            _signed = False
        End Set
    End Property

    Public Property NextThreshold() As Integer
        Get
            Return _nextThreshold
        End Get
        Set(ByVal value As Integer)
            _nextThreshold = value
        End Set
    End Property

    Public Property NextPages() As Integer
        Get
            Return _nextPages
        End Get
        Set(ByVal value As Integer)
            _nextPages = value
        End Set
    End Property

    Public ReadOnly Property InfoTwitter() As String
        Get
            Return _infoTwitter
        End Get
    End Property

    Public Property UseAPI() As Boolean
        Get
            Return _useAPI
        End Get
        Set(ByVal value As Boolean)
            _useAPI = value
        End Set
    End Property

    Public Property HubServer() As String
        Get
            Return _hubServer
        End Get
        Set(ByVal value As String)
            _hubServer = value
        End Set
    End Property

    Public Sub GetWedata()
        Dim resStatus As String = ""
        Dim resMsg As String = ""

        resMsg = DirectCast(CreateSocket.GetWebResponse(wedataUrl, resStatus, timeOut:=10 * 1000), String) 'タイムアウト時間を10秒に設定
        If resMsg.Length = 0 Then Exit Sub

        Dim rs As New System.IO.StringReader(resMsg)

        Dim mode As Integer = 0 '0:search name 1:search data 2:read data
        Dim name As String = ""

        'ストリームの末端まで繰り返す
        Dim ln As String
        While rs.Peek() > -1
            ln = rs.ReadLine

            Select Case mode
                Case 0
                    If ln.StartsWith("    ""name"": ") Then
                        name = ln.Substring(13, ln.Length - 2 - 13)
                        mode += 1
                    End If
                Case 1
                    If ln = "    ""data"": {" Then
                        mode += 1
                    End If
                Case 2
                    If ln = "    }," Then
                        mode = 0
                    Else
                        If ln.EndsWith(",") Then ln = ln.Substring(0, ln.Length - 1)
                        Select Case name
                            Case "SplitPostReply"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _splitPost = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                            Case "SplitPostRecent"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _splitPostRecent = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                            Case "StatusID"
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _statusIdTo = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                            Case "IsProtect"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _isProtect = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                            Case "IsReply"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _isReplyEng = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagfrom2"": """) Then
                                    _isReplyJpn = ln.Substring(19, ln.Length - 1 - 19).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _isReplyTo = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                            Case "GetStar"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _parseStar = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagfrom2"": """) Then
                                    _parseStarEmpty = ln.Substring(19, ln.Length - 1 - 19).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _parseStarTo = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                            Case "Follower"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _followerList = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagfrom2"": """) Then
                                    _followerMbr1 = ln.Substring(19, ln.Length - 1 - 19).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagfrom3"": """) Then
                                    _followerMbr2 = ln.Substring(19, ln.Length - 1 - 19).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _followerMbr3 = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                            Case "SplitDM"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _splitDM = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                            Case "GetMsgDM"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _parseDM1 = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagfrom2"": """) Then
                                    _parseDM11 = ln.Substring(19, ln.Length - 1 - 19).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _parseDM2 = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                            Case "GetDate"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _parseDate = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _parseDateTo = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                            Case "GetMsg"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _parseMsg1 = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _parseMsg2 = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                            Case "GetImagePath"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _parseImg = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _parseImgTo = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                            Case "GetNick"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _parseNick = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _parseNickTo = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                            Case "GetName"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _parseName = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _parseNameTo = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                                'Case "GetSiv"
                                '    If ln.StartsWith("      ""tagfrom"": """) Then
                                '        _getSiv = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                '    End If
                                '    If ln.StartsWith("      ""tagto"": """) Then
                                '        _getSivTo = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                '    End If
                            Case "AuthKey"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _getAuthKey = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _getAuthKeyTo = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                            Case "InfoTwitter"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _getInfoTwitter = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _getInfoTwitterTo = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                            Case "GetProtectMsg"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _parseProtectMsg1 = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _parseProtectMsg2 = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                            Case "GetDMCount"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _parseDMcountFrom = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _parseDMcountTo = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                            Case "GetSource"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _parseSourceFrom = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagfrom2"": """) Then
                                    _parseSource2 = ln.Substring(19, ln.Length - 1 - 19).Replace("\", "")
                                End If
                                If ln.StartsWith("      ""tagto"": """) Then
                                    _parseSource2 = ln.Substring(16, ln.Length - 1 - 16).Replace("\", "")
                                End If
                            Case "RemoveClass"
                                If ln.StartsWith("      ""tagfrom"": """) Then
                                    _removeClass = ln.Substring(18, ln.Length - 1 - 18).Replace("\", "")
                                End If
                        End Select
                    End If
            End Select
        End While

        rs.Close()

#If DEBUG Then
        GenerateAnalyzeKey()
#End If
    End Sub

    Public WriteOnly Property GetIcon() As Boolean
        Set(ByVal value As Boolean)
            _getIcon = value
        End Set
    End Property

    Public WriteOnly Property TinyUrlResolve() As Boolean
        Set(ByVal value As Boolean)
            _tinyUrlResolve = value
        End Set
    End Property

    Public WriteOnly Property ProxyType() As ProxyTypeEnum
        Set(ByVal value As ProxyTypeEnum)
            _proxyType = value
        End Set
    End Property

    Public WriteOnly Property ProxyAddress() As String
        Set(ByVal value As String)
            _proxyAddress = value
        End Set
    End Property

    Public WriteOnly Property ProxyPort() As Integer
        Set(ByVal value As Integer)
            _proxyPort = value
        End Set
    End Property

    Public WriteOnly Property ProxyUser() As String
        Set(ByVal value As String)
            _proxyUser = value
        End Set
    End Property

    Public WriteOnly Property ProxyPassword() As String
        Set(ByVal value As String)
            _proxyPassword = value
        End Set
    End Property

    Public WriteOnly Property RestrictFavCheck() As Boolean
        Set(ByVal value As Boolean)
            _restrictFavCheck = value
        End Set
    End Property

    Public WriteOnly Property IconSize() As Integer
        Set(ByVal value As Integer)
            _iconSz = value
        End Set
    End Property

    Public Function MakeShortUrl(ByVal ConverterType As UrlConverter, ByVal SrcUrl As String) As String
        Dim ret As String = ""
        Dim resStatus As String = ""
        Dim src As String = SrcUrl

        For Each svc As String In _ShortUrlService
            If SrcUrl.StartsWith(svc) Then
                Return "Can't convert"
            End If
        Next

        SrcUrl = HttpUtility.UrlEncode(SrcUrl)
        Select Case ConverterType
            Case UrlConverter.TinyUrl       'tinyurl
                If SrcUrl.StartsWith("http") Then
                    If "http://tinyurl.com/xxxxxx".Length > src.Length AndAlso Not src.Contains("?") AndAlso Not src.Contains("#") Then
                        ' 明らかに長くなると推測できる場合は圧縮しない
                        ret = src
                        Exit Select
                    End If
                    Try
                        ret = DirectCast(CreateSocket.GetWebResponse("http://tinyurl.com/api-create.php?url=" + SrcUrl, resStatus, MySocket.REQ_TYPE.ReqPOSTEncode), String)
                    Catch ex As Exception
                        Return "Can't convert"
                    End Try
                End If
                If Not ret.StartsWith("http://tinyurl.com/") Then
                    Return "Can't convert"
                End If
            Case UrlConverter.Isgd
                If SrcUrl.StartsWith("http") Then
                    If "http://is.gd/xxxx".Length > src.Length AndAlso Not src.Contains("?") AndAlso Not src.Contains("#") Then
                        ' 明らかに長くなると推測できる場合は圧縮しない
                        ret = src
                        Exit Select
                    End If
                    Try
                        ret = DirectCast(CreateSocket.GetWebResponse("http://is.gd/api.php?longurl=" + SrcUrl, resStatus, MySocket.REQ_TYPE.ReqPOSTEncode), String)
                    Catch ex As Exception
                        Return "Can't convert"
                    End Try
                End If
                If Not ret.StartsWith("http://is.gd/") Then
                    Return "Can't convert"
                End If
            Case UrlConverter.Twurl
                If SrcUrl.StartsWith("http") Then
                    If "http://twurl.nl/xxxxxx".Length > src.Length AndAlso Not src.Contains("?") AndAlso Not src.Contains("#") Then
                        ' 明らかに長くなると推測できる場合は圧縮しない
                        ret = src
                        Exit Select
                    End If
                    Try
                        ret = DirectCast(CreateSocket.GetWebResponse("http://tweetburner.com/links", resStatus, MySocket.REQ_TYPE.ReqPOSTEncode, "link[url]=" + SrcUrl), String)
                    Catch ex As Exception
                        Return "Can't convert"
                    End Try
                End If
                If Not ret.StartsWith("http://twurl.nl/") Then
                    Return "Can't convert"
                End If
            Case UrlConverter.Unu
                If SrcUrl.StartsWith("http") Then
                    If "http://u.nu/xxxx".Length > src.Length AndAlso Not src.Contains("?") AndAlso Not src.Contains("#") Then
                        ' 明らかに長くなると推測できる場合は圧縮しない
                        ret = src
                        Exit Select
                    End If
                    Try
                        ret = DirectCast(CreateSocket.GetWebResponse("http://u.nu/unu-api-simple?url=" + SrcUrl, resStatus, MySocket.REQ_TYPE.ReqPOSTEncode), String)
                    Catch ex As Exception
                        Return "Can't convert"
                    End Try
                End If
                If Not ret.StartsWith("http://u.nu") Then
                    Return "Can't convert"
                End If
            Case UrlConverter.Bitly
                Const BitlyLogin As String = "tweenapi"
                Const BitlyApiKey As String = "R_c5ee0e30bdfff88723c4457cc331886b"
                Const BitlyApiVersion As String = "2.0.1"
                If SrcUrl.StartsWith("http") Then
                    If "http://bit.ly/xxxx".Length > src.Length AndAlso Not src.Contains("?") AndAlso Not src.Contains("#") Then
                        ' 明らかに長くなると推測できる場合は圧縮しない
                        ret = src
                        Exit Select
                    End If
                    Try
                        ret = DirectCast(CreateSocket.GetWebResponse( _
                            "http://api.bit.ly/shorten?version=" + BitlyApiVersion + _
                            "&login=" + BitlyLogin + _
                            "&apiKey=" + BitlyApiKey + _
                            "&longUrl=" + SrcUrl, resStatus, MySocket.REQ_TYPE.ReqPOSTEncode), String)
                        Dim rx As Regex = New Regex("""shortUrl"": ""(?<ShortUrl>.*?)""")
                        If rx.Match(ret).Success Then
                            ret = rx.Match(ret).Groups("ShortUrl").Value
                        End If
                    Catch ex As Exception
                        Return "Can't convert"
                    End Try
                End If
                If Not ret.StartsWith("http://bit.ly") Then
                    Return "Can't convert"
                End If
        End Select

        If src.Length < ret.Length Then ret = src ' 圧縮の結果逆に長くなった場合は圧縮前のURLを返す
        Return ret
    End Function

#Region "バージョンアップ"
    Public Function GetVersionInfo() As String
        Dim resStatus As String = ""
        Dim ret As String = DirectCast(CreateSocket.GetWebResponse("http://tween.sourceforge.jp/version2.txt?" + Now.ToString("yyMMddHHmmss") + Environment.TickCount.ToString(), resStatus), String)
        If ret.Length = 0 Then Throw New Exception("GetVersionInfo: " + resStatus)
        Return ret
    End Function

    Public Function GetTweenBinary(ByVal strVer As String) As String
        Dim resStatus As String = ""
        Dim ret As String = ""
        ret = DirectCast(CreateSocket.GetWebResponse("http://tween.sourceforge.jp/Tween" + strVer + ".gz?" + Now.ToString("yyMMddHHmmss") + Environment.TickCount.ToString(), resStatus, MySocket.REQ_TYPE.ReqGETFile), String)
        If ret = "" OrElse resStatus.StartsWith("OK") Then
            '取得OKなら、続いてresources.dllダウンロード
            Return GetTweenResourcesDll(strVer)
        Else
            Return resStatus
        End If
    End Function

    Public Function GetTweenUpBinary() As String
        Dim resStatus As String = ""
        Dim ret As String = DirectCast(CreateSocket.GetWebResponse("http://tween.sourceforge.jp/TweenUp.gz?" + Now.ToString("yyMMddHHmmss") + Environment.TickCount.ToString(), resStatus, MySocket.REQ_TYPE.ReqGETFileUp), String)
        If ret = "" OrElse resStatus.StartsWith("OK") Then
            Return ""
        Else
            Return resStatus
        End If
    End Function

    Public Function GetTweenResourcesDll(ByVal strver As String) As String
        Dim resStatus As String = ""
        Dim ret As String = DirectCast(CreateSocket.GetWebResponse("http://tween.sourceforge.jp/TweenRes" + strver + ".gz?" + Now.ToString("yyMMddHHmmss") + Environment.TickCount.ToString(), resStatus, MySocket.REQ_TYPE.ReqGETFileRes), String)
        If ret = "" OrElse resStatus.StartsWith("OK") Then
            Return ""
        Else
            Return resStatus
        End If
    End Function
#End Region

    Private Function CreateSocket() As MySocket
        Return New MySocket("UTF-8", _uid, _pwd, _proxyType, _proxyAddress, _proxyPort, _proxyUser, _proxyPassword, _defaultTimeOut)
    End Function

    Public WriteOnly Property ListIcon() As ImageList
        Set(ByVal value As ImageList)
            _lIcon = value
        End Set
    End Property

    Public WriteOnly Property DetailIcon() As Dictionary(Of String, Image)
        Set(ByVal value As Dictionary(Of String, Image))
            _dIcon = value
        End Set
    End Property

    Public Property DefaultTimeOut() As Integer
        Get
            Return _defaultTimeOut
        End Get
        Set(ByVal value As Integer)
            _defaultTimeOut = value
        End Set
    End Property

    Public WriteOnly Property CountApi() As Integer
        'API時の取得件数
        Set(ByVal value As Integer)
            _countApi = value
        End Set
    End Property

    Public WriteOnly Property UsePostMethod() As Boolean
        Set(ByVal value As Boolean)
            _usePostMethod = False
#If 0 Then
            'POSTメソッドが弾かれるためGETに固定(2009/4/9)
            If value Then
                _ApiMethod = MySocket.REQ_TYPE.ReqPOSTAPI
            Else
                _ApiMethod = MySocket.REQ_TYPE.ReqGetAPI
            End If
#Else
            _ApiMethod = MySocket.REQ_TYPE.ReqGetAPI
#End If
        End Set
    End Property

    Public Function GetTimelineApi(ByVal read As Boolean, _
                            ByVal gType As WORKERTYPE) As String
        If _endingFlag Then Return ""

        Dim retMsg As String = ""
        Dim resStatus As String = ""
        Dim sck As MySocket = CreateSocket()
        'スレッド取得は行わず、countで調整
        Const COUNT_QUERY As String = "count="
        Const FRIEND_PATH As String = "/statuses/friends_timeline.xml"
        Const REPLY_PATH As String = "/statuses/mentions.xml"

        If gType = WORKERTYPE.Timeline Then
            retMsg = DirectCast(sck.GetWebResponse("https://" + _hubServer + FRIEND_PATH + "?" + COUNT_QUERY + _countApi.ToString(), resStatus, _ApiMethod), String)
        Else
            retMsg = DirectCast(sck.GetWebResponse("https://" + _hubServer + REPLY_PATH + "?" + COUNT_QUERY + _countApi.ToString(), resStatus, _ApiMethod), String)
        End If

        If retMsg = "" Then
            If resStatus.StartsWith("Err: BadRequest") Then
                Return "Maybe, the requests reached API limit."
            Else
                Return resStatus
            End If
        End If

        Dim arIdx As Integer = -1
        Dim dlgt(_countApi) As GetIconImageDelegate    'countQueryに合わせる
        Dim ar(_countApi) As IAsyncResult              'countQueryに合わせる
        Dim xdoc As New XmlDocument
        Try
            xdoc.LoadXml(retMsg)
        Catch ex As Exception
            TraceOut(retMsg)
            'MessageBox.Show("不正なXMLです。(TL-LoadXml)")
            Return "Invalid XML!"
        End Try

        For Each xentryNode As XmlNode In xdoc.DocumentElement.SelectNodes("./status")
            Dim xentry As XmlElement = CType(xentryNode, XmlElement)
            Dim post As New PostClass
            Try
                post.PDate = DateTime.ParseExact(xentry.Item("created_at").InnerText, "ddd MMM dd HH:mm:ss zzzz yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None)
                post.Id = Long.Parse(xentry.Item("id").InnerText)
                '二重取得回避
                SyncLock LockObj
                    If TabInformations.GetInstance.ContainsKey(post.Id) Then Continue For
                End SyncLock
                '本文
                post.Data = xentry.Item("text").InnerText
                'HTMLに整形
                post.OriginalData = CreateHtmlAnchor(post.Data, post.ReplyToList)
                post.Data = HttpUtility.HtmlDecode(post.Data)
                post.Data = post.Data.Replace("<3", "♡")
                'Source取得（htmlの場合は、中身を取り出し）
                post.Source = xentry.Item("source").InnerText
                If post.Source.StartsWith("<") Then
                    Dim rgS As New Regex(">(?<source>.+)<")
                    Dim mS As Match = rgS.Match(post.Source)
                    If mS.Success Then
                        post.Source = mS.Result("${source}")
                    End If
                End If
                Long.TryParse(xentry.Item("in_reply_to_status_id").InnerText, post.InReplyToId)
                post.InReplyToUser = xentry.Item("in_reply_to_screen_name").InnerText
                'in_reply_to_user_idを使うか？
                post.IsFav = Boolean.Parse(xentry.Item("favorited").InnerText)

                '以下、ユーザー情報
                Dim xUentry As XmlElement = CType(xentry.SelectSingleNode("./user"), XmlElement)
                post.Uid = Long.Parse(xUentry.Item("id").InnerText)
                post.Name = xUentry.Item("screen_name").InnerText
                post.Nickname = xUentry.Item("name").InnerText
                post.ImageUrl = xUentry.Item("profile_image_url").InnerText
                post.IsProtect = Boolean.Parse(xUentry.Item("protected").InnerText)
                post.IsMe = post.Name.ToLower.Equals(_uid)
                post.IsRead = read
                If gType = WORKERTYPE.Timeline Then
                    post.IsReply = post.ReplyToList.Contains(_uid)
                Else
                    post.IsReply = True
                End If

                If post.IsMe Then
                    post.IsOwl = False
                Else
                    If followerId.Count > 0 Then post.IsOwl = Not followerId.Contains(post.Uid)
                End If

                post.IsDm = False
            Catch ex As Exception
                TraceOut(retMsg)
                'MessageBox.Show("不正なXMLです。(TL-Parse)")
                Continue For
            End Try

            '非同期アイコン取得＆StatusDictionaryに追加
            arIdx += 1
            dlgt(arIdx) = New GetIconImageDelegate(AddressOf GetIconImage)
            ar(arIdx) = dlgt(arIdx).BeginInvoke(post, Nothing, Nothing)
        Next

        'アイコン取得完了待ち
        For i As Integer = 0 To arIdx
            Try
                dlgt(i).EndInvoke(ar(i))
            Catch ex As Exception
                '最後までendinvoke回す（ゾンビ化回避）
                ex.Data("IsTerminatePermission") = False
                Throw
            End Try
        Next

        If _ApiMethod = MySocket.REQ_TYPE.ReqGetAPI Then _remainCountApi = sck.RemainCountApi

        Return ""
    End Function

    Public Function GetDirectMessageApi(ByVal read As Boolean, _
                            ByVal gType As WORKERTYPE) As String
        If _endingFlag Then Return ""

        Dim retMsg As String = ""
        Dim resStatus As String = ""
        Dim sck As MySocket = CreateSocket()
        'スレッド取得は行わず、countで調整
        Const GET_COUNT As Integer = 20
        Const RECEIVE_PATH As String = "/direct_messages.xml"
        Const SENT_PATH As String = "/direct_messages/sent.xml"

        If gType = WORKERTYPE.DirectMessegeRcv Then
            retMsg = DirectCast(sck.GetWebResponse("https://" + _hubServer + RECEIVE_PATH, resStatus, _ApiMethod), String)
        Else
            retMsg = DirectCast(sck.GetWebResponse("https://" + _hubServer + SENT_PATH, resStatus, _ApiMethod), String)
        End If

        If retMsg = "" Then
            If resStatus.StartsWith("Err: BadRequest") Then
                Return "Maybe, the requests reached API limit."
            Else
                Return resStatus
            End If
        End If

        Dim arIdx As Integer = -1
        Dim dlgt(GET_COUNT) As GetIconImageDelegate    'countQueryに合わせる
        Dim ar(GET_COUNT) As IAsyncResult              'countQueryに合わせる
        Dim xdoc As New XmlDocument
        Try
            xdoc.LoadXml(retMsg)
        Catch ex As Exception
            TraceOut(retMsg)
            'MessageBox.Show("不正なXMLです。(DM-LoadXml)")
            Return "Invalid XML!"
        End Try

        For Each xentryNode As XmlNode In xdoc.DocumentElement.SelectNodes("./direct_message")
            Dim xentry As XmlElement = CType(xentryNode, XmlElement)
            Dim post As New PostClass
            Try
                post.Id = Long.Parse(xentry.Item("id").InnerText)
                '二重取得回避
                SyncLock LockObj
                    If TabInformations.GetInstance.ContainsKey(post.Id) Then Continue For
                End SyncLock
                'sender_id
                'recipient_id
                post.PDate = DateTime.ParseExact(xentry.Item("created_at").InnerText, "ddd MMM dd HH:mm:ss zzzz yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None)
                '本文
                post.Data = xentry.Item("text").InnerText
                'HTMLに整形
                post.OriginalData = CreateHtmlAnchor(post.Data, post.ReplyToList)
                post.Data = HttpUtility.HtmlDecode(post.Data)
                post.Data = post.Data.Replace("<3", "♡")
                post.IsFav = False
                '受信ＤＭかの判定で使用
                If gType = WORKERTYPE.DirectMessegeRcv Then
                    post.IsOwl = False
                Else
                    post.IsOwl = True
                End If

                '以下、ユーザー情報
                Dim xUentry As XmlElement
                If gType = WORKERTYPE.DirectMessegeRcv Then
                    xUentry = CType(xentry.SelectSingleNode("./sender"), XmlElement)
                    post.IsMe = False
                Else
                    xUentry = CType(xentry.SelectSingleNode("./recipient"), XmlElement)
                    post.IsMe = True
                End If
                post.Uid = Long.Parse(xUentry.Item("id").InnerText)
                post.Name = xUentry.Item("screen_name").InnerText
                post.Nickname = xUentry.Item("name").InnerText
                post.ImageUrl = xUentry.Item("profile_image_url").InnerText
                post.IsProtect = Boolean.Parse(xUentry.Item("protected").InnerText)
            Catch ex As Exception
                TraceOut(retMsg)
                'MessageBox.Show("不正なXMLです。(DM-Parse)")
                Continue For
            End Try

            post.IsRead = read
            post.IsReply = False
            post.IsDm = True

            '非同期アイコン取得＆StatusDictionaryに追加
            arIdx += 1
            dlgt(arIdx) = New GetIconImageDelegate(AddressOf GetIconImage)
            ar(arIdx) = dlgt(arIdx).BeginInvoke(post, Nothing, Nothing)
        Next

        'アイコン取得完了待ち
        For i As Integer = 0 To arIdx
            Try
                dlgt(i).EndInvoke(ar(i))
            Catch ex As Exception
                '最後までendinvoke回す（ゾンビ化回避）
                ex.Data("IsTerminatePermission") = False
                Throw
            End Try
        Next

        If _ApiMethod = MySocket.REQ_TYPE.ReqGetAPI Then _remainCountApi = sck.RemainCountApi

        Return ""
    End Function

    Public Function GetFollowersApi() As String
        If _endingFlag Then Return ""

        Dim retMsg As String = ""
        Dim resStatus As String = ""
        Const FOLLOWER_PATH As String = "/followers/ids.xml"

        retMsg = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + FOLLOWER_PATH, resStatus, _ApiMethod), String)

        If retMsg = "" Then Return resStatus

        Dim xdoc As New XmlDocument
        Try
            xdoc.LoadXml(retMsg)
        Catch ex As Exception
            TraceOut(retMsg)
            MessageBox.Show("不正なXMLです。再取得してください。(FollowerApi-LoadXml)")
            Return "Invalid XML!"
        End Try

        followerId.Clear()
        For Each xentryNode As XmlNode In xdoc.DocumentElement.SelectNodes("./id")
            Try
                followerId.Add(Long.Parse(xentryNode.InnerText))
            Catch ex As Exception
                TraceOut(retMsg)
                MessageBox.Show("不正なXMLです。再取得してください。(FollowerApi-Parse)")
                Continue For
            End Try
        Next

        Return ""
    End Function

    Private Function CreateHtmlAnchor(ByVal Text As String, ByVal AtList As List(Of String)) As String
        'uriの正規表現
        Dim rgUrl As Regex = New Regex("(?<![0-9A-Za-z])(?:https?|shttp|ftps?)://(?:(?:[-_.!~*'()a-zA-Z0-9;:&=+$,]|%[0-9A-Fa-f" + _
                         "][0-9A-Fa-f])*@)?(?:(?:[a-zA-Z0-9](?:[-a-zA-Z0-9]*[a-zA-Z0-9])?\.)" + _
                         "*[a-zA-Z](?:[-a-zA-Z0-9]*[a-zA-Z0-9])?\.?|[0-9]+\.[0-9]+\.[0-9]+\." + _
                         "[0-9]+)(?::[0-9]*)?(?:/(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f]" + _
                         "[0-9A-Fa-f])*(?:;(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f][0-9A-" + _
                         "Fa-f])*)*(?:/(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f" + _
                         "])*(?:;(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])*)*)" + _
                         "*)?(?:\?(?:[-_.!~*'()a-zA-Z0-9;/?:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])" + _
                         "*)?(?:#(?:[-_.!~*'()a-zA-Z0-9;/?:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])*)?")
        Dim retStr As String = HttpUtility.HtmlEncode(Text)     '要検証（デコードされて取得されるので再エンコード）
        '絶対パス表現のUriをリンクに置換
        retStr = rgUrl.Replace(Text, "<a href=""$&"">$&</a>")
        '@返信を抽出し、@先リスト作成
        Dim rg As New Regex("(^|[ -/:-@[-^`{-~])@([a-zA-Z0-9_]{1,20})")
        Dim m As Match = rg.Match(retStr)
        While m.Success
            AtList.Add(m.Result("$2").ToLower)
            m = m.NextMatch
        End While
        '@先をリンクに置換
        retStr = rg.Replace(retStr, "$1@<a href=""/$2"">$2</a>")
        retStr = AdjustHtml(ShortUrlResolve(PreProcessUrl(retStr))) 'IDN置換、短縮Uri解決、@リンクを相対→絶対にしてtarget属性付与
        Return retStr
    End Function

    Public ReadOnly Property RemainCountApi() As Integer
        Get
            Return _remainCountApi
        End Get
    End Property

    Public ReadOnly Property MaxCountApi() As Integer
        Get
            Dim _maxcnt As Integer = 0
            Dim resMsg As String = ""
            Dim resStatus As String = ""
            resMsg = DirectCast(CreateSocket.GetWebResponse("https://" + _hubServer + _rateLimitStatus, resStatus, MySocket.REQ_TYPE.ReqGetAPI), String)
            Dim xdoc As New XmlDocument
            Try
                xdoc.LoadXml(resMsg)
                _maxcnt = Integer.Parse(xdoc.SelectSingleNode("/hash/hourly-limit").InnerText)
            Catch ex As Exception

            End Try
            Return _maxcnt
        End Get
    End Property

#Region "デバッグモード解析キー自動生成"
#If DEBUG Then
    Public Sub GenerateAnalyzeKey()
        '解析キー情報部分のソースをwedataから作成する
        '生成したソースはプロジェクトのディレクトリにコピーする
        Dim sw As New System.IO.StreamWriter(".\AnalyzeKey.vb", _
            False, _
            System.Text.Encoding.UTF8)

        sw.WriteLine("Public Module AnalyzeKey")
        sw.WriteLine("'    このファイルはデバッグビルドのTweenにより自動作成されました   作成日時  " + DateAndTime.Now.ToString())
        sw.WriteLine("")

        sw.WriteLine("    Public _splitPost As String = " + Chr(34) + _splitPost.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _splitPostRecent As String = " + Chr(34) + _splitPostRecent.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _statusIdTo As String = " + Chr(34) + _statusIdTo.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _splitDM As String = " + Chr(34) + _splitDM.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseName As String = " + Chr(34) + _parseName.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseNameTo As String = " + Chr(34) + _parseNameTo.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseNick As String = " + Chr(34) + _parseNick.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseNickTo As String = " + Chr(34) + _parseNickTo.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseImg As String = " + Chr(34) + _parseImg.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseImgTo As String = " + Chr(34) + _parseImgTo.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseMsg1 As String = " + Chr(34) + _parseMsg1.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseMsg2 As String = " + Chr(34) + _parseMsg2.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseDM1 As String = " + Chr(34) + _parseDM1.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseDM11 As String = " + Chr(34) + _parseDM11.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseDM2 As String = " + Chr(34) + _parseDM2.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseDate As String = " + Chr(34) + _parseDate.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseDateTo As String = " + Chr(34) + _parseDateTo.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _getAuthKey As String = " + Chr(34) + _getAuthKey.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _getAuthKeyTo As String = " + Chr(34) + _getAuthKeyTo.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseStar As String = " + Chr(34) + _parseStar.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseStarTo As String = " + Chr(34) + _parseStarTo.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseStarEmpty As String = " + Chr(34) + _parseStarEmpty.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _followerList As String = " + Chr(34) + _followerList.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _followerMbr1 As String = " + Chr(34) + _followerMbr1.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _followerMbr2 As String = " + Chr(34) + _followerMbr2.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _followerMbr3 As String = " + Chr(34) + _followerMbr3.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _getInfoTwitter As String = " + Chr(34) + _getInfoTwitter.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _getInfoTwitterTo As String = " + Chr(34) + _getInfoTwitterTo.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _isProtect As String = " + Chr(34) + _isProtect.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _isReplyEng As String = " + Chr(34) + _isReplyEng.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _isReplyJpn As String = " + Chr(34) + _isReplyJpn.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _isReplyTo As String = " + Chr(34) + _isReplyTo.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseProtectMsg1 As String = " + Chr(34) + _parseProtectMsg1.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseProtectMsg2 As String = " + Chr(34) + _parseProtectMsg2.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseDMcountFrom As String = " + Chr(34) + _parseDMcountFrom.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseDMcountTo As String = " + Chr(34) + _parseDMcountTo.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseSourceFrom As String = " + Chr(34) + _parseSourceFrom.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseSource2 As String = " + Chr(34) + _parseSource2.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _parseSourceTo As String = " + Chr(34) + _parseSourceTo.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("    Public _removeClass As String = " + Chr(34) + _removeClass.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
        sw.WriteLine("End Module")

        sw.Close()
        'MessageBox.Show("解析キー情報定義ファイル AnalyzeKey.vbを生成しました")

    End Sub
#End If
#End Region
End Module

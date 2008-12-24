' Tween - Client of Twitter
' Copyright © 2007-2008 kiri_feather (@kiri_feather) <kiri_feather@gmail.com>
'           © 2008      Moz (@syo68k) <http://iddy.jp/profile/moz/>
'           © 2008      takeshik (@takeshik) <http://www.takeshik.org/>
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
Imports System.Text.RegularExpressions

Partial Public Class Twitter
    Public links As New Collections.Specialized.StringCollection

    Private _authKey As String              'StatusUpdate、発言削除で使用
    Private _authKeyDM As String              'DM送信、DM削除で使用
    'Private _authSiv As String              'StatusUpdate等で使用

    Private _uid As String
    Private _pwd As String
    Private _proxyType As ProxyTypeEnum
    Private _proxyAddress As String
    Private _proxyPort As Integer
    Private _proxyUser As String
    Private _proxyPassword As String

    'Private _lastId As String
    'Private _lastName As String
    Private _nextThreshold As Integer
    Private _nextPages As Integer

    Private _infoTwitter As String = ""

    Private _signed As Boolean
    Private _mySock As MySocket

    Private _hubServer As String

    Private _getIcon As Boolean
    Private _tinyUrlResolve As Boolean
    Private _dmCount As Integer
    Private _restrictFavCheck As Boolean
    Private _ShortUrlService As New List(Of String)

    Private Const _baseUrlStr As String = "twitter.com"
    Private Const _loginPath As String = "/sessions"
    Private Const _homePath As String = "/home"
    Private Const _replyPath As String = "/replies"
    Private Const _DMPathRcv As String = "/direct_messages"
    Private Const _DMPathSnt As String = "/direct_messages/sent"
    Private Const _DMDestroyPath As String = "/direct_messages/destroy/"
    Private Const _StDestroyPath As String = "/status/destroy/"
    Private Const _uidHeader As String = "session[username_or_email]="
    Private Const _pwdHeader As String = "session[password]="
    Private Const _pageQry As String = "?page="
    Private Const _statusHeader As String = "status="
    Private Const _statusUpdatePath As String = "/status/update?page=1&tab=home"
    Private Const _statusUpdatePathAPI As String = "/statuses/update.xml"
    Private Const _linkToOld As String = "class=""section_links"" rel=""prev"""
    Private Const _postFavAddPath As String = "/favourings/create/"
    Private Const _postFavRemovePath As String = "/favourings/destroy/"
    Private Const _authKeyHeader As String = "authenticity_token="
    Private Const _parseLink1 As String = "<a href="""
    Private Const _parseLink2 As String = """>"
    Private Const _parseLink3 As String = "</a>"
    Private Const _GetFollowers As String = "/statuses/followers.xml"
    Private Const _ShowStatus As String = "/statuses/show/"

    Private _endingFlag As Boolean
    Private _useAPI As Boolean

    '''Wedata対応
    Private Const wedataUrl As String = "http://wedata.net/databases/Tween/items.json"
    'テーブル
    Private Const tbGetMsgDM As String = "GetMsgDM"
    Private Const tbSplitDM As String = "SplitDM"
    Private Const tbFollower As String = "Follower"
    Private Const tbGetStar As String = "GetStar"
    Private Const tbIsReply As String = "IsReply"
    Private Const tbGetDate As String = "GetDate"
    Private Const tbGetMsg As String = "GetMsg"
    Private Const tbIsProtect As String = "IsProtect"
    Private Const tbGetImagePath As String = "GetImagePath"
    Private Const tbGetNick As String = "GetNick"
    Private Const tbGetName As String = "GetName"
    'Private Const tbGetSiv As String = "GetSiv"
    Private Const tbStatusID As String = "StatusID"
    Private Const tbSplitPostRecent As String = "SplitPostRecent"
    Private Const tbAuthKey As String = "AuthKey"
    Private Const tbInfoTwitter As String = "InfoTwitter"
    Private Const tbSplitPostReply As String = "SplitPostReply"
    Private Const tbGetDMCount As String = "GetDMCount"
    '属性
    Private Const tbTagFrom As String = "tagfrom"
    Private Const tbTagTo As String = "tagto"
    Private Const tbTag As String = "tag"
    Private Const tbTagMbrFrom As String = "tagmbrfrom"
    Private Const tbTagMbrFrom2 As String = "tagmbrfrom2"
    Private Const tbTagMbrTo As String = "tagmbrto"
    Private Const tbTagStatus As String = "status"
    Private Const tbTagJpnFrom As String = "tagjpnfrom"
    Private Const tbTagEngFrom As String = "tagengfrom"

    Public savePost As String

    Public Structure MyListItem
        Public Nick As String
        Public Data As String
        Public ImageUrl As String
        Public Name As String
        Public PDate As DateTime
        Public Id As String
        Public Fav As Boolean
        Public OrgData As String
        Public Readed As Boolean
        Public Reply As Boolean
        Public Protect As Boolean
        Public OWL As Boolean
    End Structure

    Public Enum GetTypes
        GET_TIMELINE
        GET_REPLY
        GET_DMRCV
        GET_DMSNT
    End Enum

    Public Sub New(ByVal Username As String, _
                ByVal Password As String, _
                ByVal ProxyType As ProxyTypeEnum, _
                ByVal ProxyAddress As String, _
                ByVal ProxyPort As Integer, _
                ByVal ProxyUser As String, _
                ByVal ProxyPassword As String)
        _mySock = New MySocket("UTF-8", Username, Password, ProxyType, ProxyAddress, ProxyPort, ProxyUser, ProxyPassword)
        _uid = Username
        _pwd = Password
        'follower.Add(_uid)
        _proxyType = ProxyType
        _proxyAddress = ProxyAddress
        _proxyPort = ProxyPort
        _proxyUser = ProxyUser
        _proxyPassword = ProxyPassword

        _ShortUrlService.AddRange(New String() { _
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
            "http://budurl.com/" _
        })
    End Sub

    Private Function SignIn() As String
        If _endingFlag Then Return ""

        'ユーザー情報からデータ部分の生成
        Dim account As String = _uidHeader + _uid + "&" + _pwdHeader + _pwd

        '未認証
        _signed = False

        Dim resStatus As String = ""
        Dim resMsg As String = ""

        resMsg = DirectCast(_mySock.GetWebResponse("https://" + _hubServer + _loginPath, resStatus, MySocket.REQ_TYPE.ReqPOST, account), String)
        If resMsg.Length = 0 Then
            Return "SignIn -> " + resStatus
        End If

        _signed = True
        Return ""
    End Function

    Public Function GetTimeline(ByVal tLine As List(Of MyListItem), ByVal page As Integer, ByVal initial As Boolean, ByRef endPage As Integer, ByVal gType As GetTypes, ByVal imgKeys As Collections.Specialized.StringCollection, ByVal imgs As ImageList, ByRef getDM As Boolean) As String
        If _endingFlag Then Return ""

        Dim retMsg As String = ""
        Dim resStatus As String = ""
        Dim moreRead As Boolean = True

        If _signed = False Then
            retMsg = SignIn()
            If retMsg.Length > 0 Then
                Return retMsg
            End If
        End If

        'リクエストメッセージを作成する
        Dim pageQuery As String

        If page = 1 Then
            pageQuery = ""
        Else
            pageQuery = _pageQry + page.ToString
        End If

        If gType = GetTypes.GET_TIMELINE Then
            retMsg = DirectCast(_mySock.GetWebResponse("https://" + _hubServer + _homePath + pageQuery, resStatus), String)
        Else
            retMsg = DirectCast(_mySock.GetWebResponse("https://" + _hubServer + _replyPath + pageQuery, resStatus), String)
        End If

        ' tr 要素の class 属性を消去
        Do
            Dim idx As Integer = retMsg.IndexOf("<tr class=""", StringComparison.Ordinal)
            If idx = -1 Then Exit Do
            retMsg = retMsg.Remove(idx + 4, retMsg.IndexOf("""", idx + 11 + 1, StringComparison.Ordinal) - idx - 2) ' 11 = "<tr class=""".Length
        Loop

        If _endingFlag Then Return ""

        If retMsg.Length = 0 Then
            _signed = False
            Return resStatus
        End If

        Dim pos1 As Integer
        Dim pos2 As Integer


        '各メッセージに分割可能か？
        Dim strSepTmp As String
        If gType = GetTypes.GET_TIMELINE Then
            strSepTmp = _splitPostRecent
        Else
            strSepTmp = _splitPost
        End If

        pos1 = retMsg.IndexOf(strSepTmp, StringComparison.Ordinal)
        If pos1 = -1 Then
            '0件 or 取得失敗
            _signed = False
            Return "GetTimeline -> Err: tweets count is 0."
        End If

        Dim strSep() As String = {strSepTmp}
        Dim posts() As String = retMsg.Split(strSep, StringSplitOptions.RemoveEmptyEntries)
        Dim strPost As String = ""
        Dim intCnt As Integer = 0
        Dim listCnt As Integer = tLine.Count
        Dim orgData As String = ""
        Dim tmpDate As DateTime = Now

        For Each strPost In posts
            savePost = strPost
            intCnt += 1

            If intCnt = 1 Then
                If page = 1 And gType = GetTypes.GET_TIMELINE Then
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

                Dim lItem As New MyListItem

                Try
                    'Get ID
                    pos1 = 0
                    pos2 = strPost.IndexOf(_statusIdTo, 0, StringComparison.Ordinal)
                    lItem.Id = HttpUtility.HtmlDecode(strPost.Substring(0, pos2))
                Catch ex As Exception
                    _signed = False
                    TraceOut("TM-ID:" + strPost)
                    Return "GetTimeline -> Err: Can't get ID."
                End Try
                'Get Name
                Try
                    pos1 = strPost.IndexOf(_parseName, pos2, StringComparison.Ordinal)
                    pos2 = strPost.IndexOf(_parseNameTo, pos1, StringComparison.Ordinal)
                    lItem.Name = HttpUtility.HtmlDecode(strPost.Substring(pos1 + _parseName.Length, pos2 - pos1 - _parseName.Length))
                Catch ex As Exception
                    _signed = False
                    TraceOut("TM-Name:" + strPost)
                    Return "GetTimeline -> Err: Can't get Name."
                End Try
                'Get Nick
                '''バレンタイン対応
                If strPost.IndexOf("twitter.com/images/heart.png", pos2, StringComparison.Ordinal) > -1 Then
                    lItem.Nick = lItem.Name
                Else
                    Try
                        pos1 = strPost.IndexOf(_parseNick, pos2, StringComparison.Ordinal)
                        pos2 = strPost.IndexOf(_parseNickTo, pos1 + _parseNick.Length, StringComparison.Ordinal)
                        lItem.Nick = HttpUtility.HtmlDecode(strPost.Substring(pos1 + _parseNick.Length, pos2 - pos1 - _parseNick.Length))
                    Catch ex As Exception
                        _signed = False
                        TraceOut("TM-Nick:" + strPost)
                        Return "GetTimeline -> Err: Can't get Nick."
                    End Try
                End If

                '二重取得回避
                If links.Contains(lItem.Id) = False Then
                    orgData = ""
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
                        lItem.ImageUrl = HttpUtility.HtmlDecode(strPost.Substring(pos1 + _parseImg.Length, pos2 - pos1 - _parseImg.Length))
                    Catch ex As Exception
                        _signed = False
                        TraceOut("TM-Img:" + strPost)
                        Return "GetTimeline -> Err: Can't get ImagePath."
                    End Try

                    'Protect
                    If strPost.IndexOf(_isProtect, pos2, StringComparison.Ordinal) > -1 Then
                        lItem.Protect = True
                    End If

                    'Get Message
                    pos1 = strPost.IndexOf(_parseMsg1, pos2, StringComparison.Ordinal)
                    If pos1 < 0 Then
                        'Valentine対応その２
                        Try
                            If strPost.IndexOf("<div id=""doyouheart", pos2, StringComparison.Ordinal) > -1 Then
                                orgData += " <3 you! Do you <3 "
                                pos1 = strPost.IndexOf("<a href", pos2, StringComparison.Ordinal)
                                pos2 = strPost.IndexOf("?", pos1, StringComparison.Ordinal)
                                orgData += strPost.Substring(pos1, pos2 - pos1 + 1)
                            Else
                                'pos1 = strPost.IndexOf("."" />", pos2)
                                pos1 = strPost.IndexOf(_parseProtectMsg1, pos2, StringComparison.Ordinal)
                                If pos1 = -1 Then
                                    'If orgData <> "You" Then
                                    '    orgData += lItem.Name + " <3 's "
                                    'Else
                                    orgData += " <3 's "
                                    'End If
                                    pos1 = strPost.IndexOf("<a href", pos2, StringComparison.Ordinal)
                                    If pos1 > -1 Then
                                        pos2 = strPost.IndexOf("!", pos1, StringComparison.Ordinal)
                                        orgData += strPost.Substring(pos1, pos2 - pos1 + 1)
                                    End If
                                Else
                                    'pos2 = strPost.IndexOf("<span class=""meta entry-meta"">", pos1)
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
                        Try
                            pos2 = strPost.IndexOf(_parseMsg2, pos1, StringComparison.Ordinal)
                            orgData = strPost.Substring(pos1 + _parseMsg1.Length, pos2 - pos1 - _parseMsg1.Length).Trim()
                        Catch ex As Exception
                            _signed = False
                            TraceOut("TM-Body:" + strPost)
                            Return "GetTimeline -> Err: Can't get body."
                        End Try
                        orgData = Regex.Replace(orgData, "<a href=""https://twitter\.com/" + lItem.Name + "/status/[0-9]+"">\.\.\.</a>$", "")
                        orgData = orgData.Replace("&lt;3", "♡")
                    End If

                    Dim posl1 As Integer
                    Dim posl2 As Integer = 0
                    Dim posl3 As Integer

                    If _tinyUrlResolve Then
                        For Each svc As String In _ShortUrlService
                            posl1 = 0
                            posl2 = 0
                            posl3 = 0
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
                                        retUrlStr = DirectCast(_mySock.GetWebResponse(urlStr, Response, MySocket.REQ_TYPE.ReqGETForwardTo), String)
                                        If retUrlStr.Length > 0 Then
                                            Dim uri As Uri = New Uri(retUrlStr)
                                            If Not uri.AbsolutePath = "/" Then
                                                Dim sb As StringBuilder = New StringBuilder(uri.Scheme + uri.SchemeDelimiter + uri.Host + "/", 256)
                                                For Each c As Char In retUrlStr.Substring(sb.Length)
                                                    If Convert.ToInt32(c) > 127 Then
                                                        sb.Append("%" + Convert.ToInt16(c).ToString("X2"))
                                                    Else
                                                        sb.Append(c)
                                                    End If
                                                Next
                                                orgData = orgData.Replace("<a href=""" + urlStr, "<a href=""" + sb.ToString())
                                            Else
                                                orgData = orgData.Replace("<a href=""" + urlStr, "<a href=""" + retUrlStr)
                                            End If
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

                    lItem.OrgData = orgData
                    lItem.OrgData = lItem.OrgData.Replace("<a href=""/", "<a href=""https://twitter.com/")
                    lItem.OrgData = lItem.OrgData.Replace("<a href=", "<a target=""_self"" href=")
                    lItem.OrgData = lItem.OrgData.Replace(vbLf, "<br>")

                    Try
                        If orgData.IndexOf(_parseLink1, StringComparison.Ordinal) = -1 Then
                            lItem.Data = HttpUtility.HtmlDecode(orgData)
                        Else
                            lItem.Data = ""

                            posl3 = 0
                            Do While True

                                posl1 = orgData.IndexOf(_parseLink1, posl3, StringComparison.Ordinal)
                                If posl1 = -1 Then Exit Do

                                If (posl3 + _parseLink3.Length <> posl1) Or posl3 = 0 Then
                                    If posl3 <> 0 Then
                                        lItem.Data += HttpUtility.HtmlDecode(orgData.Substring(posl3 + _parseLink3.Length, posl1 - posl3 - _parseLink3.Length))
                                    Else
                                        lItem.Data += HttpUtility.HtmlDecode(orgData.Substring(0, posl1))
                                    End If
                                End If
                                posl2 = orgData.IndexOf(_parseLink2, posl1, StringComparison.Ordinal)
                                posl3 = orgData.IndexOf(_parseLink3, posl2, StringComparison.Ordinal)
                                lItem.Data += HttpUtility.HtmlDecode(orgData.Substring(posl2 + _parseLink2.Length, posl3 - posl2 - _parseLink2.Length))
                            Loop

                            lItem.Data += HttpUtility.HtmlDecode(orgData.Substring(posl3 + _parseLink3.Length))
                        End If
                    Catch ex As Exception
                        _signed = False
                        TraceOut("TM-Link:" + strPost)
                        Return "GetTimeline -> Err: Can't parse links."
                    End Try

                    'Get Date
                    pos1 = strPost.IndexOf(_parseDate, pos2, StringComparison.Ordinal)
                    If pos1 > -1 Then
                        Try
                            pos2 = strPost.IndexOf(_parseDateTo, pos1 + _parseDate.Length, StringComparison.Ordinal)
                            lItem.PDate = DateTime.ParseExact(strPost.Substring(pos1 + _parseDate.Length, pos2 - pos1 - _parseDate.Length), "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz", System.Globalization.DateTimeFormatInfo.InvariantInfo, Globalization.DateTimeStyles.None)
                            tmpDate = lItem.PDate
                        Catch ex As Exception
                            _signed = False
                            TraceOut("TM-Date:" + strPost)
                            Return "GetTimeline -> Err: Can't get date."
                        End Try
                    Else
                        lItem.PDate = tmpDate
                    End If

                    'Get Reply
                    If strPost.IndexOf(_isReplyEng + _uid + _isReplyTo, StringComparison.Ordinal) > 0 Or strPost.IndexOf(_isReplyJpn + _uid + _isReplyTo, StringComparison.Ordinal) > 0 Then
                        lItem.Reply = True
                    ElseIf Regex.IsMatch(lItem.Data, "@" + _uid + "([^a-zA-Z0-9_]|$)", RegexOptions.IgnoreCase) Then
                        lItem.Reply = True
                    Else
                        lItem.Reply = False
                    End If

                    'Get Fav
                    pos1 = strPost.IndexOf(_parseStar, pos2, StringComparison.Ordinal)
                    If pos1 > -1 Then
                        Try
                            pos2 = strPost.IndexOf(_parseStarTo, pos1 + _parseStar.Length, StringComparison.Ordinal)
                            If strPost.Substring(pos1 + _parseStar.Length, pos2 - pos1 - _parseStar.Length) = _parseStarEmpty Then
                                lItem.Fav = False
                            Else
                                lItem.Fav = True
                            End If
                        Catch ex As Exception
                            _signed = False
                            TraceOut("TM-Fav:" + strPost)
                            Return "GetTimeline -> Err: Can't get fav status."
                        End Try
                    Else
                        lItem.Fav = False
                    End If

                    If imgKeys.Contains(lItem.ImageUrl) = False Then
                        If GetIconImage(lItem.ImageUrl, imgKeys, imgs) = False Then lItem.ImageUrl = ""
                    End If

                    If _endingFlag Then Return ""

                    links.Add(lItem.Id)
                    tLine.Add(lItem)
                End If

                'テスト実装：DMカウント取得
                getDM = False
                If intCnt = posts.Length And gType = GetTypes.GET_TIMELINE And page = 1 Then
                    pos1 = strPost.IndexOf(_parseDMcountFrom, pos2, StringComparison.Ordinal)
                    If pos1 > -1 Then
                        Try
                            pos2 = strPost.IndexOf(_parseDMcountTo, pos1 + _parseDMcountFrom.Length, StringComparison.Ordinal)
                            Dim dmCnt As Integer = Integer.Parse(strPost.Substring(pos1 + _parseDMcountFrom.Length, pos2 - pos1 - _parseDMcountFrom.Length))
                            If dmCnt > _dmCount Then
                                _dmCount = dmCnt
                                getDM = True
                            End If
                        Catch ex As Exception
                        End Try
                    End If
                End If
            End If
        Next

        Dim getCnt As Integer
        getCnt = tLine.Count - listCnt
        If getCnt > 0 Then
            '新規取得有
            If initial Then
                '起動時
                If moreRead Then
                    endPage = page + 1
                End If
            End If
        End If
        '通常時
        If ((page = 1 And getCnt >= _nextThreshold) Or page > 1) And initial = False Then
            '新着が閾値の件数以上なら、次のページも念のため読み込み
            endPage = _nextPages + 1
        End If

        Return ""
    End Function

    Public Function GetDirectMessage(ByVal tLine As List(Of MyListItem), ByVal page As Integer, ByRef endPage As Integer, ByVal gType As GetTypes, ByVal imgKeys As Collections.Specialized.StringCollection, ByVal imgs As ImageList) As String
        If _endingFlag Then Return ""

        Dim retMsg As String = ""
        Dim resStatus As String = ""

        endPage = page

        If _signed = False Then
            retMsg = SignIn()
            If retMsg.Length > 0 Then
                Return retMsg
            End If
        End If

        If _endingFlag Then Return ""

        'リクエストメッセージを作成する
        Dim pageQuery As String

        pageQuery = _pageQry + page.ToString

        If gType = GetTypes.GET_DMRCV Then
            retMsg = DirectCast(_mySock.GetWebResponse("https://" + _hubServer + _DMPathRcv + pageQuery, resStatus), String)
        Else
            retMsg = DirectCast(_mySock.GetWebResponse("https://" + _hubServer + _DMPathSnt + pageQuery, resStatus), String)
        End If

        ' tr 要素の class 属性を消去
        Do
            Dim idx As Integer = retMsg.IndexOf("<tr class=""", StringComparison.Ordinal)
            If idx = -1 Then Exit Do
            retMsg = retMsg.Remove(idx + 4, retMsg.IndexOf("""", idx + 11 + 1, StringComparison.Ordinal) - idx - 2) ' 11 = "<tr class=""".Length
        Loop

        If retMsg.Length = 0 Then
            _signed = False
            Return resStatus
        End If

        If _endingFlag Then Return ""

        ''AuthKeyの取得
        'If GetAuthKeyDM(retMsg) < 0 Then
        '    _signed = False
        '    Return "GetDirectMessage -> Err: Busy(1)"
        'End If

        Dim pos1 As Integer
        Dim pos2 As Integer

        ''Followerの抽出
        'If page = 1 And gType = GetTypes.GET_DMRCV Then
        '    pos1 = retMsg.IndexOf(_followerList)
        '    If pos1 = -1 Then
        '        If follower.Count = 0 Then follower.Add(_uid)
        '        '取得失敗
        '        _signed = False
        '        Return "GetDirectMessage -> Err: Busy(3)"
        '    End If
        '    follower.Clear()
        '    follower.Add(_uid)
        '    pos1 += _followerList.Length
        '    pos1 = retMsg.IndexOf(_followerMbr1, pos1)
        '    Try
        '        Do While pos1 > -1
        '            pos2 = retMsg.IndexOf(_followerMbr2, pos1)
        '            pos1 = retMsg.IndexOf(_followerMbr3, pos2)
        '            follower.Add(retMsg.Substring(pos2 + _followerMbr2.Length, pos1 - pos2 - _followerMbr2.Length))
        '            pos1 = retMsg.IndexOf(_followerMbr1, pos1)
        '        Loop
        '        follower.RemoveAt(follower.Count - 1)
        '    Catch ex As Exception
        '        _signed = False
        '        Return "GetDirectMessage -> Err: Can't get followers"
        '    End Try
        'End If

        '各メッセージに分割可能か？
        pos1 = retMsg.IndexOf(_splitDM, StringComparison.Ordinal)
        If pos1 = -1 Then
            '0件
            Return ""
        End If

        Dim strSep() As String = {_splitDM}
        Dim posts() As String = retMsg.Split(strSep, StringSplitOptions.RemoveEmptyEntries)
        Dim strPost As String = ""
        Dim intCnt As Integer = 0
        Dim listCnt As Integer = tLine.Count
        Dim orgData As String = ""
        Dim tmpDate As DateTime = Now

        For Each strPost In posts
            savePost = strPost
            intCnt += 1

            If intCnt > 1 Then
                Dim lItem As New MyListItem
                Dim flg As Boolean = False

                'Get ID
                Try
                    pos1 = 0
                    pos2 = strPost.IndexOf("""", 0, StringComparison.Ordinal)
                    lItem.Id = HttpUtility.HtmlDecode(strPost.Substring(0, pos2))
                Catch ex As Exception
                    _signed = False
                    TraceOut("DM-ID:" + strPost)
                    Return "GetDirectMessage -> Err: Can't get ID"
                End Try

                'Get Name
                Try
                    pos1 = strPost.IndexOf(_parseName, pos2, StringComparison.Ordinal)
                    pos2 = strPost.IndexOf(_parseNameTo, pos1, StringComparison.Ordinal)
                    lItem.Name = HttpUtility.HtmlDecode(strPost.Substring(pos1 + _parseName.Length, pos2 - pos1 - _parseName.Length))
                Catch ex As Exception
                    _signed = False
                    TraceOut("DM-Name:" + strPost)
                    Return "GetDirectMessage -> Err: Can't get Name"
                End Try

                'Get Nick
                Try
                    pos1 = strPost.IndexOf(_parseNick, pos2, StringComparison.Ordinal)
                    pos2 = strPost.IndexOf(_parseNickTo, pos1 + _parseNick.Length, StringComparison.Ordinal)
                    lItem.Nick = HttpUtility.HtmlDecode(strPost.Substring(pos1 + _parseNick.Length, pos2 - pos1 - _parseNick.Length))
                Catch ex As Exception
                    _signed = False
                    TraceOut("DM-Nick:" + strPost)
                    Return "GetDirectMessage -> Err: Can't get Nick."
                End Try

                If links.Contains(lItem.Id) Then
                    flg = True
                End If

                If flg = False Then
                    'Get ImagePath
                    Try
                        pos1 = strPost.IndexOf(_parseImg, pos2, StringComparison.Ordinal)
                        pos2 = strPost.IndexOf(_parseImgTo, pos1 + _parseImg.Length, StringComparison.Ordinal)
                        lItem.ImageUrl = HttpUtility.HtmlDecode(strPost.Substring(pos1 + _parseImg.Length, pos2 - pos1 - _parseImg.Length))
                    Catch ex As Exception
                        _signed = False
                        TraceOut("DM-Img:" + strPost)
                        Return "GetDirectMessage -> Err: Can't get ImagePath"
                    End Try

                    'Get Protect 
                    Try
                        pos1 = strPost.IndexOf(_isProtect, pos2, StringComparison.Ordinal)
                        If pos1 > -1 Then lItem.Protect = True
                    Catch ex As Exception
                        _signed = False
                        TraceOut("DM-Protect:" + strPost)
                        Return "GetDirectMessage -> Err: Can't get Protect"
                    End Try

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
                        orgData = Regex.Replace(orgData, "<a href=""https://twitter\.com/" + lItem.Name + "/status/[0-9]+"">\.\.\.</a>$", "")
                        orgData = orgData.Replace("&lt;3", "♡")
                    Catch ex As Exception
                        _signed = False
                        TraceOut("DM-Body:" + strPost)
                        Return "GetDirectMessage -> Err: Can't get body"
                    End Try

                    Dim posl1 As Integer
                    Dim posl2 As Integer = 0
                    Dim posl3 As Integer

                    If _tinyUrlResolve Then
                        For Each svc As String In _ShortUrlService
                            posl1 = 0
                            posl2 = 0
                            posl3 = 0
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
                                        retUrlStr = DirectCast(_mySock.GetWebResponse(urlStr, Response, MySocket.REQ_TYPE.ReqGETForwardTo), String)
                                        If retUrlStr.Length > 0 Then
                                            Dim uri As Uri = New Uri(retUrlStr)
                                            Dim sb As StringBuilder = New StringBuilder(uri.Scheme + uri.SchemeDelimiter + uri.Host + uri.AbsolutePath, 256)
                                            For Each c As Char In retUrlStr.Substring(sb.Length)
                                                If Convert.ToInt32(c) > 127 Then
                                                    sb.Append("%" + Convert.ToInt16(c).ToString("X2"))
                                                Else
                                                    sb.Append(c)
                                                End If
                                            Next
                                            orgData = orgData.Replace("<a href=""" + urlStr, "<a href=""" + sb.ToString())
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

                    lItem.OrgData = orgData
                    lItem.OrgData = lItem.OrgData.Replace("<a href=""/", "<a href=""https://twitter.com/")
                    lItem.OrgData = lItem.OrgData.Replace("<a href=", "<a target=""_self"" href=")
                    lItem.OrgData = lItem.OrgData.Replace(vbLf, "<br>")

                    Try
                        If orgData.IndexOf(_parseLink1, StringComparison.Ordinal) = -1 Then
                            lItem.Data = HttpUtility.HtmlDecode(orgData)
                        Else
                            lItem.Data = ""

                            posl3 = 0
                            Do While True

                                posl1 = orgData.IndexOf(_parseLink1, posl3, StringComparison.Ordinal)
                                If posl1 = -1 Then Exit Do

                                If (posl3 + _parseLink3.Length <> posl1) Or posl3 = 0 Then
                                    If posl3 <> 0 Then
                                        lItem.Data += HttpUtility.HtmlDecode(orgData.Substring(posl3 + _parseLink3.Length, posl1 - posl3 - _parseLink3.Length))
                                    Else
                                        lItem.Data += HttpUtility.HtmlDecode(orgData.Substring(0, posl1))
                                    End If
                                End If
                                posl2 = orgData.IndexOf(_parseLink2, posl1, StringComparison.Ordinal)
                                posl3 = orgData.IndexOf(_parseLink3, posl2, StringComparison.Ordinal)
                                lItem.Data += HttpUtility.HtmlDecode(orgData.Substring(posl2 + _parseLink2.Length, posl3 - posl2 - _parseLink2.Length))
                            Loop


                            lItem.Data += HttpUtility.HtmlDecode(orgData.Substring(posl3 + _parseLink3.Length))
                        End If
                    Catch ex As Exception
                        _signed = False
                        TraceOut("DM-Link:" + strPost)
                        Return "GetDirectMessage -> Err: Can't parse links"
                    End Try

                    'Get Date
                    pos1 = strPost.IndexOf(_parseDate, pos2, StringComparison.Ordinal)
                    If pos1 > -1 Then
                        Try
                            pos2 = strPost.IndexOf(_parseDateTo, pos1 + _parseDate.Length, StringComparison.Ordinal)
                            lItem.PDate = DateTime.ParseExact(strPost.Substring(pos1 + _parseDate.Length, pos2 - pos1 - _parseDate.Length), "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz", System.Globalization.DateTimeFormatInfo.InvariantInfo, Globalization.DateTimeStyles.None)
                            tmpDate = lItem.PDate
                        Catch ex As Exception
                            _signed = False
                            TraceOut("DM-Date:" + strPost)
                            Return "GetTimeline -> Err: Can't get date."
                        End Try
                    Else
                        lItem.PDate = tmpDate
                    End If


                    'Get Fav
                    'pos1 = strPost.IndexOf(_parseStar, pos2)
                    'pos2 = strPost.IndexOf("""", pos1 + _parseStar.Length)
                    'If strPost.Substring(pos1 + _parseStar.Length, pos2 - pos1 - _parseStar.Length) = "empty" Then
                    '    lItem.Fav = False
                    'Else
                    '    lItem.Fav = True
                    'End If
                    lItem.Fav = False

                    'Imageの取得
                    If imgKeys.Contains(lItem.ImageUrl) = False Then
                        If GetIconImage(lItem.ImageUrl, imgKeys, imgs) = False Then lItem.ImageUrl = ""
                    End If

                    If _endingFlag Then Return ""

                    links.Add(lItem.Id)
                    tLine.Add(lItem)
                End If
            End If
        Next

        Dim getCnt As Integer

        getCnt = tLine.Count - listCnt
        If getCnt = 20 Then
            endPage += 1
        End If

        Return ""
    End Function

    Private Function GetIconImage(ByVal pathUrl As String, ByVal imgKeys As Collections.Specialized.StringCollection, ByVal imgs As ImageList) As Boolean
        If _endingFlag Then Exit Function
        If _getIcon = False Then Exit Function

        Dim resStatus As String = ""
        Dim img As Image = Nothing

        img = TryCast(_mySock.GetWebResponse(pathUrl, resStatus, MySocket.REQ_TYPE.ReqGETBinary), System.Drawing.Image)
        If img Is Nothing Then Return False

        If Not img Is Nothing Then
            imgKeys.Add(pathUrl)
            imgs.Images.Add(pathUrl, img)
            Return True
        Else
            Return False
        End If
    End Function

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

    Public Function PostStatus(ByVal postStr As String, ByVal reply_to As Integer) As String

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
        Dim resMsg As String = DirectCast(_mySock.GetWebResponse("https://" + _hubServer + _statusUpdatePathAPI, resStatus, MySocket.REQ_TYPE.ReqPOSTAPI, dataStr), String)

        If resStatus.StartsWith("OK") Then
            Return _Outputz.Post(_mySock, postStr.Length)
        Else
            Return resStatus
        End If
    End Function

    Public Function RemoveStatus(ByVal id As String) As String
        If _endingFlag Then Return ""

        'データ部分の生成
        Dim resStatus As String = ""
        Dim resMsg As String = DirectCast(_mySock.GetWebResponse("https://" + _hubServer + _StDestroyPath + id + ".xml", resStatus, MySocket.REQ_TYPE.ReqPOSTAPI), String)

        If resMsg.StartsWith("<?xml") = False OrElse resStatus.StartsWith("OK") = False Then
            Return resStatus
        End If

        Return ""
    End Function

    Public Function RemoveDirectMessage(ByVal id As String) As String
        If _endingFlag Then Return ""

        'データ部分の生成
        Dim dataStr As String = _authKeyHeader + HttpUtility.UrlEncode(_authKey)
        Dim resStatus As String = ""
        Dim resMsg As String = DirectCast(_mySock.GetWebResponse("https://" + _hubServer + _DMDestroyPath + id, resStatus, MySocket.REQ_TYPE.ReqPOSTEncodeProtoVer3, dataStr, "https://" + _baseUrlStr + _DMPathRcv), String)

        If resMsg <> " " OrElse resStatus.StartsWith("OK") = False Then
            Return resStatus
        End If

        Return ""
    End Function

	' Contributed by shuyoko <http://twitter.com/shuyoko> BEGIN:
    Public Function GetBlackFavId(ByVal id As String, ByRef blackid As String) As String
        Dim dataStr As String = _authKeyHeader + HttpUtility.UrlEncode(_authKey)
        Dim resStatus As String = ""
        Dim resMsg As String = DirectCast(_mySock.GetWebResponse("http://blavotter.hocha.org/blackfav/getblack.php?format=simple&id=" + id, resStatus, MySocket.REQ_TYPE.ReqGET), String)

        If resStatus.StartsWith("OK") = False Then
            Return resStatus
        End If

        blackid = resMsg

        Return ""

    End Function
    ' Contributed by shuyoko <http://twitter.com/shuyoko> END.

    Public Function PostFavAdd(ByVal id As String) As String
        If _endingFlag Then Return ""

        'データ部分の生成
        Dim dataStr As String = _authKeyHeader + HttpUtility.UrlEncode(_authKey)
        Dim resStatus As String = ""
        Dim resMsg As String = DirectCast(_mySock.GetWebResponse("https://" + _hubServer + _postFavAddPath + id, resStatus, MySocket.REQ_TYPE.ReqPOSTEncodeProtoVer2, dataStr), String)

        If resMsg.StartsWith("$") = False And resMsg <> " " Then
            Return resStatus
        End If

        If _restrictFavCheck = False Then Return ""

        'http://twitter.com/statuses/show/id.xml APIを発行して本文を取得

        resMsg = DirectCast(_mySock.GetWebResponse("https://" + _hubServer + _ShowStatus + id + ".xml", resStatus, MySocket.REQ_TYPE.ReqPOSTEncodeProtoVer2), String)

        Dim rd As Xml.XmlTextReader = New Xml.XmlTextReader(New System.IO.StringReader(resMsg))

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

        Return ""
    End Function

    Public Function PostFavRemove(ByVal id As String) As String
        If _endingFlag Then Return ""

        'データ部分の生成
        Dim dataStr As String = _authKeyHeader + HttpUtility.UrlEncode(_authKey)
        Dim resStatus As String = ""
        Dim resMsg As String = DirectCast(_mySock.GetWebResponse("https://" + _hubServer + _postFavRemovePath + id, resStatus, MySocket.REQ_TYPE.ReqPOSTEncodeProtoVer2, dataStr), String)

        If resMsg.StartsWith("$") = False And resMsg <> " " Then
            Return resStatus
        End If

        Return ""
    End Function

    Public Function GetFollowers() As String
        Dim resStatus As String = ""
        Dim resMsg As String = ""
        Dim i As Integer = 0

        follower.Clear()
        follower.Add(_uid)
        Do While True
            i += 1
            resMsg = DirectCast(_mySock.GetWebResponse("https://" + _hubServer + _GetFollowers + _pageQry + i.ToString, resStatus, MySocket.REQ_TYPE.ReqPOSTAPI), String)
            If resStatus.StartsWith("OK") = False Then
                follower.Clear()
                follower.Add(_uid)  '途中で失敗したら片思い表示しない
                Return resStatus
            End If

            Using rd As Xml.XmlTextReader = New Xml.XmlTextReader(New System.IO.StringReader(resMsg))
                Dim lc As Integer = 0

                rd.Read()
                While rd.EOF = False
                    If rd.IsStartElement("screen_name") Then
                        follower.Add(rd.ReadElementString("screen_name"))
                        lc += 1
                    Else
                        rd.Read()
                    End If
                End While
                If lc = 0 Then Exit Do
            End Using

        Loop

        Return ""
    End Function

    Public Property Username() As String
        Get
            Return _uid
        End Get
        Set(ByVal value As String)
            _uid = value
            _mySock.Username = _uid
        End Set
    End Property

    Public Property Password() As String
        Get
            Return _pwd
        End Get
        Set(ByVal value As String)
            _pwd = value
            _mySock.Password = _pwd
            _mySock.CreateCredentialInfo()
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

    Public Property Ending() As Boolean
        Get
            Return _endingFlag
        End Get
        Set(ByVal value As Boolean)
            _endingFlag = value
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

        resMsg = DirectCast(_mySock.GetWebResponse(wedataUrl, resStatus, timeOut:=5 * 1000), String) 'タイムアウト時間を5秒に設定
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
    Public Sub CreateNewSocket()
        _mySock = Nothing
        _mySock = New MySocket("UTF-8", Username, Password, _proxyType, _proxyAddress, _proxyPort, _proxyUser, _proxyPassword)
        _signed = False
    End Sub

    Public Function GetReplyStatusID(ByVal id As Integer) As Integer
        Dim resStatus As String = ""
        Dim resMsg As String = DirectCast(_mySock.GetWebResponse("https://" + _hubServer + _ShowStatus + id.ToString() + ".xml", resStatus, MySocket.REQ_TYPE.ReqPOSTEncodeProtoVer2), String)
        Dim xdoc As Xml.XmlDocument = New Xml.XmlDocument()
        Try
            xdoc.LoadXml(resMsg)
            If xdoc.SelectSingleNode("/status/in_reply_to_status_id").InnerXml <> "" Then
                Return Integer.Parse(xdoc.SelectSingleNode("/status/in_reply_to_status_id").ChildNodes(0).Value)
            Else
                Return -1
            End If
        Catch ex As Xml.XmlException
            Return -2
        End Try
    End Function

    Public Function MakeShortUrl(ByVal ConverterType As UrlConverter, ByVal SrcUrl As String) As String
        Dim ret As String = ""
        Dim resStatus As String = ""

        For Each svc As String In _ShortUrlService
            If SrcUrl.StartsWith(svc) Then
                Return "Can't convert"
            End If
        Next

        SrcUrl = HttpUtility.UrlEncode(SrcUrl)
        Select Case ConverterType
            Case UrlConverter.TinyUrl       'tinyurl
                If SrcUrl.StartsWith("http") Then
                    If SrcUrl.StartsWith("http://tinyurl.com/") Then
                        Return "Can't convert"
                    End If
                    Try
                        ret = DirectCast(_mySock.GetWebResponse("http://tinyurl.com/api-create.php?url=" + SrcUrl, resStatus, MySocket.REQ_TYPE.ReqPOSTEncode), String)
                    Catch ex As Exception
                        Return "Can't convert"
                    End Try
                End If
                If Not ret.StartsWith("http://tinyurl.com/") Then
                    Return "Can't convert"
                End If
            Case UrlConverter.Isgd
                If SrcUrl.StartsWith("http") Then
                    If SrcUrl.StartsWith("http://is.gd/") Then
                        Return "Can't convert"
                    End If
                    Try
                        ret = DirectCast(_mySock.GetWebResponse("http://is.gd/api.php?longurl=" + SrcUrl, resStatus, MySocket.REQ_TYPE.ReqPOSTEncode), String)
                    Catch ex As Exception
                        Return "Can't convert"
                    End Try
                End If
                If Not ret.StartsWith("http://is.gd/") Then
                    Return "Can't convert"
                End If
        End Select

        Return ret
    End Function

    Public Function GetVersionInfo() As String
        Dim resStatus As String = ""
        Return DirectCast(_mySock.GetWebResponse("http://tween.sourceforge.jp/version2.txt?" + Now.ToString("yyMMddHHmmss") + Environment.TickCount.ToString(), resStatus), String)
    End Function

    Public Function GetTweenBinary(ByVal strVer As String) As String
        Dim resStatus As String = ""
        Dim ret As String = ""
        ret = DirectCast(_mySock.GetWebResponse("http://tween.sourceforge.jp/Tween" + strVer + ".gz?" + Now.ToString("yyMMddHHmmss") + Environment.TickCount.ToString(), resStatus, MySocket.REQ_TYPE.ReqGETFile), String)
        If ret.Length = 0 Then
            '取得OKなら、続いてresources.dllダウンロード
            Return Me.GetTweenResourcesDll(strVer)
        Else
            Return ret
        End If
    End Function

    Public Function GetTweenUpBinary() As String
        Dim resStatus As String = ""
        Return DirectCast(_mySock.GetWebResponse("http://tween.sourceforge.jp/TweenUp.gz?" + Now.ToString("yyMMddHHmmss") + Environment.TickCount.ToString(), resStatus, MySocket.REQ_TYPE.ReqGETFileUp), String)
    End Function

    Public Function GetTweenResourcesDll(ByVal strver As String) As String
        Dim resStatus As String = ""
        Return DirectCast(_mySock.GetWebResponse("http://tween.sourceforge.jp/TweenRes" + strver + ".gz?" + Now.ToString("yyMMddHHmmss") + Environment.TickCount.ToString(), resStatus, MySocket.REQ_TYPE.ReqGETFileRes), String)
    End Function

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
        sw.WriteLine("    Public  _parseImg As String = " + Chr(34) + _parseImg.Replace(Chr(34), Chr(34) + Chr(34)) + Chr(34))
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
        sw.WriteLine("End Module")

        sw.Close()
        MessageBox.Show("解析キー情報定義ファイル AnalyzeKey.vbを生成しました")

    End Sub
#End If
End Class

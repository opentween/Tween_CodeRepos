﻿Public Module AnalyzeKey
'    このファイルはデバッグビルドのTweenにより自動作成されました   作成日時  2009/01/15 12:50:53

    Public _splitPost As String = "<tr id=""status_"
    Public _splitPostRecent As String = "<tr id=""status_"
    Public _statusIdTo As String = """"
    Public _splitDM As String = "<tr id=""direct_message_"
    Public _parseName As String = "://twitter.com/"
    Public _parseNameTo As String = """"
    Public _parseNick As String = "<img alt="""
    Public _parseNickTo As String = """"
    Public  _parseImg As String = "src="""
    Public _parseImgTo As String = """"
    Public _parseMsg1 As String = "<span class=""entry-content"">"
    Public _parseMsg2 As String = "</span>"
    Public _parseDM1 As String = "<span class=""entry-content"">"
    Public _parseDM11 As String = "<span class=""protected"">"
    Public _parseDM2 As String = "</span>"
    Public _parseDate As String = "<span class=""published"" title="""
    Public _parseDateTo As String = """"
    Public _getAuthKey As String = "<input name=""authenticity_token"" value="""
    Public _getAuthKeyTo As String = """"
    Public _parseStar As String = "<a href=""#"" class="""
    Public _parseStarTo As String = """"
    Public _parseStarEmpty As String = "non-fav"
    Public _followerList As String = "<select id=""direct_message_user_id"" name=""user[id]""><option value="""" selected=""selected"">"
    Public _followerMbr1 As String = "/option>"
    Public _followerMbr2 As String = """>"
    Public _followerMbr3 As String = "<"
    Public _getInfoTwitter As String = "<div id=""top_alert"">"
    Public _getInfoTwitterTo As String = "</div>"
    Public _isProtect As String = "<img alt=""Icon_lock"""
    Public _isReplyEng As String = ">in reply to "
    Public _isReplyJpn As String = ">u8fd4u4fe1: "
    Public _isReplyTo As String = "<"
    Public _parseProtectMsg1 As String = "."" />"
    Public _parseProtectMsg2 As String = "<span class=""meta entry-meta"">"
    Public _parseDMcountFrom As String = "<a href=""/direct_messages"" id=""direct_messages_tab""><span id=""message_count"" class=""stat_count"">"
    Public _parseDMcountTo As String = "</span>"
    Public _parseSourceFrom As String = "<span>from <a href="
    Public _parseSource2 As String = """>"
    Public _parseSourceTo As String = "</a>"
End Module

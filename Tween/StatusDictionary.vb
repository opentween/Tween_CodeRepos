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

Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports Tween.TweenCustomControl


Public Class PostClass
    Private _Nick As String
    Private _Data As String
    Private _ImageUrl As String
    Private _Name As String
    Private _PDate As Date
    Private _Id As Long
    Private _IsFav As Boolean
    Private _OrgData As String
    Private _IsRead As Boolean
    Private _IsReply As Boolean
    Private _IsProtect As Boolean
    Private _IsOWL As Boolean
    Private _IsMark As Boolean
    Private _InReplyToUser As String
    Private _InReplyToId As Long
    Private _Source As String
    Private _ReplyToList As List(Of Long)

    Public Sub New(ByVal Nickname As String, _
            ByVal Data As String, _
            ByVal OriginalData As String, _
            ByVal ImageUrl As String, _
            ByVal Name As String, _
            ByVal PDate As Date, _
            ByVal Id As Long, _
            ByVal IsFav As Boolean, _
            ByVal IsRead As Boolean, _
            ByVal IsReply As Boolean, _
            ByVal IsProtect As Boolean, _
            ByVal IsOwl As Boolean, _
            ByVal IsMark As Boolean, _
            ByVal InReplyToUser As String, _
            ByVal InReplyToId As Long, _
            ByVal Source As String, _
            ByVal ReplyToList As List(Of Long))
        _Nick = Nickname
        _Data = Data
        _ImageUrl = ImageUrl
        _Name = Name
        _PDate = PDate
        _Id = Id
        _IsFav = IsFav
        _OrgData = OriginalData
        _IsRead = IsRead
        _IsReply = IsReply
        _IsProtect = IsProtect
        _IsOWL = IsOwl
        _IsMark = IsMark
        _InReplyToUser = InReplyToUser
        _InReplyToId = InReplyToId
        _Source = Source
        _ReplyToList = ReplyToList
    End Sub
    Public ReadOnly Property Nickname() As String
        Get
            Return _Nick
        End Get
    End Property
    Public ReadOnly Property Data() As String
        Get
            Return _Data
        End Get
    End Property
    Public ReadOnly Property ImageUrl() As String
        Get
            Return _ImageUrl
        End Get
    End Property
    Public ReadOnly Property Name() As String
        Get
            Return _Name
        End Get
    End Property
    Public ReadOnly Property PDate() As Date
        Get
            Return _PDate
        End Get
    End Property
    Public ReadOnly Property Id() As Long
        Get
            Return _Id
        End Get
    End Property
    Public Property IsFav() As Boolean
        Get
            Return _IsFav
        End Get
        Set(ByVal value As Boolean)
            _IsFav = value
        End Set
    End Property
    Public ReadOnly Property OriginalData() As String
        Get
            Return _OrgData
        End Get
    End Property
    Public Property IsRead() As Boolean
        Get
            Return _IsRead
        End Get
        Set(ByVal value As Boolean)
            _IsRead = value
        End Set
    End Property
    Public ReadOnly Property IsReply() As Boolean
        Get
            Return _IsReply
        End Get
    End Property
    Public ReadOnly Property IsProtect() As Boolean
        Get
            Return _IsProtect
        End Get
    End Property
    Public ReadOnly Property IsOwl() As Boolean
        Get
            Return _IsOWL
        End Get
    End Property
    Public Property IsMark() As Boolean
        Get
            Return _IsMark
        End Get
        Set(ByVal value As Boolean)
            _IsMark = value
        End Set
    End Property
    Public ReadOnly Property InReplyToUser() As String
        Get
            Return _InReplyToUser
        End Get
    End Property
    Public ReadOnly Property InReplyToId() As Long
        Get
            Return _InReplyToId
        End Get
    End Property
    Public ReadOnly Property Source() As String
        Get
            Return _Source
        End Get
    End Property
    Public ReadOnly Property ReplyToList() As List(Of Long)
        Get
            Return _ReplyToList
        End Get
    End Property
End Class

Public Class TabInformations
    '個別タブの情報をDictionaryで保持
    Private _sorter As ListViewItemComparerClass
    Private _tabs As New Dictionary(Of String, TabClass)
    'Private Class Statuses
    Private _statuses As Dictionary(Of Long, PostClass)
    Private _addedIds As List(Of Long)
    'Private _tabs As TabInformations
    'Private _statuses As Statuses

    Public Sub New()
        _sorter = New ListViewItemComparerClass
        _statuses = New Dictionary(Of Long, PostClass)
    End Sub

    Public Sub AddTab(ByVal TabText As String)
        _tabs.Add(TabText, New TabClass())
    End Sub

    Public Function ContainsTab(ByVal TabText As String) As Boolean
        Return _tabs.ContainsKey(TabText)
    End Function

    Public Function ContainsTab(ByVal ts As TabClass) As Boolean
        Return _tabs.ContainsValue(ts)
    End Function

    Public ReadOnly Property Tabs() As Dictionary(Of String, TabClass)
        Get
            Return _tabs
        End Get
    End Property

    Public ReadOnly Property KeysTab() As Collections.Generic.Dictionary(Of String, TabClass).KeyCollection
        Get
            Return _tabs.Keys
        End Get
    End Property

    Public Sub SortPosts()
        For Each key As String In _tabs.Keys
            _tabs(key).Sort(_sorter)
        Next
    End Sub

    Public ReadOnly Property Sorter() As ListViewItemComparerClass
        Get
            Return _sorter
        End Get
    End Property

    Public Property SortOrder() As SortOrder
        Get
            Return _sorter.Order
        End Get
        Set(ByVal value As SortOrder)
            _sorter.Order = value
        End Set
    End Property

    'Public WriteOnly Property Statuses() As Statuses
    '    Set(ByVal value As Statuses)
    '        _statuses = value
    '    End Set
    'End Property

    Private Function Distribute(ByVal AddedIDs As List(Of Long)) As List(Of Long)
        '各タブのフィルターと照合。合致したらタブにID追加
        Dim notifyIds As New List(Of Long)
        For Each id As Long In AddedIDs
            Dim post As PostClass = _statuses(id)
            Dim add As Boolean = False
            For Each tn As String In _tabs.Keys
                Dim rslt As HITRESULT = _tabs(tn).AddFiltered(post.Id, post.IsRead, post.Name, post.Data, post.OriginalData)
                If rslt = HITRESULT.CopyAndMark Then post.IsMark = True
                If rslt <> HITRESULT.None AndAlso _tabs(tn).Notify Then add = True
            Next
            If add Then notifyIds.Add(id)
        Next
        Me.SortPosts()
        Return notifyIds
    End Function

    Public Sub RemovePost(ByVal Id As Long)
        Dim post As PostClass = _statuses(Id)
        '各タブから該当ID削除
        For Each key As String In _tabs.Keys
            Dim tab As TabClass = _tabs(key)
            If tab.Contains(Id) Then
                If tab.UnreadManage AndAlso Not post.IsRead Then    '未読管理
                    tab.UnreadCount -= 1
                    Me.SetNextUnreadId(Id, tab)
                    tab.Remove(Id)
                End If
            End If
        Next
        _statuses.Remove(Id)
    End Sub

    Public Sub SetNextUnreadId(ByVal CurrentId As Long, ByVal Tab As TabClass)
        If Tab.OldestUnreadId = CurrentId Then     '次の未読探索
            If Tab.UnreadCount = 0 OrElse Tab.AllCount <= 1 Then
                Tab.OldestUnreadId = -1
            Else
                Dim idx As Integer = Tab.GetIndex(CurrentId)
                Dim toIdx As Integer = 0
                Dim stp As Integer = 1
                If _sorter.Order = Windows.Forms.SortOrder.Ascending Then
                    idx -= 1
                    toIdx = 0
                    stp = -1
                Else
                    idx += 1
                    toIdx = Tab.AllCount - 1
                    stp = 1
                End If
                For i As Integer = idx To toIdx Step stp
                    If Not _statuses(Tab.GetId(i)).IsRead Then
                        Tab.OldestUnreadId = Tab.GetId(i)
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Public Sub BeginUpdate()
        _addedIds = New List(Of Long)  'タブ追加用IDコレクション準備
    End Sub

    Public Function EndUpdate() As List(Of Long)
        If _addedIds Is Nothing Then Throw New Exception("You must call 'BeginUpdate' before to add.")
        Dim NotifyIds As List(Of Long) = Me.Distribute(_addedIds)    'タブに追加
        '_tabs.Sort()    'ソート
        _addedIds.Clear()
        _addedIds = Nothing
        Return NotifyIds     '通知用メッセージを戻す
    End Function

    Public Sub AddPost(ByRef Item As PostClass)
        If _addedIds Is Nothing Then Throw New Exception("You must call 'BeginUpdate' before to add.")
        _statuses.Add(Item.Id, Item)
        _addedIds.Add(Item.Id)
    End Sub

    Public ReadOnly Property Item(ByVal ID As Long) As PostClass
        Get
            Return _statuses(ID)
        End Get
    End Property
    'End Class
End Class

Public Class TabClass
    'Private _tabPage As System.Windows.Forms.TabPage
    'Private _listCustom As DetailsListView
    'Public colHd1 As System.Windows.Forms.ColumnHeader
    'Public colHd2 As System.Windows.Forms.ColumnHeader
    'Public colHd3 As System.Windows.Forms.ColumnHeader
    'Public colHd4 As System.Windows.Forms.ColumnHeader
    'Public colHd5 As System.Windows.Forms.ColumnHeader
    'Public idCol As System.Collections.Specialized.StringCollection
    'Private _tabName As String
    'Public sorter As ListViewItemComparer
    Private _unreadManage As Boolean
    Private _notify As Boolean
    Private _soundFile As String
    Private _filters As List(Of FiltersClass)
    'Private _modified As Boolean
    'Private _oldestUnreadItem As ListViewItem
    Private _oldestUnreadItem As Long     'ID
    Private _unreadCount As Integer
    Private _ids As List(Of Long)

    Public Sub New()
        _filters = New List(Of FiltersClass)
        _notify = True
        _soundFile = ""
        _unreadManage = True
        _ids = New List(Of Long)
    End Sub

    Public Sub Sort(ByVal Sorter As ListViewItemComparerClass)
        _ids.Sort(Sorter)
    End Sub

    Private Sub Add(ByVal ID As Long, ByVal Read As Boolean)
        If Me._ids.Contains(ID) Then Exit Sub

        Me._ids.Add(ID)

        If Not Read AndAlso Me._unreadManage Then
            Me._unreadCount += 1
            If Me._oldestUnreadItem = -1 Then
                Me._oldestUnreadItem = ID
            Else
                If ID < Me._oldestUnreadItem Then Me._oldestUnreadItem = ID
            End If
        End If
    End Sub

    Public Function AddFiltered(ByVal ID As Long, _
                                ByVal Read As Boolean, _
                                ByVal Name As String, _
                                ByVal Body As String, _
                                ByVal OrgData As String) As HITRESULT
        Dim rslt As HITRESULT = HITRESULT.None
        '全フィルタ評価（優先順位あり）
        For Each ft As FiltersClass In _filters
            Select Case ft.IsHit(Name, Body, OrgData)
                Case HITRESULT.None
                Case HITRESULT.Copy
                    If rslt <> HITRESULT.CopyAndMark Then rslt = HITRESULT.Copy
                Case HITRESULT.CopyAndMark
                    rslt = HITRESULT.CopyAndMark
                Case HITRESULT.Move
                    rslt = HITRESULT.Move
                    Exit For
            End Select
        Next

        If rslt <> HITRESULT.None Then Me.Add(ID, Read)

        Return rslt 'マーク付けは呼び出し元で行うこと
    End Function

    Public Sub Remove(ByVal Id As Long)
        If Not Me._ids.Contains(Id) Then Exit Sub

        Me._ids.Remove(Id)
    End Sub

    'Public Property TabPage() As TabPage
    '    Get
    '        Return _tabPage
    '    End Get
    '    Set(ByVal value As TabPage)
    '        _tabPage = value
    '    End Set
    'End Property

    'Public Property ListCustom() As DetailsListView
    '    Get
    '        Return _listCustom
    '    End Get
    '    Set(ByVal value As DetailsListView)
    '        _listCustom = value
    '    End Set
    'End Property

    Public Property UnreadManage() As Boolean
        Get
            Return _unreadManage
        End Get
        Set(ByVal value As Boolean)
            _unreadManage = value
        End Set
    End Property

    Public Property Notify() As Boolean
        Get
            Return _notify
        End Get
        Set(ByVal value As Boolean)
            _notify = value
        End Set
    End Property

    Public Property SoundFile() As String
        Get
            Return _soundFile
        End Get
        Set(ByVal value As String)
            _soundFile = value
        End Set
    End Property

    'Public Property Modified() As Boolean
    '    Get
    '        Return _modified
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _modified = value
    '    End Set
    'End Property

    Public Property OldestUnreadId() As Long
        Get
            Return _oldestUnreadItem
        End Get
        Set(ByVal value As Long)
            _oldestUnreadItem = value
        End Set
    End Property

    Public Property UnreadCount() As Integer
        Get
            Return _unreadCount
        End Get
        Set(ByVal value As Integer)
            _unreadCount = value
        End Set
    End Property

    Public ReadOnly Property AllCount() As Integer
        Get
            Return Me._ids.Count
        End Get
    End Property

    Public Property Filters() As List(Of FiltersClass)
        Get
            Return _filters
        End Get
        Set(ByVal value As List(Of FiltersClass))
            _filters = value
        End Set
    End Property

    Public Function Contains(ByVal ID As Long) As Boolean
        Return _ids.Contains(ID)
    End Function

    Public Sub ClearIDs()
        _ids.Clear()
        _unreadCount = 0
        _oldestUnreadItem = -1
    End Sub

    'Public ReadOnly Property StatusIDs() As List(Of Long)
    '    Get
    '        Return _ids
    '    End Get
    'End Property

    Public Function GetId(ByVal Index As Integer) As Long
        Return _ids(Index)
    End Function

    Public Function GetIndex(ByVal ID As Long) As Integer
        Return _ids.IndexOf(ID)
    End Function
End Class

Public Class FiltersClass
    Private _name As String
    Private _body As New List(Of String)
    Private _searchBoth As Boolean
    Private _moveFrom As Boolean
    Private _setMark As Boolean
    Private _searchUrl As Boolean
    Private _useRegex As Boolean

    Public Sub New(ByVal NameFilter As String, _
            ByVal BodyFilter As List(Of String), _
            ByVal SearchBoth As Boolean, _
            ByVal MoveFrom As Boolean, _
            ByVal SetMark As Boolean, _
            ByVal SearchUrl As Boolean, _
            ByVal UseRegex As Boolean)
        _name = NameFilter
        _body = BodyFilter
        _searchBoth = SearchBoth
        _moveFrom = MoveFrom
        _setMark = SetMark
        _searchUrl = SearchUrl
        _useRegex = UseRegex
        If _useRegex Then
            For Each bs As String In _body
                Try
                    Dim rgx As New System.Text.RegularExpressions.Regex(bs)
                Catch ex As Exception
                    Throw New Exception(My.Resources.ButtonOK_ClickText3 + ex.Message)
                    Exit Sub
                End Try
            Next
        End If
    End Sub

    Private Function MakeSummary() As String
        Dim fs As New System.Text.StringBuilder()
        If _searchBoth Then
            If _name <> "" Then
                fs.AppendFormat(My.Resources.SetFiltersText1, _name)
            Else
                fs.Append(My.Resources.SetFiltersText2)
            End If
        End If
        If _body.Count > 0 Then
            fs.Append(My.Resources.SetFiltersText3)
            For Each bf As String In _body
                fs.Append(bf)
                fs.Append(" ")
            Next
            fs.Length -= 1
            fs.Append(My.Resources.SetFiltersText4)
        End If
        fs.Append("(")
        If _searchBoth Then
            fs.Append(My.Resources.SetFiltersText5)
        Else
            fs.Append(My.Resources.SetFiltersText6)
        End If
        If _useRegex Then
            fs.Append(My.Resources.SetFiltersText7)
        End If
        If _searchUrl Then
            fs.Append(My.Resources.SetFiltersText8)
        End If
        If _moveFrom Then
            fs.Append(My.Resources.SetFiltersText9)
        ElseIf _setMark Then
            fs.Append(My.Resources.SetFiltersText10)
        Else
            fs.Append(My.Resources.SetFiltersText11)
        End If
        fs.Append(")")

        Return fs.ToString()
    End Function

    Public Property NameFilter() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Public Property BodyFilter() As List(Of String)
        Get
            Return _body
        End Get
        Set(ByVal value As List(Of String))
            _body = value
        End Set
    End Property

    Public Property SearchBoth() As Boolean
        Get
            Return _searchBoth
        End Get
        Set(ByVal value As Boolean)
            _searchBoth = value
        End Set
    End Property

    Public Property MoveFrom() As Boolean
        Get
            Return _moveFrom
        End Get
        Set(ByVal value As Boolean)
            _moveFrom = value
        End Set
    End Property

    Public Property SetMark() As Boolean
        Get
            Return _setMark
        End Get
        Set(ByVal value As Boolean)
            _setMark = value
        End Set
    End Property

    Public Property SearchUrl() As Boolean
        Get
            Return _searchUrl
        End Get
        Set(ByVal value As Boolean)
            _searchUrl = value
        End Set
    End Property

    Public Property UseRegex() As Boolean
        Get
            Return _useRegex
        End Get
        Set(ByVal value As Boolean)
            _useRegex = value
        End Set
    End Property

    Public ReadOnly Property Summary() As String
        Get
            Return MakeSummary()
        End Get
    End Property

    Public Function IsHit(ByVal Name As String, ByVal Body As String, ByVal OrgData As String) As HITRESULT
        Dim bHit As Boolean = True
        Dim tBody As String
        If _searchUrl Then
            tBody = OrgData
        Else
            tBody = Body
        End If
        If _searchBoth Then
            If _name = "" OrElse Name.Equals(_name, StringComparison.OrdinalIgnoreCase) Then
                For Each fs As String In _body
                    If _useRegex Then
                        If System.Text.RegularExpressions.Regex.IsMatch(tBody, fs, System.Text.RegularExpressions.RegexOptions.IgnoreCase) = False Then bHit = False
                    Else
                        If tBody.ToLower().Contains(fs.ToLower()) = False Then bHit = False
                    End If
                    If Not bHit Then Exit For
                Next
            Else
                bHit = False
            End If
        Else
            For Each fs As String In _body
                If _useRegex Then
                    If Not (System.Text.RegularExpressions.Regex.IsMatch(Name, fs, System.Text.RegularExpressions.RegexOptions.IgnoreCase) OrElse _
                            System.Text.RegularExpressions.Regex.IsMatch(tBody, fs, System.Text.RegularExpressions.RegexOptions.IgnoreCase)) Then bHit = False
                Else
                    If Not (Name.ToLower().Contains(fs.ToLower()) OrElse _
                            tBody.ToLower().Contains(fs.ToLower())) Then bHit = False
                End If
                If Not bHit Then Exit For
            Next
        End If
        If bHit Then
            If _setMark Then Return HITRESULT.CopyAndMark
            If _moveFrom Then Return HITRESULT.Move
            Return HITRESULT.Copy
        Else
            Return HITRESULT.None
        End If
    End Function
End Class


Public Class ListViewItemComparerClass
    Implements IComparer(Of Long)

    '''' <summary>
    '''' 比較する方法
    '''' </summary>
    'Public Enum ComparerMode
    '    [String]
    '    [Integer]
    '    DateTime
    '    None
    'End Enum

    'Private _column As Integer
    Private _order As SortOrder
    'Private _mode As ComparerMode
    'Private _columnModes() As ComparerMode

    '''' <summary>
    '''' 並び替えるListView列の番号
    '''' </summary>
    'Public Property Column() As Integer
    '    Get
    '        Return _column
    '    End Get
    '    Set(ByVal Value As Integer)
    '        If _column = Value Then
    '            If _order = SortOrder.Ascending Then
    '                _order = SortOrder.Descending
    '            Else
    '                If _order = SortOrder.Descending Then
    '                    _order = SortOrder.Ascending
    '                End If
    '            End If
    '        End If
    '        _column = Value
    '    End Set
    'End Property

    ''' <summary>
    ''' 昇順か降順か
    ''' </summary>
    Public Property Order() As SortOrder
        Get
            Return _order
        End Get
        Set(ByVal Value As SortOrder)
            _order = Value
        End Set
    End Property

    '''' <summary>
    '''' 並び替えの方法
    '''' </summary>
    'Public Property Mode() As ComparerMode
    '    Get
    '        Return _mode
    '    End Get
    '    Set(ByVal Value As ComparerMode)
    '        _mode = Value
    '    End Set
    'End Property

    '''' <summary>
    '''' 列ごとの並び替えの方法
    '''' </summary>
    'Public WriteOnly Property ColumnModes() As ComparerMode()
    '    Set(ByVal Value As ComparerMode())
    '        _columnModes = Value
    '    End Set
    'End Property

    ''' <summary>
    ''' ListViewItemComparerクラスのコンストラクタ
    ''' </summary>
    ''' <param name="col">並び替える列番号</param>
    ''' <param name="ord">昇順か降順か</param>
    ''' <param name="cmod">並び替えの方法</param>
    Public Sub New(ByVal ord As SortOrder)
        _order = ord
    End Sub

    Public Sub New()
        _order = SortOrder.Ascending
    End Sub

    'xがyより小さいときはマイナスの数、大きいときはプラスの数、
    '同じときは0を返す
    Public Function Compare(ByVal x As Long, ByVal y As Long) _
            As Integer Implements IComparer(Of Long).Compare
        Dim result As Integer = 0

        If x < y Then
            result = -1
        ElseIf x = y Then
            result = 0
        Else
            result = 1
        End If
        '降順の時は結果を+-逆にする
        If _order = SortOrder.Descending Then
            result = -result
        End If
        Return result

    End Function
End Class

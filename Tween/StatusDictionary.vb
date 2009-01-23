' Tween - Client of Twitter
' Copyright © 2007-2009 kiri_feather (@kiri_feather) <kiri_feather@gmail.com>
'           © 2008-2009 Moz (@syo68k) <http://iddy.jp/profile/moz/>
'           © 2008-2009 takeshik (@takeshik) <http://www.takeshik.org/>
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
    Private _ReplyToList As New List(Of String)
    Private _IsMe As Boolean
    Private _ImageIndex As Integer
    Private _IsDm As Boolean

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
            ByVal ReplyToList As List(Of String), _
            ByVal IsMe As Boolean, _
            ByVal ImageIndex As Integer, _
            ByVal IsDm As Boolean)
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
        _IsMe = IsMe
        _ImageIndex = ImageIndex
        _IsDm = IsDm
    End Sub

    Public Sub New()
    End Sub

    Public Property Nickname() As String
        Get
            Return _Nick
        End Get
        Set(ByVal value As String)
            _Nick = value
        End Set
    End Property
    Public Property Data() As String
        Get
            Return _Data
        End Get
        Set(ByVal value As String)
            _Data = value
        End Set
    End Property
    Public Property ImageUrl() As String
        Get
            Return _ImageUrl
        End Get
        Set(ByVal value As String)
            _ImageUrl = value
        End Set
    End Property
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property
    Public Property PDate() As Date
        Get
            Return _PDate
        End Get
        Set(ByVal value As Date)
            _PDate = value
        End Set
    End Property
    Public Property Id() As Long
        Get
            Return _Id
        End Get
        Set(ByVal value As Long)
            _Id = value
        End Set
    End Property
    Public Property IsFav() As Boolean
        Get
            Return _IsFav
        End Get
        Set(ByVal value As Boolean)
            _IsFav = value
        End Set
    End Property
    Public Property OriginalData() As String
        Get
            Return _OrgData
        End Get
        Set(ByVal value As String)
            _OrgData = value
        End Set
    End Property
    Public Property IsRead() As Boolean
        Get
            Return _IsRead
        End Get
        Set(ByVal value As Boolean)
            _IsRead = value
        End Set
    End Property
    Public Property IsReply() As Boolean
        Get
            Return _IsReply
        End Get
        Set(ByVal value As Boolean)
            _IsReply = value
        End Set
    End Property
    Public Property IsProtect() As Boolean
        Get
            Return _IsProtect
        End Get
        Set(ByVal value As Boolean)
            _IsProtect = value
        End Set
    End Property
    Public Property IsOwl() As Boolean
        Get
            Return _IsOWL
        End Get
        Set(ByVal value As Boolean)
            _IsOWL = value
        End Set
    End Property
    Public Property IsMark() As Boolean
        Get
            Return _IsMark
        End Get
        Set(ByVal value As Boolean)
            _IsMark = value
        End Set
    End Property
    Public Property InReplyToUser() As String
        Get
            Return _InReplyToUser
        End Get
        Set(ByVal value As String)
            _InReplyToUser = value
        End Set
    End Property
    Public Property InReplyToId() As Long
        Get
            Return _InReplyToId
        End Get
        Set(ByVal value As Long)
            _InReplyToId = value
        End Set
    End Property
    Public Property Source() As String
        Get
            Return _Source
        End Get
        Set(ByVal value As String)
            _Source = value
        End Set
    End Property
    Public Property ReplyToList() As List(Of String)
        Get
            Return _ReplyToList
        End Get
        Set(ByVal value As List(Of String))
            _ReplyToList = value
        End Set
    End Property
    Public Property IsMe() As Boolean
        Get
            Return _IsMe
        End Get
        Set(ByVal value As Boolean)
            _IsMe = value
        End Set
    End Property
    Public Property ImageIndex() As Integer
        Get
            Return _ImageIndex
        End Get
        Set(ByVal value As Integer)
            _ImageIndex = value
        End Set
    End Property
    Public Property IsDm() As Boolean
        Get
            Return _IsDm
        End Get
        Set(ByVal value As Boolean)
            _IsDm = value
        End Set
    End Property
End Class

Public Class TabInformations
    '個別タブの情報をDictionaryで保持
    Private _sorter As IdComparerClass
    Private _tabs As New Dictionary(Of String, TabClass)
    Private _statuses As Dictionary(Of Long, PostClass) = New Dictionary(Of Long, PostClass)
    Private _addedIds As List(Of Long)
    'Private _editMode As EDITMODE

    '発言の追加は4段階
    'BeginUpdate -> AddPost(複数回) -> EndUpdate          -> SubmitUpdate
    '準備        -> 発言Dicに追加   -> 仮振分・Notify決定 -> 振分確定(UIと整合とる)
    '（EndUpdateまでは裏スレッドで処理）

    'トランザクション用
    Private _addCount As Integer
    Private _soundFile As String
    Private _notifyPosts As List(Of PostClass)
    Private ReadOnly LockObj As New Object

    Private Shared _instance As TabInformations = New TabInformations

    Public Enum EDITMODE
        Post
        Dm
    End Enum

    Private Sub New()
        _sorter = New IdComparerClass(Me)
    End Sub

    Public Shared Function GetInstance() As TabInformations
        Return _instance    'singleton
    End Function

    Public Sub AddTab(ByVal TabName As String)
        _tabs.Add(TabName, New TabClass())
    End Sub

    Public Sub AddTab(ByVal TabName As String, ByVal Tab As TabClass)
        _tabs.Add(TabName, Tab)
    End Sub

    Public Sub RemoveTab(ByVal TabName As String)
        If TabName.Equals("Recent") OrElse _
           TabName.Equals("Reply") OrElse _
           TabName.Equals("Direct") Then Exit Sub '念のため

        For idx As Integer = 0 To _tabs(TabName).AllCount - 1
            Dim exist As Boolean = False
            Dim Id As Long = _tabs(TabName).GetId(idx)
            For Each key As String In _tabs.Keys
                If Not key.Equals(TabName) AndAlso Not key.Equals("Direct") Then
                    If _tabs(key).Contains(Id) Then
                        exist = True
                        Exit For
                    End If
                End If
            Next
            If Not exist Then
                _tabs("Recent").Add(_tabs(TabName).GetId(idx), _statuses(Id).IsRead)
            End If
        Next

        _tabs.Remove(TabName)
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

    Public ReadOnly Property Sorter() As IdComparerClass
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

    Public Property SortMode() As IdComparerClass.ComparerMode
        Get
            Return _sorter.Mode
        End Get
        Set(ByVal value As IdComparerClass.ComparerMode)
            _sorter.Mode = value
        End Set
    End Property

    Public Sub ToggleSortOrder(ByVal SortMode As IdComparerClass.ComparerMode)
        If _sorter.Mode = SortMode Then
            If _sorter.Order = Windows.Forms.SortOrder.Ascending Then
                _sorter.Order = Windows.Forms.SortOrder.Descending
            Else
                _sorter.Order = Windows.Forms.SortOrder.Ascending
            End If
        Else
            _sorter.Mode = SortMode
            _sorter.Order = Windows.Forms.SortOrder.Ascending
        End If
        Me.SortPosts()
    End Sub

    Public Sub RemovePost(ByVal Id As Long)
        Dim post As PostClass = _statuses(Id)
        '各タブから該当ID削除
        For Each key As String In _tabs.Keys
            Dim tab As TabClass = _tabs(key)
            If tab.Contains(Id) Then
                If tab.UnreadManage AndAlso Not post.IsRead Then    '未読管理
                    tab.UnreadCount -= 1
                    Me.SetNextUnreadId(Id, tab)
                End If
                tab.Remove(Id)
            End If
        Next
        _statuses.Remove(Id)
    End Sub

    Public Sub SetNextUnreadId(ByVal CurrentId As Long, ByVal Tab As TabClass)
        'CurrentID:今既読にしたID(OldestIDの可能性あり)
        '最古未読が設定されていて、既読の場合（1発言以上存在）
        If Tab.OldestUnreadId > -1 AndAlso _
           _statuses.ContainsKey(Tab.OldestUnreadId) AndAlso _
           _statuses.Item(Tab.OldestUnreadId).IsRead AndAlso _
           _sorter.Mode = IdComparerClass.ComparerMode.Id Then     '次の未読探索
            If Tab.UnreadCount = 0 Then
                '未読数０→最古未読なし
                Tab.OldestUnreadId = -1
            ElseIf Tab.OldestUnreadId = CurrentId Then
                '最古IDを既読にしたタイミング→次のIDから続けて探索
                Dim idx As Integer = Tab.GetIndex(CurrentId)
                If idx > -1 Then
                    '続きから探索
                    FindUnreadId(idx, Tab)
                Else
                    '頭から探索
                    FindUnreadId(-1, Tab)
                End If
            Else
                '頭から探索
                FindUnreadId(-1, Tab)
            End If
        Else
            '頭から探索
            FindUnreadId(-1, Tab)
        End If
    End Sub

    Private Sub FindUnreadId(ByVal StartIdx As Integer, ByVal Tab As TabClass)
        If Tab.AllCount = 0 Then
            Tab.OldestUnreadId = -1
            Exit Sub
        End If
        Dim toIdx As Integer = 0
        Dim stp As Integer = 1
        If _sorter.Order = Windows.Forms.SortOrder.Ascending Then
            If StartIdx = -1 Then
                StartIdx = 0
            Else
                StartIdx += 1
                If StartIdx > Tab.AllCount - 1 Then StartIdx = Tab.AllCount - 1 '念のため
            End If
            toIdx = Tab.AllCount - 1
            If toIdx < 0 Then toIdx = 0 '念のため
            stp = 1
        Else
            If StartIdx = -1 Then
                StartIdx = Tab.AllCount - 1
            Else
                StartIdx -= 1
            End If
            If StartIdx < 0 Then StartIdx = 0 '念のため
            toIdx = 0
            stp = -1
        End If
        For i As Integer = StartIdx To toIdx Step stp
            If Not _statuses(Tab.GetId(i)).IsRead Then
                Tab.OldestUnreadId = Tab.GetId(i)
                Exit For
            End If
        Next
    End Sub

    'Public Sub BeginUpdate(ByVal EditMode As EDITMODE)
    '    If _addedIds IsNot Nothing Then Throw New Exception("You must call 'EndUpdate' before begin update.")

    '    _editMode = EditMode

    '    '初期化
    '    _addedIds = New List(Of Long)  'タブ追加用IDコレクション準備
    '    If _notifyPosts IsNot Nothing Then
    '        _notifyPosts.Clear()
    '        _notifyPosts = Nothing
    '    End If
    '    _soundFile = ""
    'End Sub

    Public Function DistributePosts() As Integer
        SyncLock LockObj
            '戻り値は追加件数
            If _addedIds Is Nothing Then Exit Function
            If _addedIds.Count = 0 Then Exit Function

            If _notifyPosts Is Nothing Then _notifyPosts = New List(Of PostClass)
            Me.Distribute()    'タブに仮振分
            _addCount = _addedIds.Count
            _addedIds.Clear()
            _addedIds = Nothing     '後始末
            Return _addCount     '件数
        End SyncLock
    End Function

    Public Function SubmitUpdate(ByRef soundFile As String, ByRef notifyPosts As PostClass()) As Integer
        '注：メインスレッドから呼ぶこと
        SyncLock LockObj
            If _notifyPosts Is Nothing Then
                soundFile = ""
                notifyPosts = Nothing
                Return 0
            End If

            For Each key As String In _tabs.Keys
                _tabs(key).AddSubmit()  '振分確定（各タブに反映）
            Next
            Me.SortPosts()

            soundFile = String.Copy(_soundFile)
            _soundFile = ""
            notifyPosts = _notifyPosts.ToArray()
            _notifyPosts.Clear()
            _notifyPosts = Nothing
            Dim retCnt As Integer = _addCount
            _addCount = 0
            Return retCnt    '件数（EndUpdateの戻り値と同じ）
        End SyncLock
    End Function

    Private Sub Distribute()
        '各タブのフィルターと照合。合致したらタブにID追加
        '通知メッセージ用に、表示必要な発言リストと再生サウンドを返す
        'notifyPosts = New List(Of PostClass)
        For Each id As Long In _addedIds
            Dim post As PostClass = _statuses(id)
            If Not post.IsDm Then
                Dim add As Boolean = False  '通知リスト追加フラグ
                Dim mv As Boolean = False   '移動フラグ（Recent追加有無）
                For Each tn As String In _tabs.Keys
                    Dim rslt As HITRESULT = _tabs(tn).AddFiltered(post.Id, post.IsRead, post.Name, post.Data, post.OriginalData)
                    If rslt <> HITRESULT.None Then
                        If rslt = HITRESULT.CopyAndMark Then post.IsMark = True 'マークあり
                        If rslt = HITRESULT.Move Then mv = True '移動
                        If _tabs(tn).Notify Then add = True '通知あり
                        If Not _tabs(tn).SoundFile.Equals("") AndAlso _soundFile.Equals("") Then
                            _soundFile = _tabs(tn).SoundFile 'wavファイル（未設定の場合のみ）
                        End If
                    End If
                Next
                If Not mv Then  '移動されなかったらRecentに追加
                    _tabs("Recent").Add(post.Id, post.IsRead)
                    If Not _tabs("Recent").SoundFile.Equals("") AndAlso _soundFile.Equals("") Then _soundFile = _tabs("Recent").SoundFile
                    If _tabs("Recent").Notify Then add = True
                End If
                If post.IsReply Then    'ReplyだったらReplyタブに追加
                    _tabs("Reply").Add(post.Id, post.IsRead)
                    If Not _tabs("Reply").SoundFile.Equals("") Then _soundFile = _tabs("Reply").SoundFile
                    If _tabs("Reply").Notify Then add = True
                End If
                If add Then _notifyPosts.Add(post)
            Else
                _tabs("Direct").Add(post.Id, post.IsRead)
                If _tabs("Direct").Notify Then _notifyPosts.Add(post)
                _soundFile = _tabs("Direct").SoundFile
            End If
        Next
    End Sub

    Public Sub AddPost(ByVal Item As PostClass)
        SyncLock LockObj
            If _addedIds Is Nothing Then _addedIds = New List(Of Long) 'タブ追加用IDコレクション準備
            _statuses.Add(Item.Id, Item)    'DMと区別しない？
            _addedIds.Add(Item.Id)
        End SyncLock
    End Sub

    Public Sub SetRead(ByVal Read As Boolean, ByVal TabName As String, ByVal Index As Integer)
        'Read:True=既読へ　False=未読へ
        Dim tb As TabClass = _tabs(TabName)

        If tb.UnreadManage = False Then Exit Sub '未読管理していなければ終了

        Dim Id As Long = tb.GetId(Index)

        If _statuses(Id).IsRead.Equals(Read) Then Exit Sub '状態変更なければ終了

        _statuses(Id).IsRead = Read '指定の状態に変更

        If Read Then
            tb.UnreadCount -= 1
            Me.SetNextUnreadId(Id, tb)  '次の未読セット
            '他タブの最古未読ＩＤはタブ切り替え時に。
            For Each key As String In _tabs.Keys
                If Not key.Equals(TabName) AndAlso _tabs(key).UnreadManage AndAlso _tabs(key).Contains(Id) Then
                    _tabs(key).UnreadCount -= 1
                    If _tabs(key).OldestUnreadId = Id Then _tabs(key).OldestUnreadId = -1
                End If
            Next
        Else
            tb.UnreadCount += 1
            If tb.OldestUnreadId > Id OrElse tb.OldestUnreadId = -1 Then tb.OldestUnreadId = Id
            For Each key As String In _tabs.Keys
                If Not key.Equals(TabName) AndAlso _tabs(key).UnreadManage AndAlso _tabs(key).Contains(Id) Then
                    _tabs(key).UnreadCount += 1
                    If _tabs(key).OldestUnreadId > Id Then _tabs(key).OldestUnreadId = Id
                End If
            Next
        End If
    End Sub

    Public ReadOnly Property Item(ByVal ID As Long) As PostClass
        Get
            Return _statuses(ID)
        End Get
    End Property

    Public ReadOnly Property Item(ByVal TabName As String, ByVal Index As Integer) As PostClass
        Get
            Return _statuses(_tabs(TabName).GetId(Index))
        End Get
    End Property

    Public Sub SetUnreadManage(ByVal Manage As Boolean)
        If Manage Then
            For Each key As String In _tabs.Keys
                Dim tb As TabClass = _tabs(key)
                If tb.UnreadManage Then
                    Dim cnt As Integer = 0
                    Dim oldest As Long = Long.MaxValue
                    For Each id As Long In tb.BackupIds
                        If Not _statuses(id).IsRead Then
                            cnt += 1
                            If oldest > id Then oldest = id
                        End If
                    Next
                    tb.OldestUnreadId = oldest
                    tb.UnreadCount = cnt
                End If
            Next
        Else
            For Each key As String In _tabs.Keys
                Dim tb As TabClass = _tabs(key)
                If tb.UnreadManage AndAlso tb.UnreadCount > 0 Then
                    tb.UnreadCount = 0
                    tb.OldestUnreadId = -1
                End If
            Next
        End If
    End Sub

    Public Sub RenameTab(ByVal Original As String, ByVal NewName As String)
        Dim tb As TabClass = _tabs(Original)
        _tabs.Remove(Original)
        _tabs.Add(NewName, tb)
    End Sub

    Public Sub FilterAll()
        Dim tbr As TabClass = _tabs("Recent")
        For Each key As String In _tabs.Keys
            Dim tb As TabClass = _tabs(key)
            If tb.FilterModified Then
                tb.FilterModified = False
                Dim orgIds() As Long = tb.BackupIds()
                tb.ClearIDs()
                ''''''''''''''フィルター前のIDsを退避。どのタブにも含まれないidはrecentへ追加
                ''''''''''''''moveフィルターにヒットした際、recentに該当あればrecentから削除
                For Each id As Long In _statuses.Keys
                    Dim post As PostClass = _statuses.Item(id)
                    If post.IsDm Then Continue For
                    Dim rslt As HITRESULT = tb.AddFiltered(post.Id, post.IsRead, post.Name, post.Data, post.OriginalData)
                    Select Case rslt
                        Case HITRESULT.CopyAndMark
                            post.IsMark = True 'マークあり
                        Case HITRESULT.Move
                            tbr.Remove(post.Id)
                    End Select
                Next
                For Each id As Long In orgIds
                    Dim hit As Boolean = False
                    For Each tkey As String In _tabs.Keys
                        If _tabs(tkey).Contains(id) Then
                            hit = True
                            Exit For
                        End If
                    Next
                    If Not hit Then tbr.Add(id, _statuses(id).IsRead)
                Next
            End If
        Next

        For Each key As String In _tabs.Keys
            _tabs(key).AddSubmit()  '振分確定（各タブに反映）
        Next
        Me.SortPosts()
    End Sub

    Public Function GetId(ByVal TabName As String, ByVal IndexCollection As ListView.SelectedIndexCollection) As Long()
        If IndexCollection.Count = 0 Then Return Nothing

        Dim tb As TabClass = _tabs(TabName)
        Dim Ids(IndexCollection.Count - 1) As Long
        For i As Integer = 0 To Ids.Length - 1
            Ids(i) = tb.GetId(IndexCollection(i))
        Next
        Return Ids
    End Function

    Public Function GetId(ByVal TabName As String, ByVal Index As Integer) As Long
        Return _tabs(TabName).GetId(Index)
    End Function

    Public Function GetIndex(ByVal TabName As String, ByVal Ids() As Long) As Integer()
        If Ids Is Nothing Then Return Nothing
        Dim idx(Ids.Length - 1) As Integer
        Dim tb As TabClass = _tabs(TabName)
        For i As Integer = 0 To Ids.Length - 1
            idx(i) = tb.GetIndex(Ids(i))
        Next
        Return idx
    End Function

    Public Function GetIndex(ByVal TabName As String, ByVal Id As Long) As Integer
        Return _tabs(TabName).GetIndex(Id)
    End Function

    Public Sub ClearTabIds(ByVal TabName As String)
        '不要なPostを削除
        For Each Id As Long In _tabs(TabName).BackupIds
            Dim Hit As Boolean = False
            For Each tb As TabClass In _tabs.Values
                If tb.Contains(Id) Then
                    Hit = True
                    Exit For
                End If
            Next
            If Not Hit Then _statuses.Remove(Id)
        Next
        '指定タブをクリア
        _tabs(TabName).ClearIDs()
    End Sub

    Public Sub SetTabUnreadManage(ByVal TabName As String, ByVal Manage As Boolean)
        Dim tb As TabClass = _tabs(TabName)
        If Manage Then
            Dim cnt As Integer = 0
            Dim oldest As Long = Long.MaxValue
            For Each id As Long In tb.BackupIds
                If Not _statuses(id).IsRead Then
                    cnt += 1
                    If oldest > id Then oldest = id
                End If
            Next
            tb.OldestUnreadId = oldest
            tb.UnreadCount = cnt
        Else
            tb.OldestUnreadId = -1
            tb.UnreadCount = 0
        End If
        tb.UnreadManage = Manage
    End Sub
End Class

Public Class TabClass
    '自分のタブ名は分からない
    Private _unreadManage As Boolean
    Private _notify As Boolean
    Private _soundFile As String
    Private _filters As List(Of FiltersClass)
    Private _oldestUnreadItem As Long     'ID
    Private _unreadCount As Integer
    Private _ids As List(Of Long)
    Private _filterMod As Boolean
    Private _tmpIds As List(Of TempolaryId)
    'Private rwLock As New System.Threading.ReaderWriterLock()   'フィルタ用

    Private Structure TempolaryId
        Public Id As Long
        Public Read As Boolean

        Public Sub New(ByVal argId As Long, ByVal argRead As Boolean)
            Id = argId
            Read = argRead
        End Sub
    End Structure

    Public Sub New()
        _filters = New List(Of FiltersClass)
        _notify = True
        _soundFile = ""
        _unreadManage = True
        _ids = New List(Of Long)
    End Sub

    Public Sub Sort(ByVal Sorter As IdComparerClass)
        _ids.Sort(Sorter)
    End Sub

    '無条件に追加
    Public Sub Add(ByVal ID As Long, ByVal Read As Boolean)
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

    'フィルタに合致したら追加
    Public Function AddFiltered(ByVal ID As Long, _
                                ByVal Read As Boolean, _
                                ByVal Name As String, _
                                ByVal Body As String, _
                                ByVal OrgData As String) As HITRESULT
        'Try
        '    rwLock.AcquireReaderLock(System.Threading.Timeout.Infinite) '読み取りロック取得

        Dim rslt As HITRESULT = HITRESULT.None
        '全フィルタ評価（優先順位あり）
        For Each ft As FiltersClass In _filters
            Select Case ft.IsHit(Name, Body, OrgData)   'フィルタクラスでヒット判定
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

        If rslt <> HITRESULT.None Then
            If _tmpIds Is Nothing Then _tmpIds = New List(Of TempolaryId)
            _tmpIds.Add(New TempolaryId(ID, Read))
        End If
        'Me.Add(ID, Read)

        Return rslt 'マーク付けは呼び出し元で行うこと

        'Finally
        '    rwLock.ReleaseReaderLock()
        'End Try
    End Function

    Public Sub AddSubmit()
        If _tmpIds Is Nothing Then Exit Sub
        For Each tId As TempolaryId In _tmpIds
            Me.Add(tId.Id, tId.Read)
        Next
        _tmpIds.Clear()
        _tmpIds = Nothing
    End Sub

    Public Sub Remove(ByVal Id As Long)
        If Not Me._ids.Contains(Id) Then Exit Sub

        Me._ids.Remove(Id)
    End Sub

    Public Property UnreadManage() As Boolean
        Get
            Return _unreadManage
        End Get
        Set(ByVal value As Boolean)
            Me._unreadManage = value
            If Not value Then
                Me._oldestUnreadItem = -1
                Me._unreadCount = 0
            End If
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

    'Public Property Filters() As List(Of FiltersClass)
    '    Get
    '        Return _filters
    '    End Get
    '    Set(ByVal value As List(Of FiltersClass))
    '        _filters = value
    '    End Set
    'End Property

    Public Function GetFilters() As FiltersClass()
        'Try
        '    rwLock.AcquireReaderLock(System.Threading.Timeout.Infinite) '読み取りロック取得
        Return _filters.ToArray()
        'Finally
        '    rwLock.ReleaseReaderLock()
        'End Try
    End Function

    Public Sub RemoveFilter(ByVal filter As FiltersClass)
        'Try
        '    rwLock.AcquireWriterLock(System.Threading.Timeout.Infinite) '書き込みロック取得
        _filters.Remove(filter)
        _filterMod = True
        'Finally
        '    rwLock.ReleaseWriterLock()
        'End Try
    End Sub

    Public Sub AddFilter(ByVal filter As FiltersClass)
        'Try
        '    rwLock.AcquireWriterLock(System.Threading.Timeout.Infinite) '書き込みロック取得
        _filters.Add(filter)
        _filterMod = True
        'Finally
        '    rwLock.ReleaseWriterLock()
        'End Try
    End Sub

    Public Sub EditFilter(ByVal original As FiltersClass, ByVal modified As FiltersClass)
        'Try
        '    rwLock.AcquireWriterLock(System.Threading.Timeout.Infinite) '書き込みロック取得
        original.BodyFilter = modified.BodyFilter
        original.MoveFrom = modified.MoveFrom
        original.NameFilter = modified.NameFilter
        original.SearchBoth = modified.SearchBoth
        original.SearchUrl = modified.SearchUrl
        original.SetMark = modified.SetMark
        original.UseRegex = modified.UseRegex
        _filterMod = True
        'Finally
        '    rwLock.ReleaseWriterLock()
        'End Try
    End Sub

    Public Function Contains(ByVal ID As Long) As Boolean
        Return _ids.Contains(ID)
    End Function

    Public Sub ClearIDs()
        _ids.Clear()
        _unreadCount = 0
        _oldestUnreadItem = -1
    End Sub

    Public Function GetId(ByVal Index As Integer) As Long
        Return _ids(Index)
    End Function

    Public Function GetIndex(ByVal ID As Long) As Integer
        Return _ids.IndexOf(ID)
    End Function

    Public Property FilterModified() As Boolean
        Get
            Return _filterMod
        End Get
        Set(ByVal value As Boolean)
            _filterMod = value
        End Set
    End Property

    Public Function BackupIds() As Long()
        Return _ids.ToArray()
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

    Public Sub New()

    End Sub

    'フィルタ一覧に表示する文言生成
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

    Public Overrides Function ToString() As String
        Return MakeSummary()
    End Function

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

'ソート比較クラス：ID比較のみ
Public Class IdComparerClass
    Implements IComparer(Of Long)

    ''' <summary>
    ''' 比較する方法
    ''' </summary>
    Public Enum ComparerMode
        Id
        Data
        Name
        Nickname
        Source
    End Enum

    'Private _column As Integer
    Private _order As SortOrder
    Private _mode As ComparerMode
    Private _statuses As TabInformations
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

    ''' <summary>
    ''' 並び替えの方法
    ''' </summary>
    Public Property Mode() As ComparerMode
        Get
            Return _mode
        End Get
        Set(ByVal Value As ComparerMode)
            _mode = Value
        End Set
    End Property

    '''' <summary>
    '''' 列ごとの並び替えの方法
    '''' </summary>
    'Public WriteOnly Property ColumnModes() As ComparerMode()
    '    Set(ByVal Value As ComparerMode())
    '        _columnModes = Value
    '    End Set
    'End Property

    ''' <summary>
    ''' ListViewItemComparerクラスのコンストラクタ（引数付は未使用）
    ''' </summary>
    ''' <param name="col">並び替える列番号</param>
    ''' <param name="ord">昇順か降順か</param>
    ''' <param name="cmod">並び替えの方法</param>
    Public Sub New(ByVal ord As SortOrder, ByVal SortMode As ComparerMode)
        _order = ord
        _mode = SortMode
    End Sub

    'Public Sub New()
    '    _order = SortOrder.Ascending
    '    _mode = ComparerMode.Id
    'End Sub

    Public Sub New(ByVal TabInf As TabInformations)
        _order = SortOrder.Ascending
        _mode = ComparerMode.Id
        _statuses = TabInf
    End Sub

    'xがyより小さいときはマイナスの数、大きいときはプラスの数、
    '同じときは0を返す
    Public Function Compare(ByVal x As Long, ByVal y As Long) _
            As Integer Implements IComparer(Of Long).Compare
        Dim result As Integer = 0

        Select Case _mode
            Case ComparerMode.Data
                result = String.Compare(_statuses.Item(x).Data, _statuses.Item(y).Data)
            Case ComparerMode.Id
                If x < y Then
                    result = -1
                ElseIf x = y Then
                    result = 0
                Else
                    result = 1
                End If
            Case ComparerMode.Name
                result = String.Compare(_statuses.Item(x).Name, _statuses.Item(y).Name)
            Case ComparerMode.Nickname
                result = String.Compare(_statuses.Item(x).Nickname, _statuses.Item(y).Nickname)
            Case ComparerMode.Source
                result = String.Compare(_statuses.Item(x).Source, _statuses.Item(y).Source)
        End Select
        '降順の時は結果を+-逆にする
        If _order = SortOrder.Descending Then
            result = -result
        End If
        Return result

    End Function
End Class

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

Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports Tween.TweenCustomControl
Imports System.Text.RegularExpressions


Public NotInheritable Class PostClass
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
    Private _statuses As Statuses = Statuses.None
    Private _Uid As Long
    Private _FilterHit As Boolean

    <FlagsAttribute()> _
    Private Enum Statuses
        None = 0
        Protect = 1
        Mark = 2
        Read = 4
    End Enum

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
            ByVal IsDm As Boolean, _
            ByVal Uid As Long, _
            ByVal FilterHit As Boolean)
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
        _Uid = Uid
        _FilterHit = FilterHit
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
            If value Then
                _statuses = _statuses Or Statuses.Read
            Else
                _statuses = _statuses And Not Statuses.Read
            End If
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
            If value Then
                _statuses = _statuses Or Statuses.Protect
            Else
                _statuses = _statuses And Not Statuses.Protect
            End If
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
            If value Then
                _statuses = _statuses Or Statuses.Mark
            Else
                _statuses = _statuses And Not Statuses.Mark
            End If
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
    Public ReadOnly Property StatusIndex() As Integer
        Get
            Return _statuses
        End Get
    End Property
    Public Property Uid() As Long
        Get
            Return _Uid
        End Get
        Set(ByVal value As Long)
            _Uid = value
        End Set
    End Property
    Public Property FilterHit() As Boolean
        Get
            Return _FilterHit
        End Get
        Set(ByVal value As Boolean)
            _FilterHit = value
        End Set
    End Property
End Class

Public NotInheritable Class TabInformations
    '個別タブの情報をDictionaryで保持
    Private _sorter As IdComparerClass
    Private _tabs As New Dictionary(Of String, TabClass)
    Private _statuses As Dictionary(Of Long, PostClass) = New Dictionary(Of Long, PostClass)
    Private _addedIds As List(Of Long)
    'Private _editMode As EDITMODE

    '発言の追加
    'AddPost(複数回) -> DistributePosts          -> SubmitUpdate

    'トランザクション用
    Private _addCount As Integer
    Private _soundFile As String
    Private _notifyPosts As List(Of PostClass)
    Private ReadOnly LockObj As New Object
    Private ReadOnly LockUnread As New Object

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
        _tabs.Add(TabName, New TabClass(TabName))
    End Sub

    Public Sub AddTab(ByVal TabName As String, ByVal Tab As TabClass)
        _tabs.Add(TabName, Tab)
    End Sub

    Public Sub RemoveTab(ByVal TabName As String)
        SyncLock LockObj
            If IsDefaultTab(TabName) Then Exit Sub '念のため

            For idx As Integer = 0 To _tabs(TabName).AllCount - 1
                Dim exist As Boolean = False
                Dim Id As Long = _tabs(TabName).GetId(idx)
                For Each key As String In _tabs.Keys
                    If Not key = TabName AndAlso Not key = DEFAULTTAB.DM Then
                        If _tabs(key).Contains(Id) Then
                            exist = True
                            Exit For
                        End If
                    End If
                Next
                If Not exist Then _tabs(DEFAULTTAB.RECENT).Add(Id, _statuses(Id).IsRead, False)
            Next

            _tabs.Remove(TabName)
        End SyncLock
    End Sub

    Public Function ContainsTab(ByVal TabText As String) As Boolean
        Return _tabs.ContainsKey(TabText)
    End Function

    Public Function ContainsTab(ByVal ts As TabClass) As Boolean
        Return _tabs.ContainsValue(ts)
    End Function

    Public Property Tabs() As Dictionary(Of String, TabClass)
        Get
            Return _tabs
        End Get
        Set(ByVal value As Dictionary(Of String, TabClass))
            _tabs = value
        End Set
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

    Public Sub RemovePost(ByVal Name As String, ByVal Id As Long)
        SyncLock LockObj
            Dim post As PostClass = _statuses(Id)
            '指定タブから該当ID削除
            Dim tab As TabClass = _tabs(Name)
            If tab.Contains(Id) Then
                If tab.UnreadManage AndAlso Not post.IsRead Then    '未読管理
                    SyncLock LockUnread
                        tab.UnreadCount -= 1
                        Me.SetNextUnreadId(Id, tab)
                    End SyncLock
                End If
                tab.Remove(Id)
            End If
        End SyncLock
    End Sub

    Public Sub RemovePost(ByVal Id As Long)
        SyncLock LockObj
            Dim post As PostClass = _statuses(Id)
            '各タブから該当ID削除
            For Each key As String In _tabs.Keys
                Dim tab As TabClass = _tabs(key)
                If tab.Contains(Id) Then
                    If tab.UnreadManage AndAlso Not post.IsRead Then    '未読管理
                        SyncLock LockUnread
                            tab.UnreadCount -= 1
                            Me.SetNextUnreadId(Id, tab)
                        End SyncLock
                    End If
                    tab.Remove(Id)
                End If
            Next
            _statuses.Remove(Id)
        End SyncLock
    End Sub

    Public Function GetOldestUnreadId(ByVal TabName As String) As Integer
        Dim tb As TabClass = _tabs(TabName)
        If tb.OldestUnreadId > -1 AndAlso _
           tb.Contains(tb.OldestUnreadId) AndAlso _
           tb.UnreadCount > 0 Then
            '未読アイテムへ
            If _statuses.Item(tb.OldestUnreadId).IsRead Then
                '状態不整合（最古未読ＩＤが実は既読）
                SyncLock LockUnread
                    Me.SetNextUnreadId(-1, tb)  '頭から探索
                End SyncLock
                If tb.OldestUnreadId = -1 Then
                    Return -1
                Else
                    Return tb.IndexOf(tb.OldestUnreadId)
                End If
            Else
                Return tb.IndexOf(tb.OldestUnreadId)    '最短経路
            End If
        Else
            '一見未読なさそうだが、未読カウントはあるので探索
            If tb.UnreadCount > 0 Then
                SyncLock LockUnread
                    Me.SetNextUnreadId(-1, tb)
                End SyncLock
                If tb.OldestUnreadId = -1 Then
                    Return -1
                Else
                    Return tb.IndexOf(tb.OldestUnreadId)
                End If
            Else
                Return -1
            End If
        End If
    End Function

    Private Sub SetNextUnreadId(ByVal CurrentId As Long, ByVal Tab As TabClass)
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
                Dim idx As Integer = Tab.IndexOf(CurrentId)
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
            Tab.UnreadCount = 0
            Exit Sub
        End If
        Dim toIdx As Integer = 0
        Dim stp As Integer = 1
        Tab.OldestUnreadId = -1
        If _sorter.Order = Windows.Forms.SortOrder.Ascending Then
            If StartIdx = -1 Then
                StartIdx = 0
            Else
                'StartIdx += 1
                If StartIdx > Tab.AllCount - 1 Then StartIdx = Tab.AllCount - 1 '念のため
            End If
            toIdx = Tab.AllCount - 1
            If toIdx < 0 Then toIdx = 0 '念のため
            stp = 1
        Else
            If StartIdx = -1 Then
                StartIdx = Tab.AllCount - 1
            Else
                'StartIdx -= 1
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

    Public Function DistributePosts() As Integer
        SyncLock LockObj
            '戻り値は追加件数
            If _addedIds Is Nothing Then Return 0
            If _addedIds.Count = 0 Then Return 0

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

            soundFile = _soundFile
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
                        If rslt = HITRESULT.Move Then
                            mv = True '移動
                            post.IsMark = False
                        End If
                        If _tabs(tn).Notify Then add = True '通知あり
                        If Not _tabs(tn).SoundFile = "" AndAlso _soundFile = "" Then
                            _soundFile = _tabs(tn).SoundFile 'wavファイル（未設定の場合のみ）
                        End If
                        post.FilterHit = True
                    Else
                        post.FilterHit = False
                    End If
                Next
                If Not mv Then  '移動されなかったらRecentに追加
                    _tabs(DEFAULTTAB.RECENT).Add(post.Id, post.IsRead, True)
                    If Not _tabs(DEFAULTTAB.RECENT).SoundFile = "" AndAlso _soundFile = "" Then _soundFile = _tabs(DEFAULTTAB.RECENT).SoundFile
                    If _tabs(DEFAULTTAB.RECENT).Notify Then add = True
                End If
                If post.IsReply Then    'ReplyだったらReplyタブに追加
                    _tabs(DEFAULTTAB.REPLY).Add(post.Id, post.IsRead, True)
                    If Not _tabs(DEFAULTTAB.REPLY).SoundFile = "" Then _soundFile = _tabs(DEFAULTTAB.REPLY).SoundFile
                    If _tabs(DEFAULTTAB.REPLY).Notify Then add = True
                End If
                If post.IsFav Then    'Fav済み発言だったらFavoritesタブに追加
                    If _tabs(DEFAULTTAB.FAV).Contains(post.Id) Then
                        '取得済みなら非通知
                        _soundFile = ""
                        add = False
                    Else
                        _tabs(DEFAULTTAB.FAV).Add(post.Id, post.IsRead, True)
                        If Not _tabs(DEFAULTTAB.FAV).SoundFile = "" Then _soundFile = _tabs(DEFAULTTAB.FAV).SoundFile
                        If _tabs(DEFAULTTAB.FAV).Notify Then add = True
                    End If
                End If
                If add Then _notifyPosts.Add(post)
            Else
                _tabs(DEFAULTTAB.DM).Add(post.Id, post.IsRead, True)
                If _tabs(DEFAULTTAB.DM).Notify Then _notifyPosts.Add(post)
                _soundFile = _tabs(DEFAULTTAB.DM).SoundFile
            End If
        Next
    End Sub

    Public Sub AddPost(ByVal Item As PostClass)
        SyncLock LockObj
            If _statuses.ContainsKey(Item.Id) Then
                If Item.IsFav Then
                    _statuses.Item(Item.Id).IsFav = True
                Else
                    Exit Sub        '追加済みなら何もしない
                End If
            Else
                _statuses.Add(Item.Id, Item)    'DMと区別しない？
            End If
            If _addedIds Is Nothing Then _addedIds = New List(Of Long) 'タブ追加用IDコレクション準備
            _addedIds.Add(Item.Id)
        End SyncLock
    End Sub

    Public Sub SetRead(ByVal Read As Boolean, ByVal TabName As String, ByVal Index As Integer)
        'Read:True=既読へ　False=未読へ
        Dim tb As TabClass = _tabs(TabName)

        If tb.UnreadManage = False Then Exit Sub '未読管理していなければ終了

        Dim Id As Long = tb.GetId(Index)

        If _statuses(Id).IsRead = Read Then Exit Sub '状態変更なければ終了

        _statuses(Id).IsRead = Read '指定の状態に変更

        SyncLock LockUnread
            If Read Then
                tb.UnreadCount -= 1
                Me.SetNextUnreadId(Id, tb)  '次の未読セット
                '他タブの最古未読ＩＤはタブ切り替え時に。
                For Each key As String In _tabs.Keys
                    If key <> TabName AndAlso _
                       _tabs(key).UnreadManage AndAlso _
                       _tabs(key).Contains(Id) Then
                        _tabs(key).UnreadCount -= 1
                        If _tabs(key).OldestUnreadId = Id Then _tabs(key).OldestUnreadId = -1
                    End If
                Next
            Else
                tb.UnreadCount += 1
                If tb.OldestUnreadId > Id OrElse tb.OldestUnreadId = -1 Then tb.OldestUnreadId = Id
                For Each key As String In _tabs.Keys
                    If Not key = TabName AndAlso _tabs(key).UnreadManage AndAlso _tabs(key).Contains(Id) Then
                        _tabs(key).UnreadCount += 1
                        If _tabs(key).OldestUnreadId > Id Then _tabs(key).OldestUnreadId = Id
                    End If
                Next
            End If
        End SyncLock
    End Sub

    Public Sub SetRead()
        Dim tb As TabClass = _tabs(DEFAULTTAB.RECENT)
        If tb.UnreadManage = False Then Exit Sub

        For i As Integer = 0 To tb.AllCount - 1
            Dim id As Long = tb.GetId(i)
            If Not _statuses(id).IsDm AndAlso _
               Not _statuses(id).IsReply AndAlso _
               Not _statuses(id).IsRead AndAlso _
               Not _statuses(id).FilterHit Then
                _statuses(id).IsRead = True
                Me.SetNextUnreadId(id, tb)  '次の未読セット
                For Each key As String In _tabs.Keys
                    If _tabs(key).UnreadManage AndAlso _
                       _tabs(key).Contains(id) Then
                        _tabs(key).UnreadCount -= 1
                        If _tabs(key).OldestUnreadId = id Then _tabs(key).OldestUnreadId = -1
                    End If
                Next
            End If
        Next
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

    Public ReadOnly Property Item(ByVal TabName As String, ByVal StartIndex As Integer, ByVal EndIndex As Integer) As PostClass()
        Get
            Dim length As Integer = EndIndex - StartIndex + 1
            Dim posts() As PostClass = New PostClass(length - 1) {}
            For i As Integer = 0 To length - 1
                posts(i) = _statuses(_tabs(TabName).GetId(StartIndex + i))
            Next i
            Return posts
        End Get
    End Property

    Public ReadOnly Property ItemCount() As Integer
        Get
            SyncLock LockObj
                Return _statuses.Count
            End SyncLock
        End Get
    End Property

    Public Function ContainsKey(ByVal Id As Long) As Boolean
        SyncLock LockObj
            Return _statuses.ContainsKey(Id)
        End SyncLock
    End Function

    Public Sub SetUnreadManage(ByVal Manage As Boolean)
        If Manage Then
            For Each key As String In _tabs.Keys
                Dim tb As TabClass = _tabs(key)
                If tb.UnreadManage Then
                    SyncLock LockUnread
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
                    End SyncLock
                End If
            Next
        Else
            For Each key As String In _tabs.Keys
                Dim tb As TabClass = _tabs(key)
                If tb.UnreadManage AndAlso tb.UnreadCount > 0 Then
                    SyncLock LockUnread
                        tb.UnreadCount = 0
                        tb.OldestUnreadId = -1
                    End SyncLock
                End If
            Next
        End If
    End Sub

    Public Sub RenameTab(ByVal Original As String, ByVal NewName As String)
        Dim tb As TabClass = _tabs(Original)
        _tabs.Remove(Original)
        tb.TabName = NewName
        _tabs.Add(NewName, tb)
    End Sub

    Public Sub FilterAll()
        SyncLock LockObj
            Dim tbr As TabClass = _tabs(DEFAULTTAB.RECENT)
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
                                post.FilterHit = True
                            Case HITRESULT.Move
                                tbr.Remove(post.Id, post.IsRead)
                                post.IsMark = False
                                post.FilterHit = True
                            Case HITRESULT.Copy
                                post.IsMark = False
                                post.FilterHit = True
                            Case HITRESULT.None
                                If key = DEFAULTTAB.REPLY And post.IsReply Then _tabs(DEFAULTTAB.REPLY).Add(post.Id, post.IsRead, True)
                                If post.IsFav Then _tabs(DEFAULTTAB.FAV).Add(post.Id, post.IsRead, True)
                                post.FilterHit = False
                        End Select
                    Next
                    tb.AddSubmit()  '振分確定
                    For Each id As Long In orgIds
                        Dim hit As Boolean = False
                        For Each tkey As String In _tabs.Keys
                            If _tabs(tkey).Contains(id) Then
                                hit = True
                                Exit For
                            End If
                        Next
                        If Not hit Then tbr.Add(id, _statuses(id).IsRead, False)
                    Next
                End If
            Next

            Me.SortPosts()
        End SyncLock
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

    Public Function IndexOf(ByVal TabName As String, ByVal Ids() As Long) As Integer()
        If Ids Is Nothing Then Return Nothing
        Dim idx(Ids.Length - 1) As Integer
        Dim tb As TabClass = _tabs(TabName)
        For i As Integer = 0 To Ids.Length - 1
            idx(i) = tb.IndexOf(Ids(i))
        Next
        Return idx
    End Function

    Public Function IndexOf(ByVal TabName As String, ByVal Id As Long) As Integer
        Return _tabs(TabName).IndexOf(Id)
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
        SyncLock LockUnread
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
        End SyncLock
        tb.UnreadManage = Manage
    End Sub

    Public Sub RefreshOwl(ByVal follower As List(Of String))
        SyncLock LockObj
            For Each id As Long In _statuses.Keys
                If Not _statuses(id).IsDm Then _statuses(id).IsOwl = Not follower.Contains(_statuses(id).Name.ToLower())
            Next
        End SyncLock
    End Sub
End Class

<Serializable()> _
Public NotInheritable Class TabClass
    Private _unreadManage As Boolean = False
    Private _notify As Boolean = False
    Private _soundFile As String = ""
    Private _filters As List(Of FiltersClass)
    Private _oldestUnreadItem As Long = -1     'ID
    Private _unreadCount As Integer = 0
    Private _ids As List(Of Long)
    Private _filterMod As Boolean = False
    Private _tmpIds As List(Of TempolaryId)
    Private _tabName As String = ""
    Private _tabType As TabUsageType = TabUsageType.UserDefined
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
        _oldestUnreadItem = -1
        _tabType = TabUsageType.UserDefined
    End Sub

    Public Sub New(ByVal TabName As String)
        Me.TabName = TabName
        _filters = New List(Of FiltersClass)
        _notify = True
        _soundFile = ""
        _unreadManage = True
        _ids = New List(Of Long)
        _oldestUnreadItem = -1
    End Sub

    Public Sub Sort(ByVal Sorter As IdComparerClass)
        _ids.Sort(Sorter.CmpMethod)
    End Sub

    '無条件に追加
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

    Public Sub Add(ByVal ID As Long, ByVal Read As Boolean, ByVal Temporary As Boolean)
        If Not Temporary Then
            Me.Add(ID, Read)
        Else
            If _tmpIds Is Nothing Then _tmpIds = New List(Of TempolaryId)
            _tmpIds.Add(New TempolaryId(ID, Read))
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
                Case HITRESULT.Exclude
                    rslt = HITRESULT.None
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

    Public Sub Remove(ByVal Id As Long, ByVal Read As Boolean)
        If Not Me._ids.Contains(Id) Then Exit Sub

        If Not Read AndAlso Me._unreadManage Then
            Me._unreadCount -= 1
            Me._oldestUnreadItem = -1
        End If

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

    <Xml.Serialization.XmlIgnore()> _
    Public Property OldestUnreadId() As Long
        Get
            Return _oldestUnreadItem
        End Get
        Set(ByVal value As Long)
            _oldestUnreadItem = value
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
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

    Public Function GetFilters() As FiltersClass()
        Return _filters.ToArray()
    End Function

    Public Sub RemoveFilter(ByVal filter As FiltersClass)
        _filters.Remove(filter)
        _filterMod = True
    End Sub

    Public Function AddFilter(ByVal filter As FiltersClass) As Boolean
        If _filters.Contains(filter) Then Return False
        _filters.Add(filter)
        _filterMod = True
        Return True
    End Function

    Public Sub EditFilter(ByVal original As FiltersClass, ByVal modified As FiltersClass)
        original.BodyFilter = modified.BodyFilter
        original.NameFilter = modified.NameFilter
        original.SearchBoth = modified.SearchBoth
        original.SearchUrl = modified.SearchUrl
        original.UseRegex = modified.UseRegex
        original.CaseSensitive = modified.CaseSensitive
        original.ExBodyFilter = modified.ExBodyFilter
        original.ExNameFilter = modified.ExNameFilter
        original.ExSearchBoth = modified.ExSearchBoth
        original.ExSearchUrl = modified.ExSearchUrl
        original.ExUseRegex = modified.ExUseRegex
        original.ExCaseSensitive = modified.ExCaseSensitive
        original.MoveFrom = modified.MoveFrom
        original.SetMark = modified.SetMark
        _filterMod = True
    End Sub

    <Xml.Serialization.XmlIgnore()> _
    Public Property Filters() As List(Of FiltersClass)
        Get
            Return _filters
        End Get
        Set(ByVal value As List(Of FiltersClass))
            _filters = value
        End Set
    End Property

    Public Property FilterArray() As FiltersClass()
        Get
            Return _filters.ToArray
        End Get
        Set(ByVal value As FiltersClass())
            For Each filters As FiltersClass In value
                _filters.Add(filters)
            Next
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

    Public Function GetId(ByVal Index As Integer) As Long
        Return _ids(Index)
    End Function

    Public Function IndexOf(ByVal ID As Long) As Integer
        Return _ids.IndexOf(ID)
    End Function

    <Xml.Serialization.XmlIgnore()> _
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

    Public Property TabName() As String
        Get
            Return _tabName
        End Get
        Set(ByVal value As String)
            _tabName = value

            ''' TODO: 0.7.2.0リリース前に削除すること
            Select Case value
                Case DEFAULTTAB.RECENT
                    _tabType = TabUsageType.Home
                Case DEFAULTTAB.REPLY
                    _tabType = TabUsageType.Mentions
                Case DEFAULTTAB.DM
                    _tabType = TabUsageType.DirectMessage
                Case DEFAULTTAB.FAV
                    _tabType = TabUsageType.Favorites
                Case Else
                    _tabType = TabUsageType.UserDefined
            End Select
        End Set
    End Property
End Class

<Serializable()> _
Public NotInheritable Class FiltersClass
    Implements System.IEquatable(Of FiltersClass)
    Private _name As String = ""
    Private _body As New List(Of String)
    Private _searchBoth As Boolean = True
    Private _searchUrl As Boolean = False
    Private _caseSensitive As Boolean = False
    Private _useRegex As Boolean = False
    Private _exname As String = ""
    Private _exbody As New List(Of String)
    Private _exsearchBoth As Boolean = True
    Private _exsearchUrl As Boolean = False
    Private _exuseRegex As Boolean = False
    Private _excaseSensitive As Boolean = False
    Private _moveFrom As Boolean = False
    Private _setMark As Boolean = True

    Public Sub New(ByVal NameFilter As String, _
        ByVal BodyFilter As List(Of String), _
        ByVal SearchBoth As Boolean, _
        ByVal SearchUrl As Boolean, _
        ByVal CaseSensitive As Boolean, _
        ByVal UseRegex As Boolean, _
        ByVal ParentTab As String, _
        ByVal ExNameFilter As String, _
        ByVal ExBodyFilter As List(Of String), _
        ByVal ExSearchBoth As Boolean, _
        ByVal ExSearchUrl As Boolean, _
        ByVal ExUseRegex As Boolean, _
        ByVal ExCaseSensitive As Boolean, _
        ByVal MoveFrom As Boolean, _
        ByVal SetMark As Boolean)
        _name = NameFilter
        _body = BodyFilter
        _searchBoth = SearchBoth
        _searchUrl = SearchUrl
        _caseSensitive = CaseSensitive
        _useRegex = UseRegex
        _exname = ExNameFilter
        _exbody = ExBodyFilter
        _exsearchBoth = ExSearchBoth
        _exsearchUrl = ExSearchUrl
        _exuseRegex = ExUseRegex
        _excaseSensitive = ExCaseSensitive
        _moveFrom = MoveFrom
        _setMark = SetMark
        '正規表現検証
        If _useRegex Then
            Try
                Dim rgx As New Regex(_name)
            Catch ex As Exception
                Throw New Exception(My.Resources.ButtonOK_ClickText3 + ex.Message)
                Exit Sub
            End Try
            For Each bs As String In _body
                Try
                    Dim rgx As New Regex(bs)
                Catch ex As Exception
                    Throw New Exception(My.Resources.ButtonOK_ClickText3 + ex.Message)
                    Exit Sub
                End Try
            Next
        End If
        If _exuseRegex Then
            Try
                Dim rgx As New Regex(_exname)
            Catch ex As Exception
                Throw New Exception(My.Resources.ButtonOK_ClickText3 + ex.Message)
                Exit Sub
            End Try
            For Each bs As String In _exbody
                Try
                    Dim rgx As New Regex(bs)
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
        If _name <> "" OrElse _body.Count > 0 Then
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
            If _caseSensitive Then
                fs.Append(My.Resources.SetFiltersText13)
            End If
            'If _moveFrom Then
            '    fs.Append(My.Resources.SetFiltersText9)
            'ElseIf _setMark Then
            '    fs.Append(My.Resources.SetFiltersText10)
            'Else
            '    fs.Append(My.Resources.SetFiltersText11)
            'End If
            fs.Length -= 1
            fs.Append(")")
        End If
        If _exname <> "" OrElse _exbody.Count > 0 Then
            '除外
            fs.Append(My.Resources.SetFiltersText12)
            If _exsearchBoth Then
                If _exname <> "" Then
                    fs.AppendFormat(My.Resources.SetFiltersText1, _exname)
                Else
                    fs.Append(My.Resources.SetFiltersText2)
                End If
            End If
            If _exbody.Count > 0 Then
                fs.Append(My.Resources.SetFiltersText3)
                For Each bf As String In _exbody
                    fs.Append(bf)
                    fs.Append(" ")
                Next
                fs.Length -= 1
                fs.Append(My.Resources.SetFiltersText4)
            End If
            fs.Append("(")
            If _exsearchBoth Then
                fs.Append(My.Resources.SetFiltersText5)
            Else
                fs.Append(My.Resources.SetFiltersText6)
            End If
            If _exuseRegex Then
                fs.Append(My.Resources.SetFiltersText7)
            End If
            If _exsearchUrl Then
                fs.Append(My.Resources.SetFiltersText8)
            End If
            If _excaseSensitive Then
                fs.Append(My.Resources.SetFiltersText13)
            End If
            fs.Length -= 1
            fs.Append(")")
        End If

        fs.Append("(")
        If _moveFrom Then
            fs.Append(My.Resources.SetFiltersText9)
        Else
            fs.Append(My.Resources.SetFiltersText11)
        End If
        If Not _moveFrom AndAlso _setMark Then
            fs.Append(My.Resources.SetFiltersText10)
        ElseIf Not _moveFrom Then
            fs.Length -= 1
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

    Public Property ExNameFilter() As String
        Get
            Return _exname
        End Get
        Set(ByVal value As String)
            _exname = value
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public Property BodyFilter() As List(Of String)
        Get
            Return _body
        End Get
        Set(ByVal value As List(Of String))
            _body = value
        End Set
    End Property

    Public Property BodyFilterArray() As String()
        Get
            Return _body.ToArray
        End Get
        Set(ByVal value As String())
            _body = New List(Of String)
            For Each filter As String In value
                _body.Add(filter)
            Next
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> _
    Public Property ExBodyFilter() As List(Of String)
        Get
            Return _exbody
        End Get
        Set(ByVal value As List(Of String))
            _exbody = value
        End Set
    End Property

    Public Property ExBodyFilterArray() As String()
        Get
            Return _exbody.ToArray
        End Get
        Set(ByVal value As String())
            _exbody = New List(Of String)
            For Each filter As String In value
                _exbody.Add(filter)
            Next
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

    Public Property ExSearchBoth() As Boolean
        Get
            Return _exsearchBoth
        End Get
        Set(ByVal value As Boolean)
            _exsearchBoth = value
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

    Public Property ExSearchUrl() As Boolean
        Get
            Return _exsearchUrl
        End Get
        Set(ByVal value As Boolean)
            _exsearchUrl = value
        End Set
    End Property

    Public Property CaseSensitive() As Boolean
        Get
            Return _caseSensitive
        End Get
        Set(ByVal value As Boolean)
            _caseSensitive = value
        End Set
    End Property

    Public Property ExCaseSensitive() As Boolean
        Get
            Return _excaseSensitive
        End Get
        Set(ByVal value As Boolean)
            _excaseSensitive = value
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

    Public Property ExUseRegex() As Boolean
        Get
            Return _exuseRegex
        End Get
        Set(ByVal value As Boolean)
            _exuseRegex = value
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
        '検索オプション
        Dim compOpt As System.StringComparison
        Dim rgOpt As System.Text.RegularExpressions.RegexOptions
        If _caseSensitive Then
            compOpt = StringComparison.Ordinal
            rgOpt = RegexOptions.None
        Else
            compOpt = StringComparison.OrdinalIgnoreCase
            rgOpt = RegexOptions.IgnoreCase
        End If
        If _searchBoth Then
            If _name = "" OrElse Name.Equals(_name, compOpt) OrElse _
                            (_useRegex AndAlso Regex.IsMatch(Name, _name, rgOpt)) Then
                For Each fs As String In _body
                    If _useRegex Then
                        If Regex.IsMatch(tBody, fs, rgOpt) = False Then bHit = False
                    Else
                        If _caseSensitive Then
                            If tBody.Contains(fs) = False Then bHit = False
                        Else
                            If tBody.ToLower().Contains(fs.ToLower()) = False Then bHit = False
                        End If
                    End If
                    If Not bHit Then Exit For
                Next
            Else
                bHit = False
            End If
        Else
            For Each fs As String In _body
                If _useRegex Then
                    If Not (Regex.IsMatch(Name, fs, rgOpt) OrElse _
                            Regex.IsMatch(tBody, fs, rgOpt)) Then bHit = False
                Else
                    If _caseSensitive Then
                        If Not (Name.Contains(fs) OrElse _
                                tBody.Contains(fs)) Then bHit = False
                    Else
                        If Not (Name.ToLower().Contains(fs.ToLower()) OrElse _
                                tBody.ToLower().Contains(fs.ToLower())) Then bHit = False
                    End If
                End If
                If Not bHit Then Exit For
            Next
        End If
        If bHit Then
            '除外判定
            Dim exFlag As Boolean = False
            'If _name = "" AndAlso _body.Count = 0 Then
            '    exFlag = True
            '    'bHit = False
            'End If
            If _exname <> "" OrElse _exbody.Count > 0 Then
                If _excaseSensitive Then
                    compOpt = StringComparison.Ordinal
                    rgOpt = RegexOptions.None
                Else
                    compOpt = StringComparison.OrdinalIgnoreCase
                    rgOpt = RegexOptions.IgnoreCase
                End If
                If _exsearchBoth Then
                    If _exname = "" OrElse Name.Equals(_exname, compOpt) OrElse _
                                    (_exuseRegex AndAlso Regex.IsMatch(Name, _exname, rgOpt)) Then
                        If _exbody.Count > 0 Then
                            For Each fs As String In _exbody
                                If _exuseRegex Then
                                    If Regex.IsMatch(tBody, fs, rgOpt) Then exFlag = True
                                Else
                                    If _excaseSensitive Then
                                        If tBody.Contains(fs) Then exFlag = True
                                    Else
                                        If tBody.ToLower().Contains(fs.ToLower()) Then exFlag = True
                                    End If
                                End If
                                If exFlag Then Exit For
                            Next
                        Else
                            exFlag = True
                        End If
                    End If
                Else
                    For Each fs As String In _exbody
                        If _exuseRegex Then
                            If Regex.IsMatch(Name, fs, rgOpt) OrElse _
                               Regex.IsMatch(tBody, fs, rgOpt) Then exFlag = True
                        Else
                            If _excaseSensitive Then
                                If Name.Contains(fs) OrElse _
                                   tBody.Contains(fs) Then exFlag = True
                            Else
                                If Name.ToLower().Contains(fs.ToLower()) OrElse _
                                   tBody.ToLower().Contains(fs.ToLower()) Then exFlag = True
                            End If
                        End If
                        If exFlag Then Exit For
                    Next
                End If
            End If

            If _name = "" AndAlso _body.Count = 0 Then
                bHit = False
            End If
            If bHit Then
                If Not exFlag Then
                    'If _setMark Then Return HITRESULT.CopyAndMark
                    If _moveFrom Then
                        Return HITRESULT.Move
                    Else
                        If _setMark Then
                            Return HITRESULT.CopyAndMark
                        End If
                        Return HITRESULT.Copy
                    End If
                    'Return HITRESULT.Copy
                Else
                    Return HITRESULT.Exclude
                End If
            Else
                If exFlag Then
                    Return HITRESULT.Exclude
                Else
                    Return HITRESULT.None
                End If
            End If
        Else
            Return HITRESULT.None
        End If
    End Function

    Public Overloads Function Equals(ByVal other As FiltersClass) As Boolean _
     Implements System.IEquatable(Of Tween.FiltersClass).Equals

        If Me.BodyFilter.Count <> other.BodyFilter.Count Then Return False
        If Me.ExBodyFilter.Count <> other.ExBodyFilter.Count Then Return False
        For i As Integer = 0 To Me.BodyFilter.Count - 1
            If Me.BodyFilter(i) <> other.BodyFilter(i) Then Return False
        Next
        For i As Integer = 0 To Me.ExBodyFilter.Count - 1
            If Me.ExBodyFilter(i) <> other.ExBodyFilter(i) Then Return False
        Next

        Return (Me.MoveFrom = other.MoveFrom) And _
               (Me.SetMark = other.SetMark) And _
               (Me.NameFilter = other.NameFilter) And _
               (Me.SearchBoth = other.SearchBoth) And _
               (Me.SearchUrl = other.SearchUrl) And _
               (Me.UseRegex = other.UseRegex) And _
               (Me.ExNameFilter = other.ExNameFilter) And _
               (Me.ExSearchBoth = other.ExSearchBoth) And _
               (Me.ExSearchUrl = other.ExSearchUrl) And _
               (Me.ExUseRegex = other.ExUseRegex)
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (obj Is Nothing) OrElse Not (Me.GetType() Is obj.GetType()) Then Return False
        Return Me.Equals(CType(obj, FiltersClass))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return Me.MoveFrom.GetHashCode Xor _
               Me.SetMark.GetHashCode Xor _
               Me.BodyFilter.GetHashCode Xor _
               Me.NameFilter.GetHashCode Xor _
               Me.SearchBoth.GetHashCode Xor _
               Me.SearchUrl.GetHashCode Xor _
               Me.UseRegex.GetHashCode Xor _
               Me.ExBodyFilter.GetHashCode Xor _
               Me.ExNameFilter.GetHashCode Xor _
               Me.ExSearchBoth.GetHashCode Xor _
               Me.ExSearchUrl.GetHashCode Xor _
               Me.ExUseRegex.GetHashCode
    End Function
End Class

'ソート比較クラス：ID比較のみ
Public NotInheritable Class IdComparerClass
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

    Private _order As SortOrder
    Private _mode As ComparerMode
    Private _statuses As TabInformations
    Private _CmpMethod As Comparison(Of Long)

    ''' <summary>
    ''' 昇順か降順か Setの際は同時に比較関数の切り替えを行う
    ''' </summary>
    Public Property Order() As SortOrder
        Get
            Return _order
        End Get
        Set(ByVal Value As SortOrder)
            _order = Value
            SetCmpMethod(_mode, _order)
        End Set
    End Property

    ''' <summary>
    ''' 並び替えの方法 Setの際は同時に比較関数の切り替えを行う
    ''' </summary>
    Public Property Mode() As ComparerMode
        Get
            Return _mode
        End Get
        Set(ByVal Value As ComparerMode)
            _mode = Value
            SetCmpMethod(_mode, _order)
        End Set
    End Property

    ''' <summary>
    ''' ListViewItemComparerクラスのコンストラクタ（引数付は未使用）
    ''' </summary>
    ''' <param name="col">並び替える列番号</param>
    ''' <param name="ord">昇順か降順か</param>
    ''' <param name="cmod">並び替えの方法</param>
    Public Sub New(ByVal ord As SortOrder, ByVal SortMode As ComparerMode)
        _order = ord
        _mode = SortMode
        SetCmpMethod(_mode, _order)
    End Sub

    Public Sub New(ByVal TabInf As TabInformations)
        _order = SortOrder.Ascending
        _mode = ComparerMode.Id
        _statuses = TabInf
        SetCmpMethod(_mode, _order)
    End Sub

    ' 指定したソートモードとソートオーダーに従い使用する比較関数のアドレスを返す
    Public Overloads ReadOnly Property CmpMethod(ByVal _sortmode As ComparerMode, ByVal _sortorder As SortOrder) As Comparison(Of Long)
        Get
            Dim _method As Comparison(Of Long) = Nothing
            If _sortorder = SortOrder.Ascending Then
                ' 昇順
                Select Case _sortmode
                    Case ComparerMode.Data
                        _method = AddressOf Compare_ModeData_Ascending
                    Case ComparerMode.Id
                        _method = AddressOf Compare_ModeId_Ascending
                    Case ComparerMode.Name
                        _method = AddressOf Compare_ModeName_Ascending
                    Case ComparerMode.Nickname
                        _method = AddressOf Compare_ModeNickName_Ascending
                    Case ComparerMode.Source
                        _method = AddressOf Compare_ModeSource_Ascending
                End Select
            Else
                ' 降順
                Select Case _sortmode
                    Case ComparerMode.Data
                        _method = AddressOf Compare_ModeData_Descending
                    Case ComparerMode.Id
                        _method = AddressOf Compare_ModeId_Descending
                    Case ComparerMode.Name
                        _method = AddressOf Compare_ModeName_Descending
                    Case ComparerMode.Nickname
                        _method = AddressOf Compare_ModeNickName_Descending
                    Case ComparerMode.Source
                        _method = AddressOf Compare_ModeSource_Descending
                End Select
            End If
            Return _method
        End Get
    End Property

    ' ソートモードとソートオーダーに従い使用する比較関数のアドレスを返す
    ' (overload 現在の使用中の比較関数のアドレスを返す)
    Public Overloads ReadOnly Property CmpMethod() As Comparison(Of Long)
        Get
            Return _CmpMethod
        End Get
    End Property

    ' ソートモードとソートオーダーに従い比較関数のアドレスを切り替え
    Private Sub SetCmpMethod(ByVal mode As ComparerMode, ByVal order As SortOrder)
        _CmpMethod = Me.CmpMethod(mode, order)
    End Sub

    'xがyより小さいときはマイナスの数、大きいときはプラスの数、
    '同じときは0を返す (こちらは未使用 一応比較関数群呼び出しの形のまま残しておく)
    Public Function Compare(ByVal x As Long, ByVal y As Long) _
            As Integer Implements IComparer(Of Long).Compare
        Return _CmpMethod(x, y)
    End Function

    ' 比較用関数群 いずれもステータスIDの順序を考慮する
    ' 注：ID比較でCTypeを使用しているが、abs(x-y)がInteger(Int32)に収まらないことはあり得ないのでこれでよい
    ' 本文比較　昇順
    Public Function Compare_ModeData_Ascending(ByVal x As Long, ByVal y As Long) As Integer
        Dim result As Integer = String.Compare(_statuses.Item(x).Data, _statuses.Item(y).Data)
        If result = 0 Then result = x.CompareTo(y)
        Return result
    End Function

    ' 本文比較　降順
    Public Function Compare_ModeData_Descending(ByVal x As Long, ByVal y As Long) As Integer
        Dim result As Integer = String.Compare(_statuses.Item(y).Data, _statuses.Item(x).Data)
        If result = 0 Then result = y.CompareTo(x)
        Return result
    End Function

    ' ステータスID比較　昇順
    Public Function Compare_ModeId_Ascending(ByVal x As Long, ByVal y As Long) As Integer
        Return x.CompareTo(y)
    End Function

    ' ステータスID比較　降順
    Public Function Compare_ModeId_Descending(ByVal x As Long, ByVal y As Long) As Integer
        Return y.CompareTo(x)
    End Function

    ' 表示名比較　昇順
    Public Function Compare_ModeName_Ascending(ByVal x As Long, ByVal y As Long) As Integer
        Dim result As Integer = String.Compare(_statuses.Item(x).Name, _statuses.Item(y).Name)
        If result = 0 Then result = x.CompareTo(y)
        Return result
    End Function

    ' 表示名比較　降順
    Public Function Compare_ModeName_Descending(ByVal x As Long, ByVal y As Long) As Integer
        Dim result As Integer = String.Compare(_statuses.Item(y).Name, _statuses.Item(x).Name)
        If result = 0 Then result = y.CompareTo(x)
        Return result
    End Function

    ' ユーザー名比較　昇順
    Public Function Compare_ModeNickName_Ascending(ByVal x As Long, ByVal y As Long) As Integer
        Dim result As Integer = String.Compare(_statuses.Item(x).Nickname, _statuses.Item(y).Nickname)
        If result = 0 Then result = x.CompareTo(y)
        Return result
    End Function

    ' ユーザー名比較　降順
    Public Function Compare_ModeNickName_Descending(ByVal x As Long, ByVal y As Long) As Integer
        Dim result As Integer = String.Compare(_statuses.Item(y).Nickname, _statuses.Item(x).Nickname)
        If result = 0 Then result = y.CompareTo(x)
        Return result
    End Function

    ' Source比較　昇順
    Public Function Compare_ModeSource_Ascending(ByVal x As Long, ByVal y As Long) As Integer
        Dim result As Integer = String.Compare(_statuses.Item(x).Source, _statuses.Item(y).Source)
        If result = 0 Then result = x.CompareTo(y)
        Return result
    End Function

    ' Source比較　降順
    Public Function Compare_ModeSource_Descending(ByVal x As Long, ByVal y As Long) As Integer
        Dim result As Integer = String.Compare(_statuses.Item(y).Source, _statuses.Item(x).Source)
        If result = 0 Then result = y.CompareTo(x)
        Return result
    End Function
End Class

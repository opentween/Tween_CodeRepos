Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports Tween.TweenCustomControl

Public Class Statuses
    Private _statuses As Dictionary(Of String, MyListItem)
    Private _addedIds As Specialized.StringCollection
    Private _tabs As TabInformations

    Public Sub New()
        _statuses = New Dictionary(Of String, MyListItem)
    End Sub

    Public Sub BeginUpdate()
        _addedIds = New Specialized.StringCollection()  'タブ追加用IDコレクション準備
    End Sub

    Public Function EndUpdate() As String
        Dim NotifyString As String = _tabs.Distribute(_addedIds)    'タブに追加
        _tabs.Sort()    'ソート
        Return NotifyString     '通知用メッセージを戻す
    End Function

    Public Sub Add(ByRef Item As MyListItem)
        _statuses.Add(Item.Id, Item)
        _addedIds.Add(Item.Id)
    End Sub

    Public Function Remove(ByVal Key As String) As Integer
        _statuses.Remove(Key)
    End Function

    Public WriteOnly Property TabsClass() As TabInformations
        Set(ByVal value As TabInformations)
            _tabs = value
        End Set
    End Property
End Class



'Public Class StatusDictionary
'    Private _tabs As TabInformations  '要素TabsClassクラスのジェネリックリストインスタンス（タブ情報用）
'    Private _statuses As New Dictionary(Of String, MyListItem)
'    Private _setting As Setting

'    Private _notifymsg As New NotifyMessage

'    Public Structure NotifyMessage
'        Public Count As Integer
'        Public Message As String
'        Public Reply As Boolean
'        Public Direct As Boolean
'        Public AllCount As Integer
'        Public SoundFile As String
'    End Structure

'    Public Sub New(ByVal Section As ListSection, ByVal MySetting As Setting)
'        _tabs = New TabInformations(_section.SortColumn, _section.SortOrder, MySetting)
'        _setting = MySetting
'    End Sub

'    Public ReadOnly Property Tabs() As TabInformations
'        Get
'            Return _tabs
'        End Get
'    End Property

'    Public ReadOnly Property Items(ByVal ID As String) As MyListItem
'        Get
'            Return _statuses(ID)
'        End Get
'    End Property

'    Public Function Add(ByVal Item As MyListItem, ByVal Initial As Boolean, ByVal DirectMessage As Boolean) As Boolean
'        If _statuses.ContainsKey(Item.Id) = True Then
'            Return False
'        End If

'        DoFilter(Item, Initial, DirectMessage)

'        _statuses.Add(Item.Id, Item)
'        Return True
'    End Function

'    Public Function Remove(ByVal ID As String) As Boolean
'        Dim rd As Boolean = False

'        If _statuses.ContainsKey(ID) Then
'            rd = _statuses(ID).Readed
'            _statuses.Remove(ID)
'        Else
'            Return False
'        End If

'        For Each ts As TabsClass In _tabs.Tabs
'            If ts.Contains(ID) Then
'                ts.Remove(ID)
'                ts.AllCount -= 1
'                If ts.UnreadManage And rd = False Then
'                    ts.UnreadCount -= 1
'                    ts.OldestUnreadItem = ""
'                    For Each tsid As String In ts.StatusIDs
'                        If _statuses(tsid).Readed = False Then
'                            ts.OldestUnreadItem = tsid
'                            Exit For
'                        End If
'                    Next
'                    If ts.UnreadCount = 0 Then ts.TabPage.ImageIndex = -1
'                End If
'            End If
'        Next
'    End Function

'    Public Property Readed(ByVal ID As String) As Boolean
'        Get
'            If _statuses.ContainsKey(ID) Then
'                Return _statuses(ID).Readed
'            Else
'                Return False
'            End If
'        End Get
'        Set(ByVal value As Boolean)
'            If _statuses.ContainsKey(ID) = False Then Return False

'            For Each ts As TabsClass In _tabs.Tabs
'                If ts.Contains(ID) Then
'                    If ts.UnreadManage And _statuses(ID).Readed <> value Then
'                        If value = True Then
'                            ts.UnreadCount -= 1
'                            If ts.OldestUnreadItem = ID Then
'                                Dim idx As Integer = ts.StatusIDs.IndexOf(ID)

'                            End If
'                            ts.OldestUnreadItem = ""
'                            For Each tsid As String In ts.StatusIDs
'                                If _statuses(tsid).Readed = False Then
'                                    ts.OldestUnreadItem = tsid
'                                    Exit For
'                                End If
'                            Next
'                            If ts.UnreadCount = 0 Then ts.TabPage.ImageIndex = -1
'                        End If
'                    End If
'                End If
'            Next
'        End Set
'    End Property

'    Public Function Contains(ByVal ID As String) As Boolean
'        Return _statuses.ContainsKey(ID)
'    End Function

'    Public Sub GetNotifyInfo(ByVal nf As NotifyMessage)
'        nf.AllCount = _notifymsg.AllCount
'        nf.Count = _notifymsg.Count
'        nf.Message = _notifymsg.Message
'        nf.Reply = _notifymsg.Reply
'        nf.Direct = _notifymsg.Direct
'        nf.SoundFile = _notifymsg.SoundFile
'        _notifymsg.AllCount = 0
'        _notifymsg.Count = 0
'        _notifymsg.Message = ""
'        _notifymsg.Reply = False
'        _notifymsg.Direct = False
'        _notifymsg.SoundFile = ""
'    End Sub

'    Private Sub DoFilter(ByVal Item As MyListItem, ByVal Initial As Boolean, ByVal Direct As Boolean)
'        Dim reply As Boolean = False
'        Dim nm As String
'        Dim snd As String = ""

'        If (Initial = False Or (Initial And _setting.Readed = False)) And _setting.UnreadManage Then
'            Item.Readed = False
'        End If

'        Dim mv As Boolean = False
'        Dim nf As Boolean = False
'        Dim mk As Boolean = False
'        If Direct = False Then
'            For Each ts As TabsClass In _tabs.Tabs
'                Dim hit As Boolean = False
'                If tskey <> "Recent" And tskey <> "Reply" And tskey <> "Direct" Then
'                    For Each ft As FilterClass In ts.Filters
'                        Dim bHit As Boolean = True
'                        Dim tBody As String = IIf(ft.SearchURL, Item.OrgData, Item.Data)
'                        If ft.SearchBoth Then
'                            If ft.IDFilter = "" Or Item.Name = ft.IDFilter Then
'                                For Each fs As String In ft.BodyFilter
'                                    If ft.UseRegex Then
'                                        If Regex.IsMatch(tBody, fs, RegexOptions.IgnoreCase) = False Then bHit = False
'                                    Else
'                                        If tBody.ToLower.Contains(fs.ToLower) = False Then bHit = False
'                                    End If
'                                    If bHit = False Then Exit For
'                                Next
'                            Else
'                                bHit = False
'                            End If
'                        Else
'                            For Each fs As String In ft.BodyFilter
'                                If ft.UseRegex Then
'                                    If Regex.IsMatch(Item.Name + tBody, fs, RegexOptions.IgnoreCase) = False Then bHit = False
'                                Else
'                                    If (Item.Name + tBody).ToLower.Contains(fs.ToLower) = False Then bHit = False
'                                End If
'                                If bHit = False Then Exit For
'                            Next
'                        End If
'                        If bHit = True Then
'                            hit = True
'                            If ft.SetMark Then mk = True
'                            If ft.moveFrom Then mv = True
'                        End If
'                        If hit And mv And mk Then Exit For
'                    Next
'                ElseIf tskey = "Reply" Then
'                    If Item.Reply Or Regex.IsMatch(Item.Data, "@" + _username + "([^a-zA-Z0-9_]|$)", RegexOptions.IgnoreCase) Then
'                        hit = True
'                        reply = True
'                    End If
'                End If

'                If hit Then
'                    ts.Add(Item.Id, Item.Readed)
'                    If ts.Notify Then nf = True
'                    If snd = "" Then snd = ts.SoundFile
'                End If
'            Next
'            If mv = False Then
'                Dim ts As TabsClass = _tabs.Tabs("Recent")
'                If mk Then Item.Mark = True
'                ts.Add(Item.Id, Item.Readed)
'                If ts.Notify Then nf = True
'                If snd = "" Then snd = ts.SoundFile
'            End If
'        Else
'            Dim ts As TabsClass = _tabs.Tabs("Direct")
'            ts.Add(Item.Id, Item.Readed)
'            nf = ts.Notify
'            snd = ts.SoundFile
'        End If


'        nm = ""
'        Select Case _setting.NameBalloon
'            Case NameBalloonEnum.None
'                nm = ""
'            Case NameBalloonEnum.UserID
'                nm = lItem.Name
'            Case NameBalloonEnum.NickName
'                nm = lItem.Nick
'        End Select
'        Dim pmsg As String
'        pmsg = nm + " : " + Item.Data
'        If nf = True Then
'            _notifymsg.Count += 1
'            If _notifymsg.Message = "" Then
'                _notifymsg.Message = pmsg
'            Else
'                _notifymsg.Message += vbCrLf + pmsg
'            End If
'            If reply Then _notifymsg.Reply = True
'            If Direct Then _notifymsg.Direct = True
'        End If
'        _notifymsg.AllCount += 1
'        _notifymsg.SoundFile = snd
'    End Sub

'End Class

Public Class MyListItem
    Public Nick As String
    Public Data As String
    Public ImageUrl As String
    Public Name As String
    Public PDate As DateTime
    Public Id As String
    Public Fav As Boolean
    Public OrgData As String
    Public Read As Boolean
    Public Reply As Boolean
    Public Protect As Boolean
    Public OWL As Boolean
    Public Mark As Boolean
End Class

Public Class TabInformations
    '個別タブの情報をDictionaryで保持
    Private _sorter As ListViewItemComparerClass
    Private _tabs As New Dictionary(Of String, TabClass)()
    Private _statuses As Statuses

    Public Sub New()
        _sorter = New ListViewItemComparerClass
    End Sub

    Public Sub Add(ByVal TabText As String)
        _tabs.Add(TabText, New TabClass)
    End Sub

    Public Function Contains(ByVal TabText As String) As Boolean
        Return _tabs.ContainsKey(TabText)
    End Function

    Public Function Contains(ByVal ts As TabClass) As Boolean
        Return _tabs.ContainsValue(ts)
    End Function

    Public ReadOnly Property Tabs() As Dictionary(Of String, TabClass)
        Get
            Return _tabs
        End Get
    End Property

    Public ReadOnly Property Keys() As Collections.Generic.Dictionary(Of String, TabClass).KeyCollection
        Get
            Return _tabs.Keys
        End Get
    End Property

    Public Sub Sort()
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

    Public WriteOnly Property Statuses() As Statuses
        Set(ByVal value As Statuses)
            _statuses = value
        End Set
    End Property

    Public Function Distribute(ByVal AddedIDs As Specialized.StringCollection) As String
        '各タブのフィルターと照合。合致したらタブにID追加
        Return "通知メッセージ"
    End Function

End Class

Public Class TabClass
    Private _tabPage As System.Windows.Forms.TabPage
    Private _listCustom As DetailsListView
    'Public colHd1 As System.Windows.Forms.ColumnHeader
    'Public colHd2 As System.Windows.Forms.ColumnHeader
    'Public colHd3 As System.Windows.Forms.ColumnHeader
    'Public colHd4 As System.Windows.Forms.ColumnHeader
    'Public colHd5 As System.Windows.Forms.ColumnHeader
    'Public idCol As System.Collections.Specialized.StringCollection
    'Public tabName As String
    'Public sorter As ListViewItemComparer
    Private _unreadManage As Boolean
    Private _notify As Boolean
    Private _soundFile As String
    Private _filters As List(Of FilterClass)
    Private _modified As Boolean
    'Private _oldestUnreadItem As ListViewItem
    Private _oldestUnreadItem As String     'ID
    Private _unreadCount As Integer
    Private _allCount As Integer
    Private _ids As List(Of String)

    Public Sub New()
        _filters = New List(Of FilterClass)
        _notify = True
        _soundFile = ""
        _unreadManage = True
        _ids = New List(Of String)
    End Sub

    Public Sub Sort(ByVal Sorter As ListViewItemComparerClass)
        _ids.Sort(Sorter)
    End Sub

    Public Sub Add(ByVal ID As String, ByVal Read As Boolean)
        If _ids.Contains(ID) Then Exit Sub

        _ids.Add(ID)
        If _oldestUnreadItem = "" Then
            If ID < _oldestUnreadItem Then _oldestUnreadItem = ID
        Else
            _oldestUnreadItem = ID
        End If

        _allCount += 1
        If Read = False And _unreadManage Then _unreadCount += 1
    End Sub

    Public Sub Remove(ByVal ID As String)
        If _ids.Contains(ID) = False Then Exit Sub

        _ids.Remove(ID)
    End Sub

    Public Property TabPage() As TabPage
        Get
            Return _tabPage
        End Get
        Set(ByVal value As TabPage)
            _tabPage = value
        End Set
    End Property

    Public Property ListCustom() As DetailsListView
        Get
            Return _listCustom
        End Get
        Set(ByVal value As DetailsListView)
            _listCustom = value
        End Set
    End Property

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

    Public Property Modified() As Boolean
        Get
            Return _modified
        End Get
        Set(ByVal value As Boolean)
            _modified = value
        End Set
    End Property

    Public Property OldestUnreadItem() As String
        Get
            Return _oldestUnreadItem
        End Get
        Set(ByVal value As String)
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

    Public Property AllCount() As Integer
        Get
            Return _allCount
        End Get
        Set(ByVal value As Integer)
            _allCount = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return _tabPage.Text
    End Function

    Public Property Filters() As List(Of FilterClass)
        Get
            Return _filters
        End Get
        Set(ByVal value As List(Of FilterClass))
            _filters = value
        End Set
    End Property

    Public Function Contains(ByVal ID As String) As Boolean
        Return _ids.Contains(ID)
    End Function

    Public ReadOnly Property StatusIDs() As List(Of String)
        Get
            Return _ids
        End Get
    End Property

End Class

Public Class FiltersClass
    Public IDFilter As String
    Public BodyFilter As New List(Of String)()
    Public SearchBoth As Boolean
    Public moveFrom As Boolean
    Public SetMark As Boolean
    Public SearchURL As Boolean
    Public UseRegex As Boolean
End Class


Public Class ListViewItemComparerClass
    Implements IComparer(Of String)

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
    Public Function Compare(ByVal x As String, ByVal y As String) _
            As Integer Implements IComparer(Of String).Compare
        Dim result As Integer = 0

        result = String.Compare(x, y)
        '降順の時は結果を+-逆にする
        If _order = SortOrder.Descending Then
            result = -result
        End If
        Return result

    End Function
End Class

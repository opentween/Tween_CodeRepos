Imports System.Configuration

Public Class ListElement
    Inherits ConfigurationElement

    'Public Enum IconSizes
    '    IconNone = 0
    '    Icon16 = 1
    '    Icon24 = 2
    '    Icon48 = 3
    'End Enum

    Public Sub New()

    End Sub

    Public Sub New(ByVal name As String)
        Me("name") = name
    End Sub

    <ConfigurationProperty("name", DefaultValue:="", IsKey:=True)> _
    Public Property Name() As String
        Get
            Return CStr(Me("name"))
        End Get
        Set(ByVal value As String)
            Me("name") = value
        End Set
    End Property

    '<ConfigurationProperty("width1", DefaultValue:=30)> _
    'Public Property Width1() As Integer
    '    Get
    '        Return CInt(Me("width1"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me("width1") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("width2", DefaultValue:=80)> _
    'Public Property Width2() As Integer
    '    Get
    '        Return CInt(Me("width2"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me("width2") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("width3", DefaultValue:=290)> _
    'Public Property Width3() As Integer
    '    Get
    '        Return CInt(Me("width3"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me("width3") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("width4", DefaultValue:=120)> _
    'Public Property Width4() As Integer
    '    Get
    '        Return CInt(Me("width4"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me("width4") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("width5", DefaultValue:=50)> _
    'Public Property Width5() As Integer
    '    Get
    '        Return CInt(Me("width5"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me("width5") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("sortcolumn", DefaultValue:=3)> _
    'Public Property SortColumn() As Integer
    '    Get
    '        Return CInt(Me("sortcolumn"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me("sortcolumn") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("sortorder", DefaultValue:=1)> _
    'Public Property SortOrder() As Integer
    '    Get
    '        Return CInt(Me("sortorder"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me("sortorder") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("iconsize", DefaultValue:=IconSizes.Icon24)> _
    'Public Property IconSize() As IconSizes
    '    Get
    '        Return Me("iconsize")
    '    End Get
    '    Set(ByVal value As IconSizes)
    '        Me("iconsize") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("displayindex1", DefaultValue:=0)> _
    'Public Property DisplayIndex1() As Integer
    '    Get
    '        Return CInt(Me("displayindex1"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me("displayindex1") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("displayindex2", DefaultValue:=1)> _
    'Public Property DisplayIndex2() As Integer
    '    Get
    '        Return CInt(Me("displayindex2"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me("displayindex2") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("displayindex3", DefaultValue:=2)> _
    'Public Property DisplayIndex3() As Integer
    '    Get
    '        Return CInt(Me("displayindex3"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me("displayindex3") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("displayindex4", DefaultValue:=3)> _
    'Public Property DisplayIndex4() As Integer
    '    Get
    '        Return CInt(Me("displayindex4"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me("displayindex4") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("displayindex5", DefaultValue:=4)> _
    'Public Property DisplayIndex5() As Integer
    '    Get
    '        Return CInt(Me("displayindex5"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me("displayindex5") = value
    '    End Set
    'End Property

    <ConfigurationProperty("unreadmanage", DefaultValue:=True)> _
    Public Property UnreadManage() As Boolean
        Get
            Return CBool(Me("unreadmanage"))
        End Get
        Set(ByVal value As Boolean)
            Me("unreadmanage") = value
        End Set
    End Property

    <ConfigurationProperty("notify", DefaultValue:=True)> _
    Public Property Notify() As Boolean
        Get
            Return CBool(Me("notify"))
        End Get
        Set(ByVal value As Boolean)
            Me("notify") = value
        End Set
    End Property

    <ConfigurationProperty("soundfile", DefaultValue:="")> _
    Public Property SoundFile() As String
        Get
            Return Me("soundfile")
        End Get
        Set(ByVal value As String)
            Me("soundfile") = value
        End Set
    End Property

End Class

'Public Class LogPost
'    Inherits ConfigurationElement

'    Public Enum Unread
'        Unread = 0
'        Readed = 1
'    End Enum

'    Public Enum Fav
'        NoFav = 0
'        Fav = 1
'    End Enum

'    Public Sub New()

'    End Sub

'    Public Sub New(ByVal name As String)
'        Me("name") = name
'    End Sub

'    <ConfigurationProperty("name", DefaultValue:="")> _
'    Public Property Name() As String
'        Get
'            Return CStr(Me("name"))
'        End Get
'        Set(ByVal value As String)
'            Me("name") = value
'        End Set
'    End Property

'    <ConfigurationProperty("name2", DefaultValue:="")> _
'    Public Property Name2() As String
'        Get
'            Return CStr(Me("name2"))
'        End Get
'        Set(ByVal value As String)
'            Me("name2") = value
'        End Set
'    End Property

'    <ConfigurationProperty("id", DefaultValue:="")> _
'    Public Property Id() As String
'        Get
'            Return CStr(Me("id"))
'        End Get
'        Set(ByVal value As String)
'            Me("id") = value
'        End Set
'    End Property

'    <ConfigurationProperty("nick", DefaultValue:="")> _
'    Public Property Nick() As String
'        Get
'            Return CStr(Me("nick"))
'        End Get
'        Set(ByVal value As String)
'            Me("nick") = value
'        End Set
'    End Property

'    <ConfigurationProperty("post", DefaultValue:="")> _
'    Public Property Post() As String
'        Get
'            Return CStr(Me("post"))
'        End Get
'        Set(ByVal value As String)
'            Me("post") = value
'        End Set
'    End Property

'    <ConfigurationProperty("pdate", DefaultValue:="")> _
'    Public Property PostDate() As String
'        Get
'            Return CStr(Me("pdate"))
'        End Get
'        Set(ByVal value As String)
'            Me("pdate") = value
'        End Set
'    End Property

'    <ConfigurationProperty("imgurl", DefaultValue:="")> _
'    Public Property ImageUrl() As String
'        Get
'            Return CStr(Me("imgurl"))
'        End Get
'        Set(ByVal value As String)
'            Me("imgurl") = value
'        End Set
'    End Property

'    <ConfigurationProperty("unread", DefaultValue:=Unread.Unread)> _
'    Public Property IsUnread() As Unread
'        Get
'            Return CType(Me("unread"), Unread)
'        End Get
'        Set(ByVal value As Unread)
'            Me("unread") = value
'        End Set
'    End Property

'    <ConfigurationProperty("fav", DefaultValue:=Fav.NoFav)> _
'    Public Property IsFav() As Fav
'        Get
'            Return CType(Me("fav"), Fav)
'        End Get
'        Set(ByVal value As Fav)
'            Me("fav") = value
'        End Set
'    End Property

'End Class

Public Class SelectedUser
    Inherits ConfigurationElement

    Public Sub New()

    End Sub

    Public Sub New(ByVal name As String)
        Me("name") = name
    End Sub

    '<ConfigurationProperty("name", DefaultValue:="", IsKey:=True, IsRequired:=True)> _
    <ConfigurationProperty("name", DefaultValue:="")> _
    Public Property Name() As String
        Get
            Return CStr(Me("name"))
        End Get
        Set(ByVal value As String)
            Me("name") = value
        End Set
    End Property

    <ConfigurationProperty("tabname", DefaultValue:="")> _
    Public Property TabName() As String
        Get
            Return CStr(Me("tabname"))
        End Get
        Set(ByVal value As String)
            Me("tabname") = value
        End Set
    End Property

    <ConfigurationProperty("idfilter", DefaultValue:="")> _
    Public Property IdFilter() As String
        Get
            Return CStr(Me("idfilter"))
        End Get
        Set(ByVal value As String)
            Me("idfilter") = value
        End Set
    End Property

    <ConfigurationProperty("bodyfilter", DefaultValue:="")> _
    Public Property BodyFilter() As String
        Get
            Return CStr(Me("bodyfilter"))
        End Get
        Set(ByVal value As String)
            Me("bodyfilter") = value
        End Set
    End Property

    <ConfigurationProperty("moveFrom", DefaultValue:=False)> _
    Public Property MoveFrom() As Boolean
        Get
            Return CBool(Me("moveFrom"))
        End Get
        Set(ByVal value As Boolean)
            Me("moveFrom") = value
        End Set
    End Property

    <ConfigurationProperty("searchboth", DefaultValue:=False)> _
    Public Property SearchBoth() As Boolean
        Get
            Return CBool(Me("searchboth"))
        End Get
        Set(ByVal value As Boolean)
            Me("searchboth") = value
        End Set
    End Property

    <ConfigurationProperty("regexenable", DefaultValue:=False)> _
    Public Property RegexEnable() As Boolean
        Get
            Return CBool(Me("regexenable"))
        End Get
        Set(ByVal value As Boolean)
            Me("regexenable") = value
        End Set
    End Property

    <ConfigurationProperty("urlsearch", DefaultValue:=False)> _
    Public Property UrlSearch() As Boolean
        Get
            Return CBool(Me("urlsearch"))
        End Get
        Set(ByVal value As Boolean)
            Me("urlsearch") = value
        End Set
    End Property

    <ConfigurationProperty("setmark", DefaultValue:=False)> _
    Public Property SetMark() As Boolean
        Get
            Return CBool(Me("setmark"))
        End Get
        Set(ByVal value As Boolean)
            Me("setmark") = value
        End Set
    End Property

End Class

Public Class ListElementCollection
    Inherits ConfigurationElementCollection

    Public Sub New()
        Dim lElement As ListElement = CType(CreateNewElement(), ListElement)
        Add(lElement)
    End Sub

    Public Sub New(ByVal Name As String)
        Dim lElement As ListElement = CType(CreateNewElement(Name), ListElement)
        Add(lElement)
    End Sub

    Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
        Return New ListElement
    End Function

    Protected Overloads Overrides Function CreateNewElement(ByVal Name As String) As System.Configuration.ConfigurationElement
        Return New ListElement(Name)
    End Function

    Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
        Return CType(element, ListElement).Name
    End Function

    Public Overrides ReadOnly Property CollectionType() As ConfigurationElementCollectionType
        Get
            Return ConfigurationElementCollectionType.AddRemoveClearMap
        End Get
    End Property

    Public Shadows Property AddElementName() As String
        Get
            Return MyBase.AddElementName
        End Get
        Set(ByVal value As String)
            MyBase.AddElementName = value
        End Set
    End Property

    Public Shadows Property ClearElementName() As String
        Get
            Return MyBase.ClearElementName
        End Get
        Set(ByVal value As String)
            MyBase.ClearElementName = value
        End Set
    End Property

    Public Shadows ReadOnly Property RemoveElementName() As String
        Get
            Return MyBase.RemoveElementName
        End Get
    End Property

    Public Shadows ReadOnly Property Count() As Integer
        Get
            Return MyBase.Count
        End Get
    End Property

    Default Public Shadows Property Item(ByVal index As Integer) As ListElement
        Get
            Return CType(BaseGet(index), ListElement)
        End Get
        Set(ByVal value As ListElement)
            If Not (BaseGet(index)) Is Nothing Then
                BaseRemoveAt(index)
            End If
            BaseAdd(index, value)
        End Set
    End Property

    Default Public Shadows ReadOnly Property Item(ByVal Name As String) As ListElement
        Get
            Return CType(BaseGet(Name), ListElement)
        End Get
    End Property

    Public Function IndexOf(ByVal listElement As ListElement) As Integer
        Return BaseIndexOf(listElement)
    End Function

    Public Sub Add(ByVal listElement As ListElement)
        BaseAdd(listElement)
    End Sub

    Protected Overrides Sub BaseAdd(ByVal element As System.Configuration.ConfigurationElement)
        MyBase.BaseAdd(element)
    End Sub

    Public Overloads Sub Remove(ByVal listElement As ListElement)
        If BaseIndexOf(listElement) >= 0 Then
            BaseRemove(listElement.Name)
        End If
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        BaseRemoveAt(index)
    End Sub

    Public Overloads Sub Remove(ByVal name As String)
        BaseRemove(name)
    End Sub

    Public Sub Clear()
        BaseClear()
    End Sub
End Class

'Public Class LogPostCollection
'    Inherits ConfigurationElementCollection

'    Public Sub New()
'        Dim lPost As LogPost = CType(CreateNewElement(), LogPost)
'        Add(lPost)
'    End Sub

'    Public Sub New(ByVal Name As String)
'        Dim lPost As LogPost = CType(CreateNewElement(Name), LogPost)
'        Add(lPost)
'    End Sub

'    Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
'        Return New LogPost
'    End Function

'    Protected Overloads Overrides Function CreateNewElement(ByVal Name As String) As System.Configuration.ConfigurationElement
'        Return New LogPost(Name)
'    End Function

'    Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
'        Return CType(element, LogPost).Name
'    End Function

'    Public Overrides ReadOnly Property CollectionType() As ConfigurationElementCollectionType
'        Get
'            Return ConfigurationElementCollectionType.AddRemoveClearMap
'        End Get
'    End Property

'    Public Shadows Property AddElementName() As String
'        Get
'            Return MyBase.AddElementName
'        End Get
'        Set(ByVal value As String)
'            MyBase.AddElementName = value
'        End Set
'    End Property

'    Public Shadows Property ClearElementName() As String
'        Get
'            Return MyBase.ClearElementName
'        End Get
'        Set(ByVal value As String)
'            MyBase.ClearElementName = value
'        End Set
'    End Property

'    Public Shadows ReadOnly Property RemoveElementName() As String
'        Get
'            Return MyBase.RemoveElementName
'        End Get
'    End Property

'    Public Shadows ReadOnly Property Count() As Integer
'        Get
'            Return MyBase.Count
'        End Get
'    End Property

'    Default Public Shadows Property Item(ByVal index As Integer) As LogPost
'        Get
'            Return CType(BaseGet(index), LogPost)
'        End Get
'        Set(ByVal value As LogPost)
'            If Not (BaseGet(index)) Is Nothing Then
'                BaseRemoveAt(index)
'            End If
'            BaseAdd(index, value)
'        End Set
'    End Property

'    Default Public Shadows ReadOnly Property Item(ByVal Name As String) As LogPost
'        Get
'            Return CType(BaseGet(Name), LogPost)
'        End Get
'    End Property

'    Public Function IndexOf(ByVal lPost As LogPost) As Integer
'        Return BaseIndexOf(lPost)
'    End Function

'    Public Sub Add(ByVal lPost As LogPost)
'        BaseAdd(lPost)
'    End Sub

'    Protected Overrides Sub BaseAdd(ByVal element As System.Configuration.ConfigurationElement)
'        MyBase.BaseAdd(element)
'    End Sub

'    Public Overloads Sub Remove(ByVal lPost As LogPost)
'        If BaseIndexOf(lPost) >= 0 Then
'            BaseRemove(lPost.Name)
'        End If
'    End Sub

'    Public Sub RemoveAt(ByVal index As Integer)
'        BaseRemoveAt(index)
'    End Sub

'    Public Overloads Sub Remove(ByVal name As String)
'        BaseRemove(name)
'    End Sub

'    Public Sub Clear()
'        BaseClear()
'    End Sub
'End Class

Public Class SelectedUserCollection
    Inherits ConfigurationElementCollection

    Public Sub New()
        Dim lElement As SelectedUser = CType(CreateNewElement(), SelectedUser)
        Add(lElement)
    End Sub

    Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
        Return New SelectedUser
    End Function

    Protected Overloads Overrides Function CreateNewElement(ByVal Name As String) As System.Configuration.ConfigurationElement
        Return New SelectedUser(Name)
    End Function

    Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
        Return CType(element, SelectedUser).Name
    End Function

    Public Overrides ReadOnly Property CollectionType() As ConfigurationElementCollectionType
        Get
            Return ConfigurationElementCollectionType.AddRemoveClearMap
        End Get
    End Property

    Public Shadows Property AddElementName() As String
        Get
            Return MyBase.AddElementName
        End Get
        Set(ByVal value As String)
            MyBase.AddElementName = value
        End Set
    End Property

    Public Shadows Property ClearElementName() As String
        Get
            Return MyBase.ClearElementName
        End Get
        Set(ByVal value As String)
            MyBase.ClearElementName = value
        End Set
    End Property

    Public Shadows ReadOnly Property RemoveElementName() As String
        Get
            Return MyBase.RemoveElementName
        End Get
    End Property

    Public Shadows ReadOnly Property Count() As Integer
        Get
            Return MyBase.Count
        End Get
    End Property

    Default Public Shadows Property Item(ByVal index As Integer) As SelectedUser
        Get
            Return CType(BaseGet(index), SelectedUser)
        End Get
        Set(ByVal value As SelectedUser)
            If Not (BaseGet(index)) Is Nothing Then
                BaseRemoveAt(index)
            End If
            BaseAdd(index, value)
        End Set
    End Property

    Default Public Shadows ReadOnly Property Item(ByVal Name As String) As SelectedUser
        Get
            Return CType(BaseGet(Name), SelectedUser)
        End Get
    End Property

    Public Function IndexOf(ByVal selectedUser As SelectedUser) As Integer
        Return BaseIndexOf(selectedUser)
    End Function

    Public Sub Add(ByVal selectedUser As SelectedUser)
        BaseAdd(selectedUser)
    End Sub

    Protected Overrides Sub BaseAdd(ByVal element As System.Configuration.ConfigurationElement)
        MyBase.BaseAdd(element)
    End Sub

    Public Overloads Sub Remove(ByVal selectedUser As SelectedUser)
        If BaseIndexOf(selectedUser) >= 0 Then
            BaseRemove(selectedUser.Name)
        End If
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        BaseRemoveAt(index)
    End Sub

    Public Overloads Sub Remove(ByVal name As String)
        BaseRemove(name)
    End Sub

    Public Sub Clear()
        BaseClear()
    End Sub
End Class

Public NotInheritable Class ListSection
    Inherits ConfigurationSection

    'Public Enum LogUnitEnum
    '    Minute
    '    Hour
    '    Day
    'End Enum

    'Public Enum IconSizes
    '    IconNone = 0
    '    Icon16 = 1
    '    Icon24 = 2
    '    Icon48 = 3
    '    Icon48_2 = 4
    'End Enum

    'Public Enum NameBalloonEnum
    '    None
    '    UserID
    '    NickName
    'End Enum

    'Public Enum DispTitleEnum
    '    None
    '    Ver
    '    Post
    '    UnreadRepCount
    '    UnreadAllCount
    '    UnreadAllRepCount
    '    UnreadCountAllCount
    'End Enum

    <ConfigurationProperty("listelement", IsDefaultCollection:=False)> _
    Public Property ListElement() As ListElementCollection
        Get
            Return CType(Me("listelement"), ListElementCollection)
        End Get
        Set(ByVal value As ListElementCollection)
            Me("listelement") = value
        End Set
    End Property

    '<ConfigurationProperty("lpost", IsDefaultCollection:=False)> _
    'Public Property LogPosts() As LogPostCollection
    '    Get
    '        Return CType(Me("lpost"), LogPostCollection)
    '    End Get
    '    Set(ByVal value As LogPostCollection)
    '        Me("lpost") = value
    '    End Set
    'End Property

    <ConfigurationProperty("username", DefaultValue:="")> _
    Public Property UserName() As String
        Get
            Return CStr(Me("username"))
        End Get
        Set(ByVal value As String)
            Me("username") = value
        End Set
    End Property

    <ConfigurationProperty("password", DefaultValue:="")> _
    Public Property Password() As String
        Get
            Dim pwd As String = ""
            If CStr(Me("password")).Length > 0 Then
                Try
                    pwd = DecryptString(CStr(Me("password")))
                Catch ex As Exception
                    pwd = ""
                End Try
            End If
            Return pwd
        End Get
        Set(ByVal value As String)
            Dim pwd As String = value.Trim()
            If pwd.Length > 0 Then
                Try
                    Me("password") = EncryptString(value)
                Catch ex As Exception
                    Me("password") = ""
                End Try
            Else
                Me("password") = ""
            End If
        End Set
    End Property

    '<ConfigurationProperty("lastid", DefaultValue:="")> _
    'Public Property LastID() As String
    '    Get
    '        Return CStr(Me("lastid"))
    '    End Get
    '    Set(ByVal value As String)
    '        Me("lastid") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("lastname", DefaultValue:="")> _
    'Public Property LastName() As String
    '    Get
    '        Return CStr(Me("lastname"))
    '    End Get
    '    Set(ByVal value As String)
    '        Me("lastname") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("favname", DefaultValue:="")> _
    'Public Property FavName() As String
    '    Get
    '        Return CStr(Me("favname"))
    '    End Get
    '    Set(ByVal value As String)
    '        Me("favname") = value
    '    End Set
    'End Property

    <ConfigurationProperty("formlocation", DefaultValue:="0, 0")> _
    Public Property FormLocation() As Point
        Get
            Return CType(Me("formlocation"), Point)
        End Get
        Set(ByVal value As Point)
            Me("formlocation") = value
        End Set
    End Property

    <ConfigurationProperty("splitterdistance", DefaultValue:=320)> _
    Public Property SplitterDistance() As Integer
        Get
            Return CInt(Me("splitterdistance"))
        End Get
        Set(ByVal value As Integer)
            Me("splitterdistance") = value
        End Set
    End Property

    <ConfigurationProperty("formsize", DefaultValue:="436, 476")> _
    Public Property FormSize() As Size
        Get
            Return CType(Me("formsize"), Size)
        End Get
        Set(ByVal value As Size)
            Me("formsize") = value
        End Set
    End Property

    <ConfigurationProperty("nextpagethreshold", DefaultValue:=20), _
     IntegerValidator(MaxValue:=20, MinValue:=1)> _
    Public Property NextPageThreshold() As Integer
        Get
            Return CInt(Me("nextpagethreshold"))
        End Get
        Set(ByVal value As Integer)
            Me("nextpagethreshold") = value
        End Set
    End Property

    <ConfigurationProperty("nextpages", DefaultValue:=1), _
     IntegerValidator(MaxValue:=20, MinValue:=1)> _
    Public Property NextPages() As Integer
        Get
            Return CInt(Me("nextpages"))
        End Get
        Set(ByVal value As Integer)
            Me("nextpages") = value
        End Set
    End Property

    <ConfigurationProperty("timelineperiod", DefaultValue:=60), _
     IntegerValidator(MaxValue:=3600, MinValue:=0)> _
    Public Property TimelinePeriod() As Integer
        Get
            Return CInt(Me("timelineperiod"))
        End Get
        Set(ByVal value As Integer)
            Me("timelineperiod") = value
        End Set
    End Property

    <ConfigurationProperty("dmperiod", DefaultValue:=600), _
     IntegerValidator(MaxValue:=3600, MinValue:=0)> _
    Public Property DMPeriod() As Integer
        Get
            Return CInt(Me("dmperiod"))
        End Get
        Set(ByVal value As Integer)
            Me("dmperiod") = value
        End Set
    End Property

    '<ConfigurationProperty("logdays", DefaultValue:=3), _
    ' IntegerValidator(MaxValue:=999, MinValue:=0)> _
    'Public Property LogDays() As Integer
    '    Get
    '        Return CInt(Me("logdays"))
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me("logdays") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("logunit", DefaultValue:=LogUnitEnum.Hour)> _
    'Public Property LogUnit() As LogUnitEnum
    '    Get
    '        Return Me("logunit")
    '    End Get
    '    Set(ByVal value As LogUnitEnum)
    '        Me("logunit") = value
    '    End Set
    'End Property

    <ConfigurationProperty("readpages", DefaultValue:=10), _
     IntegerValidator(MaxValue:=999, MinValue:=0)> _
    Public Property ReadPages() As Integer
        Get
            Return CInt(Me("readpages"))
        End Get
        Set(ByVal value As Integer)
            Me("readpages") = value
        End Set
    End Property

    <ConfigurationProperty("readpagesreply", DefaultValue:=1), _
     IntegerValidator(MaxValue:=999, MinValue:=0)> _
    Public Property ReadPagesReply() As Integer
        Get
            Return CInt(Me("readpagesreply"))
        End Get
        Set(ByVal value As Integer)
            Me("readpagesreply") = value
        End Set
    End Property

    <ConfigurationProperty("readpagesdm", DefaultValue:=1), _
     IntegerValidator(MaxValue:=999, MinValue:=0)> _
    Public Property ReadPagesDM() As Integer
        Get
            Return CInt(Me("readpagesdm"))
        End Get
        Set(ByVal value As Integer)
            Me("readpagesdm") = value
        End Set
    End Property
    <ConfigurationProperty("maxpostnum", DefaultValue:=125), _
     IntegerValidator(MaxValue:=999, MinValue:=1)> _
    Public Property MaxPostNum() As Integer
        Get
            Return CInt(Me("maxpostnum"))
        End Get
        Set(ByVal value As Integer)
            Me("maxpostnum") = value
        End Set
    End Property

    <ConfigurationProperty("readed", DefaultValue:=True)> _
    Public Property Readed() As Boolean
        Get
            Return CBool(Me("readed"))
        End Get
        Set(ByVal value As Boolean)
            Me("readed") = value
        End Set
    End Property

    <ConfigurationProperty("listlock", DefaultValue:=False)> _
    Public Property ListLock() As Boolean
        Get
            Return CBool(Me("listlock"))
        End Get
        Set(ByVal value As Boolean)
            Me("listlock") = value
        End Set
    End Property

    <ConfigurationProperty("listiconsize", DefaultValue:=IconSizes.Icon16)> _
    Public Property IconSize() As IconSizes
        Get
            Return Me("listiconsize")
        End Get
        Set(ByVal value As IconSizes)
            Me("listiconsize") = value
        End Set
    End Property

    <ConfigurationProperty("selecteduser", IsDefaultCollection:=False)> _
    Public Property SelectedUser() As SelectedUserCollection
        Get
            Return CType(Me("selecteduser"), SelectedUserCollection)
        End Get
        Set(ByVal value As SelectedUserCollection)
            Me("selecteduser") = value
        End Set
    End Property

    <ConfigurationProperty("newallpop", DefaultValue:=True)> _
    Public Property NewAllPop() As Boolean
        Get
            Return CBool(Me("newallpop"))
        End Get
        Set(ByVal value As Boolean)
            Me("newallpop") = value
        End Set
    End Property

    '<ConfigurationProperty("newreplypop", DefaultValue:=False)> _
    'Public Property NewReplyPop() As Boolean
    '    Get
    '        Return CBool(Me("newreplypop"))
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me("newreplypop") = value
    '    End Set
    'End Property

    '<ConfigurationProperty("newdmpop", DefaultValue:=False)> _
    'Public Property NewDMPop() As Boolean
    '    Get
    '        Return CBool(Me("newdmpop"))
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me("newdmpop") = value
    '    End Set
    'End Property

    <ConfigurationProperty("statustext", DefaultValue:="")> _
    Public Property StatusText() As String
        Get
            Return CStr(Me("statustext"))
        End Get
        Set(ByVal value As String)
            Me("statustext") = value
        End Set
    End Property

    <ConfigurationProperty("playsound", DefaultValue:=False)> _
    Public Property PlaySound() As Boolean
        Get
            Return CBool(Me("playsound"))
        End Get
        Set(ByVal value As Boolean)
            Me("playsound") = value
        End Set
    End Property

    <ConfigurationProperty("unreadmanage", DefaultValue:=True)> _
    Public Property UnreadManage() As Boolean
        Get
            Return CBool(Me("unreadmanage"))
        End Get
        Set(ByVal value As Boolean)
            Me("unreadmanage") = value
        End Set
    End Property

    <ConfigurationProperty("onewaylove", DefaultValue:=True)> _
    Public Property OneWayLove() As Boolean
        Get
            Return CBool(Me("onewaylove"))
        End Get
        Set(ByVal value As Boolean)
            Me("onewaylove") = value
        End Set
    End Property

    <ConfigurationProperty("fontunread", DefaultValue:="MS UI Gothic, 9pt, style=Bold, Underline")> _
    Public Property FontUnread() As System.Drawing.Font
        Get
            Return Me("fontunread")
        End Get
        Set(ByVal value As Font)
            Me("fontunread") = value
        End Set
    End Property

    <ConfigurationProperty("colorunread", DefaultValue:="ControlText")> _
    Public Property ColorUnread() As Color
        Get
            Return Me("colorunread")
        End Get
        Set(ByVal value As Color)
            Me("colorunread") = value
        End Set
    End Property

    <ConfigurationProperty("fontreaded", DefaultValue:="MS UI Gothic, 9pt")> _
    Public Property FontReaded() As Font
        Get
            Return Me("fontreaded")
        End Get
        Set(ByVal value As Font)
            Me("fontreaded") = value
        End Set
    End Property

    <ConfigurationProperty("colorreaded", DefaultValue:="ControlText")> _
    Public Property ColorReaded() As Color
        Get
            Return Me("colorreaded")
        End Get
        Set(ByVal value As Color)
            Me("colorreaded") = value
        End Set
    End Property

    <ConfigurationProperty("colorfav", DefaultValue:="Red")> _
    Public Property ColorFav() As Color
        Get
            Return Me("colorfav")
        End Get
        Set(ByVal value As Color)
            Me("colorfav") = value
        End Set
    End Property

    <ConfigurationProperty("colorOWL", DefaultValue:="Blue")> _
    Public Property ColorOWL() As Color
        Get
            Return Me("colorOWL")
        End Get
        Set(ByVal value As Color)
            Me("colorOWL") = value
        End Set
    End Property

    <ConfigurationProperty("fontDetail", DefaultValue:="MS UI Gothic, 9pt")> _
    Public Property FontDetail() As Font
        Get
            Return Me("fontDetail")
        End Get
        Set(ByVal value As Font)
            Me("fontDetail") = value
        End Set
    End Property

    <ConfigurationProperty("colorSelf", DefaultValue:="AliceBlue")> _
    Public Property ColorSelf() As Color
        Get
            Return Me("colorSelf")
        End Get
        Set(ByVal value As Color)
            Me("colorSelf") = value
        End Set
    End Property

    <ConfigurationProperty("colorAtSelf", DefaultValue:="AntiqueWhite")> _
    Public Property ColorAtSelf() As Color
        Get
            Return Me("colorAtSelf")
        End Get
        Set(ByVal value As Color)
            Me("colorAtSelf") = value
        End Set
    End Property

    <ConfigurationProperty("colorTarget", DefaultValue:="LemonChiffon")> _
    Public Property ColorTarget() As Color
        Get
            Return Me("colorTarget")
        End Get
        Set(ByVal value As Color)
            Me("colorTarget") = value
        End Set
    End Property

    <ConfigurationProperty("colorAtTarget", DefaultValue:="LavenderBlush")> _
    Public Property ColorAtTarget() As Color
        Get
            Return Me("colorAtTarget")
        End Get
        Set(ByVal value As Color)
            Me("colorAtTarget") = value
        End Set
    End Property

    <ConfigurationProperty("colorAtFromTarget", DefaultValue:="Honeydew")> _
    Public Property ColorAtFromTarget() As Color
        Get
            Return Me("colorAtFromTarget")
        End Get
        Set(ByVal value As Color)
            Me("colorAtFromTarget") = value
        End Set
    End Property

    <ConfigurationProperty("nameballoon", DefaultValue:=NameBalloonEnum.NickName)> _
    Public Property NameBalloon() As NameBalloonEnum
        Get
            Return Me("nameballoon")
        End Get
        Set(ByVal value As NameBalloonEnum)
            Me("nameballoon") = value
        End Set
    End Property

    <ConfigurationProperty("width1", DefaultValue:=30)> _
    Public Property Width1() As Integer
        Get
            Return CInt(Me("width1"))
        End Get
        Set(ByVal value As Integer)
            Me("width1") = value
        End Set
    End Property

    <ConfigurationProperty("width2", DefaultValue:=80)> _
    Public Property Width2() As Integer
        Get
            Return CInt(Me("width2"))
        End Get
        Set(ByVal value As Integer)
            Me("width2") = value
        End Set
    End Property

    <ConfigurationProperty("width3", DefaultValue:=290)> _
    Public Property Width3() As Integer
        Get
            Return CInt(Me("width3"))
        End Get
        Set(ByVal value As Integer)
            Me("width3") = value
        End Set
    End Property

    <ConfigurationProperty("width4", DefaultValue:=120)> _
    Public Property Width4() As Integer
        Get
            Return CInt(Me("width4"))
        End Get
        Set(ByVal value As Integer)
            Me("width4") = value
        End Set
    End Property

    <ConfigurationProperty("width5", DefaultValue:=50)> _
    Public Property Width5() As Integer
        Get
            Return CInt(Me("width5"))
        End Get
        Set(ByVal value As Integer)
            Me("width5") = value
        End Set
    End Property

    <ConfigurationProperty("sortcolumn", DefaultValue:=3)> _
    Public Property SortColumn() As Integer
        Get
            Return CInt(Me("sortcolumn"))
        End Get
        Set(ByVal value As Integer)
            Me("sortcolumn") = value
        End Set
    End Property

    <ConfigurationProperty("sortorder", DefaultValue:=1)> _
    Public Property SortOrder() As Integer
        Get
            Return CInt(Me("sortorder"))
        End Get
        Set(ByVal value As Integer)
            Me("sortorder") = value
        End Set
    End Property

    <ConfigurationProperty("displayindex1", DefaultValue:=0)> _
    Public Property DisplayIndex1() As Integer
        Get
            Return CInt(Me("displayindex1"))
        End Get
        Set(ByVal value As Integer)
            Me("displayindex1") = value
        End Set
    End Property

    <ConfigurationProperty("displayindex2", DefaultValue:=1)> _
    Public Property DisplayIndex2() As Integer
        Get
            Return CInt(Me("displayindex2"))
        End Get
        Set(ByVal value As Integer)
            Me("displayindex2") = value
        End Set
    End Property

    <ConfigurationProperty("displayindex3", DefaultValue:=2)> _
    Public Property DisplayIndex3() As Integer
        Get
            Return CInt(Me("displayindex3"))
        End Get
        Set(ByVal value As Integer)
            Me("displayindex3") = value
        End Set
    End Property

    <ConfigurationProperty("displayindex4", DefaultValue:=3)> _
    Public Property DisplayIndex4() As Integer
        Get
            Return CInt(Me("displayindex4"))
        End Get
        Set(ByVal value As Integer)
            Me("displayindex4") = value
        End Set
    End Property

    <ConfigurationProperty("displayindex5", DefaultValue:=4)> _
    Public Property DisplayIndex5() As Integer
        Get
            Return CInt(Me("displayindex5"))
        End Get
        Set(ByVal value As Integer)
            Me("displayindex5") = value
        End Set
    End Property

    <ConfigurationProperty("postctrlenter", DefaultValue:=False)> _
    Public Property PostCtrlEnter() As Boolean
        Get
            Return CBool(Me("postctrlenter"))
        End Get
        Set(ByVal value As Boolean)
            Me("postctrlenter") = value
        End Set
    End Property

    <ConfigurationProperty("useapi", DefaultValue:=False)> _
    Public Property UseAPI() As Boolean
        Get
            Return CBool(Me("useapi"))
        End Get
        Set(ByVal value As Boolean)
            Me("useapi") = value
        End Set
    End Property

    <ConfigurationProperty("checkreply", DefaultValue:=True)> _
    Public Property CheckReply() As Boolean
        Get
            Return CBool(Me("checkreply"))
        End Get
        Set(ByVal value As Boolean)
            Me("checkreply") = value
        End Set
    End Property

    <ConfigurationProperty("userecommendstatus", DefaultValue:=False)> _
    Public Property UseRecommendStatus() As Boolean
        Get
            Return CBool(Me("userecommendstatus"))
        End Get
        Set(ByVal value As Boolean)
            Me("userecommendstatus") = value
        End Set
    End Property

    <ConfigurationProperty("dispusername", DefaultValue:=False)> _
    Public Property DispUsername() As Boolean
        Get
            Return CBool(Me("dispusername"))
        End Get
        Set(ByVal value As Boolean)
            Me("dispusername") = value
        End Set
    End Property

    <ConfigurationProperty("minimizetotray", DefaultValue:=False)> _
    Public Property MinimizeToTray() As Boolean
        Get
            Return CBool(Me("minimizetotray"))
        End Get
        Set(ByVal value As Boolean)
            Me("minimizetotray") = value
        End Set
    End Property

    <ConfigurationProperty("closetoexit", DefaultValue:=False)> _
Public Property CloseToExit() As Boolean
        Get
            Return CBool(Me("closetoexit"))
        End Get
        Set(ByVal value As Boolean)
            Me("closetoexit") = value
        End Set
    End Property

    <ConfigurationProperty("displatestpost", DefaultValue:=DispTitleEnum.Post)> _
    Public Property DispLatestPost() As DispTitleEnum
        Get
            Return Me("displatestpost")
        End Get
        Set(ByVal value As DispTitleEnum)
            Me("displatestpost") = value
        End Set
    End Property

    <ConfigurationProperty("hubserver", DefaultValue:="twitter.com")> _
    Public Property HubServer() As String
        Get
            Return CStr(Me("hubserver"))
        End Get
        Set(ByVal value As String)
            Me("hubserver") = value
        End Set
    End Property

    <ConfigurationProperty("browserpath", DefaultValue:="")> _
    Public Property BrowserPath() As String
        Get
            Return CStr(Me("browserpath"))
        End Get
        Set(ByVal value As String)
            Me("browserpath") = value
        End Set
    End Property

    <ConfigurationProperty("sortorderlock", DefaultValue:=False)> _
    Public Property SortOrderLock() As Boolean
        Get
            Return CBool(Me("sortorderlock"))
        End Get
        Set(ByVal value As Boolean)
            Me("sortorderlock") = value
        End Set
    End Property

    <ConfigurationProperty("tinyurlresolve", DefaultValue:=True)> _
    Public Property TinyURLResolve() As Boolean
        Get
            Return CBool(Me("tinyurlresolve"))
        End Get
        Set(ByVal value As Boolean)
            Me("tinyurlresolve") = value
        End Set
    End Property

    <ConfigurationProperty("proxytype", DefaultValue:=ProxyTypeEnum.IE)> _
    Public Property ProxyType() As ProxyTypeEnum
        Get
            Return Me("proxytype")
        End Get
        Set(ByVal value As ProxyTypeEnum)
            Me("proxytype") = value
        End Set
    End Property

    <ConfigurationProperty("proxyaddress", DefaultValue:="127.0.0.1")> _
    Public Property ProxyAddress() As String
        Get
            Return Me("proxyaddress")
        End Get
        Set(ByVal value As String)
            Me("proxyaddress") = value
        End Set
    End Property

    <ConfigurationProperty("proxyport", DefaultValue:=80)> _
    Public Property ProxyPort() As Integer
        Get
            Return Me("proxyport")
        End Get
        Set(ByVal value As Integer)
            Me("proxyport") = value
        End Set
    End Property

    <ConfigurationProperty("proxyuser", DefaultValue:="")> _
    Public Property ProxyUser() As String
        Get
            Return Me("proxyuser")
        End Get
        Set(ByVal value As String)
            Me("proxyuser") = value
        End Set
    End Property

    <ConfigurationProperty("proxypassword", DefaultValue:="")> _
    Public Property ProxyPassword() As String
        Get
            Dim pwd As String = ""
            If CStr(Me("proxypassword")).Length > 0 Then
                Try
                    pwd = DecryptString(CStr(Me("proxypassword")))
                Catch ex As Exception
                    pwd = ""
                End Try
            End If
            Return pwd
        End Get
        Set(ByVal value As String)
            Dim pwd As String = value.Trim()
            If pwd.Length > 0 Then
                Try
                    Me("proxypassword") = EncryptString(value)
                Catch ex As Exception
                    Me("proxypassword") = ""
                End Try
            Else
                Me("proxypassword") = ""
            End If
        End Set
    End Property

    <ConfigurationProperty("periodadjust", DefaultValue:=True)> _
    Public Property PeriodAdjust() As Boolean
        Get
            Return CBool(Me("periodadjust"))
        End Get
        Set(ByVal value As Boolean)
            Me("periodadjust") = value
        End Set
    End Property

    <ConfigurationProperty("startupversion", DefaultValue:=True)> _
    Public Property StartupVersion() As Boolean
        Get
            Return CBool(Me("startupversion"))
        End Get
        Set(ByVal value As Boolean)
            Me("startupversion") = value
        End Set
    End Property

    <ConfigurationProperty("startupkey", DefaultValue:=True)> _
    Public Property StartupKey() As Boolean
        Get
            Return CBool(Me("startupkey"))
        End Get
        Set(ByVal value As Boolean)
            Me("startupkey") = value
        End Set
    End Property

    <ConfigurationProperty("startupfollowers", DefaultValue:=True)> _
    Public Property StartupFollowers() As Boolean
        Get
            Return CBool(Me("startupfollowers"))
        End Get
        Set(ByVal value As Boolean)
            Me("startupfollowers") = value
        End Set
    End Property

    Private Function EncryptString(ByVal str As String) As String
        '文字列をバイト型配列にする
        Dim bytesIn As Byte() = System.Text.Encoding.UTF8.GetBytes(str)

        'DESCryptoServiceProviderオブジェクトの作成
        Dim des As New System.Security.Cryptography.DESCryptoServiceProvider

        '共有キーと初期化ベクタを決定
        'パスワードをバイト配列にする
        Dim bytesKey As Byte() = System.Text.Encoding.UTF8.GetBytes("_tween_encrypt_key_")
        '共有キーと初期化ベクタを設定
        des.Key = ResizeBytesArray(bytesKey, des.Key.Length)
        des.IV = ResizeBytesArray(bytesKey, des.IV.Length)

        '暗号化されたデータを書き出すためのMemoryStream
        Using msOut As New System.IO.MemoryStream
            'DES暗号化オブジェクトの作成
            Using desdecrypt As System.Security.Cryptography.ICryptoTransform = _
                des.CreateEncryptor()

                '書き込むためのCryptoStreamの作成
                Using cryptStream As New System.Security.Cryptography.CryptoStream( _
                    msOut, desdecrypt, _
                    System.Security.Cryptography.CryptoStreamMode.Write)
                    '書き込む
                    cryptStream.Write(bytesIn, 0, bytesIn.Length)
                    cryptStream.FlushFinalBlock()
                    '暗号化されたデータを取得
                    Dim bytesOut As Byte() = msOut.ToArray()

                    '閉じる
                    cryptStream.Close()
                    msOut.Close()

                    'Base64で文字列に変更して結果を返す
                    Return System.Convert.ToBase64String(bytesOut)
                End Using
            End Using
        End Using
    End Function

    Private Function DecryptString(ByVal str As String) As String
        'DESCryptoServiceProviderオブジェクトの作成
        Dim des As New System.Security.Cryptography.DESCryptoServiceProvider

        '共有キーと初期化ベクタを決定
        'パスワードをバイト配列にする
        Dim bytesKey As Byte() = System.Text.Encoding.UTF8.GetBytes("_tween_encrypt_key_")
        '共有キーと初期化ベクタを設定
        des.Key = ResizeBytesArray(bytesKey, des.Key.Length)
        des.IV = ResizeBytesArray(bytesKey, des.IV.Length)

        'Base64で文字列をバイト配列に戻す
        Dim bytesIn As Byte() = System.Convert.FromBase64String(str)
        '暗号化されたデータを読み込むためのMemoryStream
        Using msIn As New System.IO.MemoryStream(bytesIn)
            'DES復号化オブジェクトの作成
            Using desdecrypt As System.Security.Cryptography.ICryptoTransform = _
                des.CreateDecryptor()
                '読み込むためのCryptoStreamの作成
                Using cryptStreem As New System.Security.Cryptography.CryptoStream( _
                    msIn, desdecrypt, _
                    System.Security.Cryptography.CryptoStreamMode.Read)

                    '復号化されたデータを取得するためのStreamReader
                    Using srOut As New System.IO.StreamReader( _
                        cryptStreem, System.Text.Encoding.UTF8)
                        '復号化されたデータを取得する
                        Dim result As String = srOut.ReadToEnd()

                        '閉じる
                        srOut.Close()
                        cryptStreem.Close()
                        msIn.Close()

                        Return result
                    End Using
                End Using
            End Using
        End Using
    End Function

    Private Shared Function ResizeBytesArray(ByVal bytes() As Byte, _
                                ByVal newSize As Integer) As Byte()
        Dim newBytes(newSize - 1) As Byte
        If bytes.Length <= newSize Then
            Dim i As Integer
            For i = 0 To bytes.Length - 1
                newBytes(i) = bytes(i)
            Next i
        Else
            Dim pos As Integer = 0
            Dim i As Integer
            For i = 0 To bytes.Length - 1
                newBytes(pos) = newBytes(pos) Xor bytes(i)
                pos += 1
                If pos >= newBytes.Length Then
                    pos = 0
                End If
            Next i
        End If
        Return newBytes
    End Function
End Class

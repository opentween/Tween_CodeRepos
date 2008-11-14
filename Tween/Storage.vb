Imports System
Imports System.Collections.Generic
Imports System.Data
'Imports System.Data.SQLite

''' <summary>
''' 投稿の属性を表すフラグのセットを提供します。
''' </summary>
<Flags()> _
Public Enum PostAttributes
    Unknown = 0

    ''' <summary>
    ''' 投稿の種別が status であることを表します。
    ''' </summary>
    Status = 1

    ''' <summary>
    ''' 投稿の種別が direct message であることを表します。
    ''' </summary>
    ''' <remarks></remarks>
    DirectMessage = 2

    ''' <summary>
    ''' 投稿が既読であることを表します。
    ''' </summary>
    ''' <remarks></remarks>
    Read = 4

    ''' <summary>
    ''' 投稿がお気に入りに追加されていることを表します。
    ''' </summary>
    Favorite = 8

    ''' <summary>
    ''' 投稿が返信であることを表します。
    ''' </summary>
    ''' <remarks></remarks>
    Reply = 16

    ''' <summary>
    ''' 投稿の閲覧が通常より制限されていることを表します。
    ''' </summary>
    ''' <remarks><para></para></remarks>
    Protect = 32

    ''' <summary>
    ''' 投稿したユーザに対していわゆる片思いであることを表します。
    ''' </summary>
    ''' <remarks></remarks>
    OneWayLove = 64
End Enum

''' <summary>
''' Twitter の status と direct message を表します。
''' </summary>
Public Class Post
    Private ReadOnly _id As Long
    Private _attributes As PostAttributes
    Private ReadOnly _postedAt As Date
    Private ReadOnly _name As String
    Private ReadOnly _screenName As String
    Private ReadOnly _imageUri As Uri
    Private ReadOnly _text As String
    Private ReadOnly _hyperText As String
    Private ReadOnly _tags As List(Of String)
    Private ReadOnly _inReplyToUser As String
    Private ReadOnly _inReplyToId As Nullable(Of Long)

    ''' <summary>
    ''' 投稿を一意に表す ID を取得します。
    ''' </summary>
    ''' <returns>投稿を一意に表す ID。</returns>
    ''' <remarks>status と direct message での ID は互換性がないものと思われます。</remarks>
    Public ReadOnly Property Id() As Long
        Get
            Return Me._id
        End Get
    End Property

    ''' <summary>
    ''' 投稿に付与された属性を取得します。
    ''' </summary>
    ''' <value>投稿に付与された属性。</value>
    ''' <remarks>このプロパティの値はフラグとして提供されます。</remarks>
    Public ReadOnly Property Attributes() As PostAttributes
        Get
            Return Me._attributes
        End Get
    End Property

    ''' <summary>
    ''' 投稿された日時を表す整数値を取得します。
    ''' </summary>
    ''' <value>投稿された日時を表す整数値。</value>
    ''' <remarks>日時は協定世界時 (UTC) として格納されます。</remarks>
    Public ReadOnly Property Timestamp() As Long
        Get
            Return Me._postedAt.Ticks
        End Get
    End Property

    ''' <summary>
    ''' 投稿された日時を取得します。
    ''' </summary>
    ''' <value>投稿された日時。</value>
    ''' <remarks>日時は協定世界時 (UTC) として格納されます。</remarks>
    Public ReadOnly Property PostedAt() As Date
        Get
            Return Me._postedAt
        End Get
    End Property

    ''' <summary>
    ''' 投稿したユーザの名前を取得します。
    ''' </summary>
    ''' <value>投稿したユーザの名前。</value>
    ''' <remarks>名前はユーザによって頻繁に変更される可能性があり、ASCII 表示文字に限定されません。</remarks>
    Public ReadOnly Property Name() As String
        Get
            Return Me._name
        End Get
    End Property

    ''' <summary>
    ''' 投稿したユーザの表示名を取得します。
    ''' </summary>
    ''' <value>投稿したユーザの表示名。</value>
    ''' <remarks>表示名はユーザによって稀に変更される可能性があり、ASCII 表示文字に限定されるものと思われます。</remarks>
    Public ReadOnly Property ScreenName() As String
        Get
            Return Me._screenName
        End Get
    End Property

    ''' <summary>
    ''' 投稿したユーザのアイコンを示す URI を取得します。
    ''' </summary>
    ''' <value>投稿したユーザのアイコンを示す URI。</value>
    ''' <remarks>URI の示す画像はオリジナルのものから縮小されています。</remarks>
    Public ReadOnly Property ImageUri() As Uri
        Get
            Return Me._imageUri
        End Get
    End Property

    ''' <summary>
    ''' 投稿の本文を取得します。
    ''' </summary>
    ''' <value>投稿の本文。</value>
    ''' <remarks>改行やシステムにより付加された HTML 要素が存在する可能性があります。</remarks>
    Public ReadOnly Property Text() As String
        Get
            Return Me._text
        End Get
    End Property

    ''' <summary>
    ''' HTML 文章として整形された投稿の本文を取得します。
    ''' </summary>
    ''' <value>HTML 文章として整形された投稿の本文。</value>
    ''' <remarks>XML 文章としての正当性は必ずしも保証されません。</remarks>
    Public ReadOnly Property HyperText() As String
        Get
            Return Me._text
        End Get
    End Property

    ''' <summary>
    ''' 投稿に付与されたタグのコレクションを取得します。
    ''' </summary>
    ''' <value>投稿に付与されたタグのコレクション。</value>
    ''' <remarks>タグとは投稿を抽出するのに便利となる文字列で、検索や条件付操作に使用されます。</remarks>
    Public ReadOnly Property Tags() As List(Of String)
        Get
            Return Me._tags
        End Get
    End Property

    ''' <summary>
    ''' 投稿の返信元のユーザの表示名を取得します。
    ''' </summary>
    ''' <value>投稿の返信元のユーザの表示名。投稿が返信でない場合は null 参照 (Visual Basic では <c>Nothing</c>)。</value>
    ''' <remarks>URI の組み立てに使用可能であることが期待されます。</remarks>
    Public ReadOnly Property InReplyToUser() As String
        Get
            Return Me._inReplyToUser
        End Get
    End Property

    ''' <summary>
    ''' 投稿の返信元の投稿を一意に表す ID を取得します。
    ''' </summary>
    ''' <value>投稿の返信元の投稿を一意に表す ID。投稿が返信でない場合は null 参照 (Visual Basic では <c>Nothing</c>)。</value>
    ''' <remarks>URI の組み立てに使用可能であることが期待されます。</remarks>
    Public ReadOnly Property InReplyToId() As Nullable(Of Long)
        Get
            Return Me._inReplyToId
        End Get
    End Property

    ''' <summary>
    ''' 投稿がユーザによって既に読まれたかを表す値を取得します。
    ''' </summary>
    ''' <value>投稿がユーザによって既に読まれた場合は <c>true</c>。それ以外の場合は <c>false</c>。</value>
    ''' <remarks>このプロパティは Tween のユーザ インターフェイスに依存します。</remarks>
    Public ReadOnly Property IsRead() As Boolean
        Get
            Return Me.Attributes = PostAttributes.Read
        End Get
    End Property

    ''' <summary>
    ''' 投稿がお気に入りに追加されているかを表す値を取得します。
    ''' </summary>
    ''' <value>投稿がお気に入りに追加されている場合は <c>true</c>。それ以外の場合は <c>false</c>。</value>
    ''' <remarks>このプロパティは Twitter から取得できる情報に含まれる値以外に Tween のユーザ インターフェイスによっても変更される可能性があります。</remarks>
    Public ReadOnly Property IsFavorited() As Boolean
        Get
            Return Me.Attributes = PostAttributes.Favorite
        End Get
    End Property

    ''' <summary>
    ''' 投稿がユーザに対する返信かどうかを表す値を取得します。
    ''' </summary>
    ''' <value>投稿がユーザに対する返信である場合は <c>true</c>。それ以外の場合は <c>false</c>。</value>
    ''' <remarks>このプロパティは Twitter から取得できる情報のみによって導かれる不変の情報です。このプロパティの値は <see cref="InReplyToUser" /> および <see cref="InReplyToUser" /> の値が null (Visual Basic では <c>Nothing</c> であることと関係しません。</remarks>
    Public ReadOnly Property IsReply() As Boolean
        Get
            Return Me.Attributes = PostAttributes.Reply
        End Get
    End Property

    ''' <summary>
    ''' 投稿の閲覧が制限されているかどうかを表す値を取得します。
    ''' </summary>
    ''' <value>投稿の閲覧が制限されている場合は <c>true</c>。それ以外の場合は <c>false</c>。</value>
    ''' <remarks>投稿の閲覧が制限されている状態とは、投稿したユーザが投稿を公開していない設定にあることを表します。この設定はユーザによって変更される可能性があります。</remarks>
    Public ReadOnly Property IsProtected() As Boolean
        Get
            Return Me.Attributes = PostAttributes.Protect
        End Get
    End Property

    ''' <summary>
    ''' 投稿したユーザに対していわゆる片思いであるかどうかを表す値を取得します。
    ''' </summary>
    ''' <value>投稿したユーザに対していわゆる片思いである場合は <c>true</c>。それ以外の場合は <c>false</c>。</value>
    ''' <remarks>片思い (Tween では頻繁に OWL と表記されます) とは、ユーザがフォローしているが、当該ユーザからフォローされていない状態を表します。</remarks>
    Public ReadOnly Property IsOneWayLove() As Boolean
        Get
            Return Me.Attributes = PostAttributes.OneWayLove
        End Get
    End Property

    ''' <summary>
    ''' Post クラスの新しいインスタンスを初期化します。
    ''' </summary>
    ''' <param name="id">投稿を一意に表す ID。</param>
    ''' <param name="attribute">投稿に付与された属性。</param>
    ''' <param name="postedAt">投稿された日時。</param>
    ''' <param name="name">投稿したユーザの名前。</param>
    ''' <param name="screenName">投稿したユーザの表示名。</param>
    ''' <param name="imageUri">投稿したユーザのアイコンを示す URI。</param>
    ''' <param name="text">投稿の本文。</param>
    ''' <param name="tags">投稿に付与するタグのコレクション。</param>
    ''' <param name="inReplyToUser">投稿の返信元のユーザの表示名、または null 参照 (Visual Basic では <c>Nothing</c>)。</param>
    ''' <param name="inReplyToId">投稿の返信元の投稿を一意に表す ID、または null 参照 (Visual Basic では <c>Nothing</c>)。</param>
    ''' <remarks></remarks>
    Public Sub New( _
        ByVal id As Long, _
        ByVal attributes As PostAttributes, _
        ByVal postedAt As Date, _
        ByVal name As String, _
        ByVal screenName As String, _
        ByVal imageUri As Uri, _
        ByVal text As String, _
        ByVal hyperText As String, _
        ByVal tags As List(Of String), _
        ByVal inReplyToUser As String, _
        ByVal inReplyToId As Nullable(Of Long) _
    )
        Me._id = id
        Me._attributes = attributes
        Me._postedAt = postedAt.ToUniversalTime()
        Me._name = name
        Me._screenName = screenName
        Me._imageUri = imageUri
        Me._text = text
        Me._hyperText = Markup(Me.Text)
        Me._tags = tags
        Me._inReplyToUser = inReplyToUser
        Me._inReplyToId = inReplyToId
    End Sub

    ''' <summary>
    ''' 本文を HTML 文章に整形します。
    ''' </summary>
    ''' <param name="text">整形元となる文章。</param>
    ''' <returns>整形された HTML 文章。</returns>
    ''' <remarks>XML 文章としての正当性は必ずしも保証されません。</remarks>
    Public Shared Function Markup(ByVal text As String) As String
        Throw New NotImplementedException()
    End Function
End Class
